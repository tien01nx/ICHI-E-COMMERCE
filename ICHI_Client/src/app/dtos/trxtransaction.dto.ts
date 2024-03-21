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

export class TrxTransactionDTO {
  trxTransactionId: number;
  userId: string;
  fullName: string;
  phoneNumber: string;
  address: string;
  amount: number;
  constructor(
    trxTransactionId: number,
    userId: string,
    fullName: string,
    phoneNumber: string,
    address: string,
    amount: number
  ) {
    this.trxTransactionId = trxTransactionId;
    this.userId = userId;
    this.fullName = fullName;
    this.phoneNumber = phoneNumber;
    this.address = address;
    this.amount = amount;
  }
}
