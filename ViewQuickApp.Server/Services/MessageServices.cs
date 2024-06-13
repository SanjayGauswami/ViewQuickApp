using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Security.Claims;
using ViewQuickApp.Server.Core.DbContext;
using ViewQuickApp.Server.Core.Dtos;
using ViewQuickApp.Server.Core.Dtos.Message;
using ViewQuickApp.Server.Core.Entities;
using ViewQuickApp.Server.Interface;

namespace ViewQuickApp.Server.Services
{
    public class MessageServices : IMessage
    {
        private readonly ViewQuickAppDbContext _context;
        private readonly ILogServices _logServices;
        private readonly UserManager<ApplicationUser> _userManager; 

        public MessageServices(ViewQuickAppDbContext context, ILogServices logServices, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _logServices = logServices;
            _userManager = userManager; 
        }
        public async Task<GenralResponseDto> CreateNewMassageAsync(ClaimsPrincipal User, CreateMessageDto createMessageDto)
        {
            if(User.Identity.Name == createMessageDto.ReceiverUserName)
            {
                return new GenralResponseDto()
                {
                    isSucceed = false,
                    statusCode = 400,
                    message = "Sender and Reciver can not be same"
                };  
            }

            var isReciverUserNameValid = _userManager.Users.Any(e=>e.UserName ==  createMessageDto.ReceiverUserName);

            if (!isReciverUserNameValid)
            {
                return new GenralResponseDto()
                {
                    isSucceed = false,
                    statusCode = 400,
                    message = "User can not be exist"
                };
            }

            Message newMassage = new Message()
            {
                SenderUserName = User.Identity.Name,
                ReciverUserName = createMessageDto.ReceiverUserName,
                message = createMessageDto.Text
            };

            await _context.messages.AddAsync(newMassage);
            await _context.SaveChangesAsync();
            await _logServices.SaveNewLog(User.Identity.Name, "Send Message");

            return new GenralResponseDto()
            {
                isSucceed = true,
                statusCode = 201,
                message = "Message is Successfully Sent.."
            };
        }

        public async Task<IEnumerable<GetMessageDto>> GetMassgeAsync()
        {
            var data =  _context.messages.ToList();

           List<GetMessageDto> getMessageDto = new List<GetMessageDto>();
           
            foreach(var  message in data)
            {
               var result = new GetMessageDto()
                {
                    Id = message.Id,
                    SenderUserName = message.SenderUserName,
                    ReceiverUserNaem = message.ReciverUserName,
                    Text = message.message,
                    CreateAt = message.CreatedAt,
                    
                    
                };

                getMessageDto.Add(result);
             
            }

            return getMessageDto;
        }

        public async Task<IEnumerable<GetMessageDto>> GetMyMassageAsync(ClaimsPrincipal User)
        {
            var data = _context.messages.Where(e=>e.SenderUserName == User.Identity.Name).ToList();

            List<GetMessageDto> getMessageDto = new List<GetMessageDto>();

            foreach (var message in data)
            {

             var result =    new GetMessageDto()
                {
                    Id = message.Id,
                    SenderUserName = message.SenderUserName,
                    ReceiverUserNaem = message.ReciverUserName,
                    Text = message.message,
                    CreateAt = message.CreatedAt

                };

                getMessageDto.Add(result);

            }

           return getMessageDto;
        }
    }
}
