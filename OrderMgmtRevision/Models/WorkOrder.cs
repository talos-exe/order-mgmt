using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Identity.Client;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderMgmtRevision.Models
{
    public class WorkOrder
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int WarehouseId { get; set; }

        [ForeignKey("WarehouseId")]
        [ValidateNever]
        public Warehouse Warehouse { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        public string Type { get; set; }

        [Required(ErrorMessage = "Fee is required.")]
        public double Fee { get; set; }

        public string? CreatedBy { get; set; } = "System";

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
