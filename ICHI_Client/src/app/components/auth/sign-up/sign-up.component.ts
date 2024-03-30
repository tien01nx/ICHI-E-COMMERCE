import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../../service/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Utils } from '../../../Utils.ts/utils';
import { TokenService } from '../../../service/token.service';
import { ApiResponse } from '../../../models/api.response.model';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrl: './sign-up.component.css',
})
export class SignUpComponent implements OnInit {
  errorMessage: string = '';
  successMessage: string = '';
  isActive: boolean = false;
  birthday: Date = new Date();
  ngOnInit(): void {}
  constructor(
    private authServer: AuthService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private toastr: ToastrService,
    private tokenService: TokenService
  ) {}

  userForm: FormGroup = new FormGroup({
    email: new FormControl('', [
      Validators.required,
      Validators.maxLength(100),
      Validators.email,
    ]),
    password: new FormControl('', [
      Validators.required,
      Validators.minLength(8),
      Validators.maxLength(40),
      Validators.pattern(Utils.passwordPattern),
    ]),
    confirmPassword: new FormControl('', [Validators.required]),
    fullname: new FormControl('', [
      Validators.required,
      Validators.maxLength(40),
      Validators.pattern(Utils.textPattern),
    ]),
    phoneNumber: new FormControl('', [
      Validators.required,
      Validators.maxLength(10),
      Validators.minLength(10),
      Validators.pattern(Utils.phoneNumberPattern),
    ]),
    birthday: new FormControl('', [Validators.required]),
    gender: new FormControl('', [Validators.required]),
  });

  userRegister() {
    if (this.userForm.value.password !== this.userForm.value.confirmPassword) {
      this.errorMessage = 'Mật khẩu và xác nhận mật khẩu không khớp.';
      return;
    }
    this.authServer.register(this.userForm.value).subscribe({
      next: (response: ApiResponse<string>) => {
        debugger;
        if (response.message === 'Đăng ký tài khoản thành công') {
          this.userForm.reset();
          this.errorMessage = '';
          this.toastr.success(response.message);
          this.tokenService.setToken(response.data);
          this.router.navigate(['/']);
        } else {
          this.toastr.error(response.message);
        }
      },
      error: (error: any) => {
        this.errorMessage = error.error;
        // this.isDisplayNone = false;
      },
    });
  }

  signUp() {
    this.router.navigate(['/register']);
  }

  handleTermsChange(event: any) {
    this.isActive = event.target.checked;
  }
}
