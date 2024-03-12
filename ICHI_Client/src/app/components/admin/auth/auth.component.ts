import { UserModel } from './../../../models/user.model';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Utils } from '../../../Utils.ts/utils';
import { Environment } from '../../../environment/environment';
import { PaginationDTO } from '../../../dtos/pagination.dto';
import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import Swal from 'sweetalert2';
import { UserService } from '../../../service/user.service';
import { CommonModule } from '@angular/common';
import { UserDTO } from '../../../dtos/user.dto';
import { UpdateUserDTO } from '../../../dtos/update.user.dto';
import { AuthService } from '../../../service/auth.service';

@Component({
  selector: 'app-auth',
  standalone: true,
  imports: [FormsModule, ReactiveFormsModule, CommonModule],
  templateUrl: './auth.component.html',
  styleUrl: './auth.component.css',
})
export class AuthComponent implements OnInit {
  protected readonly Utils = Utils;
  protected readonly Environment = Environment;
  paginationModel: PaginationDTO<UserDTO> = PaginationDTO.createEmpty();
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
  userForm: FormGroup = new FormGroup({
    id: new FormControl(null),
    fullName: new FormControl('', [
      Validators.required,
      Validators.maxLength(50),
    ]),
    // phoneNumber: new FormControl('', [
    //   Validators.required,
    //   Validators.maxLength(10),
    //   Validators.minLength(10),
    //   Validators.pattern('^0[0-9]{9}$'),
    // ]),
    birthday: new FormControl('', [Validators.required]),
    gender: new FormControl('', [Validators.required]),
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required]),
    role: new FormControl('', [Validators.required]),
    userId: new FormControl(null),
  });

  constructor(
    private title: Title,
    private userService: UserService,
    private authService: AuthService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private toastr: ToastrService
  ) {}
  ngOnInit() {
    this.title.setTitle('Quản lý thông tin khách hàng');
    this.activatedRoute.queryParams.subscribe((params) => {
      const search = params['search'] || '';
      const pageSize = +params['page-size'] || 10;
      const pageNumber = +params['page-number'] || 1;
      const sortDir = params['sort-direction'] || 'ASC';
      const sortBy = params['sort-by'] || '';
      this.findAll(pageSize, pageNumber, sortBy, sortDir, search);
    });
  }
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
    this.isDisplayNone = true;
    // đổi dữ liệu từ userFrom sang userDTO
    const userdto: UpdateUserDTO = {
      id: this.userForm.value.id,
      fullName: this.userForm.value.fullName,
      email: this.userForm.value.email,
      role: this.userForm.value.role,
      birthday: this.userForm.value.birthday,
      gender: this.userForm.value.gender,
      password: this.userForm.value.password,
    };

    this.userService.UpdateImage(userdto).subscribe({
      next: (response: any) => {
        this.userForm.reset();
        this.btnCloseModal.nativeElement.click();
        this.updateTable();
        this.toastr.success(response.message, 'Thông báo');
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
    this.userService
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
      .navigate(['/admin/auth'], {
        queryParams: { 'page-number': pageNumber },
        queryParamsHandling: 'merge',
      })
      .then((r) => {});
  }

  changePageSize(pageSize: number): void {
    this.router
      .navigate(['/admin/auth'], {
        queryParams: { 'page-size': pageSize, 'page-number': 1 },
        queryParamsHandling: 'merge',
      })
      .then((r) => {});
  }

  sortByField(sortBy: string): void {
    this.router
      .navigate(['/admin/auth'], {
        queryParams: { 'sort-by': sortBy, 'sort-direction': this.sortDir },
        queryParamsHandling: 'merge',
      })
      .then((r) => {});
    this.sortDir = this.sortDir === 'ASC' ? 'DESC' : 'ASC';
    this.SortBy = sortBy;
  }

  search() {
    this.router
      .navigate(['/admin/auth'], {
        queryParams: { search: this.searchTemp, 'page-number': 1 },
        queryParamsHandling: 'merge',
      })
      .then((r) => {});
  }

  getGenderValue(): boolean {
    const value = this.userForm.value('gender').value;
    const isMale = value === '1';

    return isMale;
  }
  onSubmit() {
    if (this.userForm.invalid) {
      return;
    }
    if (this.userForm.value.id === null) this.create();
    else this.update();
    debugger;
  }

  create() {
    this.isDisplayNone = true;
    this.userForm.value.id = 0;
    const userdto: UpdateUserDTO = {
      id: this.userForm.value.id,
      fullName: this.userForm.value.fullName,
      password: this.userForm.value.password,
      email: this.userForm.value.email,
      role: this.userForm.value.role,
      birthday: this.userForm.value.birthday,
      gender: this.userForm.value.gender,
    };

    this.authService.register(userdto).subscribe({
      next: (response: any) => {
        if (response.code === 200) {
          this.userForm.reset();
          this.btnCloseModal.nativeElement.click();
          this.updateTable();
          this.toastr.success(response.message, 'Thông báo');
        } else {
          this.errorMessage = response.message;
          this.isDisplayNone = false;
        }
      },
      error: (error: any) => {
        this.errorMessage = error.error;
        this.isDisplayNone = false;
      },
    });
  }

  delete(id: number, status: boolean) {
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
        debugger;
        this.userService.delete(id, status).subscribe({
          next: (response: any) => {
            this.updateTable();
            this.toastr.success(response.message, 'Thông báo');
          },
          error: (error: any) => {
            this.toastr.error(error.error, 'Thất bại');
          },
        });
      }
    });
  }

  openModalCreate() {
    this.userForm.reset();
    this.titleModal = 'Thêm mới tài khoản';
    this.btnSave = 'Thêm mới';
    this.errorMessage = '';
  }

  openModalUpdate(user: UserDTO) {
    debugger;
    this.showPassword = false;
    this.userForm.patchValue({
      id: user.user.id,
      email: user.email,
      fullName: user.fullName,
      role: user.role,
      birthday: user.birthday,
      gender: user.gender,
    });
    this.userForm.value.id = user.user.id;
    const selectedRole = this.Environment.roles.find(
      (role) => role.name === user.role
    );
    if (selectedRole) {
      // this.userForm.get('role').setValue(selectedRole.name);
      this.userForm.value.role = selectedRole.name;
    }
    // this.avatarSrc = Environment.apiBaseRoot + user.user.avatar;
    this.birthday = user.birthday;
    this.titleModal = 'Cập nhật khách hàng';
    this.btnSave = 'Cập nhật';
  }

  updateTable() {
    this.isDisplayNone = false;
    this.errorMessage = '';
    this.findAll(this.paginationModel.pageSize, 1, '', '', '');
  }
}
