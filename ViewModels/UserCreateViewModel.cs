using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserOperations.ViewModels
{
    public class UserCreateViewModel
    {
    
        public string? Id { get; set; }

        public string? UserName { get; set; }
       
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? EmergencyPhoneNumber { get; set; }
        public string? EmergencyContactName { get; set; }
        public string? Address { get; set; }
        public string? Department { get; set; }
        public string? Note { get; set; }
         public string? Salary { get; set; }
        public string? Position { get; set; }
        [EmailAddress]
        public string? EmailAddress { get; set; }
        [DataType(DataType.Password)]
        public string? Password { get; set; }
         public IList<string>? SelectedRoles{ get; set; }

      

        
    }
}