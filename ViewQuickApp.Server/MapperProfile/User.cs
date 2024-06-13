using AutoMapper;
using Microsoft.AspNetCore.Identity;
using ViewQuickApp.Server.Core.Dtos.Auth;
using ViewQuickApp.Server.Core.Entities;

namespace ViewQuickApp.Server.MapperProfile
{
    public class User : Profile
    {
        public User()
        {
            
            CreateMap<RegisterDto, ApplicationUser>();


        }

    }
}
