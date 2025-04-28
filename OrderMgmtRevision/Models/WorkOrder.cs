using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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

        [Required]
        public string Description { get; set; }

        public string Type { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
