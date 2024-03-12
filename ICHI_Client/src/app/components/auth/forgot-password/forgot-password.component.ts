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
  standalone: true,
  imports: [FormsModule, ReactiveFormsModule, CommonModule],
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
    email: new FormControl('', [
      Validators.required,
      Validators.maxLength(40),
      Validators.email,
    ]),
  });

  userLogin() {
    this.authServer.forgotPassword(this.userForm.value.email).subscribe({
      next: (response: any) => {
        if (response.code === 200) {
          this.userForm.reset();
          this.toastr.success(response.message, 'Thông báo');
        } else {
          this.errorMessage = response.message;
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
