import { Data } from '@angular/router';

export interface MasterEntity {
  id: number;
  createDate: Date;
  createBy: string;
  modifiedDate: Date;
  modifiedBy: string;
}
