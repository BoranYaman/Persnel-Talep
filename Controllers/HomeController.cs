using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserOperations.Models;

namespace UserOperations.Controllers;

public class HomeController : Controller
{

           private readonly UserOperationsContext _context;

        public HomeController(UserOperationsContext context )
        {
            _context = context;

        }
  

    public async Task  <IActionResult> Index()
    {

        var announcements = await _context.Announcements
                                      .Where(a => a.Situation == "Yayında")
                                      .ToListAsync();
                                      return View(announcements);
    }
            public IActionResult Eror()
            {
                return View();
            }
    public IActionResult Privacy()
    {
        return View();
    }

}
