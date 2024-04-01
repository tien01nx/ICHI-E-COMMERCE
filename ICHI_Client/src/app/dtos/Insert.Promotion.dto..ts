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
  promotion: PromotionModel;
  promotionDetails: PromotionDetails[];
  constructor(promotion: PromotionModel, promotionDetails: PromotionDetails[]) {
    this.promotion = promotion;
    this.promotionDetails = promotionDetails;
  }
}
