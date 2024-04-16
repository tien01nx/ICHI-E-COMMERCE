import { Data } from '@angular/router';
import { MasterEntity } from './master.entity';
import { UserModel } from './user.model';

export interface CustomerModel extends MasterEntity {
  fullName: string;
  userId: number;
  phoneNumber: string;
  gender: string;
  birthday: Date;
  avatar: string;
  ward: string;
  district: string;
  city: string;
  address: string;
  user: UserModel;
}
