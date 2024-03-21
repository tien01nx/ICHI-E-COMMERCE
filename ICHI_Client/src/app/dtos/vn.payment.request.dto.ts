export class VnPaymentRequestDTO {
  trxTransactionId: number;
  fullName: string;
  amount: number;
  createDate: Date;
  constructor(
    TrxTransactionId: number,
    FullName: string,
    Amount: number,
    CreateDate: Date
  ) {
    this.trxTransactionId = TrxTransactionId;
    this.fullName = FullName;
    this.amount = Amount;
    this.createDate = CreateDate;
  }
}
