using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderMgmtRevision.Models;
using System.Threading.Tasks;
using System.Linq;
using Azure.Identity;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Microsoft.EntityFrameworkCore.Metadata;
using OrderMgmtRevision.Data;
using OrderMgmtRevision.Services;

namespace OrderMgmtRevision.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserManagementController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _dbContext;
        private readonly ILogService _logService;

        public UserManagementController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, AppDbContext dbContext, ILogService logService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _dbContext = dbContext;
            _logService = logService;
        }

        // When clicked on UserManagement Page, Index loads
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users
                .Include(u => u.Logs)
                .ToListAsync();

            var userViewModels = new List<UserViewModel>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userViewModels.Add(new UserViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    FullName = user.FullName,
                    LastLoginDate = user.LastLogin,
                    LastLoginIP = user.LastLoginIP,
                    Logs = user.Logs ?? new List<UserLog>()
                });
            }
            return View(userViewModels);
         }

        [HttpPost]
        public async Task<IActionResult> CreateConfirm(UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                              .SelectMany(v => v.Errors)
                              .Select(e => e.ErrorMessage)
                              .ToList();
                return Json(new { success = false, error = string.Join("<br/>", errors) });
            }

            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                FullName = model.FullName,
                DateCreated = DateTime.UtcNow,
                LastLoginIP = "::",
                CreatedBy = User.Identity.Name,
                LastPasswordChange = DateTime.UtcNow
            };


            var result = await _userManager.CreateAsync(user, model.Password);

            await _logService.LogUserActivityAsync(user.Id, "[Administrator] Created User " + user.UserName, GetClientIp());
            TempData["SuccessMessage"] = "Successfully created user.";

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> EditConfirm(string id, UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                              .SelectMany(v => v.Errors)
                              .Select(e => e.ErrorMessage)
                              .ToList();
                return Json(new { success = false, error = string.Join("<br/>", errors) });
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.UserName = model.UserName;
            user.Email = model.Email;
            user.FullName = model.FullName;
            user.LastPasswordChange = DateTime.UtcNow;

            await _logService.LogUserActivityAsync(user.Id, "[Administrator] Edited User " + user.UserName, GetClientIp());

            var result = await _userManager.UpdateAsync(user);

            TempData["SuccessMessage"] = "Successfully edited user.";

            return RedirectToAction("Index");
        }

        //Delete User (POST)
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            await _logService.LogUserActivityAsync(user.Id, "[Administrator] Deleted User " + user.UserName, GetClientIp());
            await _userManager.DeleteAsync(user);
            TempData["SuccessMessage"] = "Successfully deleted user.";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> UserLogs()
        {
            var userLogs = await _dbContext.UserLogs
                .Include(log => log.User)
                .Select(log => new UserLog
                {
                    UserId = log.UserId,
                    UserName = log.UserName,
                    Action = log.Action,
                    Timestamp = log.Timestamp,
                    IpAddress = log.IpAddress
                })
                .ToListAsync();
            return View(userLogs ?? new List<UserLog>());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClearLogs()
        {
            _dbContext.UserLogs.RemoveRange(_dbContext.UserLogs);
            await _dbContext.SaveChangesAsync();

            // Reset the identity seed
            await _dbContext.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('UserLogs', RESEED, 0)");

            TempData["SuccessMessage"] = "All logs have been cleared.";
            return RedirectToAction("Index");
        }

        private string GetClientIp()
        {
            return HttpContext.Connection.RemoteIpAddress?.ToString();
        }
    }
}
