import { Data } from '@angular/router';
import { MasterEntity } from './master.entity';

export interface PromotionModel extends MasterEntity {
  promotionCode: string;
  promotionName: string;
  startTime: Date;
  endTime: Date;
  discount: number;
  quantity: number;
  minimumPrice: number;
  isActive: boolean;
  isDeleted: boolean;
}
