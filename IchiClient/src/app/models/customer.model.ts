import { Data } from '@angular/router';
import { MasterEntity } from './master.entity';

export interface CustomerModel extends MasterEntity {
  customerName: string;
  phoneNumber: string;
  gender: string;
  birthday: Date;
  avatar: string;
}
