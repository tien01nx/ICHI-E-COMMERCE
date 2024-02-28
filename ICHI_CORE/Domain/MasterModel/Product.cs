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
        public string Description { get; set; } = string.Empty;
        public int CategoryProductID { get; set; }
        [ForeignKey("CategoryProductID")]
        [ValidateNever]
        public CategoryProduct? CategoryProduct { get; set; }

        public decimal Price { get; set; } = 0;
        public string Notes { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; }
        [ValidateNever]
        public List<ProductImages> ProductImages { get; set; }


        public string DisplayValue
        {
            get
            {
                return string.Format("{0} - {1}", Id, Description);
            }
        }
    }
}
