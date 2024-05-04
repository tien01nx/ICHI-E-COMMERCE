import { MasterEntity } from './master.entity';
import { ProductModel } from './product.model';
import { ProductReturn } from './product.return';

export interface ProductReturnDetail extends MasterEntity {
  productId: number;
  price : number;
  quantity: number;
  reason: string;
  returnType: boolean;
  product: ProductModel;
  productReturn: ProductReturn;
}
