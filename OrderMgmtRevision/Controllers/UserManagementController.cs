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

namespace OrderMgmtRevision.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserManagementController : Controller
    {
        private readonly UserManager<User> _userManager;

        public UserManagementController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        // When clicked on UserManagement Page, Index loads
        public IActionResult Index()
        {
            var users = _userManager.Users.Select(u => new UserViewModel
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                FullName = u.FullName
            }).ToList();

            return View(users);

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
                FullName = model.FullName
            };

            var result = await _userManager.CreateAsync(user, model.Password);
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

            var result = await _userManager.UpdateAsync(user);

            return RedirectToAction("Index");
        }

        //Delete User (POST)
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            await _userManager.DeleteAsync(user);
            return RedirectToAction("Index");
        }
    }
}
