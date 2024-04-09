import { CartModel } from './../../../models/cart.model';
import { Environment } from './../../../environment/environment';
import { Component, OnInit } from '@angular/core';
import { ClientFooterComponent } from '../client-footer/client-footer.component';
import { ClientMenuComponent } from '../client-menu/client-menu.component';
import { ClientHeaderComponent } from '../client-header/client-header.component';
import { TrxTransactionService } from '../../../service/trx-transaction.service';
import { ToastrService } from 'ngx-toastr';
import { TokenService } from '../../../service/token.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ShoppingCartDTO } from '../../../dtos/shopping.cart.dto.';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { TrxTransactionDTO } from '../../../dtos/trxtransaction.dto';
import { CartProductDTO } from '../../../dtos/cart.product.dto';
import { Utils } from '../../../Utils.ts/utils';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrl: './checkout.component.css',
})
export class CheckoutComponent implements OnInit {
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
    userId: new FormControl(''),
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
    if (this.activatedRoute.snapshot.params['id'] === undefined) {
      this.getInitData();
    } else {
      this.getInitDataId(this.activatedRoute.snapshot.params['id']);
    }
    this.paymentsType = Utils.paymentTypes;
  }

  getInitDataId(id: number) {
    this.cartService.GetTrxTransactionFindById(id).subscribe({
      next: (response: any) => {
        console.log('dataPay', response.data);
        this.shoppingcartdto = response.data;
        this.trxTransacForm.setValue({
          userId: this.tokenService.getUserEmail(),
          phoneNumber: this.shoppingcartdto.trxTransaction.phoneNumber,
          fullName: this.shoppingcartdto.trxTransaction.fullName,
          address: this.shoppingcartdto.trxTransaction.address,
          paymentTypes: this.shoppingcartdto.trxTransaction.paymentTypes,
        });
        console.log('object11', this.trxTransacForm.value);
        if (
          this.shoppingcartdto &&
          this.shoppingcartdto.trxTransaction.paymentStatus === 'Approved'
        ) {
          this.titleStatus = 'Đã thanh toán';
        }
        if (
          this.shoppingcartdto &&
          this.shoppingcartdto.trxTransaction.paymentStatus === 'Pending'
        ) {
          this.isDisplayNone = true;
          this.titleStatus = 'Thanh toán không thành công vui lòng thử lại';
        }
        console.log('id', this.shoppingcartdto);
      },
      error: (error: any) => {
        this.toastr.error('Lỗi lấy thông tin giỏ hàng', 'Thông báo');
      },
    });
  }

  getInitData() {
    this.cartProductDTO = new CartProductDTO(
      this.tokenService.getUserEmail(),
      this.cartService.getCarts()
    );

    this.cartService.GetTrxTransaction(this.cartProductDTO).subscribe({
      next: (response: any) => {
        console.log('responsecheck', response.data);
        this.shoppingcartdto = response.data;

        this.trxTransacForm.setValue({
          userId: this.tokenService.getUserEmail(),
          phoneNumber: this.shoppingcartdto.trxTransaction.phoneNumber,
          fullName: this.shoppingcartdto.trxTransaction.fullName,
          address: this.shoppingcartdto.trxTransaction.address,
          paymentTypes: this.shoppingcartdto.trxTransaction.paymentTypes,
        });
        // tính ra số tiền giảm giá của sản phẩm trong giỏ hàng bằng khi discount > 0 => sản phẩm cần tính giảm giá => số tiền giảm giá sản phẩm đó
        // duyệt qua từng sản phẩm trong giỏ hàng
        this.shoppingcartdto.cart.forEach((item) => {
          // nếu discount > 0 thì tính ra số tiền giảm giá của sản phẩm đó
          if (item.discount > 0) {
            this.priceDiscount +=
              item.price * item.quantity * (item.discount / 100);
          }
        });
        this.isDisplayNone = true;
        this.titleStatus = 'Đặt hàng';
      },
      error: (error: any) => {
        this.toastr.error('Lỗi lấy thông tin giỏ hàng', 'Thông báo');
      },
    });
  }
  getTotalPrice(): number {
    let totalPrice = 0; // Khởi tạo biến tổng giá tiền
    console.log('id', this.shoppingcartdto);
    // Kiểm tra xem this.shoppingcartdto đã được xác định và có giá trị không
    if (this.shoppingcartdto.cart === null) {
      // nếu shoppingcartdto.cart không tồn tại thì lấy dữ liệu từ shoppingcartdto.transactionDetail
      this.shoppingcartdto.transactionDetail.forEach((item) => {
        totalPrice += item.total * item.price;
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
    if (this.activatedRoute.snapshot.params['id'] === undefined) {
      this.trxTransactionDTO = new TrxTransactionDTO(
        0,
        this.tokenService.getUserEmail(),
        this.trxTransacForm.value?.fullName || '',
        this.trxTransacForm.value?.phoneNumber || '',
        this.trxTransacForm.value?.address || '',
        this.getTotalPrice(),
        this.trxTransacForm.value?.paymentTypes || '',
        this.shoppingcartdto.cart
      );
    }

    this.cartService.PaymentExecute(this.trxTransactionDTO).subscribe({
      next: (response: any) => {
        debugger;
        if (
          response.data &&
          typeof response.data === 'string' &&
          (response.data.startsWith('http://') ||
            response.data.startsWith('https://'))
        ) {
          window.location.href = response.data;
          this.cartService.removeCarts();
          this.router.navigate(['/login']);
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
