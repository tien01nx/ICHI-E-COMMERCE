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
import { ProductImage } from '../models/product.image';

export class ProductDTO {
  product: ProductModel;
  categoryProduct: CategoryProduct;
  productImages: ProductImage[];
  quantitySold: number;
  quantityOnnTheOrder: number;
  constructor(
    product: ProductModel,
    category: CategoryProduct,
    ProductImages: ProductImage[],
    quantitySold: number,
    quantityOnnTheOrder: number
  ) {
    this.product = product;
    this.categoryProduct = category;
    this.productImages = ProductImages;
    this.quantitySold = quantitySold;
    this.quantityOnnTheOrder = quantityOnnTheOrder;
  }
}
