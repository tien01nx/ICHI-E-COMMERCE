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
  constructor(
    product: ProductModel,
    category: CategoryProduct,
    ProductImages: ProductImage[]
  ) {
    this.product = product;
    this.categoryProduct = category;
    this.productImages = ProductImages;
  }
}
