using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ViewQuickApp.Server.Core.DbContext;
using ViewQuickApp.Server.Core.Dtos.Log;
using ViewQuickApp.Server.Core.Entities;
using ViewQuickApp.Server.Interface;

namespace ViewQuickApp.Server.Services
{
    public class LogServices : ILogServices
    {
        private readonly ViewQuickAppDbContext _context;

        public LogServices(ViewQuickAppDbContext context)
        {
            _context = context;
        }
        public async Task SaveNewLog(string UserName, string Description)
        {
           var newLog = new log()
            {
                UserName = UserName,
                Description = Description
            };

          await  _context.logs.AddAsync(newLog);
          await  _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<GetLogDto>> GetlogAsync()
        {
            var logs = await _context.logs
                .Select(e => new GetLogDto()
                {
                    UserName = e.UserName,
                    Description = e.Description,
                    CreatedAt = e.CreatedAt
                })
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();

            return logs;
        }

        public async Task<IEnumerable<GetLogDto>> GetMyLogAsync(ClaimsPrincipal User)
        {
            var logs = await _context.logs
               .Where(e=> e.UserName == User.Identity.Name)
              .Select(e => new GetLogDto()
              {
                  UserName = e.UserName,
                  Description = e.Description,
                  CreatedAt = e.CreatedAt
              })
              .OrderByDescending(e => e.CreatedAt)
              .ToListAsync();

            return logs;

        }


    }
}
