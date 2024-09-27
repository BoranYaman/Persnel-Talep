using System.ComponentModel.DataAnnotations;

namespace UserOperations.ViewModels
{
    public class UserEditViewModel
    {
    
    public string? Id { get; set; }
        public string? UserName { get; set; } 
        public string? LastName { get; set; } 
        public string? PhoneNumber { get; set; }
        public string? EmergencyPhoneNumber { get; set; }
        public string?  EmergencyContactName { get; set; }
        public string?  Address { get; set; }
         public string?  Department { get; set; }=string.Empty;
        public string?  Note { get; set; }
        [EmailAddress]
         public string?  EmailAddress { get; set; } 
         public string? Salary { get; set; }=string.Empty;
        public string? Position { get; set; }=string.Empty;
         public IList<string>? SelectedRoles{ get; set; }
         public string? ReasonForLeavingJob { get; set; }=string.Empty ;
         public string? ReasonForLeavingJobText { get; set; }=string.Empty ;

         public string? WorkingStatus { get; set; }=string.Empty ;
    }
}