namespace Kodee.ApiService.Models;

public class Photo
{
    public long Id { get; set; }
    public string FileName { get; set; } = string.Empty;

    // Employee와의 관계 설정(다대일)
    public long? EmployeeId { get; set; }
    public Employee? Employee { get; set; }
}
