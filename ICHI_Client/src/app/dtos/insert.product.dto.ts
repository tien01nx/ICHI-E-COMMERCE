import { CategoryProduct } from './../models/category.product';
import { IsString, IsNotEmpty, IsBoolean, IsNumber } from 'class-validator';
import { MasterEntityDTO } from './master.entity.dto';

export class InsertProductDTO extends MasterEntityDTO {
  @IsString()
  @IsNotEmpty()
  productName: string;
  @IsString()
  @IsNotEmpty()
  description: string;
  @IsNumber()
  @IsNotEmpty()
  categoryProductID: number;
  @IsString()
  @IsNumber()
  suggestedPrice: number;
  @IsString()
  @IsNumber()
  sellingPrice: number;
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
    productName: string,
    description: string,
    categoryProductID: number,
    suggestedPrice: number,
    sellingPrice: number,
    notes: string,
    isActive: boolean,
    isDeleted: boolean,
    productImages: any
  ) {
    super(id, createDatetime, createUserId, updateDatetime, updateUserId);
    this.productName = productName;
    this.description = description;
    this.categoryProductID = categoryProductID;
    this.suggestedPrice = suggestedPrice;
    this.sellingPrice = sellingPrice;
    this.notes = notes;
    this.isActive = isActive;
    this.isDeleted = isDeleted;
    this.productImages = productImages;
  }
}
