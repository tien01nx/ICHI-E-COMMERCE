import { IsString } from 'class-validator';
import { Injectable } from '@angular/core';
import { ApiServiceService } from './api.service.service';
import { UserLogin } from '../models/user.login';
import { HttpClient } from '@angular/common/http';
import { Environment } from '../environment/environment';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  constructor(
    private apiService: ApiServiceService,
    private http: HttpClient
  ) {}
  baseUrl = Environment.apiBaseUrl;

  login(userLogin: UserLogin) {
    return this.apiService.callApi<any>('/Auth/Login', 'post', null, userLogin);
  }

  register(userRegister: any) {
    return this.apiService.callApi<any>(
      '/Auth/Register',
      'post',
      null,
      userRegister
    );
  }

  registerEmployee(userRegister: any) {
    return this.apiService.callApi<any>(
      '/Auth/RegisterEmployee',
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

  // đổi mật khẩu
  changePassword(data: any) {
    return this.http.put(this.baseUrl + '/Auth/change-password', data);
  }
}
