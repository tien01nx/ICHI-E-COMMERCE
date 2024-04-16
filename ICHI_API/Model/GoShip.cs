// Định nghĩa mô hình cho Address (Địa chỉ)
using Newtonsoft.Json;

public class Address
{
  public string city { get; set; } // Mã thành phố
  public string district { get; set; } // Mã quận
  public string? wards { get; set; } // Mã phường
}

// Định nghĩa mô hình cho Parcel (Gói hàng)
public class Parcel
{
  public string cod { get; set; } // Giá trị tiền thu hộ
  public string weight { get; set; } // Trọng lượng
  public string width { get; set; } // Chiều rộng
  public string height { get; set; } // Chiều cao
  public string length { get; set; } // Chiều dài
}

// Định nghĩa mô hình cho Shipment (Đối tượng chuyển hàng)
public class Shipment
{
  public Address address_from { get; set; } // Địa chỉ nguồn
  public Address address_to { get; set; } // Địa chỉ đích
  public Parcel parcel { get; set; } // Gói hàng
}

public class ShipmentRequest
{
  public Shipment shipment { get; set; }
}
// Định nghĩa mô hình cho JSON gốc (Root Object)

public class ShipmentData
{
  [JsonProperty("id")]
  public string Id { get; set; }

  [JsonProperty("carrier_name")]
  public string CarrierName { get; set; }

  [JsonProperty("carrier_logo")]
  public string CarrierLogo { get; set; }

  [JsonProperty("carrier_short_name")]
  public string CarrierShortName { get; set; }

  [JsonProperty("service")]
  public string Service { get; set; }

  [JsonProperty("expected")]
  public string Expected { get; set; }

  [JsonProperty("is_apply_only")]
  public bool IsApplyOnly { get; set; }

  [JsonProperty("promotion_id")]
  public int PromotionId { get; set; }

  [JsonProperty("discount")]
  public double Discount { get; set; }

  [JsonProperty("weight_fee")]
  public double WeightFee { get; set; }

  [JsonProperty("location_first_fee")]
  public double LocationFirstFee { get; set; }

  [JsonProperty("location_step_fee")]
  public double LocationStepFee { get; set; }

  [JsonProperty("remote_area_fee")]
  public double RemoteAreaFee { get; set; }

  [JsonProperty("oil_fee")]
  public double OilFee { get; set; }

  [JsonProperty("location_fee")]
  public double LocationFee { get; set; }

  [JsonProperty("cod_fee")]
  public double CodFee { get; set; }

  [JsonProperty("service_fee")]
  public double ServiceFee { get; set; }

  [JsonProperty("total_fee")]
  public double TotalFee { get; set; }

  [JsonProperty("total_amount")]
  public double TotalAmount { get; set; }

  [JsonProperty("total_amount_carrier")]
  public double TotalAmountCarrier { get; set; }

  [JsonProperty("total_amount_shop")]
  public double TotalAmountShop { get; set; }

  [JsonProperty("price_table_id")]
  public int PriceTableId { get; set; }

  [JsonProperty("insurrance_fee")]
  public double InsurranceFee { get; set; }

  [JsonProperty("return_fee")]
  public double ReturnFee { get; set; }

  [JsonProperty("report")]
  public ReportData Report { get; set; }
}

public class ReportData
{
  [JsonProperty("success_percent")]
  public double SuccessPercent { get; set; }

  [JsonProperty("return_percent")]
  public double ReturnPercent { get; set; }

  [JsonProperty("avg_time_delivery")]
  public int AvgTimeDelivery { get; set; }

  [JsonProperty("avg_time_delivery_format")]
  public int AvgTimeDeliveryFormat { get; set; }
}
