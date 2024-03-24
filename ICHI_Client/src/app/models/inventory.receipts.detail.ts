import { InventoryReceiptsModel } from './inventory.receipts';
import { Data } from '@angular/router';
import { MasterEntity } from './master.entity';
import { ProductModel } from './product.model';
import { EmployeeModel } from './employee.model';
import { SupplierModel } from './supplier.model';

export interface InventoryReceiptDetailModel extends MasterEntity {
  InventoryReceiptId: number;
  productId: number;
  price: number;
  total: number;
  productDetail: ProductModel;
  inventoryReceipt: InventoryReceiptsModel;
}
