import { routes } from './../../../app.routes';
import { Component } from '@angular/core';
import { AuthService } from '../../../service/auth.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { TokenService } from '../../../service/token.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { Utils } from '../../../Utils.ts/utils';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrl: './reset-password.component.css',
})
export class ResetPasswordComponent {
  strMessage: string = '';
  passwordsNotMatching: boolean = false;

  userForm: FormGroup = new FormGroup({
    userName: new FormControl(''),
    oldPassword: new FormControl('', [Validators.required]),
    password: new FormControl('', [
      Validators.required,
      Validators.minLength(8),
      Validators.maxLength(40),
      Validators.pattern(Utils.passwordPattern),
    ]),
    newPassword: new FormControl('', [Validators.required]),
  });

  constructor(
    private authService: AuthService,
    private tokenService: TokenService,
    private toastr: ToastrService,
    private router: Router
  ) {}

  checkPasswordMatch() {
    const password = this.userForm.get('password')?.value;
    const confirmPassword = this.userForm.get('confirmPassword')?.value;

    if (password !== confirmPassword) {
      this.passwordsNotMatching = true;
    } else {
      this.passwordsNotMatching = false;
    }
  }
  changePassword() {
    // set username from token

    if (
      this.userForm.controls['password'].value !==
      this.userForm.controls['newPassword'].value
    ) {
      this.strMessage = 'Mật khẩu xác nhận không khớp';
      return;
    }

    this.userForm.controls['userName'].setValue(
      this.tokenService.getUserEmail()
    );
    console.log(this.userForm.value);
    this.authService.changePassword(this.userForm.value).subscribe(
      (res: any) => {
        debugger;
        if (res.message === 'Đổi mật khẩu thành công') {
          // xóa token đã lưu trên local storage
          this.tokenService.removeToken();
          this.router.navigate(['/login']);
          this.toastr.success('Đổi mật khẩu thành công', 'Thông báo');
          return;
        }
        this.toastr.error('Mật khẩu cũ không đúng', 'Thông báo');
        return;
      },
      (error) => {
        alert('Change password failed');
      }
    );
  }
}
