using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserOperations.Entity;
using UserOperations.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace UserOperations.Controllers
{
    public class AnnouncementController : Controller
    {
        private readonly UserOperationsContext _context;
         private readonly UserManager<AppUser> _userManager;

        public AnnouncementController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager,UserOperationsContext context )
        {
            _userManager = userManager;
            _context = context;

        }
      
        public async Task <ActionResult> MyAnnouncement(Announcement model)
        {
           var user = await _userManager.GetUserAsync(User);
            var mail = user?.Email; 
             var announcements = await _context.Announcements
                                      .Where(a => a.UEmailAddress == mail)
                                      .ToListAsync();
                                      return View(announcements);
        }
        
        [HttpGet]
        public IActionResult AnnouncementCreate( )
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AnnouncementCreate(Announcement model)
        {
            var   user = await _userManager.GetUserAsync(User);
            var departman = user?.Department;
            var mail = user?.Email;
{
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Announcements.Add(model);
                    model.AnnouncementDepartment = departman;
                    model.ThePublisher = user?.UserName;
                    model.UEmailAddress = mail;

                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index","Home");
                    
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Veritabanıan ekleme sırasında bir hata oluştu");
                }
                    }
                
                         }
                    return RedirectToAction("Index","Home");

                 }
                 [HttpGet]
                public async Task <IActionResult> AnnouncementUpdate(int? id)
                 {
                   if (id == null)
                     {
                        return NotFound();

                     }
                     var announcement = await _context.Announcements.FirstOrDefaultAsync(x => x.Id == id);
                     if (announcement == null)
                     {
                     return NotFound();
                     }
                    return View(announcement);
                 }
                 [HttpPost]
                 [ValidateAntiForgeryToken]
                 public async Task <IActionResult> AnnouncementUpdate(int id ,Announcement model )
                 {
                    var user = await _userManager.GetUserAsync(User);
                    var username = user?.UserName;
                    var departman = user?.Department;

                    var email = user?.Email;
                    if (id != model.Id)
                    {
                        return NotFound();
                    }
                    if (ModelState.IsValid)
                    {
                        try{
                                  model.AnnouncementDepartment = departman;
                    model.ThePublisher = user?.UserName;
                            model.UEmailAddress = email;
                            _context.Update(model);
                            await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            return View(model);
                        }
                    }
                       return RedirectToAction("MyAnnouncement","Announcement");
                 }

                 [HttpGet]
                 public async Task <IActionResult> AnnouncementDelete(int? id)
                 {
                    if (id == null){
                    return NotFound();
                    }
                    var announcement = await _context.Announcements.FindAsync(id);
                    if (announcement == null)
                    {
                        return NotFound();
                    }
                    return View( announcement);
                 }
                 [HttpPost]
                 public async Task<IActionResult> AnnouncementDelete([FromForm] int id)
                 {
                    var announcement = await _context.Announcements.FindAsync(id);
                    if(announcement == null)
                    {
                        return NotFound();
                    }
                    _context.Announcements.Remove(announcement);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("MyAnnouncement","Announcement");
                 }
}
}