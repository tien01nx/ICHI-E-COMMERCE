import {
  IsString,
  IsNotEmpty,
  IsPhoneNumber,
  IsBoolean,
  IsNumber,
} from 'class-validator';
import { MasterEntityDTO } from './master.entity.dto';

export class InsertCategoryDTO extends MasterEntityDTO {

  @IsNumber()
  @IsNotEmpty({ message: 'ParentID is required' })
  parentID: number;

  @IsNumber()
  @IsNotEmpty()
  categoryLevel: number;

  @IsString()
  @IsNotEmpty()
  categoryName: string;
  @IsString()
  @IsNotEmpty()
  notes: string;
  @IsBoolean()
  isDeleted: boolean;

  constructor(
    id: number,
    createDatetime: Date,
    createUserId: string,
    updateDatetime: Date,
    updateUserId: string,
    parentID: number,
    categoryLevel: number,
    categoryName: string,
    notes: string,
    isDeleted: boolean
  ) {
    super(id, createDatetime, createUserId, updateDatetime, updateUserId);
    this.parentID = parentID;
    this.categoryLevel = categoryLevel;
    this.categoryName = categoryName;
    this.notes = notes;
    this.isDeleted = isDeleted;
  }
}
