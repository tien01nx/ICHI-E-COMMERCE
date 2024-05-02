export class OrderDetailsResponse {
  id: number;
  productId: number;
  productName: string;
  productPrice: number;
  quantity: number;
  salePrice: number;
  totalPrice: number;
  quantityReturned: number;
  productImage: string;
  constructor(data: any) {
    this.id = data.id;
    this.productId = data.productId;
    this.productName = data.productName;
    this.productPrice = data.productPrice;
    this.quantity = data.quantity;
    this.salePrice = data.salePrice;
    this.totalPrice = data.totalPrice;
    this.quantityReturned = data.quantityReturned;
    this.productImage = data.productImage;
  }
}
