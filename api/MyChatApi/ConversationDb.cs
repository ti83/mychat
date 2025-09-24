using Microsoft.EntityFrameworkCore;

namespace MyChatApi
{
    public class ConversationDb : DbContext
    {
        public ConversationDb(DbContextOptions<ConversationDb> options)
            : base(options) { }
        public DbSet<ConversationDto> Conversations => Set<ConversationDto>();
    }
}
