import { CategoryProduct } from './../models/category.product';
import {
  IsString,
  IsNotEmpty,
  IsPhoneNumber,
  IsBoolean,
  IsNumber,
} from 'class-validator';
import { MasterEntityDTO } from './master.entity.dto';

export class InsertProductDTO extends MasterEntityDTO {
  @IsString()
  @IsNotEmpty()
  description: string;
  @IsNumber()
  @IsNotEmpty()
  categoryProductID: number;
  @IsString()
  @IsNumber()
  price: number;
  @IsString()
  notes: string;
  @IsBoolean()
  isActive: boolean;
  @IsBoolean()
  isDeleted: boolean;
  @IsString()
  productImages: any;

  constructor(
    id: number,
    createDatetime: Date,
    createUserId: string,
    updateDatetime: Date,
    updateUserId: string,
    description: string,
    categoryProductID: number,
    price: number,
    notes: string,
    isActive: boolean,
    isDeleted: boolean,
    productImages: any
  ) {
    super(id, createDatetime, createUserId, updateDatetime, updateUserId);
    this.description = description;
    this.categoryProductID = categoryProductID;
    this.price = price;
    this.notes = notes;
    this.isActive = isActive;
    this.isDeleted = isDeleted;
    this.productImages = productImages;
  }
}
