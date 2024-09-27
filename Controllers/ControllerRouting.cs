using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserOperations.Models;

namespace UserOperations.Controllers
{
    public class ControllerRouting: Controller
    {
         private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public ControllerRouting(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
       
       [HttpGet]
        public async Task<IActionResult> Controller()
        {
            var user = await _userManager.GetUserAsync(User);
            var department = user?.Department;

           
            if (string.IsNullOrEmpty(department))
            {
                
                return RedirectToAction("Eror", "Home");
            }

          
            if (department == "Bilgi İşlem")
            {
                return RedirectToAction("ITController", "User");
            }
            else if (department == "İnsan Kaynakları" || department == "insan Kaynakları" )
            {
                return RedirectToAction("humanResourcesController", "HumanResources");
            }
            else if(department == "İdari İşler")
            {
                return RedirectToAction("administrativeAffairsController","AdministrativeAffairs");
            }

            return RedirectToAction("Index", "Home"); 
        }

    }
}