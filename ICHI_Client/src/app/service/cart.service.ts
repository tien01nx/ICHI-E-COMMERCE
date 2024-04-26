import { UserDTO } from '../dtos/user.dto';
import { Injectable } from '@angular/core';
import { ApiServiceService } from './api.service.service';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Environment } from '../environment/environment';
import { ApiResponse } from '../models/api.response.model';
import { BehaviorSubject, Observable, map } from 'rxjs';
import { UserModel } from '../models/user.model';
import { UpdateUserDTO } from '../dtos/update.user.dto';
import { InsertCartDTO } from '../dtos/insert.cart.dto';

@Injectable({
  providedIn: 'root',
})
export class CartService {
  baseUrl = Environment.apiBaseUrl;
  private cartItems: any[] = [];
  private cartItemCount = new BehaviorSubject<number>(0);

  constructor(
    private apiService: ApiServiceService,
    private http: HttpClient
  ) {}
  Environment = Environment;

  addToCart(product: any) {
    this.cartItems.push(product);
    this.cartItemCount.next(this.cartItems.length);
  }

  AddToCart(model: InsertCartDTO) {
    return this.http.post(this.baseUrl + '/Cart/Create', model);
  }

  Update(model: InsertCartDTO) {
    return this.http.put(this.baseUrl + '/Cart/Update', model);
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
}
