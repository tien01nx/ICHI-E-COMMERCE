import { Data } from '@angular/router';
import { MasterEntity } from './master.entity';

export interface TrademarkModel extends MasterEntity {
  trademarkName: string;
}
