import {
  IsString,
  IsNotEmpty,
  IsPhoneNumber,
  IsBoolean,
  IsNumber,
} from 'class-validator';
import { MasterEntityDTO } from './master.entity.dto';

export class InsertCartDTO {
  userId: string;
  productId: number;
  price: number;
  quantity: number;

  constructor(
    userId: string,
    productId: number,
    price: number,
    quantity: number
  ) {
    this.userId = userId;
    this.productId = productId;
    this.price = price;
    this.quantity = quantity;
  }
}
