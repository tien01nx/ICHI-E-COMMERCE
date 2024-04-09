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
import { Utils } from '../../../Utils.ts/utils';
import { PromotionModel } from '../../../models/promotion.model';
import { PaginationDTO } from '../../../dtos/pagination.dto';
import { PromotionService } from '../../../service/promotion.service';

@Component({
  selector: 'app-promotion.admin',
  templateUrl: './promotion.component.html',
  styleUrl: './promotion.component.css',
})
export class PromotionComponent implements OnInit {
  protected readonly Utils = Utils;
  paginationModel: PaginationDTO<PromotionModel> = PaginationDTO.createEmpty();
  searchTemp: any = this.activatedRoute.snapshot.queryParams['Search'] || '';
  selectAll: boolean = false;
  sortDir: string = 'ASC';
  SortBy: string = 'id';

  @ViewChild('btnCloseModal') btnCloseModal!: ElementRef;
  titleModal: string = '';
  btnSave: string = '';
  isDisplayNone: boolean = false;
  errorMessage: string = '';
  startTime: Date = new Date();
  endTime: Date = new Date();
  promotionForm: FormGroup = new FormGroup({
    id: new FormControl('0'),
    promotionName: new FormControl('', [
      Validators.required,
      Validators.maxLength(50),
    ]),
    promotionCode: new FormControl('', [
      Validators.required,
      Validators.maxLength(20),
    ]),
    startDate: new FormControl('', [Validators.required]),
    endDate: new FormControl('', [Validators.required]),
    discount: new FormControl('', [Validators.required]),
    quantity: new FormControl('', [Validators.required]),
    minimumPrice: new FormControl('', [Validators.required]),
    isActive: new FormControl(true),
    isDeleted: new FormControl(false),
  });

  constructor(
    private title: Title,
    private promotionService: PromotionService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private toastr: ToastrService
  ) {}
  ngOnInit() {
    this.title.setTitle('Quản lý thông tin khuyến mãi');
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

  updateTable() {
    this.findAll(this.paginationModel.pageSize, 1, '', '', '');
  }

  findAll(
    pageSize: number,
    pageNumber: number,
    sortBy: string,
    sortDir: string,
    search: string
  ) {
    this.promotionService
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
      .navigate(['/admin/promotion'], {
        queryParams: { 'page-number': pageNumber },
        queryParamsHandling: 'merge',
      })
      .then((r) => {});
  }

  changePageSize(pageSize: number): void {
    this.router
      .navigate(['/admin/promotion'], {
        queryParams: { 'page-size': pageSize, 'page-number': 1 },
        queryParamsHandling: 'merge',
      })
      .then((r) => {});
  }

  sortByField(sortBy: string): void {
    this.router
      .navigate(['/admin/promotion'], {
        queryParams: { 'sort-by': sortBy, 'sort-direction': this.sortDir },
        queryParamsHandling: 'merge',
      })
      .then((r) => {});
    this.sortDir = this.sortDir === 'ASC' ? 'DESC' : 'ASC';
    this.SortBy = sortBy;
  }

  search() {
    this.router
      .navigate(['/admin/promotion'], {
        queryParams: { search: this.searchTemp, 'page-number': 1 },
        queryParamsHandling: 'merge',
      })
      .then((r) => {});
  }

  onSubmit() {
    // if (this.promotionForm.invalid) {
    //   return;
    // }
    // if (this.promotionForm.value.id == null) this.create();
    // else this.update();
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
        this.promotionService.delete(id).subscribe({
          next: (response: any) => {
            if (response.message === 'Xóa chương trình khuyến mãi thành công') {
              this.toastr.success(response.message, 'Thông báo');
              this.updateTable();
            }
            this.toastr.error(response.message, 'Thông báo');
          },
          error: (error: any) => {
            this.toastr.error(error.error, 'Thất bại');
          },
        });
      }
    });
  }

  insertPromotion() {
    this.router.navigate(['/admin/promotion_insert']);
  }

  lockAccount(id: number, status: boolean) {
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
        this.promotionService.updateStatus(id, status).subscribe({
          next: (response: any) => {
            if (response.message === 'Mở khóa tài khoản thành công') {
              // this.updateTable();
              this.toastr.success(response.message, 'Thông báo');
            } else if (response.message === 'Khóa tài khoản thành công') {
              // this.updateTable();
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
  promotionDetail(id: number) {
    this.router.navigate(['/admin/promotion_insert/' + id]);
  }
}
