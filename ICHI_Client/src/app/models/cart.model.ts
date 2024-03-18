import { Data } from '@angular/router';
import { MasterEntity } from './master.entity';
import { ProductModel } from './product.model';

export interface CartModel extends MasterEntity {
  userId: string;
  productId: number;
  price: number;
  quantity: number;
  product: ProductModel;
  productImage: string;
}
