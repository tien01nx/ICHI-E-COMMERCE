// Định nghĩa DTO cho Address
export interface Address {
  city: string; // Mã thành phố
  district: string; // Mã quận
  wards: string; // Mã phường
}

// Định nghĩa DTO cho Parcel
export interface Parcel {
  cod: string; // Giá trị tiền thu hộ
  weight: string; // Trọng lượng
  width: string; // Chiều rộng
  height: string; // Chiều cao
  length: string; // Chiều dài
}

// Định nghĩa DTO cho Shipment
export interface Shipment {
  address_from: Address; // Địa chỉ nguồn
  address_to: Address; // Địa chỉ đích
  parcel: Parcel; // Gói hàng
}

// Định nghĩa DTO cho ShipmentDTO
export interface ShipmentDTO {
  shipment: Shipment; // Đối tượng Shipment
}

// Hàm tạo đối tượng ShipmentDTO với các giá trị mặc định và addressTo do người dùng truyền vào
export function createShipmentDTO(addressTo: Address): ShipmentDTO {
  // Các giá trị mặc định cho address_from và parcel
  const defaultAddressFrom: Address = {
    city: '170000',
    district: '170600',
    wards: '1048',
  };

  const defaultParcel: Parcel = {
    cod: '0',
    weight: '250',
    width: '10',
    height: '15',
    length: '15',
  };

  // Tạo và trả về đối tượng ShipmentDTO
  return {
    shipment: {
      address_from: defaultAddressFrom,
      address_to: addressTo,
      parcel: defaultParcel,
    },
  };
}
