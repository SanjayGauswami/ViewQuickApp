using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ViewQuickApp.Server.Core.Dtos;
using ViewQuickApp.Server.Core.Dtos.Auth;


namespace ViewQuickApp.Server.Interface
{
    public interface IAuthServices
    {
         Task<GenralResponseDto> SeedRoleAsync();

         Task<GenralResponseDto> RegisterAsync(RegisterDto registerDto);
         
        Task<LoginServicesResponseDto?> LoginAsync(LoginDto loginDto);

        Task<GenralResponseDto> UpdateRoleAsync(ClaimsPrincipal User, UpdateDto updateDto);

       Task<LoginServicesResponseDto?> MeAsync(MeDto meDto);

        Task<IEnumerable<UserInfoResult>> GetUserListAsync();

        Task<UserInfoResult> GetUserDetailsByUserName(string userName);

        Task<IEnumerable<string>> GetUsernameListAsync();
            

    }


}
