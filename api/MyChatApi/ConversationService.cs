using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OllamaSharp;
using OllamaSharp.Models;

namespace MyChatApi
{
    public class ConversationService
    {
        private ConversationDb _db;

        public ConversationService(ConversationDb db) 
        {
            _db = db;
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
        public async Task<IResult> GetConversation(int id)
        {
            var conversation = await _db.Conversations.FindAsync(id);
            return conversation is not null ? TypedResults.Ok(conversation) : TypedResults.NotFound();
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

            // Retrieve previous messages.
            var conversation = await _db.Conversations.FindAsync(id);
            var messages = conversation.Messages.OrderBy(m => m.TimeStamp)
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

            await foreach (var response in chat.SendAsync(message.Text, cancellationToken))
            {
                responseMessage.Text += response;
                await context.Response.WriteAsync(response);
                await context.Response.Body.FlushAsync(cancellationToken);
            }

            await AddMessageToConversation(id, responseMessage);

        }
    }
}
