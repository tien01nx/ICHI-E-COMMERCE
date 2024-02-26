import { Data } from '@angular/router';

export interface MasterEntity {
  id: number;
  createDatetime: Date;  
  createUserId: string;
  updateDatetime: Date;
  updateUserId: string;
}
