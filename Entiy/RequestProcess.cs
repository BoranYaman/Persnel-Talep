using UserOperations.Entity;

public class RequestProcess
{
    public int Id { get; set; }
    public string ?UserMail { get; set; } 
    public string ?UserName { get; set; } 

    
    public string ?ProcessDetail { get; set; } 
    public DateTime ProcessDate { get; set; } 
    public int RequestId { get; set; } 
    public Request? Request { get; set; } 
}