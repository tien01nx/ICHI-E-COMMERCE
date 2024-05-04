import { Data } from '@angular/router';
import { MasterEntity } from './master.entity';
import { ProductModel } from './product.model';
import { ProductReturn } from './product.return';
import { ProductReturnDetail } from './product.return.detail';

export interface ProductReturnMV {
  productReturn: ProductReturn;
  productRetuenDetails: ProductReturnDetail[];
}
