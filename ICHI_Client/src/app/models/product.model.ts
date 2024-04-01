import { Data } from '@angular/router';
import { MasterEntity } from './master.entity';
import { CategoryProduct } from './category.product';

export interface ProductModel extends MasterEntity {
  productName: string;
  description: string;
  categoryId: number;
  price: number;
  notes: string;
  priorityLevel: number;
  color: string;
  image: string;
  isActive: boolean;
  isDeleted: boolean;
  productImages: any;
  discount: number;
  category: CategoryProduct;
}
