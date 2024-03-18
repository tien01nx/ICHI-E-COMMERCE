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

export class CartDTO {
  cart: CartModel[] = [];
  productimages: ProductImage;
  customer: CustomerModel;
  constructor(
    cart: CartModel[],
    productimages: ProductImage,
    customer: CustomerModel
  ) {
    this.cart = cart;
    this.productimages = productimages;
    this.customer = customer;
  }
}
