import { EmployeeModel } from '../models/employee.model';
import { ProductReturn } from '../models/product.return';
import { TrxTransactionModel } from '../models/trx.transaction.model';

export class ProductReturnDTO {
  id: number;
  status: string;
  reason: string;
  employee: EmployeeModel;
  trxTransaction: TrxTransactionModel;
  productReturn: ProductReturn;
  createDate: Date;
  createBy: string;
  modifiedDate: Date;
  modifiedBy: string;
  constructor(data: any) {
    this.employee = data.employee;
    this.trxTransaction = data.trxTransaction;
    this.productReturn = data.productReturn;
    this.status = data.status;
    this.reason = data.reason;
    this.id = data.id;
    this.createDate = data.createDate;
    this.createBy = data.createBy;
    this.modifiedDate = data.modifiedDate;
    this.modifiedBy = data.modifiedBy;
  }
}
