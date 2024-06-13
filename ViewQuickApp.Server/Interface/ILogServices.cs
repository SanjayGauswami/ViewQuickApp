using System.Security.Claims;
using ViewQuickApp.Server.Core.Dtos.Log;

namespace ViewQuickApp.Server.Interface
{
    public interface ILogServices
    {

        Task SaveNewLog(string UserName, string Description);

        Task<IEnumerable<GetLogDto>> GetlogAsync();

        Task<IEnumerable<GetLogDto>> GetMyLogAsync(ClaimsPrincipal User);

    }
}
