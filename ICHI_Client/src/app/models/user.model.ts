import { Data } from '@angular/router';
import { MasterEntity } from './master.entity';

export interface UserModel extends MasterEntity {
  userName: string;
  email: string;
  avatar: string;
  password: string;
  isLocked: boolean;
  failedPassAttemptCount: number;
}
