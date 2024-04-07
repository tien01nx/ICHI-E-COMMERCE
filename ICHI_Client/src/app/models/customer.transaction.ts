import { Data } from '@angular/router';
import { MasterEntity } from './master.entity';
import { ProductModel } from './product.model';
import { CustomerModel } from './customer.model';
import { TrxTransactionModel } from './trx.transaction.model';

export interface CustomerTransaction {
  customer: CustomerModel;
  trxTransactions: TrxTransactionModel[];
}
