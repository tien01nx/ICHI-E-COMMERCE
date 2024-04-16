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
  isCheck: boolean = false;
  selectAll: boolean = false;
  selectedItems: number[] = [];
  cartsOrder: CartModel[] = [];

  constructor(
    private cartService: TrxTransactionService,
    private toastr: ToastrService,
    private tokenService: TokenService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.getCartByUserId();
    this.cartService.getCartItemCount(this.tokenService.getUserEmail());
  }

  toggleSelectAll() {
    if (this.selectAll) {
      this.selectedItems = [];
    } else {
      this.selectedItems = this.carts.map((item) => item.id);
    }
    this.cartsOrder = this.carts.filter((item) =>
      this.selectedItems.includes(item.id)
    );
    this.selectAll = !this.selectAll;
    console.log('cart: ', this.cartsOrder);
  }

  selectItem(itemId: number) {
    const index = this.selectedItems.indexOf(itemId);
    if (index > -1) {
      // Nếu mục đã được chọn, loại bỏ nó khỏi mảng
      this.selectedItems.splice(index, 1);
    } else {
      // Nếu mục chưa được chọn, thêm nó vào mảng
      this.selectedItems.push(itemId);
    }

    console.log('item', this.selectedItems);

    // từ selectedItem lấy ra các item trong carts
    this.cartsOrder = this.carts.filter((item) =>
      this.selectedItems.includes(item.id)
    );
    // nếu cartsOrder có ít nhất 1 phần tử thì hiển thị isCheck = true
    this.isCheck = this.cartsOrder.length > 0;
    console.log(this.isCheck);
    console.log('cart: ', this.cartsOrder);
  }

  getCartByUserId() {
    this.cartService
      .getCartByUserId(this.tokenService.getUserEmail())
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
        if (response.message === 'Xóa sản phẩm khỏi giỏ hàng thành công') {
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
    if (!this.isCheck) {
      this.cartsOrder = this.carts;
    }
    this.cartService.setCarts(this.cartsOrder);
    this.router.navigate(['/checkout']);
  }

  updateQuantity(cart: CartModel, change: number) {
    const newQuantity = cart.quantity + change;
    if (newQuantity > 0) {
      cart.quantity = newQuantity;
      this.cartService.UpdateQuantityCart(cart).subscribe({
        next: (response: any) => {
          if (response.message === 'Cập nhật giỏ hàng thành công') {
            this.getCartByUserId();
          } else {
            this.toastr.error(response.message, 'Thông báo');
            this.getCartByUserId();
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
  getTotalPriceItem(cart: CartModel): number {
    // return cart.quantity * cart.product.price;
    return (
      cart.quantity * cart.product.price -
      (cart.discount * cart.product.price) / 100
    );
  }

  decreaseQuantity(cart: CartModel) {
    this.updateQuantity(cart, -1);
  }

  getTotalPrice(): number {
    let totalPrice = 0;
    if (!this.isCheck) {
      console.log('object111', this.carts);
      // nếu discount ===0 thì => discount ===1 để tránh lỗi chia cho 0
      // suy ra totalPrice = quantity * price - (discount * price) / 100 ( xử lý nếu discount === 0 => discount === 1)
      this.carts.forEach((cart) => {
        if (cart.discount === 0) {
          totalPrice += cart.quantity * cart.product.price;
        } else {
          totalPrice += cart.quantity * cart.product.price;
        }
      });
      return totalPrice;
    } else {
      this.cartsOrder?.forEach((cart) => {
        if (cart.discount === 0) {
          totalPrice += cart.quantity * cart.product.price;
        } else {
          totalPrice += cart.quantity * cart.product.price;
        }
      });
      return totalPrice;
    }
  }
  // Tổng số tiền các sanr phẩm giảm gía trong giỏ hàng = Số lượng * giảm giá * % giảm giá
  getTotalDiscount(): number {
    let totalDiscount = 0;
    if (!this.isCheck) {
      this.carts.forEach((cart) => {
        totalDiscount +=
          cart.quantity * (cart.discount / 100) * cart.product.price;
      });
      return totalDiscount;
    }
    this.cartsOrder?.forEach((cart) => {
      totalDiscount +=
        cart.quantity * (cart.discount / 100) * cart.product.price;
    });
    return totalDiscount;
  }

  getTotalIntoMoney(): number {
    let totalPrice = 0;
    totalPrice += this.getTotalPrice(); // Tổng giá trị hàng hóa
    totalPrice -= this.getTotalDiscount(); // Giảm giá
    return totalPrice;
  }

  dataShow() {
    this.router.navigate(['/product/cart']);
  }
}
