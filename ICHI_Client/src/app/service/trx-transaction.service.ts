import { TrxTransactionDTO } from './../dtos/trxtransaction.dto';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Environment } from '../environment/environment';
import { InsertCartDTO } from '../dtos/insert.cart.dto';
import { catchError, map, mergeMap, throwError } from 'rxjs';
import { VnPaymentRequestDTO } from '../dtos/vn.payment.request.dto';

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

  AddTrxTransaction(model: TrxTransactionDTO) {
    return this.http.post(
      this.baseUrl + '/TrxTransaction/InsertTxTransaction',
      model
    );
  }
  vnpaydto!: VnPaymentRequestDTO;

  PaymentExecute(model: TrxTransactionDTO) {
    return this.http
      .post(this.baseUrl + '/TrxTransaction/InsertTxTransaction', model)
      .pipe(
        mergeMap((response: any) => {
          // Tạo và gửi request tới /TrxTransaction/CreatePaymentUrl
          const vnpaydto = new VnPaymentRequestDTO(
            response.data.trxTransactionId,
            response.data.fullName,
            response.data.amount,
            new Date()
          );
          return this.http
            .post(this.baseUrl + '/TrxTransaction/CreatePaymentUrl', vnpaydto)
            .pipe(
              map((paymentResponse: any) => {
                // Xử lý kết quả từ /TrxTransaction/CreatePaymentUrl ở đây
                // Ví dụ: Trả về kết quả hoặc thực hiện các thao tác khác
                
                return paymentResponse;
              })
            );
        }),
        catchError((error: any) => {
          // Xử lý lỗi ở đây, nếu cần
          return throwError(error);
        })
      )
     
  }
  // PaymentExecute(model: any) {
  //   return this.http.post(
  //     this.baseUrl + '/TrxTransaction/CreatePaymentUrl',
  //     model
  //   );
  // }
}
