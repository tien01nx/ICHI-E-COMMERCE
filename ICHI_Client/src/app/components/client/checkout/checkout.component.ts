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
    // if (this.activatedRoute.snapshot.params['id'] === undefined) {
    // } else {
    //   this.findProductById(this.activatedRoute.snapshot.params['id']);
    // }
    this.getInitData();
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
          console.log(this.shoppingcartdto);
          console.log('trxForm', this.trxTransacForm.value);
        },
        error: (error: any) => {
          this.toastr.error('Lỗi lấy thông tin giỏ hàng', 'Thông báo');
        },
      });
  }
  getTotalPrice(): number {
    return this.shoppingcartdto.cart.reduce((acc, item) => {
      return acc + item.quantity * item.price;
    }, 0);
  }
  submit() {
    const trxTransactionDTO = new TrxTransactionDTO(
      0,
      this.tokenService.getUserEmail(),
      this.trxTransacForm.value?.fullName || '',
      this.trxTransacForm.value?.phoneNumber || '',
      this.trxTransacForm.value?.address || '',
      this.getTotalPrice()
    );
    this.cartService.PaymentExecute(trxTransactionDTO).subscribe({
      next: (response: any) => {
        debugger;
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
