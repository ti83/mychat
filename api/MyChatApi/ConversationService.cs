using Microsoft.EntityFrameworkCore;

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
    }
}
