using ICHI_CORE.Domain.MasterModel;

namespace ICHI_API.Model
{
    public class PromotionDTO
    {
        public Promotion Promotion { get; set; }
        public IEnumerable<PromotionDetail> PromotionDetails { get; set; }
    }
}
