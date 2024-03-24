import { InventoryReceiptDetailModel } from '../models/inventory.receipts.detail';
import { InventoryReceiptsModel } from './../models/inventory.receipts';

export class InsertInvertoryReceiptsDTO {
  supplierId: number;
  supplierName: string;
  employeeId: number;
  fullname: string;
  notes: string;
  inventoryReceiptDetails: InventoryReceiptDetailModel[];
  constructor(
    supplierId: number,
    supplierName: string,
    employeeId: number,
    fullname: string,
    notes: string,
    inventoryReceiptDetails: InventoryReceiptDetailModel[]
  ) {
    this.supplierId = supplierId;
    this.supplierName = supplierName;
    this.employeeId = employeeId;
    this.fullname = fullname;
    this.notes = notes;
    this.inventoryReceiptDetails = inventoryReceiptDetails;
  }
}
