using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OllamaSharp;
using OllamaSharp.Models;
using System.Threading;

namespace MyChatApi
{
    public class ConversationService
    {
        private ConversationDb _db;

        public ConversationService(ConversationDb db) 
        {
            _db = db;
        }

        public async Task<IResult> SuggestConversationTitleFromPrompt(HttpContext context, [FromServices] OllamaApiClient ollama, string prompt)
        {
            if (prompt == null) return TypedResults.NotFound();
            var cancellationToken = context.RequestAborted;
            var request = new GenerateRequest
            {
                Model = "llama3",
                Prompt = $"Suggest a short title (only return the suggested title) for the following conversation:\n\n{prompt}",
                Stream = false,
            };
            var response = string.Empty;
            await foreach (var stream in ollama.GenerateAsync(request, cancellationToken))
            {
                response+=(stream?.Response);                
            }
            var title = response.Trim().Trim('"').Trim('\'');
            return TypedResults.Ok(new ConversationTitleSuggestionDto { Title = title });
        }

        public async Task<IResult> CreateNewConversation(ConversationDto conversation)
        {
            _db.Conversations.Add(conversation);
            await _db.SaveChangesAsync();
            return TypedResults.Created($"/conversations/{conversation.Id}", conversation);
        }

        public async Task<IResult> UpdateConversation(int id, ConversationDto conversation)
        {
            var existingConversation = await _db.Conversations.FindAsync(id);
            if (existingConversation is null) return TypedResults.NotFound();
            existingConversation.Title = conversation.Title;
            existingConversation.Messages = conversation.Messages;
            await _db.SaveChangesAsync();
            return TypedResults.NoContent();
        }

        public async Task<IResult> AddMessageToConversation(int id, MessageDto message)
        {
            var conversation = await _db.Conversations.FindAsync(id);
            if (conversation is null) return TypedResults.NotFound();
            conversation.Messages.Add(message);
            await _db.SaveChangesAsync();
            return TypedResults.NoContent();
        }

        private async Task UpdateMessage(int id, int messageId, string messageText)
        {
            var conversation = await _db.Conversations.FindAsync(id);
            if (conversation is null) return;
            var existingMessage = conversation.Messages.FirstOrDefault(m => m.Id == messageId);
            if (existingMessage is null) return ;
            existingMessage.Text = messageText;
            await _db.SaveChangesAsync();
        }

        public async Task<IResult> GetConversation(int id)
        {
            var conversation = await _db.Conversations.FindAsync(id);
            return conversation is not null ? TypedResults.Ok(conversation) : TypedResults.NotFound();
        }
        public async Task<IResult> GetConversationHeader(int id)
        {
            var conversation = await _db.Conversations.FindAsync(id);
            return conversation is not null ? TypedResults.Ok(new ConversationItemDto(conversation)) : TypedResults.NotFound();
        }

        public async Task<IResult> GetAllConversations()
        {
            var conversations = await _db.Conversations.Select(c => new ConversationItemDto(c)).ToArrayAsync();
            return TypedResults.Ok(conversations);
        }

        public async Task AskQuestion(HttpContext context, [FromServices] OllamaApiClient ollama, int id, MessageDto message)
        {

            var existingConversation = await _db.Conversations.FindAsync(id);
            if (existingConversation is null) 
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                return;
            }

            // Validate the request body
            if (string.IsNullOrEmpty(message.Text))
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Prompt is required in the request body.");
                return;
            }

            if (string.IsNullOrWhiteSpace(existingConversation.Title))
            {
                var response = await SuggestConversationTitleFromPrompt(context, ollama, message.Text);
                var okResponse = response as Microsoft.AspNetCore.Http.HttpResults.Ok<ConversationTitleSuggestionDto>;
                var suggestion = okResponse?.Value;
                if (suggestion != null)
                {
                    existingConversation.Title = suggestion.Title;
                }
            }
            
            // Retrieve previous messages.
            var messages = existingConversation.Messages.OrderBy(m => m.TimeStamp)
                .Select(m => m.GetMessage())
                .ToList();

            await AddMessageToConversation(id, message);

            // Set the appropriate content type for server-sent events
            context.Response.ContentType = "text/event-stream";
            context.Response.Headers.Add("Cache-Control", "no-cache");
            context.Response.Headers.Add("Connection", "keep-alive");

            var cancellationToken = context.RequestAborted;

            var chat = new Chat(ollama);
            chat.Model = "llama3";
            chat.Messages = messages;

            var responseMessage = new MessageDto
            {
                Source = chat.Model,
                TimeStamp = DateTime.UtcNow,                
            };

            existingConversation.Messages.Add(responseMessage);

            await foreach (var response in chat.SendAsync(message.Text, cancellationToken))
            {
                responseMessage.Text += response;
            }

            await _db.SaveChangesAsync();
            var json = JsonConvert.SerializeObject(responseMessage, Formatting.None);
            await context.Response.WriteAsync(json);
            await context.Response.Body.FlushAsync(cancellationToken);
        }
    }
}
