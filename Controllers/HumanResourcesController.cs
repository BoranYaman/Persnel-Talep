using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserOperations.Models;
using UserOperations.ViewModels;

namespace UserOperations.Controllers
{
    public class HumanResourcesController : Controller
    {
         private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public HumanResourcesController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
          [HttpPost]
        public async Task<IActionResult> PersonnelExitt(string id, UserEditViewModel model)
        
        {
           
            
            if (id != model.Id)
            {
                return RedirectToAction("UserList", "User");
            }

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.Position = model.Position;

                    var emailResult = await _userManager.SetEmailAsync(user, model.EmailAddress);
                    if (!emailResult.Succeeded)
                    {
                        foreach (var err in emailResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, err.Description);
                        }
                        return View(model);
                    }

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        await _userManager.RemoveFromRolesAsync(user, await _userManager.GetRolesAsync(user));

                        if (model.SelectedRoles != null)
                        {
                            await _userManager.AddToRolesAsync(user, model.SelectedRoles);
                        }

                        return RedirectToAction("UserUpdate", "User");
                    }

                    foreach (var err in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, err.Description);
                    }
                }
            }

            return View(model);
        }
            [HttpGet]
        public async Task<IActionResult> PersonnelExitt(string id, UserViewModel model)
        {
            var user2 = await _userManager.GetUserAsync(User);
            var department = user2?.Department;

            if ( department != "İnsan Kaynakları" || department != "İnsan Kaynakları")
            {
                return RedirectToAction("Eror", "Home");
            }

            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return RedirectToAction("UserList", "User");
            }

            if (user != null)
            {
                ViewBag.Roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();

                return View(new UserEditViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    EmergencyPhoneNumber = user.EmergencyPhoneNumber,
                    EmergencyContactName = user.EmergencyContactName,
                    EmailAddress = user.Email,
                    Department = user.Department,
                    Address = user.Address,
                    Note = user.Note,
                    Salary=user.Salary,
                    Position=user.Position,
                    SelectedRoles = await _userManager.GetRolesAsync(user),
                });
            }

            return RedirectToAction("Index");
        }

        public async Task <IActionResult>humanResourcesController(UserViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            var department = user?.Department;
             if ( department != "İnsan Kaynakları" || department != "İnsan Kaynakları")
            {
                return RedirectToAction("Eror", "Home");
            }
            return View(model); 
        }
     
          public async Task<IActionResult> PersonnelExit(UserViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            var department = user?.Department;

            if (!User.IsInRole("Admin") || department != "İnsan Kaynakları" || department != "Insan Kaynakları")
            {
                return RedirectToAction("Eror", "Home");
            }

            var users = _userManager.Users.ToList();
            var userViewModel = new UserViewModel { Users = users };
            return View(userViewModel);
        }

        
    }
}