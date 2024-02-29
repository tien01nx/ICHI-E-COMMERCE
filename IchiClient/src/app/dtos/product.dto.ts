import { ProductModel } from './../models/product.model';
import { CategoryProduct } from '../models/category.product';
import {
  IsString,
  IsNotEmpty,
  IsPhoneNumber,
  IsBoolean,
  IsNumber,
} from 'class-validator';
import { MasterEntityDTO } from './master.entity.dto';

export class ProductDTO {
  product: ProductModel;
  category: CategoryProduct;
  ProductImages: any;
  constructor(
    product: ProductModel,
    category: CategoryProduct,
    ProductImages: any
  ) {
    this.product = product;
    this.category = category;
    this.ProductImages = ProductImages;
  }
}
