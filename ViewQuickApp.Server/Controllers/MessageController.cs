using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ViewQuickApp.Server.Core.Dtos.Message;
using ViewQuickApp.Server.Core.OtherObject;
using ViewQuickApp.Server.Interface;

namespace ViewQuickApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessage _message;

        public MessageController(IMessage message)
        {
            _message = message;
        }


        [HttpPost]
        [Route("Create-Message")]
        [Authorize]

        public async Task<IActionResult> CreateNewMassge([FromBody] CreateMessageDto createMessageDto)
        {
            var result = await _message.CreateNewMassageAsync(User, createMessageDto);

            if (result.isSucceed)
            {
                return Ok(result);
            }
            return StatusCode(result.statusCode, result.message);
        }

        [HttpGet]
        [Route("Mine-Message")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<GetMessageDto>>> GetMyMessage()
        {
            var result = await _message.GetMyMassageAsync(User);
            return Ok(result);
        }

        [HttpGet]
        [Route("All-Message")]
        [Authorize(Roles = StaticUserRole.OwnerAdmin)]
        public async Task<ActionResult<IEnumerable<GetMessageDto>>> GetAllMessage()
        {
            var result = await _message.GetMassgeAsync();
            return Ok(result);
        }


    }
}
