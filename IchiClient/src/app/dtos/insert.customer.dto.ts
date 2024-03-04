import {
  IsString,
  IsNotEmpty,
  IsPhoneNumber,
  IsBoolean,
  IsNumber,
} from 'class-validator';
import { MasterEntityDTO } from './master.entity.dto';

export class InsertCustomerDTO extends MasterEntityDTO {
  fullName: string;
  phoneNumber: string;
  gender: string;
  birthday: Date;
  avatar: string;

  constructor(
    id: number,
    createDatetime: Date,
    createUserId: string,
    updateDatetime: Date,
    updateUserId: string,
    fullName: string,
    phoneNumber: string,
    gender: string,
    birthday: Date,
    avatar: string
  ) {
    super(id, createDatetime, createUserId, updateDatetime, updateUserId);
    this.fullName = fullName;
    this.phoneNumber = phoneNumber;
    this.gender = gender;
    this.birthday = birthday;
    this.avatar = avatar;
  }
}
