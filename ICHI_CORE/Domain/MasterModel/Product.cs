using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ICHI_CORE.Domain.MasterModel
{
  public class Product : MasterEntity
  {
    public int TrademarkId { get; set; }
    [ForeignKey("TrademarkId")]
    [ValidateNever]
    public Trademark? Trademark { get; set; }
    [Required]
    public int CategoryId { get; set; }
    [ForeignKey("CategoryId")]
    [ValidateNever]
    public Category? Category { get; set; }
    [Required]
    [StringLength(255)]

    public string Color { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    [Required]
    public string Description { get; set; } = string.Empty;
    [Required]
    public decimal Price { get; set; } = 0;
    public int? PriorityLevel { get; set; } = 0;
    public double Quantity { get; set; } = 0;
    public string? Notes { get; set; } = string.Empty;
    public bool isActive { get; set; } = false;
    public bool isDeleted { get; set; } = false;

    //[ValidateNever]
    //public List<ProductImages> ProductImages { get; set; }
  }
}
