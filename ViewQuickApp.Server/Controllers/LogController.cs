using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using ViewQuickApp.Server.Core.Dtos.Log;
using ViewQuickApp.Server.Core.OtherObject;
using ViewQuickApp.Server.Interface;

namespace ViewQuickApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly ILogServices _logServices;

        public LogController(ILogServices logServices)
        {
            _logServices = logServices;
        }

        [HttpGet]
        [Route("All-Log")]
        [Authorize(Roles = StaticUserRole.OwnerAdmin)]

        public async Task<ActionResult<IEnumerable<GetLogDto>>> GetLogs()
        {
            var logs =await _logServices.GetlogAsync();
            return Ok(logs);

        }

        [HttpGet]
        [Route("Mine-Log")]
        [Authorize]

        public async Task<ActionResult<IEnumerable<GetLogDto>>> GetMyLog()
        {
            var log = await _logServices.GetMyLogAsync(User);
            return Ok(log);
        }
    }
}
