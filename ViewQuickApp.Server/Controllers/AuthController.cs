using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ViewQuickApp.Server.Core.Dtos.Auth;
using ViewQuickApp.Server.Core.OtherObject;
using ViewQuickApp.Server.Interface;
using ViewQuickApp.Server.Core.Dtos;


namespace ViewQuickApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthServices _authServices;

        public AuthController(IConfiguration configuration, IAuthServices authServices)
        {
            _configuration = configuration;
            _authServices = authServices;
        }


        [HttpPost]
        [Route("Role-seeding")]
        [Authorize(Roles = StaticUserRole.OwnerAdmin)]

        public async Task<IActionResult> SeedRole()
        {
           var res = await _authServices.SeedRoleAsync();
            return StatusCode(res.statusCode, res.message);
            
        }

        [HttpPost("Register")]

        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
          var data = await _authServices.RegisterAsync(registerDto);

            return StatusCode(data.statusCode, data.message);

        }

        [HttpPost]
        [Route("Login")]

        public async Task<ActionResult<LoginServicesResponseDto>> Login([FromBody] LoginDto loginDto)
        {
            var userLogin = await _authServices.LoginAsync(loginDto);

           if(userLogin == null)
            {
                return Unauthorized("Your Credential is inCorrect.."); 
            }

           return Ok(userLogin);
        }

        
        //Update Role
        //Owner and Admin only chnage the role
        //user and manger can not access this route
        [HttpPost]
        [Route("UpdateRoles")]
        [Authorize(Roles = StaticUserRole.OwnerAdmin)]

        public async Task<IActionResult> UpdateRole([FromBody] UpdateDto updateDto)
        {
            var result = await _authServices.UpdateRoleAsync(User,updateDto);
            if (result.isSucceed)
                return Ok(result.message);
            else 
                return StatusCode(result.statusCode, result.message);
        }

        [HttpPost]
        [Route("Me")]

         public async Task<ActionResult<LoginServicesResponseDto>> Me(MeDto meDto)
        {
            var result = await _authServices.MeAsync(meDto);
            { 
                if(result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return Unauthorized("Token Invalid");
                }
            }
           
        }

        // List of all user Details

        [HttpGet]
        [Route("User")]
        [Authorize(Roles = StaticUserRole.OwnerAdmin)]

        public async Task<ActionResult<IEnumerable<UserInfoResult>>> GetUserList()
        {
            var result = await _authServices.GetUserListAsync(); 
            return Ok(result);

        }


        // get user by username

        [HttpGet]
        [Route("user/{userName}")]

        public async Task<ActionResult<UserInfoResult>> GetUserByUserName(string userName)
        {
            var result = await _authServices.GetUserDetailsByUserName(userName);

            if(result != null)
            {
                return Ok(result);
            }
            else
            {
                return Unauthorized("UserName Not Found");
            }
        }


        // get all username for sending a message
        [HttpGet]
        [Route("GetUserName")]

        public async Task<ActionResult<IEnumerable<string>>> GetUserName()
        {
            var result = await _authServices.GetUsernameListAsync();

            return Ok(result);
        }

    }
}
