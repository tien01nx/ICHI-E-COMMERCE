namespace ICHI_API.Model
{
  public class OrderStatusVM
  {
    public int Pending { get; set; } = 0;
    public int OnHold { get; set; } = 0;
    public int WaitingForPickup { get; set; } = 0;
    public int WaitingForDelivery { get; set; } = 0;
    public int Delivered { get; set; } = 0;
    public int Cancelled { get; set; } = 0;
  }
}
