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
  selectedImageUrl: string = '';
  selectedImageFile: File = new File([''], 'filename');
  selectedImageProductUrl: string[] = [];
  selectedImageProductFiles: File[] = [];
  trademarks: TrademarkModel[] = [];
  productImage: ProductImage[] = [];

  // data gốc
  productdtos: ProductDTO[] = [];
  products: ProductModel[] = [];

  // getData khách hàng
  customers: CustomerModel[] = [];
  customer: CustomerModel | undefined;

  color: any;
  selectedItem: any; // Biến để lưu trữ giá trị được chọn

  isDisplayNone: boolean = false;
  btnSave: string = '';
  productForm: FormGroup = new FormGroup({
    id: new FormControl(0),
    trademarkId: new FormControl(null, [Validators.required]),
    userId: new FormControl(null, [Validators.required]),
    productName: new FormControl('', [
      Validators.required,
      Validators.maxLength(50),
    ]),
    description: new FormControl('', [Validators.required]),
    price: new FormControl(0),
    imageProductFiles: new FormControl(null, [Validators.required]),
    color: new FormControl('', [Validators.maxLength(30)]),
    priorityLevel: new FormControl(0, [Validators.required]),
    notes: new FormControl('', [Validators.maxLength(200)]),
    isActive: new FormControl(true, [Validators.required]),
    quantity: new FormControl(0, [Validators.required]),
    inventoryReceiptDetails: new FormArray([
      new FormGroup({
        id: new FormControl(0),
        inventoryReceiptId: new FormControl(0),
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
    // isActive: new FormControl('false', [Validators.required]),
  });

  constructor(
    private title: Title,
    private productService: ProductsService,
    private categoryService: CategoryService,
    private customerService: CustomerService,
    private trademarkService: TrademarkService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private toastr: ToastrService,
    private fb: FormBuilder
  ) {
    // this.receiptForm = this.fb.group({
    //   id: [0],
    //   notes: ['', Validators.required],
    //   employeeId: [null, Validators.required],
    //   supplierId: [null, Validators.required],
    //   inventoryReceiptDetails: this.fb.array([this.createReceiptDetail()]),
    // });
  }
  ngOnInit(): void {
    this.titleString = 'Thêm sản phẩm';
    this.btnSave = 'Thêm mới';
    this.getDatacombobox();

    const inventoryReceiptDetailsArray = this.productForm.get(
      'inventoryReceiptDetails'
    ) as FormArray;

    inventoryReceiptDetailsArray.valueChanges.subscribe(() => {
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
    if (this.productForm.invalid) {
      return;
    }
    this.createProduct();
  }

  createProduct() {
    this.productService
      .create(this.productForm.value, this.selectedImageProductFiles)
      .subscribe({
        next: (respon: any) => {
          if (
            respon.message === 'Tạo mới thành công' ||
            respon.message === 'Cập nhật sản phẩm thành công'
          ) {
            this.toastr.success(respon.message, 'Thành công');
            this.router.navigateByUrl('/admin/products');
          } else {
            this.toastr.error(respon.message, 'Thất bại');
          }
        },
        error: (err: any) => {
          this.toastr.error(err.error, 'Thất bại');
        },
      });
  }

  getDistrictsControl(): FormControl {
    const customerId = this.productForm.get('userId') as FormControl;

    customerId.valueChanges.pipe().subscribe((id: any) => {
      this.customer = this.customers.find((customer) => customer.id === id);
    });
    return customerId;
  }
  get inventoryReceiptDetails() {
    return this.productForm.get('inventoryReceiptDetails') as FormArray;
  }

  createReceiptDetail(): FormGroup {
    return this.fb.group({
      id: [0],
      inventoryReceiptId: [0],
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
    const allFieldsFilled = this.inventoryReceiptDetails.controls.every(
      (control) => {
        const productId = control.get('productId')?.value;
        const price = control.get('price')?.value;
        const total = control.get('total')?.value;
        return productId && price && total;
      }
    );
    this.updateProductSelect();
    this.inventoryReceiptDetails.push(this.createReceiptDetail());
  }

  updateProductSelect() {
    // lấy ra sản phẩm được chọn trong inventoryReceiptDetails
    let selectedProducts = this.productForm.value.inventoryReceiptDetails.map(
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
  //     this.productForm.value.promotionDetails.every(
  //       (detail: { productId: any }) => detail.productId !== product.id
  //     )
  //   );
  //   console.log('chưa chọn', unselectedProducts);
  //   // Cập nhật danh sách sản phẩm chỉ hiển thị các sản phẩm chưa được chọn
  //   this.products = unselectedProducts;
  // }
  getTotalMoney(): number {
    const inventoryReceiptDetails = this.productForm.get(
      'inventoryReceiptDetails'
    ) as FormArray;
    let total = 0;

    inventoryReceiptDetails.controls.forEach(
      (control: AbstractControl<any, any>) => {
        const price = (control.get('price') as FormControl)?.value || 0;
        const quantity = (control.get('total') as FormControl)?.value || 0;
        total += price * quantity;
      }
    );

    return total;
  }
  getTotalPrice(): number[] {
    const inventoryReceiptDetails = this.productForm.get(
      'inventoryReceiptDetails'
    ) as FormArray;
    const totals: number[] = [];

    inventoryReceiptDetails.controls.forEach(
      (control: AbstractControl<any, any>) => {
        const price = (control.get('price') as FormControl)?.value || 0;
        const quantity = (control.get('total') as FormControl)?.value || 0;
        const total = price * quantity;
        totals.push(total);
      }
    );

    return totals;
  }

  onProductSelection(event: any, index: number) {
    let pricePromotion;
    const receiptDetails = this.productForm.get(
      'inventoryReceiptDetails'
    ) as FormArray;
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
    if (this.inventoryReceiptDetails.length > 1) {
      this.inventoryReceiptDetails.removeAt(index);
      this.updateProductSelect();
    }
  }
}
