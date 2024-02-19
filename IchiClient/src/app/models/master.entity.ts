import { Data } from '@angular/router';

export interface MasterEntity {
  id: number;
  createDatetime: Date;
  createUserId: string;
  updateDatetime: Data;
  updateUserId: string;
}
