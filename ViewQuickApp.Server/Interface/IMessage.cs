using System.Security.Claims;
using ViewQuickApp.Server.Core.Dtos;
using ViewQuickApp.Server.Core.Dtos.Message;

namespace ViewQuickApp.Server.Interface
{
    public interface IMessage
    {

        Task<GenralResponseDto> CreateNewMassageAsync(ClaimsPrincipal User, CreateMessageDto createMessageDto);


        Task<IEnumerable<GetMessageDto>> GetMassgeAsync();

        Task<IEnumerable<GetMessageDto>> GetMyMassageAsync(ClaimsPrincipal User);


    }
}
