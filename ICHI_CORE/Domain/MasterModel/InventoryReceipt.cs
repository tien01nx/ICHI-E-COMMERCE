namespace ICHI_CORE.Domain.MasterModel
{
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class InventoryReceipt : MasterEntity
    {
        public int EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        [ValidateNever]
        public Employee? Employee { get; set; }

        public int SupplierId { get; set; }

        [ForeignKey("SupplierId")]
        [ValidateNever]
        public Supplier? Supplier { get; set; }

        [StringLength(255, ErrorMessage = "Ghi chú phải có tối đa 255 ký tự")]
        public string? Notes { get; set; } = string.Empty;

        public bool isActive { get; set; } = false;


    }
}
