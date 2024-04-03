import { Environment } from './../../../environment/environment';
import { TokenService } from './../../../service/token.service';
import { Component, OnInit } from '@angular/core';
import { ClientFooterComponent } from '../client-footer/client-footer.component';
import { ClientHeaderComponent } from '../client-header/client-header.component';
import { ClientMenuComponent } from '../client-menu/client-menu.component';
import { CartModel } from '../../../models/cart.model';
import { TrxTransactionService } from '../../../service/trx-transaction.service';
import { Toast, ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.css',
})
export class CartComponent implements OnInit {
  carts: CartModel[] = [];
  Environment = Environment;
  constructor(
    private cartService: TrxTransactionService,
    private toastr: ToastrService,
    private tokenService: TokenService,
    private router: Router
  ) {}

  priceShip: number = 30000;
  ngOnInit(): void {
    this.getCartByUserId();
  }

  getCartByUserId() {
    this.cartService
      .GetCartByUserId(this.tokenService.getUserEmail())
      .subscribe({
        next: (response: any) => {
          this.carts = response.data;
          console.log(this.carts);
        },
        error: (error: any) => {
          this.toastr.error('Lỗi lấy thông tin giỏ hàng', 'Thông báo');
        },
      });
  }

  deleteItemCart(cart: CartModel) {
    this.cartService.DeleteItemCart(cart).subscribe({
      next: (response: any) => {
        if (response.code === 200) {
          this.toastr.success(response.message, 'Thông báo');
          this.getCartByUserId();
        } else {
          this.toastr.error(response.message, 'Thông báo');
        }
      },
      error: (error: any) => {
        this.toastr.error('Lỗi xóa sản phẩm khỏi giỏ hàng', 'Thông báo');
      },
    });
  }

  checkout() {
    this.router.navigate(['/checkout']);
  }

  updateQuantity(cart: CartModel, change: number) {
    const newQuantity = cart.quantity + change;
    if (newQuantity > 0) {
      cart.quantity = newQuantity;
      this.cartService.UpdateQuantityCart(cart).subscribe({
        next: (response: any) => {
          if (response.code === 200) {
            this.getCartByUserId();
          } else {
            this.toastr.error(response.message, 'Thông báo');
          }
        },
        error: (error: any) => {
          this.toastr.error('Lỗi cập nhật số lượng sản phẩm', 'Thông báo');
        },
      });
    }
  }

  increaseQuantity(cart: CartModel) {
    this.updateQuantity(cart, 1);
  }

  decreaseQuantity(cart: CartModel) {
    this.updateQuantity(cart, -1);
  }
  getTotalPrice(): number {
    let totalPrice = 0;
    this.carts.forEach((cart) => {
      totalPrice += cart.quantity * cart.product.price;
    });
    return totalPrice;
  }
  // Tổng số tiền các sanr phẩm giảm gía trong giỏ hàng = Số lượng * giảm giá * % giảm giá
  getTotalDiscount(): number {
    let totalDiscount = 0;
    this.carts.forEach((cart) => {
      totalDiscount +=
        cart.quantity * (cart.discount / 100) * cart.product.price;
    });
    return totalDiscount;
  }

  getTotalIntoMoney(): number {
    let totalPrice = 0;
    totalPrice += this.getTotalPrice(); // Tổng giá trị hàng hóa
    totalPrice -= this.getTotalDiscount(); // Giảm giá
    totalPrice += this.priceShip; // Giá vận chuyển
    return totalPrice;
  }
}
