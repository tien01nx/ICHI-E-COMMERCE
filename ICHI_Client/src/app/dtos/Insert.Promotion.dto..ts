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
import { PromotionModel } from '../models/promotion.model';
import { PromotionDetails } from '../models/PromotionDetails.model';

export class InsertPromotion {
  id: number;
  promotionName: string;
  startTime: Date;
  endTime: Date;
  discount: number;
  quantity: number;
  isActive: boolean;
  isDeleted: boolean;
  promotionDetails: PromotionDetails[];
  constructor(
    id: number,
    promotionName: string,
    startTime: Date,
    endTime: Date,
    discount: number,
    quantity: number,
    isActive: boolean,
    isDeleted: boolean,
    promotionDetails: PromotionDetails[]
  ) {
    this.id = id;
    this.promotionName = promotionName;
    this.startTime = startTime;
    this.endTime = endTime;
    this.discount = discount;
    this.quantity = quantity;
    this.isActive = isActive;
    this.isDeleted = isDeleted;
    this.promotionDetails = promotionDetails;
  }
}
