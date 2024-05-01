import { TrxTransactionService } from './../../../service/trx-transaction.service';
import { TokenService } from './../../../service/token.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AfterViewInit, Component, OnInit } from '@angular/core';
import { Utils } from '../../../Utils.ts/utils';
import { Environment } from '../../../environment/environment';
import { ProductModel } from '../../../models/product.model';
import { CategoryProduct } from '../../../models/category.product';
import { TrademarkModel } from '../../../models/trademark.model';
import { ProductImage } from '../../../models/product.image';
import {
  AbstractControl,
  FormArray,
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { Title } from '@angular/platform-browser';
import { ProductsService } from '../../../service/products.service';
import { CategoryService } from '../../../service/category-product.service';
import { TrademarkService } from '../../../service/trademark.service';
import { ToastrService } from 'ngx-toastr';
import Swal from 'sweetalert2';
import { ProductDTO } from '../../../dtos/product.dto';
import { CustomerModel } from '../../../models/customer.model';
import { CustomerService } from '../../../service/customer.service';
import { ShoppingCartDTO } from '../../../dtos/shopping.cart.dto.';

@Component({
  selector: 'app-order-detail',
  templateUrl: './order-detail.component.html',
  styleUrl: './order-detail.component.css',
})
export class OrderDetailComponent implements OnInit {
  protected readonly Environment = Environment;
  protected readonly Utils = Utils;
  product: ProductModel | undefined = undefined;
  totalMoney: number = 0;
  titleString: string = '';
  isReadOnly: boolean = true;
  shoppingcartdto!: ShoppingCartDTO;
  strError: string = '';

  // getData khách hàng
  customers: CustomerModel[] = [];
  isDisplayNone: boolean = false;
  oderStatusValue: any;

  btnSave: string = '';
  strMessage: string = '';
  trxTransactionForm: FormGroup = new FormGroup({
    transactionId: new FormControl('', [Validators.required]),
    orderStatus: new FormControl('', [Validators.required]),
    paymentStatus: new FormControl('', [Validators.required]),
  });

  constructor(
    private title: Title,
    private tokenService: TokenService,
    private trxTransactionService: TrxTransactionService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private toastr: ToastrService,
    private fb: FormBuilder
  ) {}
  ngOnInit(): void {
    this.oderStatusValue = Utils.statusOrder;
    const id = this.activatedRoute.snapshot.params['id'];
    if (id) {
      this.titleString = 'Chi tiết hóa đơn';
      this.btnSave = 'Cập nhật';
      this.trxTransactionService.GetTrxTransactionFindById(id).subscribe({
        next: (response: any) => {
          this.shoppingcartdto = response.data;
          console.log('shoppingcartdto', this.shoppingcartdto);
          this.changeValue(this.shoppingcartdto.trxTransaction.orderStatus);
          // set data vào trxTransactionForm
          this.trxTransactionForm.setValue({
            transactionId: this.shoppingcartdto.trxTransaction.id.toString(),
            paymentStatus: this.shoppingcartdto.trxTransaction.paymentStatus,
            orderStatus: this.shoppingcartdto.trxTransaction.orderStatus,
          });
        },
        error: (error: any) => {
          this.toastr.error('Lỗi lấy thông tin giỏ hàng', 'Thông báo');
        },
      });
    }
    this.trxTransactionForm.get('orderStatus')?.valueChanges.subscribe({
      next: (value: any) => {
        if (value === null) {
          this.strError = 'Trạng thái đơn hàng là bắt buộc';
        }
      },
    });
  }
  valueStatusOrder: any;
  changeValue(value: any) {
    // thực hiện gán giá trị vào valueStatusOrder theo value truyền vào
    // PENDING => List data là danh sách ONHOLD, CANCELLED
    // ONHOLD => List data là danh sách WAITINGFORPICKUP, CANCELLED
    // WAITINGFORPICKUP => List data là danh sách WAITINGFORDELIVERY
    // WAITINGFORDELIVERY => List data là danh sách DELIVERED

    // Nếu là chưa xác nhận => sẽ là đã xác nhận, chờ lấy hàng, đã hủy
    // Nếu là đã xác nhận => sẽ là chờ lấy hàng, đã hủy
    // Nếu là chờ lấy hàng => sẽ là đang giao hàng, đã giao hàng
    // Nếu là đang giao hàng => sẽ là đã giao hàng
    // Nếu là đã giao hàng => sẽ là đã giao hàng
    


    if (value === 'PENDING') {
      this.valueStatusOrder = [
        { name: 'PENDING', value: 'Chờ xác nhận' },
        { name: 'ONHOLD', value: 'Đã xác nhận' },
        { name: 'CANCELLED', value: 'Đã hủy' },
      ];
    } else if (value === 'ONHOLD') {
      this.valueStatusOrder = [
        { name: 'ONHOLD', value: 'Đã xác nhận' },
        { name: 'WAITINGFORPICKUP', value: 'Chờ lấy hàng' },
        { name: 'CANCELLED', value: 'Đã hủy' },
      ];
    } else if (value === 'WAITINGFORPICKUP') {
      this.valueStatusOrder = [
        { name: 'WAITINGFORPICKUP', value: 'Đang chờ lấy hàng' },
        { name: 'WAITINGFORDELIVERY', value: 'Đang giao hàng' },
      ];
    } else if (value === 'WAITINGFORDELIVERY') {
      this.valueStatusOrder = [
        { name: 'WAITINGFORDELIVERY', value: 'Đang giao hàng' },
        { name: 'DELIVERED', value: 'Đã giao hàng' },
      ];
    } else if (value === 'CANCELLED') {
      this.valueStatusOrder = [{ name: 'CANCELLED', value: 'Đã hủy' }];
    } else {
      this.valueStatusOrder = [{ name: 'DELIVERED', value: 'Đã hủy' }];
    }
  }

  onSubmit() {
    // Kiểm tra form đã hợp lệ chưa
    if (this.trxTransactionForm.invalid) {
      this.toastr.error('Vui lòng điền đầy đủ thông tin', 'Thông báo');
      return;
    }

    // Gọi API cập nhật trạng thái đơn hàng
    this.trxTransactionService
      .updateTrxTransaction(this.trxTransactionForm.value)
      .subscribe({
        next: (response: any) => {
          if (response.message === 'Cập nhật đơn hàng thành công') {
            this.toastr.success(
              'Cập nhật trạng thái đơn hàng thành công',
              'Thông báo'
            );
            this.router.navigate(['/admin/list_order']);
          } else {
            this.toastr.error(response.message, 'Thông báo');
          }
        },
        error: (error: any) => {
          this.toastr.error(
            'Cập nhật trạng thái đơn hàng thất bại',
            'Thông báo'
          );
        },
      });
  }
}
