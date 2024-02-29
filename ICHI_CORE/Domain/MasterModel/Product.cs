using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICHI_CORE.Domain.MasterModel
{
    [Table("product")]
    public class Product : MasterEntity
    {
        [Column("description")]
        [StringLength(500)]
        public string ProductName { get; set; } = string.Empty;
        [Column("description")]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;
        public int CategoryProductID { get; set; }
        [ForeignKey("CategoryProductID")]
        [ValidateNever]
        public CategoryProduct? CategoryProduct { get; set; }

        public decimal SuggestedPrice { get; set; } = 0;
        public decimal SellingPrice { get; set; } = 0;
        public string Notes { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; }

        public string DisplayValue
        {
            get
            {
                return string.Format("{0} - {1}", Id, Description);
            }
        }
    }
}
