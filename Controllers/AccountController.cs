using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserOperations.Models;
using UserOperations.ViewModels;


namespace UserOperations.Controllers
{
    
    public class AccountController : Controller
    {
          private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager <AppUser> _signInManager;
        public AccountController(
        RoleManager<AppRole> roleManager, 
        UserManager<AppUser> userManager,
        SignInManager <AppUser> signInManager
        )
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;

        }
        public ActionResult Login()
        {
            return View();
        }
        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Home");
        }

       [HttpPost]
public async Task<ActionResult> Login(LoginViewModel model)
{
    if (ModelState.IsValid)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user != null)
        {
            await _signInManager.SignOutAsync();
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, true);

           if (result.Succeeded)
{
    var department = user.Department;

    if (department == "Bilgi İşlem")
    {
        return RedirectToAction("ITController", "User");
    }
    else if (department == "İnsan Kaynakları")
    {
        return RedirectToAction("HumanResourcesController", "HumanResources");
    }
    else
    {
        return RedirectToAction("Index", "Home");
    }
}
        }
        else
        {
            ModelState.AddModelError("", "Hatalı E-Posta veya Parola");
        }
    }
    return View(model);
}
}
}
