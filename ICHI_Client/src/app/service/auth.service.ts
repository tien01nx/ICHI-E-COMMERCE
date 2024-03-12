import { Injectable } from '@angular/core';
import { ApiServiceService } from './api.service.service';
import { UserLogin } from '../models/user.login';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  constructor(private apiService: ApiServiceService) {}

  login(userLogin: UserLogin) {
    debugger;
    return this.apiService.callApi<UserLogin>(
      '/Auth/Login',
      'post',
      null,
      userLogin
    );
  }

  register(userRegister: any) {
    return this.apiService.callApi<any>(
      '/Auth/Register',
      'post',
      null,
      userRegister
    );
  }

  forgotPassword(email: string) {
    // ?email=tien01nx%40gmail.com
    return this.apiService.callApi<any>(
      '/Auth/forgot-password?email=' + email,
      'post'
    );
  }
}
