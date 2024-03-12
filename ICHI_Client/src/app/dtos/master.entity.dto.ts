import { MasterEntity } from '../models/master.entity';
export class MasterEntityDTO {
  id: number;
  createDatetime: Date;
  createUserId: string;
  updateDatetime: Date;
  updateUserId: string;

  constructor(
    id : number,
    createDatetime: Date,
    createUserId: string,
    updateDatetime: Date,
    updateUserId: string
  ) {
    this.id = id;
    this.createDatetime = createDatetime;
    this.createUserId = createUserId;
    this.updateDatetime = updateDatetime;
    this.updateUserId = updateUserId;
  }
}
