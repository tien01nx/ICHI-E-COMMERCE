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

  shoppingcartdto!: ShoppingCartDTO;

  // getData khách hàng
  customers: CustomerModel[] = [];
  isDisplayNone: boolean = false;
  btnSave: string = '';
  strMessage: string = '';

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
    const id = this.activatedRoute.snapshot.params['id'];
    if (id) {
      this.titleString = 'Chi tiết hóa đơn';
      this.btnSave = 'Cập nhật';
      this.trxTransactionService.GetTrxTransactionFindById(id).subscribe({
        next: (response: any) => {
          this.shoppingcartdto = response.data;
          console.log('shoppingcartdto', this.shoppingcartdto);
          // thực hiện set data vào trxTransactionForm
        },
        error: (error: any) => {
          this.toastr.error('Lỗi lấy thông tin giỏ hàng', 'Thông báo');
        },
      });
    }
  }

  onSubmit() {}
}
