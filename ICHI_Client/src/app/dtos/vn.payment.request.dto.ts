import { IsString } from 'class-validator';
export class VnPaymentRequestDTO {
  trxTransactionId: number;
  fullName: string;
  amount: number;
  orderStatus: string;
  createDate: Date;
  constructor(
    TrxTransactionId: number,
    FullName: string,
    Amount: number,
    OrderStatus: string,
    CreateDate: Date
  ) {
    this.trxTransactionId = TrxTransactionId;
    this.fullName = FullName;
    this.amount = Amount;
    this.orderStatus = OrderStatus;
    this.createDate = CreateDate;
  }
}
