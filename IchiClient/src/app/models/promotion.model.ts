import { Data } from '@angular/router';
import { MasterEntity } from './master.entity';

export interface PromotionModel extends MasterEntity {
  promotionCode: string;
  promotionName: string;
  description: string;
  startDate: Date;
  endDate: Date;
  discount: number;
  quantity: number;
  isActive: boolean;
  isDeleted: boolean;
}
