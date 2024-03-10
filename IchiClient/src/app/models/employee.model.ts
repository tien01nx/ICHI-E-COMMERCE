import { Data } from '@angular/router';
import { MasterEntity } from './master.entity';

export interface EmployeeModel extends MasterEntity {
  fullName: string;
  userId: number;
  phoneNumber: string;
  gender: string;
  birthday: Date;
  avatar: string;
  user: any;
  address: string;
}
