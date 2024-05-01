import { EmployeeModel } from './employee.model';
import { MasterEntity } from './master.entity';
import { TrxTransactionModel } from './trx.transaction.model';

export interface ProductReturn extends MasterEntity {
  employeeId: number;
  trxTransactionId: number;
  status: string;
  reason: string;
  employee: EmployeeModel;
  trxTransaction: TrxTransactionModel;
}
