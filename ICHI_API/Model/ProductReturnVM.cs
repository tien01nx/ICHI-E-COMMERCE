using ICHI_CORE.Domain.MasterModel;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ICHI_API.Model
{
    public class ProductReturnVM
    {
        public ProductReturn ProductReturn { get; set; }
        public IEnumerable<ProductReturnDetail> ProductReturnDetails { get; set; }
    }
}
