import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, Router, Data } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { SupplierModel } from '../../../../models/supplier.model';
import { CommonModule } from '@angular/common';
import { PaginationDTO } from '../../../../dtos/pagination.dto';
import { SupplierService } from '../../../../service/supplier.service';
import Swal from 'sweetalert2';
import { Utils } from '../../../../Utils.ts/utils';

@Component({
  selector: 'app-supplier.admin',
  standalone: true,
  imports: [FormsModule, ReactiveFormsModule, CommonModule],
  templateUrl: './supplier.admin.component.html',
  styleUrl: './supplier.admin.component.css',
})
export class SupplierAdminComponent implements OnInit {
  protected readonly Utils = Utils;
  // paginationModel!: PaginationDTO<SupplierModel> =
  //   new PaginationDTO<SupplierModel>();
  paginationModel: PaginationDTO<SupplierModel> = PaginationDTO.createEmpty();
  searchTemp: any = this.activatedRoute.snapshot.queryParams['Search'] || '';
  selectAll: boolean = false;
  sortDir: string = 'ASC';
  SortBy: string = 'id';

  @ViewChild('btnCloseModal') btnCloseModal!: ElementRef;
  titleModal: string = '';
  btnSave: string = '';
  isDisplayNone: boolean = false;
  errorMessage: string = '';
  supplierForm: FormGroup = new FormGroup({
    id: new FormControl(null),
    bankName: new FormControl('', [
      Validators.required,
      Validators.maxLength(50),
    ]),
    phone: new FormControl('', [
      Validators.required,
      Validators.maxLength(10),
      Validators.minLength(10),
      Validators.pattern('^0[0-9]{9}$'),
    ]),
    email: new FormControl('', [
      Validators.required,
      Validators.maxLength(10),
      Validators.minLength(10),
      Validators.pattern('^0[0-9]{9}$'),
    ]),
    tax_code: new FormControl('', [
      Validators.required,
      Validators.maxLength(10),
      Validators.minLength(10),
      Validators.pattern('^0[0-9]{9}$'),
    ]),
    banK_account: new FormControl('', [
      Validators.required,
      Validators.maxLength(10),
      Validators.minLength(10),
      Validators.pattern('^0[0-9]{9}$'),
    ]),
    bank_name: new FormControl('', [
      Validators.required,
      Validators.maxLength(10),
      Validators.minLength(10),
      Validators.pattern('^0[0-9]{9}$'),
    ]),
    address: new FormControl('', [Validators.maxLength(200)]),
  });

  constructor(
    private title: Title,
    private supplierService: SupplierService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private toastr: ToastrService
  ) {}
  ngOnInit() {
    this.title.setTitle('Quản lý nhà cung cấp');
    this.activatedRoute.queryParams.subscribe((params) => {
      const search = params['Search'] || '';
      const pageSize = +params['PageSize'] || 10;
      const pageNumber = +params['PageNumber'] || 1;
      const sortDir = params['SortDirection'] || 'ASC';
      const sortBy = params['SortBy'] || '';
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
    debugger  // tslint:disable-line
    this.supplierService
      .findAllByName(pageNumber, pageSize, sortDir, sortBy, search)
      .subscribe({
        next: (response: any) => {
          console.log(response);
          this.paginationModel.content = response.data;
          this.paginationModel.totalPages = response.totalPages;
          this.paginationModel.totalElements = response.totalElements;
          this.paginationModel.numberOfElements = response.numberOfElements;
          this.paginationModel.pageSize = response.pageSize;
          this.paginationModel.pageNumber = response.pageNumber;
          this.paginationModel.firstElementOnPage = response.firstElementOnPage;
          this.paginationModel.lastElementOnPage = response.lastElementOnPage;
          this.paginationModel.sortBy = response.sortBy;
          this.paginationModel.sortDirection = response.sortDirection;
          console.log(response);
        },
        error: (error: any) => {
          console.log(error);
        },
      });
  }

  changePageNumber(pageNumber: number): void {
    this.router
      .navigate(['/admin/supplier'], {
        queryParams: { PageNumber: pageNumber },
        queryParamsHandling: 'merge',
      })
      .then((r) => {});
  }

  changePageSize(pageSize: number): void {
    this.router
      .navigate(['/admin/supplier'], {
        queryParams: { PageSize: pageSize, PageNumber: 1 },
        queryParamsHandling: 'merge',
      })
      .then((r) => {});
  }

  sortByField(sortBy: string): void {
    this.router
      .navigate(['/admin/supplier'], {
        queryParams: { SortBy: sortBy, SortDirection: this.sortDir },
        queryParamsHandling: 'merge',
      })
      .then((r) => {});
    this.sortDir = this.sortDir === 'ASC' ? 'DESC' : 'ASC';
    this.SortBy = sortBy;
    // let newSortDir = this.sortDir === 'ASC' ? 'DESC' : 'ASC';
    // this.router.navigate(['/admin/supplier'], {
    //   queryParams: { SortBy: sortBy, SortDirection: newSortDir },
    //   queryParamsHandling: 'merge',
    // });
    // debugger;
  }

  search() {
    this.router
      .navigate(['/admin/supplier'], {
        queryParams: { Search: this.searchTemp, PageNumber: 1 },
        queryParamsHandling: 'merge',
      })
      .then((r) => {});
  }

  onSubmit() {
    if (this.supplierForm.invalid) {
      return;
    }
    if (this.supplierForm.value.id == null) this.create();
    else this.update();
  }

  create() {
    this.isDisplayNone = true;
    this.supplierService.create(this.supplierForm.value).subscribe({
      next: (response: any) => {
        this.supplierForm.reset();
        this.btnCloseModal.nativeElement.click();
        this.updateTable();
        this.toastr.success('Thêm nhà cung cấp thành công', 'Thông báo');
      },
      error: (error: any) => {
        this.errorMessage = error.error;
        this.isDisplayNone = false;
      },
    });
  }

  update() {
    this.isDisplayNone = true;
    this.supplierService.update(this.supplierForm.value).subscribe({
      next: (response: any) => {
        this.supplierForm.reset();
        this.btnCloseModal.nativeElement.click();
        this.updateTable();
        this.toastr.success('Cập nhật nhà cung cấp thành công', 'Thông báo');
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
        this.supplierService.delete(id).subscribe({
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
    this.supplierForm.reset();
    this.titleModal = 'Thêm nhà cung cấp';
    this.btnSave = 'Thêm mới';
  }

  openModalUpdate(supplier: SupplierModel) {
    this.supplierForm.patchValue({
      id: supplier.id,
      bankName: supplier.bankName,
      phone: supplier.phone,
      address: supplier.address,
      email: supplier.email,
      tax_code: supplier.taxCode,
      banK_account: supplier.bankAccount,
      bank_name: supplier.bankName,
    });
    this.titleModal = 'Cập nhật nhà cung cấp';
    this.btnSave = 'Cập nhật';
  }

  updateTable() {
    this.isDisplayNone = false;
    this.errorMessage = '';
    this.findAll(this.paginationModel.pageSize, 1, '', '', '');
  }
}
