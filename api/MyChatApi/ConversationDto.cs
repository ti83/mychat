using Newtonsoft.Json;
using OllamaSharp.Models.Chat;
using System.Text.Json.Serialization;

namespace MyChatApi
{
    public class MessageDto
    {
        [JsonProperty(PropertyName = "id")]
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "text")]
        [JsonPropertyName("text")]
        public string? Text { get; set; }
        [JsonProperty(PropertyName = "source")]
        [JsonPropertyName("source")]
        public string Source { get; set; }
        [JsonProperty(PropertyName = "timestamp")]
        [JsonPropertyName("timestamp")]
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
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "title")]
        [JsonPropertyName("title")]
        public string? Title { get; set; }
        [JsonProperty(PropertyName = "messages")]
        [JsonPropertyName("messages")]
        public List<MessageDto> Messages { get; set; } = new();
    }

    public class ConversationItemDto
    {
        [JsonProperty(PropertyName = "id")]
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "title")]
        [JsonPropertyName("title")]
        public string? Title { get; set; }
        public ConversationItemDto() { }
        public ConversationItemDto(ConversationDto conversation) =>
        (Id, Title) = (conversation.Id, conversation.Title);
    }

    public class ConversationTitleSuggestionDto
    {
        [JsonProperty(PropertyName = "title")]
        [JsonPropertyName("title")]
        public string? Title { get; set; }
    }

    public class ResponseFragment
    {
        [JsonProperty(PropertyName = "response")]
        public string? Response { get; set; }
        [JsonProperty(PropertyName = "messageid")]
        [JsonPropertyName("messageid")]
        public int MessageId { get; set; }
    }
}
