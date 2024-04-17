import { Component, ElementRef, ViewChild } from '@angular/core';
import { CustomerModel } from '../../../models/customer.model';
import { PaginationDTO } from '../../../dtos/pagination.dto';
import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Utils } from '../../../Utils.ts/utils';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import Swal from 'sweetalert2';
import { CommonModule } from '@angular/common';
import { Environment } from '../../../environment/environment';
import { EmployeeService } from '../../../service/employee.service';
import { UserService } from '../../../service/user.service';
import { UserDTO } from '../../../dtos/user.dto';
import { UpdateUserDTO } from '../../../dtos/update.user.dto';
import { AuthService } from '../../../service/auth.service';
import { EmployeeModel } from '../../../models/employee.model';

@Component({
  selector: 'app-customer',
  templateUrl: './employee.component.html',
  styleUrl: './employee.component.css',
})
export class EmployeeComponent {
  protected readonly Utils = Utils;
  protected readonly Environment = Environment;
  paginationModel: PaginationDTO<CustomerModel> = PaginationDTO.createEmpty();
  searchTemp: any = this.activatedRoute.snapshot.queryParams['Search'] || '';
  selectAll: boolean = false;
  sortDir: string = 'ASC';
  SortBy: string = 'id';
  file: File | null = null;
  avatarSrc: string = '';
  showPassword: boolean = true;
  birthday: Date = new Date();
  @ViewChild('btnCloseModal') btnCloseModal!: ElementRef;
  titleModal: string = '';
  btnSave: string = '';
  isDisplayNone: boolean = false;
  errorMessage: string = '';
  employeeForm: FormGroup = new FormGroup({
    id: new FormControl(null),
    fullName: new FormControl('', [
      Validators.required,
      Validators.minLength(6),
      Validators.maxLength(50),
      Validators.pattern(Utils.textPattern),
    ]),
    phoneNumber: new FormControl('', [
      Validators.required,
      Validators.maxLength(10),
      Validators.minLength(10),
      Validators.pattern(Utils.textPhoneNumber),
    ]),
    gender: new FormControl('', [Validators.required]),
    birthday: new FormControl('', [Validators.required]),
    email: new FormControl('', [
      Validators.required,
      Validators.pattern(Utils.checkEmail),
    ]),
    address: new FormControl('', [
      Validators.required,
      Validators.maxLength(100),
    ]),
    userId: new FormControl(null),
    password: new FormControl('', [
      Validators.minLength(6),
      Validators.required,
    ]),
  });

  constructor(
    private title: Title,
    private employeeService: EmployeeService,
    private activatedRoute: ActivatedRoute,
    private authService: AuthService,
    private userServide: UserService,
    private router: Router,
    private toastr: ToastrService
  ) {}
  ngOnInit() {
    this.title.setTitle('Quản lý thông tin nhân viên');
    this.activatedRoute.queryParams.subscribe((params) => {
      const search = params['search'] || '';
      const pageSize = +params['page-size'] || 10;
      const pageNumber = +params['page-number'] || 1;
      const sortDir = params['sort-direction'] || 'DESC';
      const sortBy = params['sort-by'] || 'CreateDate';
      this.findAll(pageSize, pageNumber, sortBy, sortDir, search);
    });
  }

  // avatarSrc: string =
  //   'https://localhost:7150//images//products//product-47//5b986978-35b8-4c3f-b6cf-b989d816f3f3.png';
  // onFileSelected(event: any) {
  //   const file = event.target.files[0];
  //   if (file) {
  //     const reader = new FileReader();
  //     reader.onload = (e: any) => {
  //       this.avatarSrc = e.target.result;
  //       console.log(this.avatarSrc);
  //     };
  //     reader.readAsDataURL(file);
  //   }
  // }
  onFileSelected(event: any) {
    this.file = event.target.files[0];
    if (this.file) {
      const reader = new FileReader();
      reader.onload = (e: any) => {
        this.avatarSrc = e.target.result;
      };
      reader.readAsDataURL(this.file);
    }
  }

  update() {
    debugger;
    this.isDisplayNone = true;
    this.employeeService
      .UpdateImage(this.employeeForm.value, this.file)
      .subscribe({
        next: (response: any) => {
          if (response.message === 'Cập nhật nhân viên thành công') {
            this.employeeForm.reset();
            this.btnCloseModal.nativeElement.click();
            this.updateTable();
            this.toastr.success(response.message, 'Thông báo');
          }

          this.toastr.error(response.message, 'Thông báo');
        },
        error: (error: any) => {
          this.errorMessage = error.error;
          this.isDisplayNone = false;
        },
      });
  }

  toggleSelectAll() {
    this.selectAll = !this.selectAll;
  }

  findAll(
    pageSize: number,
    pageNumber: number,
    sortBy: string,
    sortDir: string,
    search: string
  ) {
    this.employeeService
      .findAllByName(pageNumber, pageSize, sortDir, sortBy, search)
      .subscribe({
        next: (response: any) => {
          console.log(response);
          this.paginationModel.content = response.data.items;
          this.paginationModel.totalPages = response.data.pageCount;
          this.paginationModel.totalElements = response.data.totalCount;
          this.paginationModel.numberOfElements = response.numberOfElements;
          this.paginationModel.pageSize = response.data.pageSize;
          this.paginationModel.pageNumber = response.data.currentPage;
          this.paginationModel.firstElementOnPage = response.firstElementOnPage;
          this.paginationModel.lastElementOnPage = response.lastElementOnPage;
          this.paginationModel.sortBy = response.sortBy;
          this.paginationModel.sortDirection = response.sortDirection;
          console.log(this.paginationModel);
        },
        error: (error: any) => {
          console.log(error);
        },
      });
  }

  changePageNumber(pageNumber: number): void {
    this.router
      .navigate(['/admin/employee'], {
        queryParams: { 'page-number': pageNumber },
        queryParamsHandling: 'merge',
      })
      .then((r) => {});
  }

  changePageSize(pageSize: number): void {
    this.router
      .navigate(['/admin/employee'], {
        queryParams: { 'page-size': pageSize, 'page-number': 1 },
        queryParamsHandling: 'merge',
      })
      .then((r) => {});
  }

  sortByField(sortBy: string): void {
    this.router
      .navigate(['/admin/employee'], {
        queryParams: { 'sort-by': sortBy, 'sort-direction': this.sortDir },
        queryParamsHandling: 'merge',
      })
      .then((r) => {});
    this.sortDir = this.sortDir === 'ASC' ? 'DESC' : 'ASC';
    this.SortBy = sortBy;
  }

  search() {
    this.router
      .navigate(['/admin/employee'], {
        queryParams: { search: this.searchTemp, 'page-number': 1 },
        queryParamsHandling: 'merge',
      })
      .then((r) => {});
  }

  getGenderValue(): boolean {
    const value = this.employeeForm.value('gender').value;
    const isMale = value === '1';

    return isMale;
  }
  onSubmit() {
    debugger;
    if (this.employeeForm.invalid) {
      return;
    }
    if (this.employeeForm.value.id === null) {
      // this.create();
      this.createEmployee();
      return;
    } else this.update();
  }

  // create() {
  //   this.isDisplayNone = true;
  //   this.employeeForm.value.id = 0;
  //   this.employeeService.create(this.employeeForm.value).subscribe({
  //     next: (response: any) => {
  //       if (response.code === 200) {
  //         this.employeeForm.reset();
  //         this.btnCloseModal.nativeElement.click();
  //         this.updateTable();
  //         this.toastr.success(response.message, 'Thông báo');
  //       } else {
  //         this.errorMessage = response.message;
  //         this.isDisplayNone = false;
  //       }
  //     },
  //     error: (error: any) => {
  //       this.errorMessage = error.error;
  //       this.isDisplayNone = false;
  //     },
  //   });
  // }

  delete(id: number) {
    Swal.fire({
      title: 'Bạn có chắc chắn muốn xóa?',
      text: 'Dữ liệu sẽ không thể phục hồi sau khi xóa!',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Xác nhận',
      cancelButtonText: 'Hủy',
      buttonsStyling: false,
      customClass: {
        confirmButton: 'btn btn-danger me-1',
        cancelButton: 'btn btn-secondary',
      },
    }).then((result) => {
      if (result.isConfirmed) {
        this.employeeService.delete(id).subscribe({
          next: (response: any) => {
            debugger;
            if (response.message === 'Mở khóa tài khoản thành công') {
              this.updateTable();
              this.toastr.success(response.message, 'Thông báo');
            } else if (response.message === 'Khóa tài khoản thành công') {
              this.updateTable();
              this.toastr.error(response.message, 'Thông báo');
            } else {
              this.toastr.error(response.message, 'Thông báo');
            }
          },
          error: (error: any) => {
            this.toastr.error(error.error, 'Thất bại');
          },
        });
      }
    });
  }

  openModalCreate() {
    this.employeeForm.reset();
    this.titleModal = 'Thêm nhân viên';
    this.btnSave = 'Thêm mới';
    this.errorMessage = '';
    this.showPassword = true;
  }

  // openModalUpdate(employee: CustomerModel) {
  //   this.showPassword = false;
  //   this.employeeForm.patchValue({
  //     id: customer.id,
  //     fullName: customer.fullName,
  //     phoneNumber: customer.phoneNumber,
  //     gender: customer.gender,
  //     birthday: customer.birthday,
  //     userId: customer.userId,
  //     email: customer.user.email,
  //     address: customer.address,
  //   });
  //   this.avatarSrc = Environment.apiBaseRoot + customer.user.avatar;
  //   this.birthday = customer.birthday;
  //   this.titleModal = 'Cập nhật khách hàng';
  //   this.btnSave = 'Cập nhật';
  // }

  openModalUpdate(user: EmployeeModel) {
    debugger;
    // hiện thị lỗi từ formGroup nếu không nhập đủ trường thông tin

    this.showPassword = false;
    this.employeeForm.patchValue({
      id: user.id,
      email: user.userId,
      fullName: user.fullName,
      birthday: user.birthday,
      gender: user.gender,
      userId: user.userId,
      address: user.address,
      phoneNumber: user.phoneNumber,
      password: '1234567',
    });
    this.birthday = user.birthday;
    this.errorMessage = '';
    this.titleModal = 'Cập nhật nhân viên';
    this.btnSave = 'Cập nhật';
    if (this.employeeForm.invalid) {
      console.log('data invalid', this.employeeForm.value);
      return;
    }
  }

  updateTable() {
    this.isDisplayNone = false;
    this.errorMessage = '';
    this.findAll(this.paginationModel.pageSize, 1, 'CreateDate', 'DESC', '');
  }

  lockAccount(id: number, status: boolean) {
    Swal.fire({
      title: 'Bạn có chắc chắn muốn thực hiện thao tác này không?',
      text: 'Khóa tài khoản sẽ thực hiện không đăng nhập vào hệ thống!',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Xác nhận',
      cancelButtonText: 'Hủy',
      buttonsStyling: false,
      customClass: {
        confirmButton: 'btn btn-danger me-1',
        cancelButton: 'btn btn-secondary',
      },
    }).then((result) => {
      if (result.isConfirmed) {
        debugger;
        this.userServide.delete(id, status).subscribe({
          next: (response: any) => {
            debugger;
            if (response.message === 'Mở khóa tài khoản thành công') {
              this.updateTable();
              this.toastr.success(response.message, 'Thông báo');
            } else if (response.message === 'Khóa tài khoản thành công') {
              this.updateTable();
              this.toastr.info(response.message, 'Thông báo');
            } else {
              this.toastr.error(response.message, 'Thông báo');
            }
          },
          error: (error: any) => {
            this.toastr.error(error.error, 'Thất bại');
          },
        });
      }
    });
  }

  createEmployee() {
    debugger;
    this.isDisplayNone = true;
    this.employeeForm.value.id = 0;
    const userdto: UpdateUserDTO = {
      id: this.employeeForm.value.id,
      fullName: this.employeeForm.value.fullName,
      password: this.employeeForm.value.password,
      email: this.employeeForm.value.email,
      role: 'EMPLOYEE',
      birthday: this.employeeForm.value.birthday,
      gender: this.employeeForm.value.gender,
      phoneNumber: this.employeeForm.value.phoneNumber,
      address: this.employeeForm.value.address,
    };

    debugger;
    this.authService.register(userdto).subscribe({
      next: (response: any) => {
        if (response.message === 'Đăng ký tài khoản thành công') {
          this.employeeForm.reset();
          this.btnCloseModal.nativeElement.click();
          this.updateTable();
          this.toastr.success(response.message, 'Thông báo');
        } else {
          this.errorMessage = response.message;
          this.isDisplayNone = false;
          this.employeeForm.value.id = null;
        }
      },
      error: (error: any) => {
        this.errorMessage = error.error;
        this.isDisplayNone = false;
        this.employeeForm.value.id = null;
      },
    });
  }
}
