using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Bikepark.Data;
using Bikepark.Models;

namespace Bikepark.Controllers
{
    [Authorize(Roles = "BikeparkManagers")]
    public class AdministratorsController : Controller
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly BikeparkContext _context;

        public AdministratorsController(BikeparkContext context, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var managers = await _userManager.GetUsersInRoleAsync(BikeparkConfig.ManagersRole);
            var admins = await _userManager.GetUsersInRoleAsync(BikeparkConfig.AdministratorsRole);
            var noadmins = await _userManager.Users.Where(user => !managers.Contains(user) && !admins.Contains(user)).ToListAsync();
            var users = new List<UserRole>();
            users.AddRange(admins.Select(user => new UserRole { User = user, IsAdmin = true }).ToList());
            users.AddRange(noadmins.Select(user => new UserRole { User = user, IsAdmin = false }).ToList());
            return View(users);
        }

        public async Task<IActionResult> WithdrawAdminRole(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            await _userManager.RemoveFromRoleAsync(user, BikeparkConfig.AdministratorsRole);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> AssignAdminRole(string? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            await _userManager.AddToRoleAsync(user, BikeparkConfig.AdministratorsRole);
            return RedirectToAction("Index");
        }

        //DeleteUser -> Archive
        //ARCHIVE

    }
}
