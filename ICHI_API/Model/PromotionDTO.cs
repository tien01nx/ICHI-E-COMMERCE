using ICHI_CORE.Domain.MasterModel;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class PromotionDTO
{
  public int Id { get; set; }

  [Required(ErrorMessage = "Tên khuyến mãi là bắt buộc")]
  [StringLength(255, ErrorMessage = "Tên khuyến mãi không được vượt quá 255 ký tự")]
  public string PromotionName { get; set; }

  [Required(ErrorMessage = "Thời gian bắt đầu là bắt buộc")]
  public DateTime StartTime { get; set; }

  [Required(ErrorMessage = "Thời gian kết thúc là bắt buộc")]
  public DateTime EndTime { get; set; }

  [Required(ErrorMessage = "Số lượng là bắt buộc")]
  public int Quantity { get; set; }

  [Required(ErrorMessage = "Giảm giá là bắt buộc")]
  public double Discount { get; set; }

  public bool isActive { get; set; }

  public bool isDeleted { get; set; }

  public DateTime? CreateDate { get; set; }

  [Required(ErrorMessage = "Người tạo là bắt buộc")]
  [StringLength(100, ErrorMessage = "Người tạo không được vượt quá 100 ký tự")]
  public string CreateBy { get; set; }

  public DateTime? ModifiedDate { get; set; }

  [StringLength(100, ErrorMessage = "Người sửa không được vượt quá 100 ký tự")]
  public string ModifiedBy { get; set; }


  public Promotion? _promotion;


  [ValidateNever]
  public Promotion? Promotion
  {
    get { return _promotion; }
    set
    {
      _promotion = value;

      Id = _promotion.Id;
      PromotionName = _promotion.PromotionName;
      StartTime = _promotion.StartTime;
      EndTime = _promotion.EndTime;
      Quantity = _promotion.Quantity;
      Discount = _promotion.Discount;
      isActive = _promotion.isActive;
      isDeleted = _promotion.isDeleted;
      CreateDate = _promotion.CreateDate;
      CreateBy = _promotion.CreateBy;
      ModifiedDate = _promotion.ModifiedDate;
      ModifiedBy = _promotion.ModifiedBy;
    }
  }

  public IEnumerable<PromotionDetail> PromotionDetails { get; set; }

  public PromotionDTO()
  {
    Promotion = new Promotion();
    PromotionDetails = new List<PromotionDetail>();
  }
}
