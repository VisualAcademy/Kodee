namespace Kodee.ApiService.Models;

public class Employee
{
    public long Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    // Photos와의 관계 설정(일대다)
    public ICollection<Photo> Photos { get; set; } = new List<Photo>();
}
