using Microsoft.AspNetCore.Identity;
using OrderMgmtRevision.Models;
using System.ComponentModel.DataAnnotations;

public class User : IdentityUser
{
    public string FullName { get; set;}

    [DataType(DataType.DateTime)]
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;

    [DataType(DataType.DateTime)]
    public DateTime? LastLogin { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime? LastPasswordChange { get; set; }

    public bool IsActive { get; set; } = true;

    public string? CreatedBy { get; set; }

    public string? LastLoginIP {  get; set; }

    public List<UserLog>? Logs { get; set; } = new List<UserLog>();

}