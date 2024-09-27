namespace UserOperations.Entity
{
  public class Request
  {
    public int Id { get; set; }
    public string? RequestTitle { get; set; }
    public string? RequestText { get; set;}
    public string? Subject { get; set;}
     public string? RUserName { get; set; } 
     public string? DDepartment { get; set; } 
     public string? UDepartment { get; set; } 

     public string? GetProcessing { get; set;}
     public string? Status { get; set;}
     public string? EMail { get; set;}
      public string? Final { get; set;}
      public ICollection<RequestProcess>? Processes { get; set; }
  }  
}