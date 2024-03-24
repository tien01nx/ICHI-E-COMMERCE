import { routes } from './../../../app.routes';
import { Component } from '@angular/core';
import { AuthService } from '../../../service/auth.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { TokenService } from '../../../service/token.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrl: './reset-password.component.css',
})
export class ResetPasswordComponent {
  passwordPattern =
    /^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!@#$%^&*])[A-Za-z\d!@#$%^&*]{8,}$/;
  userForm: FormGroup = new FormGroup({
    userName: new FormControl(''),
    oldPassword: new FormControl('', [Validators.required]),
    password: new FormControl('', [
      Validators.required,
      Validators.maxLength(100),
      Validators.pattern(this.passwordPattern),
    ]),
    newPassword: new FormControl('', [Validators.required]),
  });

  constructor(
    private authService: AuthService,
    private tokenService: TokenService,
    private toastr: ToastrService,
    private router: Router
  ) {}
  changePassword() {
    // set username from token

    this.userForm.controls['userName'].setValue(
      this.tokenService.getUserEmail()
    );
    console.log(this.userForm.value);
    this.authService.changePassword(this.userForm.value).subscribe(
      (res: any) => {
        if (res.message === 'Mật khẩu cũ không đúng') {
          this.toastr.error('Mật khẩu cũ không đúng', 'Thông báo');
          return;
        }
        // chuyeenr url ddeens trang /login
        this.router.navigate(['/']);
        this.toastr.success('Thay đổi mật khẩu thành công', 'Thông báo');
      },
      (error) => {
        alert('Change password failed');
      }
    );
  }
}
