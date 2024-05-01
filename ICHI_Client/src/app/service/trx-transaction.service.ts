import { TrxTransactionDTO } from './../dtos/trxtransaction.dto';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Environment } from '../environment/environment';
import { InsertCartDTO } from '../dtos/insert.cart.dto';
import {
  BehaviorSubject,
  Observable,
  catchError,
  map,
  mergeMap,
  of,
  tap,
  throwError,
} from 'rxjs';
import { VnPaymentRequestDTO } from '../dtos/vn.payment.request.dto';
import { ApiServiceService } from './api.service.service';
import { ShoppingCartDTO } from '../dtos/shopping.cart.dto.';
import { Utils } from '../Utils.ts/utils';
import { CartModel } from '../models/cart.model';
import { CartProductDTO } from '../dtos/cart.product.dto';

@Injectable({
  providedIn: 'root',
})
export class TrxTransactionService {
  baseUrl = Environment.apiBaseUrl;
  private jsonUrl = 'assets/data.json';
  private cartItems: any[] = [];
  private cartItemCount = new BehaviorSubject<number>(0);
  constructor(private http: HttpClient) {}

  getJsonDataAddress(): Observable<any> {
    return this.http.get<any>(this.jsonUrl);
  }

  getFindAllTransaction(
    PageNumber: number,
    PageSize: number,
    SortDirection: string,
    SortBy: string,
    Search: string,
    OrderStatis: string,
    PaymentStatus: string
  ) {
    let params = new HttpParams();
    if (PageNumber && PageNumber.toString().trim() !== '') {
      params = params.set('page-number', PageNumber.toString());
    }
    if (PageSize && PageSize.toString().trim() !== '') {
      params = params.set('page-size', PageSize.toString());
    }
    if (SortDirection && SortDirection.trim() !== '') {
      params = params.set('sort-direction', SortDirection);
    }
    if (SortBy && SortBy.trim() !== '') {
      params = params.set('sort-by', SortBy);
    }
    if (Search && Search.trim() !== '') {
      params = params.set('search', Search);
    }

    if (OrderStatis && OrderStatis.trim() !== '') {
      params = params.set('order-status', OrderStatis);
    }

    if (OrderStatis && OrderStatis.trim() !== '') {
      params = params.set('payment-status', PaymentStatus);
    }
    // console.log(params);
    // return this.apiService.callApi<PromotionModel>(
    //   '/Promotion/FindAllPaged',
    //   'get',
    //   params
    // );
    return this.http.get(this.baseUrl + '/TrxTransaction/FindAllPaged', {
      params: params,
    });
  }
  GetDatawards(id: number) {
    return this.http.get(this.baseUrl + '/TrxTransaction/GetWards?code=' + id);
  }

  findAll() {
    return this.http.get(
      this.baseUrl +
        'TrxTransaction/FindAllPaged?page-size=10000&page-number=1&sort-direction=desc&sort-by=Id'
    );
  }

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

  removeCarts(): void {
    localStorage.removeItem(Utils.cartList);
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

  getOrderStatus() {
    return this.http.get(this.baseUrl + '/TrxTransaction/GetOrderStatus');
  }

  getCost(id: number) {
    return this.http.get(
      this.baseUrl + '/TrxTransaction/GetMonneyRevenue?year=' + id
    );
  }

  getExcel(year: number) {
    return this.http.get(
      `${this.baseUrl}/TrxTransaction/GenerateExcelReport?year=${year}`,
      {
        responseType: 'blob', // Set responseType to 'blob'
      }
    );
  }

  getReportBill(id: number) {
    return this.http.get(`${this.baseUrl}/Report/bill/` + id, {
      responseType: 'blob', // Set responseType to 'blob'
    });
  }

  getGetMonneyTotal() {
    return this.http.get(this.baseUrl + '/TrxTransaction/GetMonneyTotal');
  }

  getGetMonneyMoth(id: number) {
    return this.http.get(
      this.baseUrl + '/TrxTransaction/GetMonneyTotalByMonth?year=' + id
    );
  }

  checkProductPromotion(carts: any) {
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

    return this.http.delete(this.baseUrl + '/Cart/Delete', options);
  }

  GetTrxTransaction(cartProduct: CartProductDTO) {
    return this.http.post(this.baseUrl + '/Cart/GetShoppingCart', cartProduct);
  }

  GetTrxTransactionFindById(id: number) {
    return this.http.get(
      this.baseUrl + '/TrxTransaction/GetTrxTransactionFindById?id=' + id
    );
  }

  AddTrxTransaction(model: any) {
    return this.http.post(this.baseUrl + '/TrxTransaction/Insert', model);
  }

  vnpaydto!: VnPaymentRequestDTO;

  PaymentExecute(model: TrxTransactionDTO): Observable<any> {
    return this.http.post(this.baseUrl + '/TrxTransaction/Create', model).pipe(
      mergeMap((response: any) => {
        if (
          response.data &&
          response.data.paymentTypes === Utils.PaymentViaCard
        ) {
          if (response.data.checkOrder === true) {
            const vnpaydto = new VnPaymentRequestDTO(
              response.data.trxTransactionId,
              response.data.fullName,
              response.data.amount,
              'EMPLOYEE_CREATED',
              new Date()
            );
            return this.createPaymentUrl(vnpaydto);
          } else {
            const vnpaydto = new VnPaymentRequestDTO(
              response.data.trxTransactionId,
              response.data.fullName,
              response.data.amount,
              'USER_CREATED',
              new Date()
            );
            return this.createPaymentUrl(vnpaydto);
          }
        } else {
          // Return data for routing to another page if payment type is not via card
          return of(response.data);
        }
      })
    );
  }

  PaymentExecuteOrder(model: TrxTransactionDTO): Observable<any> {
    return this.http.post(this.baseUrl + '/TrxTransaction/Insert', model).pipe(
      mergeMap((response: any) => {
        if (
          response.data &&
          response.data.paymentTypes === Utils.PaymentViaCard
        ) {
          if (response.data.checkOrder === true) {
            const vnpaydto = new VnPaymentRequestDTO(
              response.data.trxTransactionId,
              response.data.fullName,
              response.data.amount,
              'EMPLOYEE_CREATED',
              new Date()
            );
            return this.createPaymentUrl(vnpaydto);
          } else {
            const vnpaydto = new VnPaymentRequestDTO(
              response.data.trxTransactionId,
              response.data.fullName,
              response.data.amount,
              'USER_CREATED',
              new Date()
            );
            return this.createPaymentUrl(vnpaydto);
          }
        } else {
          // Return data for routing to another page if payment type is not via card
          return of(response.data);
        }
      })
    );
  }

  updateTrxTransaction(model: any) {
    return this.http.put(this.baseUrl + '/TrxTransaction/Update', model);
  }

  createPaymentUrl(vnpaydto: VnPaymentRequestDTO): Observable<any> {
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

  getOrderTracking(orderId: string) {
    return this.http.get(
      this.baseUrl + '/Cart/CheckTransactionPromotion?transactionId=' + orderId
    );
  }

  listGoShip(data: any) {
    return this.http.post(this.baseUrl + '/TrxTransaction/PriceGoShip', data);
  }
}
