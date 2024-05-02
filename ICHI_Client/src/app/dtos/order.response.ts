import { OrderDetailsResponse } from './order.details.response';

export class OrderResponse {
  id: number;
  name: string;
  address: string;
  phone: string;
  paymentTypes: string;
  paymentStatus: string;
  deliveredDate: Date;
  note: string;
  orderStatus: string;
  orderDate: Date;
  cancelledDate: Date;
  onholDate: Date;
  cancelReason: string;
  totalMoney: number;
  totalSaleMoney: number;
  totalDiscount: number;
  totalQuantity: number;
  customerId: number;
  customerName: string;
  customerPhone: string;
  employeeId: number;
  employeeName: string;
  carrierName: string;
  carrierLogo: string;
  service: string;
  totalFree: number;
  orderDetailsResponses: OrderDetailsResponse[];

  constructor(data: any) {
    this.id = data.id;
    this.name = data.name;
    this.address = data.address;
    this.phone = data.phone;
    this.paymentTypes = data.paymentTypes;
    this.paymentStatus = data.paymentStatus;
    this.deliveredDate = data.deliveredDate;
    this.note = data.note;
    this.orderStatus = data.orderStatus;
    this.orderDate = data.orderDate;
    this.cancelledDate = data.cancelledDate;
    this.onholDate = data.onholDate;
    this.cancelReason = data.cancelReason;
    this.totalMoney = data.totalMoney;
    this.totalSaleMoney = data.totalSaleMoney;
    this.totalDiscount = data.totalDiscount;
    this.totalQuantity = data.totalQuantity;
    this.customerId = data.customerId;
    this.customerName = data.customerName;
    this.customerPhone = data.customerPhone;
    this.employeeId = data.employeeId;
    this.employeeName = data.employeeName;
    this.carrierName = data.carrierName;
    this.carrierLogo = data.carrierLogo;
    this.service = data.service;
    this.totalFree = data.totalFree;
    this.orderDetailsResponses = data.orderDetailsResponses.map(
      (item: any) => new OrderDetailsResponse(item)
    );
  }
}
