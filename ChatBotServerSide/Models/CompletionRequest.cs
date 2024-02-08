namespace ChatBotServerSide.Models
{
    public class CompletionRequest
    {
        public string Prompt { get; set; }
        public int MaxTokens { get; set; }
    }
}
