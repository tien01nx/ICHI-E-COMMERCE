import { UserModel } from '../models/user.model';

export class UpdateUserDTO {
  id: number;
  fullName: string;
  password: string;
  role: string;
  email: string;
  birthday: Date;
  gender: string;
  phoneNumber: string;
  constructor(
    id: number,
    fullName: string,
    password: string,
    role: string,
    email: string,
    birthday: Date,
    gender: string,
    phoneNumber: string
  ) {
    this.id = id;
    this.fullName = fullName;
    this.password = password;
    this.role = role;
    this.email = email;
    this.birthday = birthday;
    this.gender = gender;
    this.phoneNumber = phoneNumber;
  }
}
