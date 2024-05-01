import { ProductReturn } from '../models/product.return';
import { ProductReturnDetail } from '../models/product.return.detail';

export class ProductReturnDTO {
  productReturn: ProductReturn;
  productReturnDetail: ProductReturnDetail[];
  constructor(
    productReturn: ProductReturn,
    productReturnDetail: ProductReturnDetail[]
  ) {
    this.productReturn = productReturn;
    this.productReturnDetail = productReturnDetail;
  }
}
