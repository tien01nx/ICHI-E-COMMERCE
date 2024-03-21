namespace ICHI_CORE.Model
{
    using System;

    public class VnPaymentRequestModel
    {
        public int TrxTransactionId { get; set; }

        public string FullName { get; set; }

        public double Amount { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
