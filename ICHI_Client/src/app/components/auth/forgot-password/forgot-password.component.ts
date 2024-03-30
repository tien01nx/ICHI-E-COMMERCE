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

@Component({
  selector: 'app-forget-password',
  templateUrl: './forgot-password.component.html',
  styleUrl: './forgot-password.component.css',
})
export class ForgotPasswordComponent {
  errorMessage: string = '';

  constructor(
    private authServer: AuthService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private toastr: ToastrService
  ) {}

  userForm: FormGroup = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email]),
  });

  userLogin() {
    this.authServer.forgotPassword(this.userForm.value.email).subscribe({
      next: (response: any) => {
        if (response.code === 200) {
          this.userForm.reset();
          debugger; // tslint:disable-line
          console.log(response.message);

          // nếu message trả về là "Gửi email thành công" thì hiển thị thông báo thành công
          if (response.message === 'Gửi email thành công') {
            this.router.navigate(['/login']);
            this.toastr.success(response.message, 'Thông báo');
          } else {
            this.toastr.error(response.message, 'Thông báo');
          }
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
  login() {
    this.router.navigate(['/login']);
  }
}
