using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using UserOperations.Entity;
using UserOperations.Models;
using UserOperations.ViewModels;

namespace UserOperations.Controllers
{
  public class  RequestController:Controller
  {
    private readonly UserOperationsContext _context;
    private readonly UserManager <AppUser> _userManager;

    public RequestController(UserOperationsContext context ,UserManager<AppUser> userManager)
    {
      _context = context;
      _userManager = userManager;

    }
    public async Task <IActionResult>FinalRequest(int? id)
    {
       var user = await _userManager.GetUserAsync(User);
    if (user == null || string.IsNullOrEmpty(user.Department))
    {
        return NotFound("Kullanıcı veya departman bilgisi bulunamadı.");
    }

    var department = user.Department;

    var finalrequest = await _context.Requests
                                    .Where(r =>  r.Status == "Sonuçlandı" && r.UDepartment == department)
                                    .ToListAsync();
    
   

    var viewModel = new RequestViewModel
    {
        FinalRequest = finalrequest,
    };

    return View(viewModel);

    }
        [HttpGet]
    public async Task  <IActionResult> RequestProcess(int? id )
    {
      if (id == null)
      {
        return NotFound();
      }
      var request = await _context.Requests.FirstOrDefaultAsync(r => r.Id == id);
    var request2 = await _context.Requests
                                .Include(r => r.Processes)
                                .FirstOrDefaultAsync(x => x.Id == id);
      if (request == null)
      {
        return NotFound();
      }
      return View(request);

    }
    
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> RequestProcess(int? id, string comments, Request model)
{
    var user = await _userManager.GetUserAsync(User);
    var departman = user?.Department;

    var request = await _context.Requests
                                .Include(r => r.Processes) 
                                .FirstOrDefaultAsync(x => x.Id == id);

    if (id == null || request == null)
    {
        return NotFound();
    }
    if (ModelState.IsValid)
    {
        try
        {
            var process = new RequestProcess
            {
              UserName = user?.UserName,
                UserMail = user?.Email,
                ProcessDetail = comments,
                ProcessDate = DateTime.Now,
                RequestId = request.Id
            };
            request.Processes?.Add(process);
            
            
            request.Status = model.Status; 
            
            

            _context.Update(request);  


            await _context.SaveChangesAsync(); 
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Requests.Any(r => r.Id == request.Id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
            
        }
          


    }
    

    return RedirectToAction("RequestList", "Request");
    
}



 public async Task<IActionResult> MyRequest()
    {
      var user = await _userManager.GetUserAsync(User);
      var email = user?.Email;
       var myrequests = await _context.Requests
                                          .Where(r => r.EMail == email)
                                          .ToListAsync();

    var viewModel = new RequestViewModel
    {
        MyRequests = myrequests,
    };

      return View(viewModel);
    }




public async Task<IActionResult> MyRequestDetails(int? id)
{
    if (id == null)
    {
        return NotFound();
    }

    var request = await _context.Requests
                                .Include(r => r.Processes) 
                                .FirstOrDefaultAsync(x => x.Id == id);
    
    if (request == null)
    {
        return NotFound();
    }

    return View(request);
}

   public IActionResult RequestCreate()
   {
    return View();
   }
   [HttpPost]
   public async Task  <IActionResult> RequestCreate(Request model)
   {
    var user = await _userManager.GetUserAsync(User);
    var username = user?.UserName;
    var department = user?.Department;
    var email = user?.Email;
    var status = "Yeni Talep";
    if( ModelState.IsValid)
    {
      try{
        model.RUserName=username;
        model.Status = status;
        model.DDepartment = department;
        model.EMail = email;
        _context.Requests.Add(model);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index","Home");
      }
      catch (Exception ex)
      {
        ModelState.AddModelError("", "Hata");
      }
    }

    return View();
   }
   public async Task <IActionResult> RequestDelete(int? id)
   {
      if (id == null)
      {
        return NotFound();
      }
      var request = await _context.Requests.FindAsync(id);
      if (request == null)
      {
        return NotFound();
      }
      _context.Requests.Remove(request);
      await _context.SaveChangesAsync();
      return RedirectToAction("MyRequest","Request");
   }

 public async Task<IActionResult> RequestList()
{
    var user = await _userManager.GetUserAsync(User);
    if (user == null || string.IsNullOrEmpty(user.Department))
    {
        return NotFound("Kullanıcı veya departman bilgisi bulunamadı.");
    }

    var department = user.Department;

    var newRequests = await _context.Requests
                                    .Where(r =>  r.Status == "Yeni Talep" && r.UDepartment == department)
                                    .ToListAsync();
    
    var processedRequests = await _context.Requests
                                          .Where(r =>  r.Status == "Isleme Alinan" && r.UDepartment == department)
                                          .ToListAsync();

    var viewModel = new RequestViewModel
    {
        NewRequests = newRequests,
        ProcessedRequests = processedRequests
    };

    return View(viewModel);
}
  
 
}
}

  
