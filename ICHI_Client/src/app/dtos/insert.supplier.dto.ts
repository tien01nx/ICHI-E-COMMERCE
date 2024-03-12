import {
  IsString,
  IsNotEmpty,
  IsPhoneNumber,
  IsBoolean,
  IsNumber,
} from 'class-validator';
import { MasterEntityDTO } from './master.entity.dto';

export class InsertSupplierDTO extends MasterEntityDTO {
  @IsString()
  @IsNotEmpty()
  supplierCode: string;
  @IsString()
  @IsNotEmpty()
  supplierName: string;
  @IsString()
  @IsNotEmpty()
  address: string;
  @IsPhoneNumber('VN')
  @IsNotEmpty()
  phone: string;
  @IsString()
  @IsNotEmpty()
  email: string;
  @IsString()
  @IsNotEmpty()
  taxCode: string;
  @IsString()
  @IsNotEmpty()
  bankAccount: string;
  @IsString()
  @IsNotEmpty()
  bankName: string;
  @IsBoolean()
  isActive: boolean;
  @IsBoolean()
  isDeleted: boolean;

  constructor(
    id: number,
    createDatetime: Date,
    createUserId: string,
    updateDatetime: Date,
    updateUserId: string,
    supplierCode: string,
    supplierName: string,
    address: string,
    phone: string,
    email: string,
    taxCode: string,
    bankAccount: string,
    bankName: string,
    isActive: boolean,
    isDeleted: boolean
  ) {
    super(id, createDatetime, createUserId, updateDatetime, updateUserId);
    this.supplierCode = supplierCode;
    this.supplierName = supplierName;
    this.address = address;
    this.phone = phone;
    this.email = email;
    this.taxCode = taxCode;
    this.bankAccount = bankAccount;
    this.bankName = bankName;
    this.isActive = isActive;
    this.isDeleted = isDeleted;
  }
}
