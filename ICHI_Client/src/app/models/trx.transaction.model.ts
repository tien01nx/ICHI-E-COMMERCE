import { Data } from '@angular/router';
import { MasterEntity } from './master.entity';
import { ProductModel } from './product.model';

export interface TrxTransactionModel extends MasterEntity {
  userId: string;
  orderDate: Date;
  shoppingDate: Date;
  orderTotal: number;
  orderStatus: boolean;
  paymentStatus: boolean;
  paymentDate: Date;
  sessionId: string;
  paymentIntentId: string;
  fullName: string;
  phoneNumber: string;
  address: string;
  notes: string;
}
