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
import { ProductModel } from '../models/product.model';

export class OrderTrackingDTO {
  trxTransactionId: number;
  productId: number;
  price: number;
  quantity: number;
  productImage: string;
  product: ProductModel;
  trxTransaction: TrxTransactionModel;
  discount: number;

  constructor(data: any) {
    this.trxTransactionId = data.trxTransactionId;
    this.productId = data.productId;
    this.price = data.price;
    this.quantity = data.quantity;
    this.productImage = data.productImage;
    this.product = data.product;
    this.trxTransaction = data.trxTransaction;
    this.discount = data.discount;
  }
}
