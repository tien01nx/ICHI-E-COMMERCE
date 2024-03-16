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
  categoryId: number;

  @IsString()
  @IsNotEmpty()
  color: string;

  @IsNumber()
  @IsNotEmpty()
  price: number;

  @IsNumber()
  @IsNotEmpty()
  priorityLevel: number;

  @IsNumber()
  @IsNotEmpty()
  trademarkId: number;
  @IsString()
  notes: string;
  @IsBoolean()
  isActive: boolean;
  @IsBoolean()
  isDeleted: boolean;
  @IsString()
  productImages: any;

  quantity: number;
  constructor(
    id: number,
    createDatetime: Date,
    createUserId: string,
    updateDatetime: Date,
    updateUserId: string,
    productName: string,
    description: string,
    categoryProductID: number,
    priorityLevel: number,
    price: number,
    trademarkId: number,
    color: string,
    notes: string,
    isActive: boolean,
    isDeleted: boolean,
    productImages: any,
    quantity: number
  ) {
    super(id, createDatetime, createUserId, updateDatetime, updateUserId);
    this.productName = productName;
    this.description = description;
    this.categoryId = categoryProductID;
    this.color = color;
    this.price = price;
    this.priorityLevel = priorityLevel;
    this.trademarkId = trademarkId;
    this.notes = notes;
    this.isActive = isActive;
    this.isDeleted = isDeleted;
    this.productImages = productImages;
    this.quantity = quantity;
  }
}
