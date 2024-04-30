import { Component, OnInit } from '@angular/core';
import { Utils } from '../../../../Utils.ts/utils';
import { ShoppingCartDTO } from '../../../../dtos/shopping.cart.dto.';
import { CartProductDTO } from '../../../../dtos/cart.product.dto';
import { CartModel } from '../../../../models/cart.model';
import { Environment } from '../../../../environment/environment';
import { TrxTransactionDTO } from '../../../../dtos/trxtransaction.dto';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { TrxTransactionService } from '../../../../service/trx-transaction.service';
import { ToastrService } from 'ngx-toastr';
import { TokenService } from '../../../../service/token.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-buy-now',
  templateUrl: './buy-now.component.html',
  styleUrl: './buy-now.component.css',
})
export class BuyNowComponent implements OnInit {
  protected readonly Utils = Utils;
  shoppingcartdto!: ShoppingCartDTO;
  cartProductDTO!: CartProductDTO;
  carts: CartModel[] = [];
  priceDiscount: number = 0;
  priceShip = 30000;
  paymentsType: any;
  Environment = Environment;
  trxTransactionDTO!: TrxTransactionDTO;
  titleStatus: string = '';
  isDisplayNone: boolean = false;
  trxTransacForm = new FormGroup({
    fullName: new FormControl('', [
      Validators.required,
      Validators.maxLength(50),
    ]),
    phoneNumber: new FormControl('', [
      Validators.required,
      Validators.maxLength(10),
      Validators.minLength(10),
      Validators.pattern('^0[0-9]{9}$'),
    ]),
    address: new FormControl('', [
      Validators.required,
      Validators.maxLength(100),
    ]),
    paymentTypes: new FormControl('', [Validators.required]),
  });

  constructor(
    private cartService: TrxTransactionService,
    private toastr: ToastrService,
    private tokenService: TokenService,
    private activatedRoute: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    localStorage.setItem(Utils.cartBuyNow, JSON.stringify(this.carts));
  }

  getInitData() {}
  getTotalPrice(): number {
    let totalPrice = 0; // Khởi tạo biến tổng giá tiền
    console.log('id', this.shoppingcartdto);
    // Kiểm tra xem this.shoppingcartdto đã được xác định và có giá trị không
    if (this.shoppingcartdto.cart === null) {
      // nếu shoppingcartdto.cart không tồn tại thì lấy dữ liệu từ shoppingcartdto.transactionDetail
      this.shoppingcartdto.transactionDetail.forEach((item) => {
        totalPrice += item.quantity * item.price;
      });
    }

    // Nếu shoppingcartdto hoặc shoppingcartdto.cart không tồn tại, hoặc có giá trị,
    // thì trả về 0 hoặc một giá trị mặc định khác tùy thuộc vào yêu cầu của bạn.
    // if (!this.shoppingcartdto || !this.shoppingcartdto.cart) {
    //   return this.shoppingcartdto.transactionDetail.reduce((acc, item) => {
    //     return acc + item.total * item.price;
    //   }, 0);
    // }

    // Nếu shoppingcartdto.cart tồn tại, duyệt qua từng sản phẩm trong giỏ hàng và tính tổng giá tiền
    // Nếu shoppingcartdto.cart tồn tại, duyệt qua từng sản phẩm trong giỏ hàng và tính tổng giá tiền
    if (this.shoppingcartdto.cart) {
      this.shoppingcartdto.cart.forEach((item) => {
        totalPrice += item.price * item.quantity; // Tính tổng giá tiền
      });
    }
    totalPrice = totalPrice - this.priceDiscount + this.priceShip; // Trừ giảm giá và cộng phí ship

    return totalPrice;
  }

  submit() {
    // nếu url có id thì set giá trị cho trxtransactionId và không có thì set = 0
    this.cartService.PaymentExecute(this.trxTransactionDTO).subscribe({
      next: (response: any) => {
        if (
          response.data &&
          typeof response.data === 'string' &&
          (response.data.startsWith('http://') ||
            response.data.startsWith('https://'))
        ) {
          window.location.href = response.data;
          this.cartService.removeCarts();
          // this.titleStatus='Đã thanh toán';
        } else {
          this.toastr.success(
            'Đặt hàng thành công vui lòng đợi phê duyệt!',
            'Thông báo'
          );
          this.isDisplayNone = false;
          this.router.navigate(['/']);
          // Handle other cases, such as routing to another page
          // For example, you can use this.router.navigate([some_other_route]);
        }
      },
      error: (error: any) => {
        this.toastr.error('Lỗi đặt hàng', 'Thông báo');
      },
    });
  }
}
