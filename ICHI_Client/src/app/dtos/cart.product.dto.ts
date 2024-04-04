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

export class CartProductDTO {
  email: string;
  carts: any;
  constructor(email: string, carts: any) {
    this.email = email;
    this.carts = carts;
  }
}
