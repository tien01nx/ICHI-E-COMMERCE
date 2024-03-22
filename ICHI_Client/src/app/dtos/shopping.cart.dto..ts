import {
  IsString,
  IsNotEmpty,
  IsPhoneNumber,
  IsBoolean,
  IsNumber,
} from 'class-validator';
import { MasterEntityDTO } from './master.entity.dto';
import { ProductImage } from '../models/product.image';
import { CartModel } from '../models/cart.model';
import { CustomerModel } from '../models/customer.model';
import { TrxTransactionModel } from '../models/trx.transaction.model';
import { TrxTransactionDetailModel } from '../models/trx.transaction.detail.model';

export class ShoppingCartDTO {
  cart: CartModel[] = [];
  customer: CustomerModel;
  trxTransaction: TrxTransactionModel;
  transactionDetail: TrxTransactionDetailModel[] = [];
  constructor(
    cart: CartModel[],
    customer: CustomerModel,
    trxtransaction: TrxTransactionModel,
    trxtransactiondetail: TrxTransactionDetailModel[]
  ) {
    this.cart = cart;
    this.customer = customer;
    this.trxTransaction = trxtransaction;
    this.transactionDetail = trxtransactiondetail;
  }
}
