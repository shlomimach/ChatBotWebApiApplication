using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace ChatBotServerSide.Services
{
    public class ChatGPTService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private string? _apiKey;

        public ChatGPTService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");

        }



        /// <summary>
        /// In the GetResponseFromOpenAI method, 
        /// we encapsulate the process of querying the OpenAI API to retrieve responses based on a given prompt. 
        /// This method demonstrates a structured approach to interact with external APIs using the HttpClient
        /// provided by the IHttpClientFactory, 
        /// ensuring efficient management and reuse of HTTP connections.
        /// </summary>
        /// <param name="prompt"></param>
        /// <returns></returns>
        public async Task<string> GetResponseFromOpenAI(string prompt)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://api.openai.com/v1/");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);


            // יצירת אובייקט הבקשה עם הפרומפט ופרמטרים נוספים.
            var request = new { model = "davinci-002", prompt = prompt, temperature = 0.5, max_tokens = 100 };

            // סיריאליזציה של הבקשה ל-JSON ויצירת StringContent לשליחה.
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            //בדיקה שהכתובת היא הנכונה
            Console.WriteLine($"Sending request to: {client.BaseAddress}/completions");

            // ביצוע הבקשה POST עם הגוף שהוגדר ל-API.
            var response = await client.PostAsync("completions", content);

            if (response.IsSuccessStatusCode)
            {
                // אם הבקשה הצליחה, קרא את התגובה והחזר אותה.
                var responseContent = await response.Content.ReadAsStringAsync();
                return responseContent;
            }
            else
            {
                // במקרה של תגובת שגיאה מה-API, החזר הודעת שגיאה מתאימה.
                return $"Error: {response.StatusCode}";
            }
        }
    }
}

