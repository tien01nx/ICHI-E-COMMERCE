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

@Component({
  selector: 'app-sign-up',
  standalone: true,
  imports: [FormsModule, ReactiveFormsModule, CommonModule],
  templateUrl: './sign-up.component.html',
  styleUrl: './sign-up.component.css',
})
export class SignUpComponent implements OnInit {
  errorMessage: string = '';
  successMessage: string = '';
  isActive: boolean = false;
  birthday: Date = new Date();
  passwordPattern =
    /^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!@#$%^&*])[A-Za-z\d!@#$%^&*]{8,}$/;
  ngOnInit(): void {}
  constructor(
    private authServer: AuthService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private toastr: ToastrService
  ) {}

  userForm: FormGroup = new FormGroup({
    email: new FormControl('', [
      Validators.required,
      Validators.maxLength(100),
      Validators.email,
    ]),
    password: new FormControl('', [
      Validators.required,
      Validators.maxLength(100),
      Validators.pattern(this.passwordPattern),
    ]),
    confirmPassword: new FormControl('', [Validators.required]),
    fullname: new FormControl('', [
      Validators.required,
      Validators.maxLength(100),
    ]),
    phoneNumber: new FormControl('', [
      Validators.required,
      Validators.maxLength(10),
      Validators.minLength(10),
      Validators.pattern('^0[0-9]{9}$'),
    ]),
    birthday: new FormControl('', [Validators.required]),
    gender: new FormControl('', [Validators.required]),
  });

  userRegister() {
    debugger;
    if (this.userForm.value.password !== this.userForm.value.confirmPassword) {
      this.errorMessage = 'Mật khẩu và xác nhận mật khẩu không khớp.';
      return;
    }
    this.authServer.register(this.userForm.value).subscribe({
      next: (response: any) => {
        if (response.code === 200) {
          this.userForm.reset();
          this.errorMessage = '';

          this.successMessage = 'Đăng ký thành công';
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

  handleTermsChange(event: any) {
    this.isActive = event.target.checked;
  }
}
