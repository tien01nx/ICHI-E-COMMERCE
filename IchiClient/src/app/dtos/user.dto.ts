import { UserModel } from '../models/user.model';

export class UserDTO {
  id: number;
  user: any;
  fullName: string;
  role: string;
  email: string;
  password: string;
  birthday: Date;
  gender: string;
  constructor(
    id: number,
    user: any,
    fullName: string,
    role: string,
    email: string,
    password: string,
    birthday: Date,
    gender: string
  ) {
    this.id = id;
    this.user = user;
    this.fullName = fullName;
    this.role = role;
    this.email = email;
    this.password = password;
    this.birthday = birthday;
    this.gender = gender;
  }
}
