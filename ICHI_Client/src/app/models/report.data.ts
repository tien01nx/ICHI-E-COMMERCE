import { Data } from '@angular/router';
import { MasterEntity } from './master.entity';
import { ProductModel } from './product.model';

export interface ReportDataModel {
  success_percent: number;
  return_percent: number;
  avg_time_delivery: number;
  avg_time_delivery_format: number;
}
