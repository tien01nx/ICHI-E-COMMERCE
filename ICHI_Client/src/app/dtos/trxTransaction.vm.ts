import { CustomerModel } from '../models/customer.model';
import { EmployeeModel } from '../models/employee.model';
import { ProductReturn } from '../models/product.return';
import { ProductReturnDetail } from '../models/product.return.detail';
import { TrxTransactionDetailModel } from '../models/trx.transaction.detail.model';
import { TrxTransactionModel } from '../models/trx.transaction.model';

export class TrxTransactionVM {
  trxTransaction: TrxTransactionModel;
  transactionDetail: TrxTransactionDetailModel[];
  customer: CustomerModel;
  employee: EmployeeModel;
  constructor(
    trxTransaction: TrxTransactionModel,
    transactionDetail: TrxTransactionDetailModel[],
    customer: CustomerModel,
    employee: EmployeeModel
  ) {
    this.trxTransaction = trxTransaction;
    this.transactionDetail = transactionDetail;
    this.customer = customer;
    this.employee = employee;
  }
}
