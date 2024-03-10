import { Data } from '@angular/router';
import { MasterEntity } from './master.entity';

export interface RoleModel extends MasterEntity {
  roleName: string;
  description: string;
}
