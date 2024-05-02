import { Data } from '@angular/router';
import { MasterEntity } from './master.entity';
import { ProductModel } from './product.model';
import { EmployeeModel } from './employee.model';
import { CustomerModel } from './customer.model';

export interface TrxTransactionModel extends MasterEntity {
  // userId: string;
  customerId: string;
  employeeId: string;
  orderDate: Date;
  onholDate: Date;
  waitingForPickupDate: Date;
  waitingForDeliveryDate: Date;
  deliveredDate: Date;
  cancelledDate: Date;
  orderTotal: number;
  priceShip: number;
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
  employee: EmployeeModel;
  customer: CustomerModel;
}
