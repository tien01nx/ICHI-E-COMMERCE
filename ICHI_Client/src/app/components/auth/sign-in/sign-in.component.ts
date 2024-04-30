import { Component, OnInit } from '@angular/core';
import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../../../service/auth.service';
import { TokenService } from '../../../service/token.service';
import { ApiResponse } from '../../../models/api.response.model';
import { Utils } from '../../../Utils.ts/utils';

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',
  styleUrl: './sign-in.component.css',
})
export class SignInComponent implements OnInit {
  errorMessage: string = '';
  protected readonly Utils = Utils;
  ngOnInit(): void {
    throw new Error('Method not implemented.');
  }
  constructor(
    private authServer: AuthService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private toastr: ToastrService,
    private tokenService: TokenService
  ) {}

  userForm: FormGroup = new FormGroup({
    userName: new FormControl('', [
      Validators.required,
      Validators.maxLength(100),
      Validators.pattern(Utils.checkEmail),
    ]),
    password: new FormControl('', [
      Validators.required,
      Validators.maxLength(40),
    ]),
  });

  userLogin() {
    this.authServer.login(this.userForm.value).subscribe({
      next: (response: ApiResponse<string>) => {
        if (response.code === 200) {
          // Đăng nhập thành công
          if (response.message === 'Đăng nhập thành công') {
            this.tokenService.setToken(response.data);
            let roles = this.tokenService.getUserRoles();

            if (!Array.isArray(roles)) {
              roles = [roles];
            }

            const userId = this.tokenService.getUserEmail();

            const requiredRole = ['ADMIN', 'EMPLOYEE'];
            const hasRequiredRole = roles.some((role: any) =>
              requiredRole.includes(role)
            );
            if (hasRequiredRole) {
              // xóa lịch sử trước đó
              this.router.navigate(['/admin'], { replaceUrl: true });
            } else {
              // window.location.href = '/';
              this.router.navigate(['/'], { replaceUrl: true });
            }
            this.userForm.reset();
            // this.router.navigate(['/']);
            this.toastr.success(response.message, 'Thông báo');
          } else {
            this.errorMessage = response.message;
          }
        } else {
          this.errorMessage = response.message;
          // this.isDisplayNone = false;
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
}
