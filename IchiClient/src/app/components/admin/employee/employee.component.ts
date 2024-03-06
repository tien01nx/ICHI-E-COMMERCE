import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Utils } from '../../../Utils.ts/utils';
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
import { EmployeeModel } from '../../../models/employee.model';
import { CommonModule } from '@angular/common';
import { EmployeeService } from '../../../service/employee.service';

@Component({
  selector: 'app-employee',
  standalone: true,
  imports: [FormsModule, ReactiveFormsModule, CommonModule],
  templateUrl: './employee.component.html',
  styleUrl: './employee.component.css',
})
export class EmployeeComponent implements OnInit {
  protected readonly Utils = Utils;
  // paginationModel!: PaginationDTO<SupplierModel> =
  //   new PaginationDTO<SupplierModel>();
  paginationModel: PaginationDTO<EmployeeModel> = PaginationDTO.createEmpty();
  searchTemp: any = this.activatedRoute.snapshot.queryParams['Search'] || '';
  selectAll: boolean = false;
  sortDir: string = 'ASC';
  SortBy: string = 'id';

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
      Validators.maxLength(50),
    ]),
    phoneNumber: new FormControl('', [
      Validators.required,
      Validators.maxLength(10),
      Validators.minLength(10),
      Validators.pattern('^0[0-9]{9}$'),
    ]),
    gender: new FormControl('', [Validators.required]),
    birthday: new FormControl('', [Validators.required]),
  });

  constructor(
    private title: Title,
    private employeeService: EmployeeService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private toastr: ToastrService
  ) {}
  ngOnInit() {
    this.title.setTitle('Quản lý thông tin nhân viên');
    this.activatedRoute.queryParams.subscribe((params) => {
      const search = params['search'] || '';
      const pageSize = +params['page-size'] || 10;
      const pageNumber = +params['page-number'] || 1;
      const sortDir = params['sort-direction'] || 'ASC';
      const sortBy = params['sort-by'] || '';
      this.findAll(pageSize, pageNumber, sortBy, sortDir, search);
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
    if (this.employeeForm.value.id === null) this.create();
    else this.update();
  }

  create() {
    this.isDisplayNone = true;
    this.employeeForm.value.id = 0;
    this.employeeService.create(this.employeeForm.value).subscribe({
      next: (response: any) => {
        if (response.code === 200) {
          this.employeeForm.reset();
          this.btnCloseModal.nativeElement.click();
          this.updateTable();
          this.toastr.success('Thêm nhân viên thành công', 'Thông báo');
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

  update() {
    this.isDisplayNone = true;
    debugger;
    this.employeeService.update(this.employeeForm.value).subscribe({
      next: (response: any) => {
        this.employeeForm.reset();
        this.btnCloseModal.nativeElement.click();
        this.updateTable();
        this.toastr.success('Cập nhật nhân viên thành công', 'Thông báo');
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
        this.employeeService.delete(id).subscribe({
          next: (response: any) => {
            this.updateTable();
            this.toastr.success('Xóa nhà cung cấp thành công', 'Thông báo');
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
  }

  openModalUpdate(employee: EmployeeModel) {
    this.employeeForm.patchValue({
      id: employee.id,
      fullName: employee.fullName,
      phoneNumber: employee.phoneNumber,
      gender: employee.gender,
      birthday: employee.birthday,
      userId: employee.userId,
    });
    this.birthday = employee.birthday;
    this.titleModal = 'Cập nhật nhân viên';
    this.btnSave = 'Cập nhật';
  }

  updateTable() {
    this.isDisplayNone = false;
    this.errorMessage = '';
    this.findAll(this.paginationModel.pageSize, 1, '', '', '');
  }
}
