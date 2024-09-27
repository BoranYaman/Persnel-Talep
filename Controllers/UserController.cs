using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserOperations.Models;
using UserOperations.ViewModels;

namespace UserOperations.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public UserController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

     

        [HttpPost]
        public async Task<IActionResult> UserDeleteForm(string id)
        {
            if (!User.IsInRole("Admin"))
            {
                return RedirectToAction("Controller", "User");
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }

            return RedirectToAction("UserDelete", "User");
        }

        public async Task<IActionResult> UserDelete(UserViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            var department = user?.Department;

            if (!User.IsInRole("Admin") || department != "Bilgi İşlem")
            {
                return RedirectToAction("Eror", "Home");
            }

            var users = _userManager.Users.ToList();
            var userViewModel = new UserViewModel { Users = users };
            return View(userViewModel);
        }

        public async Task<IActionResult> UserUpdate(UserViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            var department = user?.Department;

            if (!User.IsInRole("Admin") || department != "Bilgi İşlem" && department != "İnsan Kaynakları")
            {
                return RedirectToAction("Eror", "Home");
            }

            var users = _userManager.Users.ToList();
            var userViewModel = new UserViewModel { Users = users };
            return View(userViewModel);
        }

        public async Task <IActionResult> ITController(UserViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            var department = user?.Department;
             if (!User.IsInRole("Admin") || department != "Bilgi İşlem")
            {
                return RedirectToAction("Eror", "Home");
            }
            return View(model); 
        }

        [HttpPost]
        public async Task<IActionResult> UserEdit(string id, UserEditViewModel model)
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
                    user.UserName = model.UserName;
                    user.LastName = model.LastName;
                    user.PhoneNumber = model.PhoneNumber;
                    user.EmergencyPhoneNumber = model.EmergencyPhoneNumber;
                    user.EmergencyContactName = model.EmergencyContactName;
                    user.Address = model.Address;
                    user.Note = model.Note;
                    user.Department = model.Department;
                    user.Salary =model.Salary;
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

        public async Task<IActionResult> UserEdit(string id, UserViewModel model)
        {
            var user2 = await _userManager.GetUserAsync(User);
            var department = user2?.Department;

            if (!User.IsInRole("Admin") || department != "Bilgi İşlem" && department != "İnsan Kaynakları")
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

       

        public async Task<IActionResult> UserCreate(UserViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            var department = user?.Department;

            if (!User.IsInRole("Admin") || department != "Bilgi İşlem" && department != "İnsan Kaynakları")
            {
                return RedirectToAction("Eror", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UserCreate(UserCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    UserName = model.UserName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    EmergencyPhoneNumber = model.EmergencyPhoneNumber,
                    EmergencyContactName = model.EmergencyContactName,
                    Address = model.Address,
                    Note = model.Note,
                    EmailAddress = model.EmailAddress,
                    Department = model.Department,
                    Salary = model.Salary,
                    Position = model.Position,

                };

                IdentityResult result = await _userManager.CreateAsync(user, model.Password=null!);
                if (result.Succeeded)
                {
                    return RedirectToAction("ITController", "User");
                }

                foreach (IdentityError err in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, err.Description);
                }
            }

            return View(model);
        }
       [HttpGet]
public async Task<IActionResult> MyAccount()
{
    var user = await _userManager.GetUserAsync(User);

    if (user == null)
    {
        return RedirectToAction("Index");
    }

    var model = new UserEditViewModel
    {
        UserName = user.UserName,
        LastName = user.LastName,
        PhoneNumber = user.PhoneNumber,
        EmailAddress = user.EmailAddress,
        Department = user.Department,
        Address = user.Address,
        Position = user.Position,
    };

    return View(model);
}

[HttpPost]
public async Task<IActionResult> MyAccount(string id, UserEditViewModel model)
{
    if (id == null)
    {
        return RedirectToAction("Index");
    }

    var user = await _userManager.FindByIdAsync(id);
    
    if (user == null)
    {
        return RedirectToAction("Index");
    }

    if (ModelState.IsValid)
    {
        user.UserName = model.UserName;
        user.LastName = model.LastName;
        
        
        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded)
        {
            return RedirectToAction("MyAccount");
        }
        
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
    }

    return View(model);
}

    }
}
        
      
      

