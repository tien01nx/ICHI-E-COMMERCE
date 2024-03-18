import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Environment } from '../environment/environment';
import { InsertCartDTO } from '../dtos/insert.cart.dto';
import { map } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class TrxTransactionService {
  baseUrl = Environment.apiBaseUrl;
  constructor(private http: HttpClient) {}

  AddToCart(model: InsertCartDTO) {
    return this.http.post(this.baseUrl + '/Cart/AddtoCart', model);
  }

  DeleteItemCart(model: InsertCartDTO) {
    const options = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
      body: model,
    };

    return this.http.delete(this.baseUrl + '/Cart/DeleteCart', options);
  }

  GetCartByUserId(userId: string) {
    return this.http.get(this.baseUrl + '/Cart/GetCarts?email=' + userId);
  }

  GetTrxTransaction(userId: string) {
    return this.http.get(
      this.baseUrl + '/Cart/GetShoppingCart?email=' + userId
    );
  }

  AddTrxTransaction(model: any) {
    return this.http.post(
      this.baseUrl + '/TrxTransaction/InsertTxTransaction',
      model
    );
  }
}
