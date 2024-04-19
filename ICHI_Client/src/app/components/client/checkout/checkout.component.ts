import { ShipmentDTO } from './../../../dtos/go.ship.dto';
import { CartModel } from './../../../models/cart.model';
import { Environment } from './../../../environment/environment';
import {
  Component,
  DEFAULT_CURRENCY_CODE,
  ElementRef,
  OnInit,
  ViewChild,
} from '@angular/core';
import { ClientFooterComponent } from '../client-footer/client-footer.component';
import { ClientMenuComponent } from '../client-menu/client-menu.component';
import { ClientHeaderComponent } from '../client-header/client-header.component';
import { TrxTransactionService } from '../../../service/trx-transaction.service';
import { ToastrService } from 'ngx-toastr';
import { TokenService } from '../../../service/token.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ShoppingCartDTO } from '../../../dtos/shopping.cart.dto.';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { TrxTransactionDTO } from '../../../dtos/trxtransaction.dto';
import { CartProductDTO } from '../../../dtos/cart.product.dto';
import { Utils } from '../../../Utils.ts/utils';
import { createShipmentDTO, Address } from '../../../dtos/go.ship.dto';
import { ShipmentData } from '../../../models/shipment.data';
import { __makeTemplateObject } from 'tslib';
@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrl: './checkout.component.css',
})
export class CheckoutComponent implements OnInit {
  shipment: ShipmentDTO | null = null;

  protected readonly Utils = Utils;
  shoppingcartdto!: ShoppingCartDTO;
  cartProductDTO!: CartProductDTO;
  @ViewChild('btnCloseModal') btnCloseModal!: ElementRef;
  shipData: ShipmentData[] = [];
  carts: CartModel[] = [];
  // shipdata : ShipmentData[] = [];
  priceDiscount: number = 0;
  priceShip = 0;
  paymentsType: any;
  Environment = Environment;
  trxTransactionDTO!: TrxTransactionDTO;
  titleStatus: string = '';
  isDisplayNone: boolean = false;
  trxTransacForm = new FormGroup({
    customerId: new FormControl(''),
    fullName: new FormControl('', [
      Validators.required,
      Validators.maxLength(50),
    ]),
    phoneNumber: new FormControl('', [
      Validators.required,
      Validators.maxLength(10),
      Validators.minLength(10),
      Validators.pattern('^0[0-9]{9}$'),
    ]),
    address: new FormControl(''),
    shipData: new FormControl(''),
    paymentTypes: new FormControl('', [Validators.required]),
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

  constructor(
    private cartService: TrxTransactionService,
    private toastr: ToastrService,
    private tokenService: TokenService,
    private activatedRoute: ActivatedRoute,
    private router: Router
  ) {}
  cities: any;
  districts: any;
  wards: any;

  onAddress() {
    console.log('test dataa');
    this.trxTransacForm
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

  getAddressFull(city: any, district: any, ward: any): string {
    const cityName = Utils.city?.find((c) => c.id === city)?.name;
    const districtName = Utils.district?.find((d) => d.id === district)?.name;
    const wardName = Utils.wards?.find((w) => w.id === ward)?.name;
    return `${wardName}, ${districtName}, ${cityName}`;
  }

  ngOnInit(): void {
    if (this.activatedRoute.snapshot.params['id'] === undefined) {
      this.getInitData();
    } else {
      this.getInitDataId(this.activatedRoute.snapshot.params['id']);
    }
    this.paymentsType = Utils.paymentTypes.filter((x) => x.name !== 'CASH');
    this.addressForm.get('city')?.valueChanges.subscribe((id: any) => {
      const filteredDistricts = Utils.district?.filter(
        (district: any) => district.city_id === id
      );
      this.districts = filteredDistricts || [];
      if (this.districts.length > 0) {
        this.addressForm.get('district')?.setValue(this.districts[0]?.id);
      }
    });

    this.addressForm.get('district')?.valueChanges.subscribe((id: any) => {
      const filteredWards = Utils.wards?.filter(
        (ward: any) => ward.district_id === id
      );
      this.wards = filteredWards || [];
      if (this.wards.length > 0) {
        this.addressForm.get('ward')?.setValue(this.wards[0]?.id);
      }
    });
  }

  getInitDataId(id: number) {
    this.cartService.GetTrxTransactionFindById(id).subscribe({
      next: (response: any) => {
        console.log('dataPay', response.data);
        this.shoppingcartdto = response.data;
        this.trxTransacForm.setValue({
          customerId: this.tokenService.getUserEmail(),
          phoneNumber: this.shoppingcartdto.trxTransaction.phoneNumber,
          fullName: this.shoppingcartdto.trxTransaction.fullName,
          address: this.shoppingcartdto.trxTransaction.address,
          shipData: '',
          paymentTypes: this.shoppingcartdto.trxTransaction.paymentTypes,
        });
        console.log('object11', this.trxTransacForm.value);
        if (
          this.shoppingcartdto &&
          this.shoppingcartdto.trxTransaction.paymentStatus === 'APPROVED'
        ) {
          this.titleStatus = 'Đã thanh toán';
        }
        if (
          this.shoppingcartdto &&
          this.shoppingcartdto.trxTransaction.paymentStatus === 'PENDING'
        ) {
          this.isDisplayNone = true;
          this.titleStatus = 'Thanh toán không thành công vui lòng thử lại';
        }
        console.log('id', this.shoppingcartdto);
      },
      error: (error: any) => {
        this.toastr.error('Lỗi lấy thông tin giỏ hàng', 'Thông báo');
      },
    });
  }

  getInitData() {
    this.cartProductDTO = new CartProductDTO(
      this.tokenService.getUserEmail(),
      this.cartService.getCarts()
    );

    this.cartService.GetTrxTransaction(this.cartProductDTO).subscribe({
      next: (response: any) => {
        console.log('responsecheck', response.data);
        this.shoppingcartdto = response.data;

        this.trxTransacForm.setValue({
          customerId: this.tokenService.getUserEmail(),
          phoneNumber: this.shoppingcartdto.trxTransaction.phoneNumber,
          fullName: this.shoppingcartdto.trxTransaction.fullName,
          address: this.shoppingcartdto.trxTransaction.address,
          shipData: '',
          paymentTypes: this.shoppingcartdto.trxTransaction.paymentTypes,
        });

        // set data Address vào form
        this.addressForm
          .get('city')
          ?.setValue(this.shoppingcartdto.customer.city);
        this.addressForm
          .get('district')
          ?.setValue(this.shoppingcartdto.customer.district);
        this.addressForm
          .get('ward')
          ?.setValue(this.shoppingcartdto.customer.ward);

        this.addressForm
          .get('addressDetail')
          ?.setValue(this.shoppingcartdto.customer.address);

        this.trxTransacForm
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
        this.priceGoShip();
        // lấy data từ wards theo this.addressForm.value.district từ api

        // tính ra số tiền giảm giá của sản phẩm trong giỏ hàng bằng khi discount > 0 => sản phẩm cần tính giảm giá => số tiền giảm giá sản phẩm đó
        // duyệt qua từng sản phẩm trong giỏ hàng
        this.shoppingcartdto.cart.forEach((item) => {
          // nếu discount > 0 thì tính ra số tiền giảm giá của sản phẩm đó
          if (item.discount > 0) {
            this.priceDiscount +=
              item.price * item.quantity * (item.discount / 100);
          }
        });
        this.isDisplayNone = true;
        this.titleStatus = 'Đặt hàng';
      },
      error: (error: any) => {
        this.toastr.error('Lỗi lấy thông tin giỏ hàng', 'Thông báo');
      },
    });
  }

  getTotalPrice(): number {
    let totalPrice = 0; // Khởi tạo biến tổng giá tiền
    console.log('id', this.shoppingcartdto);
    // Kiểm tra xem this.shoppingcartdto đã được xác định và có giá trị không
    if (this.shoppingcartdto.cart === null) {
      // nếu shoppingcartdto.cart không tồn tại thì lấy dữ liệu từ shoppingcartdto.transactionDetail
      this.shoppingcartdto.transactionDetail.forEach((item) => {
        totalPrice += item.total * item.price;
      });
    }

    // Nếu shoppingcartdto hoặc shoppingcartdto.cart không tồn tại, hoặc có giá trị,
    // thì trả về 0 hoặc một giá trị mặc định khác tùy thuộc vào yêu cầu của bạn.
    // if (!this.shoppingcartdto || !this.shoppingcartdto.cart) {
    //   return this.shoppingcartdto.transactionDetail.reduce((acc, item) => {
    //     return acc + item.total * item.price;
    //   }, 0);
    // }

    // Nếu shoppingcartdto.cart tồn tại, duyệt qua từng sản phẩm trong giỏ hàng và tính tổng giá tiền
    // Nếu shoppingcartdto.cart tồn tại, duyệt qua từng sản phẩm trong giỏ hàng và tính tổng giá tiền
    if (this.shoppingcartdto.cart) {
      this.shoppingcartdto.cart.forEach((item) => {
        totalPrice += item.price * item.quantity; // Tính tổng giá tiền
      });
    }
    totalPrice = totalPrice - this.priceDiscount + this.priceShip; // Trừ giảm giá và cộng phí ship

    return totalPrice;
  }

  submit() {
    // nếu url có id thì set giá trị cho trxtransactionId và không có thì set = 0
    if (this.activatedRoute.snapshot.params['id'] === undefined) {
      this.trxTransactionDTO = new TrxTransactionDTO(
        0,
        this.tokenService.getUserEmail(),
        '',
        this.trxTransacForm.value?.fullName || '',
        this.trxTransacForm.value?.phoneNumber || '',
        this.trxTransacForm.value?.address || '',
        this.getTotalPrice(),
        this.trxTransacForm.value?.paymentTypes || '',
        false,
        this.shoppingcartdto.cart
      );
    }

    this.cartService.PaymentExecute(this.trxTransactionDTO).subscribe({
      next: (response: any) => {
        if (
          response.data &&
          typeof response.data === 'string' &&
          (response.data.startsWith('http://') ||
            response.data.startsWith('https://'))
        ) {
          window.location.href = response.data;
          this.cartService.removeCarts();
          // this.titleStatus='Đã thanh toán';
        } else {
          this.toastr.success(
            'Đặt hàng thành công vui lòng đợi phê duyệt!',
            'Thông báo'
          );
          this.isDisplayNone = false;
          this.router.navigate(['/']);
          // Handle other cases, such as routing to another page
          // For example, you can use this.router.navigate([some_other_route]);
        }
      },
      error: (error: any) => {
        this.toastr.error('Lỗi đặt hàng', 'Thông báo');
      },
    });
  }

  priceGoShip() {
    const userAddressTo: Address = {
      city: this.addressForm.value.city,
      district: this.addressForm.value.district,
      wards: this.addressForm.value.ward,
    };

    this.shipment = createShipmentDTO(userAddressTo);
    this.cartService.listGoShip(this.shipment).subscribe({
      next: (response: any) => {
        this.shipData = response.data;
        console.log('shipData', this.shipData);
      },
      error: (error: any) => {
        console.log(error);
      },
    });
  }

  onProductSelection(event: any) {
    this.priceShip = event.totalAmount;
  }
}
