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
  standalone: true,
  imports: [FormsModule, ReactiveFormsModule, CommonModule],
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
    if (this.promotionForm.invalid) {
      return;
    }
    if (this.promotionForm.value.id == null) this.create();
    else this.update();
  }

  create() {
    this.isDisplayNone = true;
    this.promotionForm.value.id = 0;
    this.promotionForm.value.isDeleted = false;
    this.promotionForm.value.isActive = true;
    this.promotionService.create(this.promotionForm.value).subscribe({
      next: (response: any) => {
        if (response.code === 200) {
          debugger;
          this.promotionForm.reset();
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

  update() {
    this.isDisplayNone = true;
    this.promotionService.update(this.promotionForm.value).subscribe({
      next: (response: any) => {
        console.log(response);
        debugger;
        this.promotionForm.reset();
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
    this.promotionForm.reset();
    this.titleModal = 'Thêm thông tin khuyến mãi';
    this.btnSave = 'Thêm mới';
    this.errorMessage = '';
  }

  openModalUpdate(promotion: PromotionModel) {
    const startDate = new Date(promotion.startTime);
    const endDate = new Date(promotion.endTime);
    this.promotionForm.patchValue({
      id: promotion.id,
      promotionCode: promotion.promotionCode,
      promotionName: promotion.promotionName,
      startDate: startDate,
      endDate: endDate,
      discount: promotion.discount,
      quantity: promotion.quantity,
      isActive: promotion.isActive,
      isDeleted: promotion.isDeleted,
    });
    debugger;
    this.startTime = startDate;
    this.endTime = endDate;
    this.titleModal = 'Cập nhật thông tin khuyến mãi';
    this.btnSave = 'Cập nhật';
  }

  updateTable() {
    this.isDisplayNone = false;
    this.errorMessage = '';
    this.findAll(this.paginationModel.pageSize, 1, '', '', '');
  }
}
