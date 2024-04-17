import { TrxTransactionService } from './../../../service/trx-transaction.service';
import { TokenService } from './../../../service/token.service';
import { ActivatedRoute, Router } from '@angular/router';
import {
  AfterViewInit,
  Component,
  ElementRef,
  OnInit,
  ViewChild,
} from '@angular/core';
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
import { ShipmentData } from '../../../models/shipment.data';
import {
  Address,
  ShipmentDTO,
  createShipmentDTO,
} from '../../../dtos/go.ship.dto';

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrl: './order.component.css',
})
export class OrderComponent implements OnInit {
  protected readonly Environment = Environment;
  protected readonly Utils = Utils;
  product: ProductModel | undefined = undefined;
  shipment: ShipmentDTO | null = null;

  @ViewChild('btnCloseModal') btnCloseModal!: ElementRef;

  totalMoney: number = 0;
  titleString: string = '';
  productImage: ProductImage[] = [];
  statusShip: boolean = false;
  // data gốc
  productdtos: ProductDTO[] = [];
  products: ProductModel[] = [];

  // getData khách hàng
  customers: CustomerModel[] = [];
  customer: CustomerModel | undefined;
  isDisplayNone: boolean = false;
  btnSave: string = '';
  strMessage: string = '';
  shipData: ShipmentData[] = [];
  priceShip = 0;
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
    amount: new FormControl(0),
    checkOrder: new FormControl(true),
    carts: new FormArray([
      new FormGroup({
        id: new FormControl(0),
        userId: new FormControl(0),
        price: new FormControl(null, [
          Validators.required,
          Validators.min(0),
          Validators.max(1000000000),
        ]),
        quantity: new FormControl(null, [
          Validators.required,
          Validators.min(1),
          Validators.max(1000),
        ]),
        productId: new FormControl(null),
      }),
    ]),
  });

  addressForm: FormGroup = new FormGroup({
    city: new FormControl(null, [Validators.required]),
    district: new FormControl(null, [Validators.required]),
    ward: new FormControl(null, [Validators.required]),
    addressDetail: new FormControl('', [
      Validators.required,
      Validators.minLength(2),
      Validators.maxLength(100),
    ]),
  });

  // static paymentTypes = [
  //   { name: 'PAYMENTONDELIVERY', value: 'Thanh toán khi nhận hàng' },
  //   { name: 'PAYMENTVIACARD', value: 'Thanh toán qua thẻ' },
  //   { name: 'CASH', value: 'Tiền mặt' },
  // ];
  // paymentTypes: any;

  paymentTypes = Utils.paymentTypes.filter(
    (x) => x.name !== 'PAYMENTONDELIVERY'
  );
  // nếu trạng thái là
  // lấy ra paymentTypes và không lấy ra PAYMENTONDELIVERY
  // get paymentTypes() {
  //   return OrderComponent.paymentTypes.filter(
  //     (paymentType) => paymentType.name !== 'PAYMENTONDELIVERY'
  //   );
  // }

  cities: any;
  districts: any;
  wards: any;
  onAddress() {
    this.trxTransactionForm
      .get('address')
      ?.setValue(
        this.addressForm.get('addressDetail')?.value +
          ', ' +
          this.getAddressFull(
            this.addressForm.get('city')?.value,
            this.addressForm.get('district')?.value,
            this.addressForm.get('ward')?.value
          )
      );
    console.log('address: ', this.addressForm.value);
    this.priceGoShip();
    this.addressForm.reset();
    this.btnCloseModal.nativeElement.click();
  }

  getJsonDataAddress() {
    this.trxTransactionService.getJsonDataAddress().subscribe({
      next: (response) => {
        this.cities = response;
        console.log('địa chỉ:', response);
      },
      error: (error) => {
        console.log(error);
      },
    });
  }

  isOptionDisabled(option: any) {
    return option.name === 'PENDING';
  }

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
      orderStatus: new FormControl('', [Validators.required]),
      paymentTypes: new FormControl('', [Validators.required]),
      notes: new FormControl(''),
      amount: new FormControl(0),
      checkOrder: new FormControl(true),
      carts: this.fb.array([this.createReceiptDetail()]),
    });
    this.trxTransactionForm.get('address')?.setValue('Cửa Hàng');
    this.trxTransactionForm.get('phoneNumber')?.setValue('0123456789');
    this.trxTransactionForm.get('fullName')?.setValue('Khách Lẻ');
    this.trxTransactionForm
      .get('employeeId')
      ?.setValue(this.tokenService.getUserEmail());
  }

  totalPrice() {
    const cartsArray = this.trxTransactionForm.get('carts') as FormArray;
    cartsArray.valueChanges.subscribe(() => {
      this.totalMoney = this.getTotalMoney();
    });
    this.totalMoney += this.priceShip;
  }

  ngOnInit(): void {
    this.titleString = 'Thêm sản phẩm';
    this.btnSave = 'Thêm mới';
    this.trxTransactionForm.get('customerId')?.setValue('khachle@gmail.com');

    this.getDatacombobox();
    this.totalPrice();

    this.getJsonDataAddress();

    this.addressForm.get('city')?.valueChanges.subscribe((id: any) => {
      const filteredDistricts = Utils.district?.filter(
        (district: any) => district.city_id === id
      );

      // Gán danh sách quận lọc được cho this.districts
      this.districts = filteredDistricts || [];

      // Đặt giá trị mặc định cho trường 'district' trong form nếu cần
      if (this.districts.length > 0) {
        this.addressForm.get('district')?.setValue(this.districts[0]?.id);
        // lấy ra các giá trị xã phường tương ứng với quận
        // const filteredWards = Utils.wards?.filter(
        //   (ward: any) => ward.district_id === this.districts[0]?.id
        // );
        // this.wards = filteredWards || [];
        // // Đặt giá trị mặc định cho trường 'ward' trong form nếu cần
        // if (this.wards.length > 0) {
        //   this.addressForm.get('ward')?.setValue(this.wards[0]?.id);
        // }
      }
    });

    this.addressForm.get('district')?.valueChanges.subscribe((id: any) => {
      const filteredWards = Utils.wards?.filter(
        (ward: any) => ward.district_id === id
      );
      this.wards = filteredWards || [];
      // Đặt giá trị mặc định cho trường 'ward' trong form nếu cần
      if (this.wards.length > 0) {
        this.addressForm.get('ward')?.setValue(this.wards[0]?.id);
      }
    });

    this.trxTransactionForm
      .get('customerId')
      ?.valueChanges.subscribe((id: any) => {
        this.customer = this.customers.find(
          (customer) => customer.userId === id
        );
        console.log('data address :', this.cities);
        this.addressForm.get('city')?.setValue(this.customer?.city);

        console.log('data districts :', this.districts);

        this.addressForm.get('district')?.setValue(this.customer?.district);
        console.log('data wards :', this.wards);
        this.addressForm.get('ward')?.setValue(this.customer?.ward);

        this.trxTransactionForm
          .get('address')
          ?.setValue(
            this.customer?.address +
              ', ' +
              this.getAddressFull(
                this.customer?.city,
                this.customer?.district,
                this.customer?.ward
              )
          );
        this.priceGoShip();

        // khi paymentTypes thay đổi có giá trị là PAYMENTONDELIVERY thì statusShip = true
        // this.trxTransactionForm
        //   .get('paymentTypes')
        //   ?.valueChanges.subscribe((paymentTypes: any) => {
        //     if (paymentTypes === 'PAYMENTONDELIVERY') {
        //       this.statusShip = true;
        //     } else {
        //       this.statusShip = false;
        //     }
        //   });
      });

    // this.addressForm.get('city')?.setValue(this.customer?.city);
    // this.addressForm.get('district')?.setValue(this.customer?.district);
    // this.addressForm.get('ward')?.setValue(this.customer?.ward);
    // this.addressForm.get('addressDetail')?.setValue(this.customer?.address);

    // this.addressForm.get('city')?.valueChanges.subscribe((id: any) => {
    //   const filteredDistricts = Utils.district?.filter(
    //     (district: any) => district.city_id === id
    //   );

    //   // Gán danh sách quận lọc được cho this.districts
    //   this.districts = filteredDistricts || [];

    //   // Đặt giá trị mặc định cho trường 'district' trong form nếu cần
    //   if (this.wards.length > 0) {
    //     this.addressForm.get('ward')?.setValue(this.wards[0]?.id);
    //   }
    // });
  }
  onChangePaymentTypes(event: any) {
    console.log('change payment type', event.name);
    if (event.name === 'PAYMENTONDELIVERY') {
      this.statusShip = true;
      this.totalPrice();
    } else {
      this.statusShip = false;
      this.priceShip = 0;
      this.totalPrice();
    }
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

  // viết hàm từ id ra name của các city, district, ward trả về string để hiển thị các tham số truyền vào là city: any district: any ward: any
  getAddressFull(city: any, district: any, ward: any): string {
    const cityName = Utils.city?.find((c) => c.id === city)?.name;
    const districtName = Utils.district?.find((d) => d.id === district)?.name;
    const wardName = Utils.wards?.find((w) => w.id === ward)?.name;
    return `${wardName}, ${districtName}, ${cityName}`;
  }

  onSubmit() {
    // set giá trị cho các trường trong form trước khi submit

    // nếu trxTransactionForm.get('customerId') có giá trị là khachle@gmail.com thì set các giá trị từ customer

    if (
      this.trxTransactionForm.get('customerId')?.value !== 'khachle@gmail.com'
    ) {
      this.trxTransactionForm
        .get('fullName')
        ?.setValue(this.customer?.fullName) ?? '';
      this.trxTransactionForm
        .get('phoneNumber')
        ?.setValue(this.customer?.phoneNumber) ?? '';
    }

    this.trxTransactionForm.get('amount')?.setValue(this.getTotalMoney());
    console.log(this.trxTransactionForm.value);

    if (this.trxTransactionForm.invalid) {
      return;
    }
    console.log('qua day');
    this.createProduct();
  }

  createProduct() {
    this.trxTransactionService
      .PaymentExecute(this.trxTransactionForm.value)
      .subscribe({
        next: (response: any) => {
          if (
            response.data &&
            typeof response.data === 'string' &&
            (response.data.startsWith('http://') ||
              response.data.startsWith('https://'))
          ) {
            window.location.href = response.data;
            // this.titleStatus='Đã thanh toán';
          } else {
            debugger;
            this.toastr.success('Thêm hóa đơn thành công!', 'Thông báo');
            this.isDisplayNone = false;
            this.router.navigate(['/admin/list_order']);
            // Handle other cases, such as routing to another page
            // For example, you can use this.router.navigate([some_other_route]);
          }
        },
        error: (error: any) => {
          this.toastr.error('Lỗi đặt hàng', 'Thông báo');
        },
      });
  }

  getCustomer(): FormControl {
    const customerId = this.trxTransactionForm.get('customerId') as FormControl;
    // trả customerId = customer.userId trong
    customerId.valueChanges.pipe().subscribe((userId: any) => {
      this.customer = this.customers.find(
        (customer) => customer.userId === userId
      );
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
      quantity: [
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
    const allFieldsFilled = this.carts.controls.every((control) => {
      const productId = control.get('productId')?.value;
      const price = control.get('price')?.value;
      const total = control.get('quantity')?.value;
      return productId && price && total;
    });
    // kiểm tra xem các trường đã được điền đầy đủ chưa
    if (!allFieldsFilled) {
      // this.toastr.error('Vui lòng điền đầy đủ thông tin sản phẩm');
      this.strMessage = 'Vui lòng điền đầy đủ thông tin';
      return;
    }
    this.strMessage = '';
    this.updateProductSelect();
    this.carts.push(this.createReceiptDetail());
  }

  updateProductSelect() {
    // lấy ra sản phẩm được chọn trong carts
    let selectedProducts = this.trxTransactionForm.value.carts.map(
      (detail: { productId: any }) => detail.productId
    );
    console.log('listItemId: ', selectedProducts.toString());
    if (!Array.isArray(selectedProducts)) {
      selectedProducts = [selectedProducts];
    }

    console.log('Các productId đã chọn: ', selectedProducts.toString());

    this.products = this.productdtos
      .map((productDto) => productDto.product)
      .filter((product) => !selectedProducts.includes(product.id));

    console.log('Mảng sản phẩm được cập nhật:', selectedProducts);
    console.log('mảng product update', this.products);
  }

  getTotalMoney(): number {
    const carts = this.trxTransactionForm.get('carts') as FormArray;
    let total = 0;

    carts.controls.forEach((control: AbstractControl<any, any>) => {
      const price = (control.get('price') as FormControl)?.value || 0;
      const quantity = (control.get('quantity') as FormControl)?.value || 0;
      total += price * quantity;
    });

    return total;
  }

  getTotalPrice(): number[] {
    const carts = this.trxTransactionForm.get('carts') as FormArray;
    const totals: number[] = [];
    carts.controls.forEach((control: AbstractControl<any, any>) => {
      const price = (control.get('price') as FormControl)?.value || 0;
      const quantity = (control.get('quantity') as FormControl)?.value || 0;
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
      formGroup.get('quantity')?.setValue(1);
    }
  }
  priceGoShip() {
    const userAddressTo: Address = {
      city: this.addressForm.value.city,
      district: this.addressForm.value.district,
      wards: this.addressForm.value.ward,
    };
    console.log('userAddressTo', userAddressTo);

    this.shipment = createShipmentDTO(userAddressTo);
    this.trxTransactionService.listGoShip(this.shipment).subscribe({
      next: (response: any) => {
        this.shipData = response.data;
        console.log('shipData', this.shipData);
      },
      error: (error: any) => {
        console.log(error);
      },
    });
  }
  removeProduct(index: number) {
    if (this.carts.length > 1) {
      this.carts.removeAt(index);
      this.updateProductSelect();
    }
  }
  onGoShip(event: any) {
    this.priceShip = event.totalAmount;
  }
}
