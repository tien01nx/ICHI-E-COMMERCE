using ICHI_CORE.Domain.MasterModel;
using System.ComponentModel.DataAnnotations;

namespace ICHI_API.Model
{
  public class PromotionDTO
  {
    public int Id { get { return Promotion.Id; } set { Promotion.Id = value; } }

    [Required]
    [StringLength(255)]
    public string PromotionName { get { return Promotion.PromotionName; } set { Promotion.PromotionName = value; } }
    [Required]
    public DateTime StartTime { get { return Promotion.StartTime; } set { Promotion.StartTime = value; } }
    [Required]
    public DateTime EndTime { get { return Promotion.EndTime; } set { Promotion.EndTime = value; } }
    [Required]
    public int Quantity { get { return Promotion.Quantity; } set { Promotion.Quantity = value; } }
    [Required]
    public double Discount { get { return Promotion.Discount; } set { Promotion.Discount = value; } }
    public bool isActive { get { return Promotion.isActive; } set { Promotion.isActive = value; } }
    public bool isDeleted { get { return Promotion.isDeleted; } set { Promotion.isDeleted = value; } }
    public DateTime? CreateDate { get { return Promotion.CreateDate; } set { Promotion.CreateDate = DateTime.Now; } }

    [Required]
    [StringLength(100)]
    public string CreateBy { get { return Promotion.CreateBy; } set { Promotion.CreateBy = "Admin"; } }

    public DateTime? ModifiedDate { get { return Promotion.ModifiedDate; } set { Promotion.ModifiedDate = DateTime.Now; } }

    [StringLength(100)]
    public string? ModifiedBy { get { return Promotion.ModifiedBy; } set { Promotion.ModifiedBy = "Admin"; } }

    public Promotion Promotion { get; set; } = new Promotion();

    public IEnumerable<PromotionDetail> PromotionDetails { get; set; }

    public PromotionDTO() : base()
    {
      Promotion = new Promotion();
      PromotionDetails = new List<PromotionDetail>();
    }
  }
}
