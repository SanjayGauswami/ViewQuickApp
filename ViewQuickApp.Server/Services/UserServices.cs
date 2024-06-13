using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ViewQuickApp.Server.Core.Dtos.Auth;
using ViewQuickApp.Server.Interface;

namespace ViewQuickApp.Server.Services
{
    public class UserServices : IUserServices
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserServices(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public RegisterDto getUser()
        {
            throw new NotImplementedException();
        }
    }
}
