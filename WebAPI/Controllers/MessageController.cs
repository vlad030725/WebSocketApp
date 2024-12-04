using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly WebSocketHandler _webSocketHandler;

        public MessageController(WebSocketHandler webSocketHandler)
        {
            _webSocketHandler = webSocketHandler;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] MessageRequest request)
        {
            if (string.IsNullOrEmpty(request.Message))
            {
                return BadRequest("Message cannot be empty.");
            }

            await _webSocketHandler.BroadcastMessage(request.Message);
            return Ok("Message sent to all clients.");
        }
    }
    public class MessageRequest
    {
        public string Message { get; set; }
    }
}
