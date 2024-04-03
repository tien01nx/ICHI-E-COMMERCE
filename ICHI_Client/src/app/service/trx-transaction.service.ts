import { TrxTransactionDTO } from './../dtos/trxtransaction.dto';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Environment } from '../environment/environment';
import { InsertCartDTO } from '../dtos/insert.cart.dto';
import {
  BehaviorSubject,
  Observable,
  catchError,
  map,
  mergeMap,
  tap,
  throwError,
} from 'rxjs';
import { VnPaymentRequestDTO } from '../dtos/vn.payment.request.dto';
import { ApiServiceService } from './api.service.service';
import { ShoppingCartDTO } from '../dtos/shopping.cart.dto.';
import { Utils } from '../Utils.ts/utils';
import { CartModel } from '../models/cart.model';

@Injectable({
  providedIn: 'root',
})
export class TrxTransactionService {
  baseUrl = Environment.apiBaseUrl;
  private cartItems: any[] = [];
  private cartItemCount = new BehaviorSubject<number>(0);
  constructor(
    private http: HttpClient,
    private apiService: ApiServiceService
  ) {}

  getCarts(): CartModel[] {
    const cartsString = localStorage.getItem(Utils.cartList);
    if (cartsString) {
      return JSON.parse(cartsString);
    } else {
      return [];
    }
  }

  setCarts(carts: CartModel[]): void {
    const cartsString = JSON.stringify(carts);
    localStorage.setItem(Utils.cartList, cartsString);
  }

  getCartByUserId(userId: string): Observable<any[]> {
    return this.http.get<any[]>(
      `${this.baseUrl}/Cart/GetCarts?email=${userId}`
    );
  }

  getCartItemCount(userId: string): Observable<number> {
    return this.getCartByUserId(userId).pipe(
      map((cartItems: any) => {
        const data = cartItems.data;
        console.log('data', data);
        // Kiểm tra xem cartItems có phải là một mảng không
        if (Array.isArray(data)) {
          // Đếm số lượng sản phẩm trong giỏ hàng
          let itemCount = 0;
          data.forEach((cartItem: any) => {
            itemCount++;
          });
          return itemCount;
        } else {
          // Nếu cartItems không phải là một mảng, trả về 0 hoặc giá trị mặc định khác
          return 0;
        }
      })
    );
  }

  checkProductPromotion(carts: any) {
    debugger;
    return this.http.post(`${this.baseUrl}/Cart/CheckCartPromotion`, carts);
  }

  addToCart(product: any) {
    this.cartItems.push(product);
    this.cartItemCount.next(this.cartItems.length);
  }

  AddToCart(model: InsertCartDTO) {
    return this.http.post(this.baseUrl + '/Cart/AddtoCart', model);
  }

  UpdateQuantityCart(model: InsertCartDTO) {
    return this.http.put(this.baseUrl + '/Cart/UpdateCart', model);
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

  GetTrxTransaction(userId: string) {
    return this.http.get(
      this.baseUrl + '/Cart/GetShoppingCart?email=' + userId
    );
  }

  GetTrxTransactionFindById(id: number) {
    return this.apiService.callApi<ShoppingCartDTO>(
      '/TrxTransaction/GetTrxTransactionFindById',
      'post',
      null,
      id
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
    debugger;
    let requestObservable: Observable<any>;

    if (model.trxTransactionId === 0) {
      requestObservable = this.http
        .post(this.baseUrl + '/TrxTransaction/InsertTxTransaction', model)
        .pipe(
          mergeMap((response: any) => {
            const vnpaydto = new VnPaymentRequestDTO(
              response.data.trxTransactionId,
              response.data.fullName,
              response.data.amount,
              new Date()
            );
            return this.createPaymentUrl(vnpaydto);
          }),
          catchError((error: any) => throwError(error))
        );
    } else {
      const vnpaydto = new VnPaymentRequestDTO(
        model.trxTransactionId,
        model.fullName,
        model.amount,
        new Date()
      );
      requestObservable = this.createPaymentUrl(vnpaydto);
    }

    return requestObservable;
  }

  private createPaymentUrl(vnpaydto: VnPaymentRequestDTO): Observable<any> {
    return this.http
      .post(this.baseUrl + '/TrxTransaction/CreatePaymentUrl', vnpaydto)
      .pipe(
        tap((paymentResponse: any) => {
          // Xử lý kết quả từ /TrxTransaction/CreatePaymentUrl ở đây
          // Ví dụ: Trả về kết quả hoặc thực hiện các thao tác khác
        }),
        catchError((error: any) => throwError(error))
      );
  }

  // PaymentExecute(model: TrxTransactionDTO) {
  //   if (model.trxTransactionId === 0) {
  //     return this.http
  //       .post(this.baseUrl + '/TrxTransaction/InsertTxTransaction', model)
  //       .pipe(
  //         mergeMap((response: any) => {
  //           // Tạo và gửi request tới /TrxTransaction/CreatePaymentUrl
  //           const vnpaydto = new VnPaymentRequestDTO(
  //             response.data.trxTransactionId,
  //             response.data.fullName,
  //             response.data.amount,
  //             new Date()
  //           );
  //           return this.http
  //             .post(this.baseUrl + '/TrxTransaction/CreatePaymentUrl', vnpaydto)
  //             .pipe(
  //               map((paymentResponse: any) => {
  //                 // Xử lý kết quả từ /TrxTransaction/CreatePaymentUrl ở đây
  //                 // Ví dụ: Trả về kết quả hoặc thực hiện các thao tác khác

  //                 return paymentResponse;
  //               })
  //             );
  //         }),
  //         catchError((error: any) => {
  //           // Xử lý lỗi ở đây, nếu cần
  //           return throwError(error);
  //         })
  //       );
  //   }

  //   // Nếu model.trxTransactionId !== 0, thực hiện các thao tác khác
  //   const vnpaydto = new VnPaymentRequestDTO(
  //     model.trxTransactionId,
  //     model.fullName,
  //     model.amount,
  //     new Date()
  //   );
  //   return this.http
  //     .post(this.baseUrl + '/TrxTransaction/CreatePaymentUrl', vnpaydto)
  //     .pipe(
  //       map((paymentResponse: any) => {
  //         // Xử lý kết quả từ /TrxTransaction/CreatePaymentUrl ở đây
  //         // Ví dụ: Trả về kết quả hoặc thực hiện các thao tác khác

  //         return paymentResponse;
  //       })
  //     );
  // }
}
