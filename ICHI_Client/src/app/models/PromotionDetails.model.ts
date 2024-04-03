import { Data } from '@angular/router';
import { MasterEntity } from './master.entity';
import { ProductModel } from './product.model';

export interface PromotionDetails extends MasterEntity {
  promotionId: number;
  productId: number;
  quantity: number;
  usedCodesCount: number;
}
