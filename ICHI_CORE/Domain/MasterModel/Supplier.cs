using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICHI_CORE.Domain.MasterModel
{
    [Table("supplier")]
    public class Supplier : MasterEntity
    {
        // các thông tin cơ bản của nhà cung cấp
        [Column("supplier_code")]
        public string SupplierCode { get; set; } = string.Empty;
        [Column("supplier_name")]
        public string SupplierName { get; set; } = string.Empty;
        [Column("address")]
        public string Address { get; set; } = string.Empty;
        [Column("phone")]
        public string Phone { get; set; } = string.Empty;
        [Column("email")]
        public string Email { get; set; } = string.Empty;
        [Column("tax_code")]
        public string TaxCode { get; set; } = string.Empty;
        [Column("banK_account")]
        public string BankAccount { get; set; } = string.Empty;
        [Column("bank_name")]
        public string BankName { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; }

    }
}
