using Azure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OrderMgmtRevision.Data;
using OrderMgmtRevision.Models;

namespace OrderMgmtRevision.Services
{
    public interface ILogService
    {
        Task LogUserActivityAsync(string userId, string action, string ipAddress);
        Task<List<UserLog>> GetUserLogsAsync(string userId);
    }

    public class LogService : ILogService
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<User> _userManager;
        public LogService(AppDbContext dbContext, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task LogUserActivityAsync(string userId, string action, string ipAddress)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) { throw new Exception("User not found."); }
            var userName = user?.UserName ?? "Anonymous";
            var log = new UserLog()
            {
                UserId = userId,
                UserName = userName,
                Action = action,
                IpAddress = ipAddress,
                Timestamp = DateTime.UtcNow
            };

            _dbContext.UserLogs.Add(log);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<UserLog>> GetUserLogsAsync(string userId)
        {
            return await _dbContext.UserLogs
                                    .Where(log => log.UserId == userId)
                                    .OrderByDescending(log => log.Timestamp)
                                    .ToListAsync();
        }
    }


}
