import { Data } from '@angular/router';
import { MasterEntity } from './master.entity';

export interface CustomerModel extends MasterEntity {
  name: string;
  phoneNumber: string;
  gender: boolean;
  birthday: Date;
  avatar: string;
  isDeleted: boolean;
  userId: string;
}
