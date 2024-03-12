import { Data } from '@angular/router';
import { MasterEntity } from './master.entity';

export interface ProductImage extends MasterEntity {
  id : number;
  productId: number;
  imageName: string;
  imagePath: string;
  isDefault: boolean;
  isActive: boolean;
  isDeleted: boolean;
}
