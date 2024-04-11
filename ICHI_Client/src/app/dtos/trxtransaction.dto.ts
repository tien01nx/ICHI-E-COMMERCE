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
  customerId: string;
  employeeId: string;
  fullName: string;
  phoneNumber: string;
  address: string;
  amount: number;
  paymentTypes: string;
  checkOrder: boolean;
  carts: any;
  constructor(
    trxTransactionId: number,
    customerId: string,
    employeeId: string,
    fullName: string,
    phoneNumber: string,
    address: string,
    amount: number,
    paymentTypes: string,
    checkOrder: boolean,
    carts: any
  ) {
    this.trxTransactionId = trxTransactionId;
    this.customerId = customerId;
    this.employeeId = employeeId;
    this.fullName = fullName;
    this.phoneNumber = phoneNumber;
    this.address = address;
    this.amount = amount;
    this.paymentTypes = paymentTypes;
    this.checkOrder = checkOrder;
    this.carts = carts;
  }
}
