﻿using Microsoft.AspNetCore.Identity;
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

    [Range(0, double.MaxValue, ErrorMessage = "Balance must be a positive value.")]
    public decimal? AccountBalance { get; set; } = 0;

    public bool IsAdmin {  get; set; }

    public int? ProductsActive { get; set; } = 0;
    public int? ProductsTotal { get; set; } = 0;
    public int? ShipmentsActive { get; set; } = 0;
    public int? ShipmentsTotal { get; set; } = 0;

    public List<UserLog>? Logs { get; set; } = new List<UserLog>();

    public List<UserInvoice> UserInvoices { get; set; }

}