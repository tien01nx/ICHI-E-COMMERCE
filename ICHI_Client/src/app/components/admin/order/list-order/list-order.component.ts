import { Utils } from './../../../../Utils.ts/utils';
import { TrxTransactionModel } from './../../../../models/trx.transaction.model';
import { TrxTransactionService } from './../../../../service/trx-transaction.service';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { PaginationDTO } from '../../../../dtos/pagination.dto';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-list-order',
  templateUrl: './list-order.component.html',
  styleUrl: './list-order.component.css',
})
export class ListOrderComponent implements OnInit {
  protected readonly Utils = Utils;
  paginationModel: PaginationDTO<TrxTransactionModel> =
    PaginationDTO.createEmpty();
  searchTemp: any = this.activatedRoute.snapshot.queryParams['Search'] || '';
  orderStatusTemp: any =
    this.activatedRoute.snapshot.queryParams['order-status'] || '';
  selectAll: boolean = false;
  sortDir: string = 'ASC';
  SortBy: string = 'id';
  titleOrder: string = 'Trạng thái đơn hàng';

  @ViewChild('btnCloseModal') btnCloseModal!: ElementRef;
  titleModal: string = '';
  btnSave: string = '';
  isDisplayNone: boolean = false;
  errorMessage: string = '';
  transactionForm: FormGroup = new FormGroup({
    id: new FormControl('0'),
    transactionName: new FormControl('', [
      Validators.required,
      Validators.maxLength(50),
      Validators.pattern(Utils.textPattern),
    ]),
    address: new FormControl('', [
      Validators.required,
      Validators.minLength(10),
      Validators.maxLength(100),
    ]),
    phoneNumber: new FormControl('', [
      Validators.required,
      Validators.maxLength(10),
      Validators.minLength(10),
      Validators.pattern(Utils.textPhoneNumber),
    ]),
    email: new FormControl('', [
      Validators.required,
      Validators.pattern(Utils.checkEmail),
    ]),
    taxCode: new FormControl('', [
      Validators.required,
      Validators.minLength(10),
      Validators.maxLength(13),
      Validators.pattern(Utils.numberPattern),
    ]),
    bankAccount: new FormControl('', [
      Validators.required,
      Validators.maxLength(20),
      Validators.minLength(5),
    ]),
    bankName: new FormControl('', [
      Validators.required,
      Validators.minLength(3),
      Validators.maxLength(10),
      Validators.pattern(Utils.textPattern),
    ]),
  });

  constructor(
    private title: Title,
    private activatedRoute: ActivatedRoute,
    private transactitonService: TrxTransactionService,
    private router: Router,
    private toastr: ToastrService
  ) {}
  ngOnInit() {
    this.title.setTitle('Quản lý nhà cung cấp');
    this.activatedRoute.queryParams.subscribe((params) => {
      const search = params['search'] || '';
      const order = params['order-status'] || '';
      const pageSize = +params['page-size'] || 10;
      const pageNumber = +params['page-number'] || 1;
      const sortDir = params['sort-direction'] || 'DESC';
      const sortBy = params['sort-by'] || 'CreateDate';
      this.findAll(pageSize, pageNumber, sortBy, sortDir, search, order);
    });
  }

  toggleSelectAll() {
    this.selectAll = !this.selectAll;
  }

  getStatusOrderName(value: string) {
    const status = Utils.statusOrder.find((x) => x.value === value);
    const name = status ? status.name : ''; // Kiểm tra nếu status không null thì lấy tên, ngược lại trả về ''
    debugger;
    return name;
  }

  findAll(
    pageSize: number,
    pageNumber: number,
    sortBy: string,
    sortDir: string,
    search: string,
    order: string
  ) {
    this.transactitonService
      .getFindAllTransaction(
        pageNumber,
        pageSize,
        sortDir,
        sortBy,
        search,
        order
      )
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
      .navigate(['/admin/list_order'], {
        queryParams: { 'page-number': pageNumber },
        queryParamsHandling: 'merge',
      })
      .then((r) => {});
  }

  changePageSize(pageSize: number): void {
    this.router
      .navigate(['/admin/list_order'], {
        queryParams: { 'page-size': pageSize, 'page-number': 1 },
        queryParamsHandling: 'merge',
      })
      .then((r) => {});
  }

  sortByField(sortBy: string): void {
    this.router
      .navigate(['/admin/list_order'], {
        queryParams: { 'sort-by': sortBy, 'sort-direction': this.sortDir },
        queryParamsHandling: 'merge',
      })
      .then((r) => {});
    this.sortDir = this.sortDir === 'ASC' ? 'DESC' : 'ASC';
    this.SortBy = sortBy;
  }

  search() {
    this.router
      .navigate(['/admin/list_order'], {
        queryParams: { search: this.searchTemp, 'page-number': 1 },
        queryParamsHandling: 'merge',
      })
      .then((r) => {});
  }

  orderStatus(order: string) {
    this.router
      .navigate(['/admin/list_order'], {
        queryParams: {
          'order-status': order,
        },
        queryParamsHandling: 'merge',
      })
      .then((r) => {});
    this.titleOrder = Utils.getPaymentStatus(order);
  }

  onSubmit() {
    if (this.transactionForm.invalid) {
      return;
    }
    if (this.transactionForm.value.id == null) this.create();
    else this.update();
  }

  create() {
    // this.isDisplayNone = true;
    // this.transactionForm.value.id = 0;
    // this.transactionService.create(this.transactionForm.value).subscribe({
    //   next: (response: any) => {
    //     if (response.message === 'Tạo mới thành công') {
    //       this.transactionForm.reset();
    //       this.btnCloseModal.nativeElement.click();
    //       this.updateTable();
    //       this.toastr.success(response.message, 'Thông báo');
    //     } else {
    //       this.errorMessage = response.message;
    //       this.isDisplayNone = false;
    //     }
    //   },
    //   error: (error: any) => {
    //     this.errorMessage = error.error;
    //     this.isDisplayNone = false;
    //   },
    // });
  }

  update() {
    // this.isDisplayNone = true;
    // this.transactionService.update(this.transactionForm.value).subscribe({
    //   next: (response: any) => {
    //     if (response.message === 'Cập nhật nhà cung cấp thành công') {
    //       this.transactionForm.reset();
    //       this.btnCloseModal.nativeElement.click();
    //       this.updateTable();
    //       this.toastr.success(response.message, 'Thông báo');
    //     } else {
    //       this.toastr.error(response.message, 'Thất bại');
    //     }
    //   },
    //   error: (error: any) => {
    //     this.errorMessage = error.error;
    //     this.isDisplayNone = false;
    //   },
    // });
  }

  delete(id: number) {
    // Swal.fire({
    //   title: 'Bạn có chắc chắn muốn xóa?',
    //   text: 'Dữ liệu sẽ không thể phục hồi sau khi xóa!',
    //   icon: 'warning',
    //   showCancelButton: true,
    //   confirmButtonText: 'Xác nhận',
    //   cancelButtonText: 'Hủy',
    //   buttonsStyling: false,
    //   customClass: {
    //     confirmButton: 'btn btn-danger me-1',
    //     cancelButton: 'btn btn-secondary',
    //   },
    // }).then((result) => {
    //   if (result.isConfirmed) {
    //     this.transactionService.delete(id).subscribe({
    //       next: (response: any) => {
    //         if (response.message === 'Xóa nhà cung cấp thành công') {
    //           this.updateTable();
    //           this.toastr.success(response.message, 'Thông báo');
    //         } else {
    //           this.toastr.error(response.message, 'Thất bại');
    //         }
    //       },
    //       error: (error: any) => {
    //         this.toastr.error(error.error, 'Thất bại');
    //       },
    //     });
    //   }
    // });
  }

  openModalCreate() {
    this.transactionForm.reset();
    this.titleModal = 'Thêm nhà cung cấp';
    this.btnSave = 'Thêm mới';
    this.errorMessage = '';
  }

  openModalUpdate(transaction: any) {
    // this.transactionForm.patchValue({
    //   id: transaction.id,
    //   bankName: transaction.bankName,
    //   phoneNumber: transaction.phoneNumber,
    //   address: transaction.address,
    //   email: transaction.email,
    //   taxCode: transaction.taxCode,
    //   bankAccount: transaction.bankAccount,
    //   transactionCode: transaction.transactionCode,
    //   transactionName: transaction.transactionName,
    // });
    // this.titleModal = 'Cập nhật nhà cung cấp';
    // this.btnSave = 'Cập nhật';
  }

  updateTable() {
    this.isDisplayNone = false;
    this.errorMessage = '';
    this.findAll(
      this.paginationModel.pageSize,
      1,
      'CreateDate',
      'DESC',
      '',
      ''
    );
  }
  clearUrl() {
    this.titleOrder = 'Trạng thái đơn hàng';
    this.router.navigate(['/admin/list_order']);
  }
  shouldDisplayClearButton(): boolean {
    const queryParams = this.activatedRoute.snapshot.queryParams;
    return Object.keys(queryParams).length > 0; // Trả về true nếu URL có chứa bất kỳ tham số nào
  }
}
