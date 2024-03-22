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

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrl: './checkout.component.css',
})
export class CheckoutComponent implements OnInit {
  shoppingcartdto!: ShoppingCartDTO;
  Environment = Environment;
  trxTransactionDTO!: TrxTransactionDTO;
  titleStatus: string = '';
  isDisplayNone: boolean = false;
  trxTransacForm = new FormGroup({
    userId: new FormControl(''),
    fullName: new FormControl('', [
      Validators.required,
      Validators.maxLength(100),
    ]),
    phoneNumber: new FormControl('', [
      Validators.required,
      Validators.maxLength(10),
      Validators.minLength(10),
      Validators.pattern('^0[0-9]{9}$'),
    ]),
    address: new FormControl('', [Validators.required]),
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
  }

  getInitDataId(id: number) {
    this.cartService.GetTrxTransactionFindById(id).subscribe({
      next: (response: any) => {
        this.shoppingcartdto = response.data;
        this.trxTransacForm.setValue({
          userId: this.tokenService.getUserEmail(),
          phoneNumber: this.shoppingcartdto.trxTransaction.phoneNumber,
          fullName: this.shoppingcartdto.trxTransaction.fullName,
          address: this.shoppingcartdto.trxTransaction.address,
        });
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
    this.cartService
      .GetTrxTransaction(this.tokenService.getUserEmail())
      .subscribe({
        next: (response: any) => {
          this.shoppingcartdto = response.data;

          this.trxTransacForm.setValue({
            userId: this.tokenService.getUserEmail(),
            phoneNumber: this.shoppingcartdto.trxTransaction.phoneNumber,
            fullName: this.shoppingcartdto.trxTransaction.fullName,
            address: this.shoppingcartdto.trxTransaction.address,
          });
          this.isDisplayNone = true;

          this.titleStatus = 'Tiến hành thanh toán';
        },
        error: (error: any) => {
          this.toastr.error('Lỗi lấy thông tin giỏ hàng', 'Thông báo');
        },
      });
  }
  getTotalPrice(): number {
    // debugger;
    // Kiểm tra xem this.shoppingcartdto đã được xác định và có giá trị không
    if (this.shoppingcartdto && this.shoppingcartdto.cart === undefined) {
      // nếu shoppingcartdto.cart không tồn tại thì lấy dữ liệu từ shoppingcartdto.transactionDetail
      return this.shoppingcartdto.transactionDetail.reduce((acc, item) => {
        return acc + item.total * item.price;
      }, 0);
    }

    // Nếu shoppingcartdto hoặc shoppingcartdto.cart không tồn tại, hoặc có giá trị,
    // thì trả về 0 hoặc một giá trị mặc định khác tùy thuộc vào yêu cầu của bạn.
    if (!this.shoppingcartdto || !this.shoppingcartdto.cart) {
      return this.shoppingcartdto.transactionDetail.reduce((acc, item) => {
        return acc + item.total * item.price;
      }, 0);
    }

    // Nếu shoppingcartdto và shoppingcartdto.cart đều tồn tại và có giá trị,
    // thực hiện tính toán tổng giá trị của các mặt hàng trong giỏ hàng.
    return this.shoppingcartdto.cart.reduce((acc, item) => {
      return acc + item.quantity * item.price;
    }, 0);
  }

  submit() {
    trxtransactionId: Number;
    // nếu url có id thì set giá trị cho trxtransactionId và không có thì set = 0
    if (this.activatedRoute.snapshot.params['id'] === undefined) {
      this.trxTransactionDTO = new TrxTransactionDTO(
        0,
        this.tokenService.getUserEmail(),
        this.trxTransacForm.value?.fullName || '',
        this.trxTransacForm.value?.phoneNumber || '',
        this.trxTransacForm.value?.address || '',
        this.getTotalPrice()
      );
    } else {
      this.trxTransactionDTO = new TrxTransactionDTO(
        this.activatedRoute.snapshot.params['id'],
        this.tokenService.getUserEmail(),
        this.trxTransacForm.value?.fullName || '',
        this.trxTransacForm.value?.phoneNumber || '',
        this.trxTransacForm.value?.address || '',
        this.getTotalPrice()
      );
    }
    this.cartService.PaymentExecute(this.trxTransactionDTO).subscribe({
      next: (response: any) => {
        console.log('response', response.data);
        //chuyển trang sau khi đặt hàng thành công
        window.location.href = response.data;
        this.toastr.success('Đặt hàng thành công', 'Thông báo');
        // this.router.navigate(['/client/home']);
      },
      error: (error: any) => {
        this.toastr.error('Lỗi đặt hàng', 'Thông báo');
      },
    });
  }
}
