import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Environment } from '../environment/environment';
import { TokenService } from './token.service';

@Injectable({
  providedIn: 'root',
})
export class ProfileService {
  baseUrl = Environment.apiBaseUrl;
  constructor(private http: HttpClient, private tokenService: TokenService) {}

  getProfile() {
    const email = this.tokenService.getUserEmail();
    return this.http.get(
      this.baseUrl + '/TrxTransaction/GetCustomerTransaction?email=' + email
    );
  }
}
