import { Utils } from './../../../Utils.ts/utils';
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
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import Swal from 'sweetalert2';
import { CommonModule } from '@angular/common';
import { CustomerService } from '../../../service/customer.service';
import { Environment } from '../../../environment/environment';
import { UserService } from '../../../service/user.service';

@Component({
  selector: 'app-customer',
  templateUrl: './customer.component.html',
  styleUrl: './customer.component.css',
})
export class CustomerComponent {
  protected readonly Utils = Utils;
  protected readonly Environment = Environment;
  paginationModel: PaginationDTO<CustomerModel> = PaginationDTO.createEmpty();
  searchTemp: any = this.activatedRoute.snapshot.queryParams['Search'] || '';
  selectAll: boolean = false;
  sortDir: string = 'ASC';
  SortBy: string = 'id';
  file: File | null = null;
  avatarSrc: string = '';

  birthday: Date = new Date();
  @ViewChild('btnCloseModal') btnCloseModal!: ElementRef;
  titleModal: string = '';
  btnSave: string = '';
  isDisplayNone: boolean = false;
  errorMessage: string = '';
  customerForm: FormGroup = new FormGroup({
    id: new FormControl(null),
    fullName: new FormControl('', [
      Validators.required,
      Validators.maxLength(40),
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
    // email: new FormControl('', [
    //   Validators.required,
    //   Validators.pattern(Utils.checkEmail),
    // ]),
    city: new FormControl(null, [Validators.required]),
    district: new FormControl(null, [Validators.required]),
    ward: new FormControl(null, [Validators.required]),
    address: new FormControl('', [
      Validators.required,
      Validators.maxLength(100),
    ]),
    userId: new FormControl('', [
      Validators.required,
      Validators.pattern(Utils.checkEmail),
    ]),
  });
  cities: any;
  districts: any;
  wards: any;

  constructor(
    private title: Title,
    private customerService: CustomerService,
    private userService: UserService,
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
      const sortDir = params['sort-direction'] || 'DESC';
      const sortBy = params['sort-by'] || 'CreateDate';
      this.findAll(pageSize, pageNumber, sortBy, sortDir, search);
    });
    this.customerForm.get('city')?.valueChanges.subscribe((id: any) => {
      const filteredDistricts = Utils.district?.filter(
        (district: any) => district.city_id === id
      );
      this.districts = filteredDistricts || [];
      if (this.districts.length > 0) {
        this.customerForm.get('district')?.setValue(this.districts[0]?.id);
      }
    });

    this.customerForm.get('district')?.valueChanges.subscribe((id: any) => {
      const filteredWards = Utils.wards?.filter(
        (ward: any) => ward.district_id === id
      );
      this.wards = filteredWards || [];
      // Đặt giá trị mặc định cho trường 'ward' trong form nếu cần
      if (this.wards.length > 0) {
        this.customerForm.get('ward')?.setValue(this.wards[0]?.id);
      }
    });
  }

  onFileSelected(event: any) {
    debugger;
    const maxFileSize = 5 * 1024 * 1024; // 5MB
    this.file = event.target.files[0];

    // Kiểm tra xem tệp đã được chọn chưa
    if (this.file) {
      // Kiểm tra kích thước của tệp
      if (this.file.size > maxFileSize) {
        // Hiển thị thông báo lỗi về kích thước của tệp
        this.toastr.error('Kích thước của tệp lớn hơn 5MB', 'Lỗi');
        return; // Ngưng xử lý tiếp theo
      }

      // Kiểm tra kiểu MIME của tệp
      if (!this.file.type.startsWith('image/')) {
        // Hiển thị thông báo lỗi về kiểu MIME của tệp
        this.toastr.error('Tệp không phải là ảnh', 'Lỗi');
        return; // Ngưng xử lý tiếp theo
      }

      // Tiếp tục xử lý tệp nếu không có lỗi
      const reader = new FileReader();
      reader.onload = (e: any) => {
        this.avatarSrc = e.target.result;
      };
      reader.readAsDataURL(this.file);
    }
  }

  update() {
    this.isDisplayNone = true;
    this.customerService.Update(this.customerForm.value, this.file).subscribe({
      next: (response: any) => {
        if (response.code === 200) {
          this.isDisplayNone = false;
          this.customerForm.reset();
          this.btnCloseModal.nativeElement.click();
          this.updateTable();
          this.toastr.success(response.message, 'Thông báo');
        } else {
          this.errorMessage = response.message;
          this.isDisplayNone = false;
        }
      },
      error: (error: any) => {
        this.errorMessage = 'Có lỗi xảy ra';
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
    this.customerService
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
      .navigate(['/admin/customer'], {
        queryParams: { 'page-number': pageNumber },
        queryParamsHandling: 'merge',
      })
      .then((r) => {});
  }

  changePageSize(pageSize: number): void {
    this.router
      .navigate(['/admin/customer'], {
        queryParams: { 'page-size': pageSize, 'page-number': 1 },
        queryParamsHandling: 'merge',
      })
      .then((r) => {});
  }

  sortByField(sortBy: string): void {
    this.router
      .navigate(['/admin/customer'], {
        queryParams: { 'sort-by': sortBy, 'sort-direction': this.sortDir },
        queryParamsHandling: 'merge',
      })
      .then((r) => {});
    this.sortDir = this.sortDir === 'ASC' ? 'DESC' : 'ASC';
    this.SortBy = sortBy;
  }

  search() {
    this.router
      .navigate(['/admin/customer'], {
        queryParams: { search: this.searchTemp, 'page-number': 1 },
        queryParamsHandling: 'merge',
      })
      .then((r) => {});
  }

  getGenderValue(): boolean {
    const value = this.customerForm.value('gender').value;
    const isMale = value === '1';

    return isMale;
  }
  onSubmit() {
    if (this.customerForm.invalid) {
      return;
    }
    if (this.customerForm.value.id === null) this.create();
    else this.update();
    debugger;
  }

  create() {
    this.isDisplayNone = true;
    this.customerForm.value.id = 0;
    this.customerService.create(this.customerForm.value).subscribe({
      next: (response: any) => {
        if (response.code === 200) {
          this.customerForm.reset();
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
        this.customerService.delete(id).subscribe({
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
    this.customerForm.reset();
    this.titleModal = 'Thêm khách hàng';
    this.btnSave = 'Thêm mới';
    this.errorMessage = '';
  }

  openModalUpdate(customer: CustomerModel) {
    debugger;
    this.customerForm.patchValue({
      id: customer.id,
      fullName: customer.fullName,
      phoneNumber: customer.phoneNumber,
      gender: customer.gender,
      birthday: customer.birthday,
      userId: customer.userId,
      address: customer.address,
      city: customer.city,
      district: customer.district,
      ward: customer.ward,
    });
    this.avatarSrc = Environment.apiBaseRoot + customer.user.avatar;
    this.birthday = customer.birthday;
    this.titleModal = 'Cập nhật khách hàng';
    this.btnSave = 'Cập nhật';
  }

  updateTable() {
    this.isDisplayNone = false;
    this.errorMessage = '';
    this.findAll(this.paginationModel.pageSize, 1, 'CreateDate', 'DESC', '');
  }
  lockAccount(id: number, status: boolean) {
    Swal.fire({
      title: 'Thông báo',
      text: 'Bạn có chắc chắn muốn khóa tài khoản này không?',
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
}
