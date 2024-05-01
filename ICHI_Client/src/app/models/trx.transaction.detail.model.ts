import { Data } from '@angular/router';
import { MasterEntity } from './master.entity';
import { ProductModel } from './product.model';

export interface TrxTransactionDetailModel extends MasterEntity {
  trxTransactionId: number;
  productId: number;
  price: number;
  quantity: number;
  productImage: string;
  product: ProductModel;
  discount: number;
}
