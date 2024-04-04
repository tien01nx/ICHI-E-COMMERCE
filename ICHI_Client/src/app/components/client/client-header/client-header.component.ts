import { Component } from '@angular/core';
import { TokenService } from '../../../service/token.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { TrxTransactionService } from '../../../service/trx-transaction.service';

@Component({
  selector: 'app-client-header',
  templateUrl: './client-header.component.html',
  styleUrl: './client-header.component.css',
})
export class ClientHeaderComponent {
  cartItemCount: number = 0;
  email = this.tokenService.getUserEmail();
  constructor(
    private tokenService: TokenService,
    private router: Router,
    private toastr: ToastrService,
    private cartService: TrxTransactionService
  ) {}

  ngOnInit(): void {
    this.cartService
      .getCartItemCount(this.tokenService.getUserEmail())
      .subscribe((count) => {
        this.cartItemCount = count;
      });
    console.log('object111', this.email);
  }

  signout() {
    this.tokenService.removeToken();
    // this.router.navigate(['/']);
    window.location.href = '/';
    this.toastr.success('Đăng xuất thành công');
  }

  login() {
    this.router.navigate(['login']);
  }

  resetPassword() {
    this.router.navigate(['reset-password']);
  }
}
