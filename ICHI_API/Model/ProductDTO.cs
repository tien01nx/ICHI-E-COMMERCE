using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ICHI_CORE.Domain;
using ICHI_CORE.Domain.MasterModel;

namespace ICHI_API.Model
{
    public class ProductDTO
    {
        public Product Product { get; set; }
        public List<ProductImages> ProductImages { get; set; }
        public CategoryProduct CategoryProduct { get; set; }
    }
}
