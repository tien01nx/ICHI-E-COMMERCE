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
import { CommonModule } from '@angular/common';
import Swal from 'sweetalert2';
import { PaginationDTO } from '../../../dtos/pagination.dto';
import { Utils } from '../../../Utils.ts/utils';
import { TrademarkModel } from '../../../models/trademark.model';
import { TrademarkService } from '../../../service/trademark.service';
import validator from 'validator';

@Component({
  selector: 'app-trademark',
  templateUrl: './trademark.component.html',
  styleUrl: './trademark.component.css',
})
export class TrademarkComponent implements OnInit {
  protected readonly Utils = Utils;
  paginationModel: PaginationDTO<TrademarkModel> = PaginationDTO.createEmpty();
  searchTemp: any = this.activatedRoute.snapshot.queryParams['Search'] || '';
  selectAll: boolean = false;
  sortDir: string = 'ASC';
  SortBy: string = 'id';

  @ViewChild('btnCloseModal') btnCloseModal!: ElementRef;
  titleModal: string = '';
  btnSave: string = '';
  isDisplayNone: boolean = false;
  errorMessage: string = '';
  trademarkForm: FormGroup = new FormGroup({
    id: new FormControl('0'),
    trademarkName: new FormControl('', [
      Validators.required,
      Validators.maxLength(50),
      Validators.minLength(6),
      Validators.pattern(Utils.textPattern),
    ]),
  });

  constructor(
    private title: Title,
    private trademarkService: TrademarkService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private toastr: ToastrService
  ) {}
  ngOnInit() {
    this.title.setTitle('Quản lý thông tin thương hiệu');
    this.activatedRoute.queryParams.subscribe((params) => {
      const search = params['search'] || '';
      const pageSize = +params['page-size'] || 10;
      const pageNumber = +params['page-number'] || 1;
      const sortDir = params['sort-direction'] || 'DESC';
      const sortBy = params['sort-by'] || 'CreateDate';
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
    this.trademarkService
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
      .navigate(['/admin/trademark'], {
        queryParams: { 'page-number': pageNumber },
        queryParamsHandling: 'merge',
      })
      .then((r) => {});
  }

  changePageSize(pageSize: number): void {
    this.router
      .navigate(['/admin/trademark'], {
        queryParams: { 'page-size': pageSize, 'page-number': 1 },
        queryParamsHandling: 'merge',
      })
      .then((r) => {});
  }

  sortByField(sortBy: string): void {
    this.router
      .navigate(['/admin/trademark'], {
        queryParams: { 'sort-by': sortBy, 'sort-direction': this.sortDir },
        queryParamsHandling: 'merge',
      })
      .then((r) => {});
    this.sortDir = this.sortDir === 'ASC' ? 'DESC' : 'ASC';
    this.SortBy = sortBy;
  }

  search() {
    this.router
      .navigate(['/admin/trademark'], {
        queryParams: { search: this.searchTemp, 'page-number': 1 },
        queryParamsHandling: 'merge',
      })
      .then((r) => {});
  }

  onSubmit() {
    if (this.trademarkForm.invalid) {
      return;
    }
    if (this.trademarkForm.value.id == null) this.create();
    else this.update();
  }

  create() {
    this.isDisplayNone = true;
    this.trademarkForm.value.id = 0;
    this.trademarkService.create(this.trademarkForm.value).subscribe({
      next: (response: any) => {
        if (response.message === 'Tạo mới thương hiệu thành công') {
          this.trademarkForm.reset();
          this.btnCloseModal.nativeElement.click();
          this.updateTable();
          this.toastr.success(response.message, 'Thông báo');
        } else {
          this.errorMessage = response.message;
          this.isDisplayNone = false;
          // this.toastr.error(response.message, 'Thông báo');
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
    this.trademarkService.update(this.trademarkForm.value).subscribe({
      next: (response: any) => {
        debugger;
        if (response.message === 'Cập nhật thương hiệu thành công') {
          this.trademarkForm.reset();
          this.btnCloseModal.nativeElement.click();
          this.updateTable();
          this.toastr.success(response.message, 'Thông báo');
        } else {
          this.isDisplayNone = false;
          this.toastr.error(response.message, 'Thông báo');
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
        this.trademarkService.delete(id).subscribe({
          next: (response: any) => {
            if (response.message === 'Xóa thành công') {
              this.updateTable();
              this.toastr.success(response.message, 'Thông báo');
            } else this.toastr.error(response.message, 'Thông báo');
          },
          error: (error: any) => {
            this.toastr.error(error.error, 'Thất bại');
          },
        });
      }
    });
  }

  openModalCreate() {
    this.trademarkForm.reset();
    this.titleModal = 'Thêm thông tin thương hiệu';
    this.btnSave = 'Thêm mới';
    this.errorMessage = '';
  }

  openModalUpdate(trademark: TrademarkModel) {
    this.trademarkForm.patchValue({
      id: trademark.id,
      trademarkName: trademark.trademarkName,
    });
    this.titleModal = 'Cập nhật thương hiệu';
    this.btnSave = 'Cập nhật';
  }

  updateTable() {
    this.isDisplayNone = false;
    this.errorMessage = '';
    this.findAll(this.paginationModel.pageSize, 1, 'CreateDate', 'DESC', '');
  }
}
