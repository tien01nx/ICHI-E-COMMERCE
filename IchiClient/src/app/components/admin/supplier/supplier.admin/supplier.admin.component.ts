import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
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
  paginationModel!: PaginationDTO<SupplierModel>;
  searchTemp: any = this.activatedRoute.snapshot.queryParams['bankName'] || '';
  selectAll: boolean = false;
  sortDir: string = 'ASC';
  sortBy: string = 'id';

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
    debugger;
    this.activatedRoute.queryParams.subscribe((params) => {
      const bankName = params['supplier_name'] || '';
      const pageSize = +params['page-size'] || 10;
      const pageNumber = +params['page-number'] || 1;
      const sortDir = params['sort-direction'] || '';
      const sortBy = params['sort-by'] || '';

      this.findAll(bankName, pageSize, pageNumber, sortDir, sortBy);
    });
  }

  toggleSelectAll() {
    this.selectAll = !this.selectAll;
  }

  findAll(
    supplier_name: string,
    pageSize: number,
    pageNumber: number,
    sortDir: string,
    sortBy: string
  ) {
    this.supplierService
      .findAllByName(pageSize, pageNumber, sortDir, sortBy, supplier_name)
      .subscribe({
        next: (response: any) => {
          this.paginationModel.content = response.content;
          this.paginationModel.totalPages = response.totalPages;
          this.paginationModel.totalElements = response.totalElements;
          this.paginationModel.numberOfElements = response.numberOfElements;
          this.paginationModel.pageSize = response.pageSize;
          this.paginationModel.pageNumber = response.pageNumber;
          this.paginationModel.firstElementOnPage = response.firstElementOnPage;
          this.paginationModel.lastElementOnPage = response.lastElementOnPage;
          this.paginationModel.sortBy = response.sortBy;
          this.paginationModel.sortDirection = response.sortDirection;
        },
        error: (error: any) => {
          console.log(error);
        },
      });
  }

  changePageNumber(pageNumber: number): void {
    this.router
      .navigate(['/admin/supplier'], {
        queryParams: { 'page-number': pageNumber },
        queryParamsHandling: 'merge',
      })
      .then((r) => {});
  }

  changePageSize(pageSize: number): void {
    this.router
      .navigate(['/admin/supplier'], {
        queryParams: { 'page-size': pageSize, 'page-number': 1 },
        queryParamsHandling: 'merge',
      })
      .then((r) => {});
  }

  sortByField(sortBy: string): void {
    this.router
      .navigate(['/admin/supplier'], {
        queryParams: { 'sort-by': sortBy, 'sort-direction': this.sortDir },
        queryParamsHandling: 'merge',
      })
      .then((r) => {});
    this.sortDir = this.sortDir === 'ASC' ? 'DESC' : 'ASC';
    this.sortBy = sortBy;
  }

  search() {
    this.router
      .navigate(['/admin/supplier'], {
        queryParams: { bankName: this.searchTemp, 'page-number': 1 },
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
    });
    this.titleModal = 'Cập nhật nhà cung cấp';
    this.btnSave = 'Cập nhật';
  }

  updateTable() {
    this.isDisplayNone = false;
    this.errorMessage = '';
    this.findAll('', this.paginationModel.pageSize, 1, '', '');
  }
}
