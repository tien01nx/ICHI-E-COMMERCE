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
  protected readonly Utils = Utils;
  errorMessage: string = '';
  successMessage: string = '';
  isActive: boolean = false;
  birthday: Date = new Date();
  passwordsNotMatching: boolean = false;
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
      Validators.pattern(Utils.checkEmail),
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
      Validators.minLength(6),
      Validators.maxLength(40),
      Validators.pattern(Utils.textPattern),
    ]),

    phoneNumber: new FormControl('', [
      Validators.required,
      Validators.maxLength(10),
      Validators.minLength(10),
      Validators.pattern(Utils.textPhoneNumber),
    ]),
    birthday: new FormControl('', [Validators.required]),
    gender: new FormControl('', [Validators.required]),
  });

  // onKeyPress(event: KeyboardEvent) {
  //   const inputValue = event.key.trim();

  //   // Kiểm tra nếu ký tự nhập vào không phù hợp với biểu thức chính quy
  //   if (
  //     !/^[a-zA-Z0-9_ÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂưăạảấầẩẫậắằẳẵặẹẻẽềềểỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễếệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵỷỹ]$/u.test(
  //       inputValue
  //     )
  //   ) {
  //     event.preventDefault(); // Ngăn chặn sự kiện mặc định của phím được nhập
  //   }
  // }

  onKeyPress(event: KeyboardEvent) {
    const inputValue = event.key.trim();
    const regex =
      /^[a-zA-Z_ÀÁÂÃÈÉÊẾÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêếìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂưăạảấầẩẫậắằẳẵặẹẻẽềềểỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵỷỹ\s]+$/;
    if (!regex.test(inputValue)) {
      event.preventDefault(); // Ngăn chặn sự kiện mặc định của phím được nhập
    }
  }

  checkPasswordMatch() {
    const password = this.userForm.get('password')?.value;
    const confirmPassword = this.userForm.get('confirmPassword')?.value;

    if (password !== confirmPassword) {
      this.passwordsNotMatching = true;
    } else {
      this.passwordsNotMatching = false;
    }
  }

  userRegister() {
    if (this.userForm.value.password !== this.userForm.value.confirmPassword) {
      this.errorMessage = 'Mật khẩu và xác nhận mật khẩu không khớp.';
      return;
    }
    this.authServer.register(this.userForm.value).subscribe({
      next: (response: ApiResponse<string>) => {
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
