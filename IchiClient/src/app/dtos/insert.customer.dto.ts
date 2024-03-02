import {
  IsString,
  IsNotEmpty,
  IsPhoneNumber,
  IsBoolean,
  IsNumber,
} from 'class-validator';
import { MasterEntityDTO } from './master.entity.dto';

export class InsertCustomerDTO extends MasterEntityDTO {
  name: string;
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
    name: string,
    phoneNumber: string,
    gender: string,
    birthday: Date,
    avatar: string
  ) {
    super(id, createDatetime, createUserId, updateDatetime, updateUserId);
    this.name = name;
    this.phoneNumber = phoneNumber;
    this.gender = gender;
    this.birthday = birthday;
    this.avatar = avatar;
  }
}
