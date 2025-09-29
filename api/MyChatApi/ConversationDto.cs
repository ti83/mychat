using Newtonsoft.Json;
using OllamaSharp.Models.Chat;

namespace MyChatApi
{
    public class MessageDto
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "text")]
        public string? Text { get; set; }
        [JsonProperty(PropertyName = "source")]
        public string Source { get; set; }
        [JsonProperty(PropertyName = "timestamp")]
        public DateTime TimeStamp { get; set; }

        public Message GetMessage() => new Message
        {
            Role = Source != "user" ? "assistant" : Source,
            Content = Text ?? string.Empty
        };
    }

    public class ConversationDto
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "title")]
        public string? Title { get; set; }
        [JsonProperty(PropertyName = "messages")]
        public List<MessageDto> Messages { get; set; } = new();
    }

    public class ConversationItemDto
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "title")]
        public string? Title { get; set; }
        public ConversationItemDto() { }
        public ConversationItemDto(ConversationDto conversation) =>
        (Id, Title) = (conversation.Id, conversation.Title);
    }

    public class ConversationTitleSuggestionDto
    {
        [JsonProperty(PropertyName = "title")]
        public string? Title { get; set; }
    }

    public class ResponseFragment
    {
        [JsonProperty(PropertyName = "response")]
        public string? Response { get; set; }
        [JsonProperty(PropertyName = "messageid")]
        public int MessageId { get; set; }
    }
}
