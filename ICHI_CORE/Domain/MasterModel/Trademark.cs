namespace ICHI_CORE.Domain.MasterModel
{
    using System.ComponentModel.DataAnnotations;

    public class Trademark : MasterEntity
    {

        private string _trademarkName;

        [Required(ErrorMessage = "Tên thương hiệu là bắt buộc")]
        [StringLength(255, ErrorMessage = "Số điện thoại phải có tối đa 255 ký tự")]
        public string TrademarkName
        {
            get => _trademarkName;
            set => _trademarkName = value?.Trim();
        }
    }
}
