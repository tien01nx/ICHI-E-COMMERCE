import { MasterEntity } from '../models/master.entity';
export class MasterEntityDTO {
  createDatetime: Date;
  createUserId: string;
  updateDatetime: Date;
  updateUserId: string;

  constructor(
    createDatetime: Date,
    createUserId: string,
    updateDatetime: Date,
    updateUserId: string
  ) {
    this.createDatetime = createDatetime;
    this.createUserId = createUserId;
    this.updateDatetime = updateDatetime;
    this.updateUserId = updateUserId;
  }
}
