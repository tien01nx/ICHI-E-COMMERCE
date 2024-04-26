namespace ICHI_API.Model
{
  public class MoneyTotal
  {
    // ngày hôm nay
    public int TotalOrder { get; set; } = 0;
    public decimal TotalOrderAmount { get; set; } = 0;
    public decimal TotalRealAmount { get; set; } = 0;
    public decimal TotalRemainAmount { get; set; } = 0;

    // ngày hôm qua
    public int TotalOrderYesterday { get; set; } = 0;
    public decimal TotalOrderAmountYesterday { get; set; } = 0;
    public decimal TotalRealAmountYesterday { get; set; } = 0;
    public decimal TotalRemainAmountYesterday { get; set; } = 0;

    // phần trăm so với hôm qua
    public decimal PercentTotalOrder { get; set; } = 0;
    public decimal PercentTotalOrderAmount { get; set; } = 0;
    public decimal PercentTotalRealAmount { get; set; } = 0;
    public decimal PercentTotalRemainAmount { get; set; } = 0;


  }
}
