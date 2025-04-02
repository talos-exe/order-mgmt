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
using X.PagedList;
using X.PagedList.Mvc.Core;
using X.PagedList.Extensions;
using NuGet.Protocol.Plugins;
using static System.Runtime.InteropServices.JavaScript.JSType;



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

        [HttpGet]
        public IActionResult _CreateUser()
        {
            return PartialView("_CreateUser", new UserViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateConfirm(UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid model state.";

                await _logService.LogUserActivityAdmin("[Administrator] Failed user creation due to invalid model state", GetClientIp());

                return RedirectToAction("Index");
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

            //var existingUser = await _userManager.FindByNameAsync(model.UserName);
            //if (existingUser != null)
            //{
            //    ModelState.AddModelError("UserName", "Username is already taken.");
            //    await _logService.LogUserActivityAdmin("[Administrator] Username conflict: " + model.UserName + " already taken.", GetClientIp());
            //}

            //var existingUserEmail = await _userManager.FindByEmailAsync(model.Email);
            //if (existingUserEmail != null)
            //{
            //    ModelState.AddModelError("Email", "Email is already taken.");
            //    await _logService.LogUserActivityAdmin("[Administrator] Email conflict: " + model.Email + " already taken.", GetClientIp());
            //}

            //if (!ModelState.IsValid)
            //{
            //    return View(model);
            //}

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
                await _logService.LogUserActivityAdmin("[Administrator] Created User " + user.UserName, GetClientIp());
                TempData["SuccessMessage"] = "Successfully created user.";

                return RedirectToAction("Index", "UserManagement");
            }

            if (!result.Succeeded)
            {
                TempData["Errors"] = result.Errors.Select(e => e.Description).ToList();
                await _logService.LogUserActivityAdmin("[Administrator] Error creating user " + model.UserName, GetClientIp());

                return RedirectToAction("Index", "UserManagement");
            }

            return RedirectToAction("Index", "UserManagement");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConfirm(string id, UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid model state.";
                await _logService.LogUserActivityAdmin("[Administrator] User edit failed due to invalid model state. UserId: " + id, GetClientIp());
                return RedirectToAction("Index");
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                TempData["Error"] = "User not found.";
                await _logService.LogUserActivityAdmin("[Administrator] User not found during edit attempt. Attempted UserId: " + id, GetClientIp());
                return RedirectToAction("Index");
            }

            var existingUser = await _userManager.FindByNameAsync(model.UserName);
            if (existingUser != null && existingUser.Id != id)
            {
                TempData["Error"] = "Username is already taken.";
                await _logService.LogUserActivityAdmin("[Administrator] Username conflict while updating. Username " + existingUser.UserName + " is already taken.", GetClientIp());
                return RedirectToAction("Index");
            }

            var existingUserEmail = await _userManager.FindByEmailAsync(model.Email);
            if (existingUserEmail != null && existingUserEmail.Id != id)
            {
                TempData["Error"] = "Email is already taken.";
                await _logService.LogUserActivityAdmin("[Administrator] Email conflict while updating. Email " + existingUserEmail.Email + " is already taken by user " + model.UserName, GetClientIp());
                return RedirectToAction("Index");
            }
            
                                        // Enable for specific logging. 
                                        //
                                        //// Check if username is different and log it
                                        //if (user.UserName != model.UserName)
                                        //{
                                        //    await _logService.LogUserActivityAdmin("[Administrator] Changed Username from " + user.UserName + " to " + model.UserName, GetClientIp());
                                        //    user.UserName = model.UserName;
                                        //}

                                        //// Check if email is different and log it
                                        //if (user.Email != model.Email)
                                        //{
                                        //    await _logService.LogUserActivityAdmin($"[Administrator] Changed User {model.Email} Email Address from " + user.Email + " to " + model.Email, GetClientIp());
                                        //    user.Email = model.Email;
                                        //}

                                        //// Check if full name is different and log it
                                        //if (user.FullName != model.FullName)
                                        //{
                                        //    await _logService.LogUserActivityAdmin("[Administrator] Changed Full Name from "+ user.FullName + " to " + model.FullName, GetClientIp());
                                        //    user.FullName = model.FullName;
                                        //}

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                await _logService.LogUserActivityAdmin("[Administrator] User " + user.UserName + " successfully updated.", GetClientIp());
                TempData["SuccessMessage"] = "Successfully edited user.";
                return RedirectToAction("Index", "UserManagement");
            }

            //If Editing user failed, log errors.
            if (!result.Succeeded)
            {
                TempData["Errors"] = result.Errors.Select(e => e.Description).ToList(); 
                await _logService.LogUserActivityAdmin("[Administrator] Error occurred while updating user. UserId: " + user.Id, GetClientIp());
                return RedirectToAction("Index", "UserManagement");
            }

            TempData["SuccessMessage"] = "Successfully edited user.";

            return RedirectToAction("Index", "UserManagement");
        }

        //Delete User (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) { TempData["Errors"] = "User not found.".ToList(); return RedirectToAction("Index"); }
            await _logService.LogUserActivityAdmin("[Administrator] Deleted User " + user.UserName, GetClientIp());
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
            await _logService.LogUserActivityAdmin("[Administrator] Cleared logs.", GetClientIp());

            TempData["SuccessMessage"] = "All logs have been cleared.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> GetUserDetails(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var userViewModel = await _dbContext.Users
                .Where(u => u.Id == userId)
                .Select(u => new UserViewModel
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    FullName = u.FullName,
                    LastLoginDate = u.LastLogin,
                    LastLoginIP = u.LastLoginIP,
                    Logs = _dbContext.UserLogs
                    .Where(log => log.UserId == u.Id)
                    .OrderByDescending(log => log.Timestamp)
                    .ToList()
                })
                .FirstOrDefaultAsync();

            return PartialView("_UserDetails", userViewModel);
        }

        private string GetClientIp()
        {
            return HttpContext.Connection.RemoteIpAddress?.ToString();
        }
    }
}
