import { Data } from '@angular/router';
import { MasterEntity } from './master.entity';
import { ProductModel } from './product.model';
import { CategoryProduct } from './category.product';

export interface TreeNode {
  key: string;
  label: string;
  data: CategoryProduct;
  icon?: string;
  children?: TreeNode[];
}
