using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserOperations.Models;
using UserOperations.ViewModels;

namespace IdentityApp.Controllers
{
    public class RolesController : Controller
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public RolesController(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        [HttpGet]
        public async Task  <IActionResult> RoleList(UserViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            var department = user?.Department;

            if (!User.IsInRole("Admin") || department != "Bilgi İşlem")
            {
                return RedirectToAction("Eror", "Home");
            }

            return View(_roleManager.Roles);
        }
        [HttpPost]
        public  IActionResult RoleList()
        {
            

            return  View (  _roleManager.Roles);
        }

        

        [HttpPost]
        public async Task<IActionResult> RoleDelete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Invalid role Id");
            }

            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return NotFound("Role not found");
            }

            var result = await _roleManager.DeleteAsync(role);

            if (result.Succeeded)
            {
                return RedirectToAction("RoleList");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View("RoleList");
        }

        public async Task<IActionResult> RoleEdit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role != null && role.Name != null)
            {
                ViewBag.Users = await _userManager.GetUsersInRoleAsync(role.Name);
                return View(role);
            }

            return RedirectToAction("RoleList");
        }

        [HttpPost]
        public async Task<IActionResult> RoleEdit(AppRole model)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByIdAsync(model.Id);

                if (role != null)
                {
                    role.Name = model.Name;
                    var result = await _roleManager.UpdateAsync(role);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("RoleList");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    ViewBag.Users = await _userManager.GetUsersInRoleAsync(role.Name=null!);
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> RoleCreate(UserViewModel model2)
        {
            var user = await _userManager.GetUserAsync(User);
            var department = user?.Department;

            if (!User.IsInRole("Admin") || department != "Bilgi İşlem")
            {
                return RedirectToAction("Eror", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RoleCreate(AppRole model)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleManager.CreateAsync(model);

                if (result.Succeeded)
                {
                    return RedirectToAction("RoleList");
                }

                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, err.Description);
                }
            }

            return View(model);
        }
    }
}
