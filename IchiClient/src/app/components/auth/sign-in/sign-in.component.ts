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
  selector: 'app-sign-in',
  standalone: true,
  imports: [FormsModule, ReactiveFormsModule, CommonModule],
  templateUrl: './sign-in.component.html',
  styleUrl: './sign-in.component.css',
})
export class SignInComponent implements OnInit {
  errorMessage: string = '';

  ngOnInit(): void {
    throw new Error('Method not implemented.');
  }
  constructor(
    private authServer: AuthService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private toastr: ToastrService
  ) {}

  userForm: FormGroup = new FormGroup({
    userName: new FormControl('', [
      Validators.required,
      Validators.maxLength(100),
    ]),
    password: new FormControl('', [
      Validators.required,
      Validators.maxLength(100),
    ]),
  });

  userLogin() {
    this.authServer.login(this.userForm.value).subscribe({
      next: (response: any) => {
        if (response.code === 200) {
          this.userForm.reset();
          // this.toastr.success('Đăng nhập thành công', 'Thông báo');
          this.router.navigate(['/']);
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
