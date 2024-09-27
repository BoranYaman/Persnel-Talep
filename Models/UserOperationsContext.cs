using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserOperations.Entity;
using UserOperations.Models;

namespace UserOperations.Models
{
    public class UserOperationsContext:IdentityDbContext<AppUser ,AppRole ,string>
    {
        public UserOperationsContext(DbContextOptions<UserOperationsContext>options):base(options)
        {
           

        }
        public DbSet<Announcement>Announcements => Set <Announcement>();
        public DbSet<Request>Requests => Set <Request>();
        public DbSet<Car>Cars => Set <Car>();


          
    }
}
