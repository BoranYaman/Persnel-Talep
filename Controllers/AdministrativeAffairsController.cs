using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserOperations.Entity;
using UserOperations.Models;
using UserOperations.ViewModels;

namespace UserOperations.Controllers
{
    public class AdministrativeAffairsController:Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserOperationsContext _contex;
        public  AdministrativeAffairsController (UserManager<AppUser> userManager ,RoleManager<AppRole>roleManager, UserOperationsContext context)
        {
            _contex = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task <IActionResult> administrativeAffairsController(UserViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            var department=user?.Department;
            if(department !="İdari İşler")
            {
                return RedirectToAction("Eror", "Home");
            }
            return View();
        }
        [HttpGet]
        public IActionResult AddACar()
        {
            return View();
        }
        [HttpPost]
public async Task <IActionResult> AddCar(Car model)
{
     var carstatus= "Yeni Araç";
    if (ModelState.IsValid)
    {
        model.CarStatus=carstatus;
        _contex.Cars.Add(model);
        await _contex.SaveChangesAsync();
        return RedirectToAction("administrativeAffairsController" ,"AdministrativeAffairs");

    }

    return View(model); 
}


        public async Task <IActionResult> CarTransactions( )
        {
        return View( await _contex.Cars.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> CarEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var car = await _contex.Cars.FirstOrDefaultAsync(x => x.Id==id);
            if (car == null)
            {
                return NotFound();
            }
            return View(car);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CarEdit(int? id, Car model)
        {
            if (id == null)
            {
                return NotFound();
            }
            if(ModelState.IsValid)
            {
                _contex.Update(model);
                await _contex.SaveChangesAsync();
            }
            return RedirectToAction("CarTransactions","AdministrativeAffairs");
        }
        [HttpPost]

    public async Task<IActionResult> CarDelete(int?id ,Car model)

    {
        if (id == null)
        {
            return NotFound();
        }
        var car = await _contex.Cars.FindAsync(id);
        if(car == null)
        {
            return NotFound();
        }
        _contex.Cars.Remove(car);
        await _contex.SaveChangesAsync();
        return RedirectToAction("CarTransactions","AdministrativeAffairs");

    }

        public async  Task<IActionResult> CarDelivery()
        {
            return View(await _contex.Cars.ToListAsync());
        }
        //

    }
}