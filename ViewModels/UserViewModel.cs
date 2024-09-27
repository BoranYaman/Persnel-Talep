using UserOperations.Models;

namespace UserOperations.ViewModels
{
    public class UserViewModel
    {
        public List<AppUser>? Users { get; set; }
        public List<AppRole>? Roles { get; set; }

    }
}