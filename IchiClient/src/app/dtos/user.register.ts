export class UserResgiter {
  userName: string;
  userPassword: string;
  fullName: string;
  phoneNumber: string;
  constructor(
    userName: string,
    userPassword: string,
    fullName: string,
    phoneNumber: string
  ) {
    this.userName = userName;
    this.userPassword = userPassword;
    this.fullName = fullName;
    this.phoneNumber = phoneNumber;
  }
}
