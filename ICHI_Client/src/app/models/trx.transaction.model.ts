import { Data } from '@angular/router';
import { MasterEntity } from './master.entity';
import { ProductModel } from './product.model';

export interface TrxTransactionModel extends MasterEntity {
  // userId: string;
  customerId: string;
  employeeId: string;
  orderDate: Date;
  shoppingDate: Date;
  orderTotal: number;
  orderStatus: string;
  paymentTypes: string;
  paymentStatus: string;
  paymentDate: Date;
  sessionId: string;
  paymentIntentId: string;
  fullName: string;
  phoneNumber: string;
  address: string;
  notes: string;
}
