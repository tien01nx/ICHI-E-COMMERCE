using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ICHI_CORE.Domain.MasterModel
{
    [Table("customer")]

    public class Customer : MasterEntity
    {
        public string CustomerName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public DateTime Birthday { get; set; }
        public string? Avatar { get; set; } = string.Empty;
    }
}
