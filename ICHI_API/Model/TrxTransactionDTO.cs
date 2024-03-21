namespace ICHI_API.Model
{
    public class TrxTransactionDTO
    {
        public int? TrxTransactionId { get; set; }

        public string UserId { get; set; }

        public string FullName { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public decimal? Amount { get; set; }
    }
}
