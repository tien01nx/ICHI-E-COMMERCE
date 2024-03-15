import { IsNotEmpty, IsNumber, IsString } from 'class-validator';

export class UpdatePromotionDTO {
  @IsString()
  @IsNotEmpty()
  promotionCode: string;
  @IsString()
  @IsNotEmpty()
  promotionName: string;
  @IsString()
  @IsNotEmpty()
  startTime: string;
  @IsString()
  @IsNotEmpty()
  endTime: string;
  @IsString()
  @IsNotEmpty()
  quantity: string;
  @IsNumber()
  @IsNotEmpty()
  discount: string;
  @IsNumber()
  minimumPrice: string;
  isActive: string;
  isDeleted: string;

  constructor(data: any) {
    this.promotionCode = data.promotionCode;
    this.promotionName = data.promotionName;
    this.startTime = data.startTime;
    this.endTime = data.endTime;
    this.quantity = data.quantity;
    this.discount = data.discount;
    this.minimumPrice = data.minimumPrice;
    this.isActive = data.isActive;
    this.isDeleted = data.isDeleted;
  }
}
