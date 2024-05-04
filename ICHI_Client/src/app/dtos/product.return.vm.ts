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
import { ProductReturn } from '../models/product.return';
import { ProductReturnDetail } from '../models/product.return.detail';

export class ProductReturnVM {
  productReturn: ProductReturn;
  productRetuenDetails: ProductReturnDetail[];
  constructor(
    productReturn: ProductReturn,
    productRetuenDetails: ProductReturnDetail[]
  ) {
    this.productReturn = productReturn;
    this.productRetuenDetails = productRetuenDetails;
  }
}
