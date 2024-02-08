using ChatBotServerSide.Models;
using ChatBotServerSide.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChatBotServerSide.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatBotController : ControllerBase
    {

        private readonly ChatGPTService _chatGPTService;

        public ChatBotController(ChatGPTService chatGPTService)
        {
            _chatGPTService = chatGPTService;
        }

        [HttpGet("{prompt}")]
        public async Task<IActionResult> Get(string prompt)
        {
            try
            {
                var response = await _chatGPTService.GetResponseFromOpenAI(prompt);
                return Ok(response);
            }
            catch (HttpRequestException e)
            {
                // לוג ו/או החזרת תגובת שגיאה מתאימה
                return StatusCode(500, "An error occurred while requesting data from ChatGPT");
            }
        }
    }
}
