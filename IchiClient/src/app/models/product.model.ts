import { Data } from '@angular/router';
import { MasterEntity } from './master.entity';
import { CategoryProduct } from './category.product';

export interface ProductModel extends MasterEntity {
  productName: string;
  description: string;
  categoryProductID: number;
  suggestedPrice: number;
  sellingPrice: number;
  notes: string;
  isActive: boolean;
  isDeleted: boolean;
  productImages: any;
  categoryProduct: CategoryProduct;
}
