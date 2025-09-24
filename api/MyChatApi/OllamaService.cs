namespace MyChatApi
{
    public class OllamaService
    {
        private OllamaSharp.OllamaApiClient _ollamaClient;
        public void Initialize()
        {
            _ollamaClient = new OllamaSharp.OllamaApiClient(new Uri("http://localhost:11434"));
        }

        public Stream AskQuestion(string question)
        {
            return null;
        }
    }
}
