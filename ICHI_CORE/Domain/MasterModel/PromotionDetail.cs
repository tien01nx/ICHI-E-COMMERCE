namespace ICHI_CORE.Domain.MasterModel
{
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

    public class PromotionDetail : MasterEntity
    {
        public int PromotionId { get; set; } = 0;

        [ForeignKey("PromotionId")]
        [ValidateNever]
        public Promotion? Promotion { get; set; }

        public int ProductId { get; set; } = 0;

        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product? Product { get; set; }
    }
}
