using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace UserOperations.Models
{
    public class AppUser:IdentityUser
    {
         
     
        public string? Department { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? EmergencyPhoneNumber { get; set; }
        public string? EmergencyContactName { get; set; }
        public string? Address { get; set; }
        public string? Note { get; set; }
        public string? EmailAddress { get; set; }
        public string? Position { get; set; }
        public string? Salary { get; set; }
    }
}