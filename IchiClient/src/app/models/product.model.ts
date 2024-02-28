import { Data } from '@angular/router';
import { MasterEntity } from './master.entity';
import { CategoryProduct } from './category.product';

export interface ProductModel extends MasterEntity {
  description: string;
  categoryProductID: number;
  price: number;
  notes: string;
  isActive: boolean;
  isDeleted: boolean;
  productImages: any;
  categoryProduct: CategoryProduct;
}
