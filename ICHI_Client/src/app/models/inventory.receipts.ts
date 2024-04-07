import { Data } from '@angular/router';
import { MasterEntity } from './master.entity';
import { ProductModel } from './product.model';
import { EmployeeModel } from './employee.model';
import { SupplierModel } from './supplier.model';

export interface InventoryReceiptsModel extends MasterEntity {
  employeeId: number;
  supplierId: number;
  notes: string;
  isActive: boolean;
  batchNumber: number;
  employee: EmployeeModel;
  supplier: SupplierModel;
}
