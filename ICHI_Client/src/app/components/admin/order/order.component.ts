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

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrl: './order.component.css',
})
export class OrderComponent implements OnInit {
  protected readonly Environment = Environment;
  protected readonly Utils = Utils;
  product: ProductModel | undefined = undefined;
  totalMoney: number = 0;
  titleString: string = '';
  productImage: ProductImage[] = [];

  // data gốc
  productdtos: ProductDTO[] = [];
  products: ProductModel[] = [];

  // getData khách hàng
  customers: CustomerModel[] = [];
  customer: CustomerModel | undefined;

  isDisplayNone: boolean = false;
  btnSave: string = '';
  trxTransactionForm: FormGroup = new FormGroup({
    trxTransactionId: new FormControl(0),
    customerId: new FormControl(''),
    employeeId: new FormControl('', [Validators.required]),
    fullName: new FormControl(''),
    phoneNumber: new FormControl(''),
    address: new FormControl(''),
    orderStatus: new FormControl(''),
    paymentTypes: new FormControl('', [Validators.required]),
    notes: new FormControl(''),
    carts: new FormArray([
      new FormGroup({
        id: new FormControl(0),
        userId: new FormControl(0),
        price: new FormControl(null, [
          Validators.required,
          Validators.min(0),
          Validators.max(1000000000),
        ]),
        total: new FormControl(null, [
          Validators.required,
          Validators.min(1),
          Validators.max(1000),
        ]),
        productId: new FormControl(null),
      }),
    ]),
  });

  constructor(
    private title: Title,
    private productService: ProductsService,
    private tokenService: TokenService,
    private trxTransactionService: TrxTransactionService,
    private customerService: CustomerService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private toastr: ToastrService,
    private fb: FormBuilder
  ) {
    this.trxTransactionForm = this.fb.group({
      trxTransactionId: new FormControl(0),
      customerId: new FormControl(''),
      employeeId: new FormControl('', [Validators.required]),
      fullName: new FormControl(''),
      phoneNumber: new FormControl(''),
      address: new FormControl(''),
      orderStatus: new FormControl(''),
      paymentTypes: new FormControl('', [Validators.required]),
      notes: new FormControl(''),
      carts: this.fb.array([this.createReceiptDetail()]),
    });
  }
  ngOnInit(): void {
    this.titleString = 'Thêm sản phẩm';
    this.btnSave = 'Thêm mới';
    this.getDatacombobox();

    const cartsArray = this.trxTransactionForm.get('carts') as FormArray;

    cartsArray.valueChanges.subscribe(() => {
      this.totalMoney = this.getTotalMoney();
    });
  }

  getDatacombobox() {
    this.productService.findAll().subscribe((data: any) => {
      this.productdtos = data.data.items;
      this.products = this.productdtos.map((product) => product.product);
      this.products;
      console.log('products', this.products);
    });
    this.customerService.findAll().subscribe((data: any) => {
      this.customers = data.data.items;
      // console.log(data.data);
      console.log('customer', this.customers);
    });
  }

  onSubmit() {
    // set giá trị cho các trường trong form trước khi submit
    this.trxTransactionForm
      .get('employeeId')
      ?.setValue(this.tokenService.getUserEmail());
    console.log(this.trxTransactionForm.value);
    if (this.trxTransactionForm.invalid) {
      return;
    }
    console.log('qua day');
    this.createProduct();
  }

  createProduct() {}

  getDistrictsControl(): FormControl {
    const customerId = this.trxTransactionForm.get('customerId') as FormControl;

    customerId.valueChanges.pipe().subscribe((id: any) => {
      this.customer = this.customers.find((customer) => customer.id === id);
    });
    return customerId;
  }
  get carts() {
    return this.trxTransactionForm.get('carts') as FormArray;
  }

  createReceiptDetail(): FormGroup {
    return this.fb.group({
      id: [0],
      userId: ['khachle@gmail.com'],
      price: [
        null,
        [Validators.required, Validators.min(0), Validators.max(1000000000)],
      ],
      total: [
        null,
        [
          Validators.required,
          Validators.min(1),
          Validators.max(1000),
          this.customTotalValidator.bind(this),
        ],
      ],
      productId: [null],
    });
  }

  customTotalValidator(control: any) {
    if (control.value > 1000) {
      return { maxExceeded: true };
    }
    return null;
  }

  addProduct() {
    // Kiểm tra tất cả các trường có giá trị không
    const allFieldsFilled = this.carts.controls.every((control) => {
      const productId = control.get('productId')?.value;
      const price = control.get('price')?.value;
      const total = control.get('total')?.value;
      return productId && price && total;
    });
    this.updateProductSelect();
    this.carts.push(this.createReceiptDetail());
  }

  updateProductSelect() {
    // lấy ra sản phẩm được chọn trong carts
    let selectedProducts = this.trxTransactionForm.value.carts.map(
      (detail: { productId: any }) => detail.productId
    );
    console.log('listItemId: ', selectedProducts.toString());

    // this.products = this.productdtos
    //   .map((product) => product.product)
    //   .filter(
    //     (product) =>
    //       !selectedProducts.some((selectedId: any) =>
    //         selectedId.includes(product.id)
    //       )
    //   );
    if (!Array.isArray(selectedProducts)) {
      selectedProducts = [selectedProducts];
    }

    console.log('Các productId đã chọn: ', selectedProducts.toString());

    // Cập nhật mảng products bằng cách lọc ra các sản phẩm chưa được chọn từ mảng gốc là productdtos.product và đảm bảo không trùng với selectedProducts
    this.products = this.productdtos
      .map((productDto) => productDto.product)
      .filter((product) => !selectedProducts.includes(product.id));

    console.log('Mảng sản phẩm được cập nhật:', selectedProducts);
    console.log('mảng product update', this.products);
  }

  // onProductSelection(event: any) {
  //   // Xử lý sự kiện khi người dùng chọn một sản phẩm từ ng-select
  //   const unselectedProducts = this.products.filter((product) =>
  //     this.trxTransactionForm.value.promotionDetails.every(
  //       (detail: { productId: any }) => detail.productId !== product.id
  //     )
  //   );
  //   console.log('chưa chọn', unselectedProducts);
  //   // Cập nhật danh sách sản phẩm chỉ hiển thị các sản phẩm chưa được chọn
  //   this.products = unselectedProducts;
  // }
  getTotalMoney(): number {
    const carts = this.trxTransactionForm.get('carts') as FormArray;
    let total = 0;

    carts.controls.forEach((control: AbstractControl<any, any>) => {
      const price = (control.get('price') as FormControl)?.value || 0;
      const quantity = (control.get('total') as FormControl)?.value || 0;
      total += price * quantity;
    });

    return total;
  }
  getTotalPrice(): number[] {
    const carts = this.trxTransactionForm.get('carts') as FormArray;
    const totals: number[] = [];

    carts.controls.forEach((control: AbstractControl<any, any>) => {
      const price = (control.get('price') as FormControl)?.value || 0;
      const quantity = (control.get('total') as FormControl)?.value || 0;
      const total = price * quantity;
      totals.push(total);
    });

    return totals;
  }

  onProductSelection(event: any, index: number) {
    let pricePromotion;
    const receiptDetails = this.trxTransactionForm.get('carts') as FormArray;
    const formGroup = receiptDetails.at(index) as FormGroup;
    if (event.discount > 0) {
      pricePromotion = event.price - event.price * (event.discount / 100);
    } else {
      pricePromotion = event.price;
    }

    if (formGroup !== null) {
      formGroup.get('price')?.setValue(pricePromotion);
      // formGroup.get('productId')?.setValue(event.id);
      formGroup.get('total')?.setValue(1);
    }
  }

  removeProduct(index: number) {
    if (this.carts.length > 1) {
      this.carts.removeAt(index);
      this.updateProductSelect();
    }
  }
}
