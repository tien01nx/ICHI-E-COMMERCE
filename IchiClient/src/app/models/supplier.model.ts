import { Data } from '@angular/router';
import { MasterEntity } from './master.entity';

export interface SupplierModel extends MasterEntity {
  supplierCode: string;
  supplierName: string;
  address: string;
  phone: string;
  email: string;
  taxCode: string;
  bankAccount: string;
  bankName: string;
  isActive: boolean;
  isDeleted: boolean;
}
