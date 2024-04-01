import { Data } from '@angular/router';
import { MasterEntity } from './master.entity';

export interface PromotionModel extends MasterEntity {
  promotionName: string;
  startTime: Date;
  endTime: Date;
  discount: number;
  quantity: number;
  isActive: boolean;
  isDeleted: boolean;
}
