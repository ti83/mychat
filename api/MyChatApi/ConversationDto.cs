namespace MyChatApi
{
    public class MessageDto
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public string Source { get; set; }
        public DateTime TimeStamp { get; set; }
    }

    public class ConversationDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public List<MessageDto> Messages { get; set; } = new();
    }

    public class ConversationItemDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public ConversationItemDto() { }
        public ConversationItemDto(ConversationDto conversation) =>
        (Id, Title) = (conversation.Id, conversation.Title);
    }
}
