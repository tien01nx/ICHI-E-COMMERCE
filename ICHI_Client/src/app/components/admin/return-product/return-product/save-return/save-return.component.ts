import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import {
  AbstractControl,
  FormArray,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Environment } from '../../../../../environment/environment';
import { TrxTransactionVM } from '../../../../../dtos/trxTransaction.vm';
import { TrxTransactionService } from '../../../../../service/trx-transaction.service';
import { ProductsService } from '../../../../../service/products.service';
import { ReturnProductService } from '../../../../../service/ReturnService.service';
import { OrderResponse } from '../../../../../dtos/order.response';

@Component({
  selector: 'app-save-return',
  templateUrl: './save-return.component.html',
  styleUrls: ['./save-return.component.css'],
})
export class SaveReturnComponent implements OnInit {
  @ViewChild('btnAddReturn') btnAddReturn!: ElementRef;
  @ViewChild('cartRight') cartRight!: ElementRef;
  @ViewChild('cartBottom') cartBottom!: ElementRef;
  Environment = Environment;
  baseUrl: string = `${Environment.apiBaseUrl}`;
  titleString: string = '';
  btnSave: string = '';

  listProductSelected: any[] = [];
  listProductSecondary: any[] = [];

  totalMoney: number = 0;
  totalPrices: number[] = [];

  listProductInOrder: any[] = [];

  orderCompleted: OrderResponse[] = [];

  order: any;

  returnForm: FormGroup = new FormGroup({
    id: new FormControl(0),
    status: new FormControl('APPROVED'),
    trxTransactionId: new FormControl(null, [Validators.required]),
    returnProductDetails: new FormArray([
      new FormGroup({
        id: new FormControl(0),
        price: new FormControl(null),
        quantity: new FormControl(null, [
          Validators.required,
          Validators.min(1),
          Validators.max(1000),
        ]),
        productId: new FormControl(null, [Validators.required]),
        reason: new FormControl(null, [Validators.maxLength(100)]),
        returnType: new FormControl(true, [Validators.required]),
      }),
    ]),
  });

  constructor(
    private title: Title,
    private orderService: TrxTransactionService,
    private productService: ProductsService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private returnService: ReturnProductService,
    private toastr: ToastrService
  ) {}

  ngOnInit() {
    if (this.activatedRoute.snapshot.params['id'] === undefined) {
      this.btnSave = 'Thêm mới';
      this.titleString = 'Tạo phiếu đổi trả';
    } else {
      this.titleString = 'Cập nhật phiếu đổi trả';
      this.btnSave = 'Cập nhật';
    }
    this.title.setTitle(this.titleString);
    this.getAllOrder();

    this.returnForm.controls['trxTransactionId'].valueChanges.subscribe({
      next: (value) => {
        this.listProductSelected = [];
        this.listProductInOrder = [];

        // hiện cartRight và cartBottom
        this.cartRight.nativeElement.classList.remove('d-none');
        this.cartBottom.nativeElement.classList.remove('d-none');

        // this.order = this.orderCompleted.find(
        //   (x) => x.trxTransaction.id === value
        // );

        const foundOrder = this.orderCompleted.find((x) => x.id === value);
        if (foundOrder !== undefined) {
          this.order = foundOrder;
        } else {
          // Handle the case where no order is found, for example:
          console.error('No matching transaction found');
          // Or set this.order to a default value, if appropriate
        }

        this.listProductInOrder = this.order.orderDetailsResponses.slice();
        this.listProductSecondary = this.order.orderDetailsResponses.slice();

        const returnProductDetailsArray = this.returnForm.get(
          'returnProductDetails'
        ) as FormArray;
        returnProductDetailsArray.clear();
        this.addNewDetails();
      },
    });

    const returnProductDetailsArray = this.returnForm.get(
      'returnProductDetails'
    ) as FormArray;
    returnProductDetailsArray.valueChanges.subscribe(() => {
      this.totalMoney = this.getTotalMoney();
      this.totalPrices = this.getTotalPrice();
    });
  }

  get returnProductDetails() {
    return this.returnForm.get('returnProductDetails') as FormArray;
  }

  addNewDetails() {
    // kiểm tra returnForm invalid thì không thêm mới
    if (this.returnForm.invalid) {
      this.toastr.error('Vui lòng nhập đầy đủ thông tin', 'Thông báo');
      return;
    }

    this.listProductSelected = [];

    const receiptDetails = this.returnForm.get(
      'returnProductDetails'
    ) as FormArray;
    receiptDetails.push(
      new FormGroup({
        id: new FormControl(0),
        price: new FormControl(null),
        quantity: new FormControl(null, [
          Validators.required,
          Validators.min(1),
          Validators.max(1000),
        ]),
        productId: new FormControl(null, [Validators.required]),
        reason: new FormControl(null, [Validators.maxLength(100)]),
        returnType: new FormControl(null, [Validators.required]),
      })
    );
  }

  onSubmit() {
    if (this.returnForm.invalid) {
      console.log('Form invalid');
      return;
    }
    this.create();
  }

  create() {
    // đổi tất cả các returnType từ string sang boolean
    const returnProductDetails = this.returnForm.get(
      'returnProductDetails'
    ) as FormArray;
    returnProductDetails.controls.forEach(
      (control: AbstractControl<any, any>) => {
        control
          .get('returnType')
          ?.setValue(control.get('returnType')?.value === 'true');
      }
    );
    console.log(this.returnForm.value);

    debugger;
    this.returnService.create(this.returnForm.value).subscribe({
      next: (response: any) => {
        console.log(response);
        if (response.code === 200) {
          this.toastr.success(response.message, 'Thông báo');
          this.router.navigate(['/admin/list_return']);
        }
        this.toastr.error(response.message, 'Thông báo');
      },
      error: (error) => {
        this.toastr.error('Lỗi tải dữ liệu', 'Thông báo');
      },
    });
  }

  getAllOrder() {
    this.orderService.findAllGetOrderDelivered().subscribe({
      next: (response: any) => {
        this.orderCompleted = response.data;
        console.log('data products', this.orderCompleted);
      },
      error: (error) => {
        this.toastr.error('Lỗi tải dữ liệu', 'Thông báo');
      },
    });
  }

  getTotalPrice(): number[] {
    const returnProductDetails = this.returnForm.get(
      'returnProductDetails'
    ) as FormArray;
    const totals: number[] = [];

    returnProductDetails.controls.forEach(
      (control: AbstractControl<any, any>) => {
        const price = (control.get('price') as FormControl)?.value || 0;
        const quantity = (control.get('quantity') as FormControl)?.value || 0;
        console.log('price', price);
        console.log('quantity', quantity);
        if (control.get('returnType')?.value === 'true') {
          const total = price * quantity;
          totals.push(total);
        } else {
          totals.push(0);
        }
      }
    );

    return totals;
  }

  getTotalMoney(): number {
    const receiptDetails = this.returnForm.get(
      'returnProductDetails'
    ) as FormArray;
    let total = 0;

    let haveReturnProduct = 0;

    receiptDetails.controls.forEach((control: AbstractControl<any, any>) => {
      const price = (control.get('price') as FormControl)?.value || 0;
      const quantity = (control.get('quantity') as FormControl)?.value || 0;
      if (control.get('returnType')?.value === 'true') {
        total += price * quantity;
        haveReturnProduct++;
      }
    });
    if (haveReturnProduct === 0) {
      return total;
    }
    // return total + this.order?.shippingFee;
    return total;
  }

  removeProductDetails(index: number) {
    const receiptDetails = this.returnForm.get(
      'returnProductDetails'
    ) as FormArray;
    if (receiptDetails.length > 1) {
      const productId = receiptDetails.at(index).get('productId')?.value;
      const indexProductSelected = this.listProductSecondary.findIndex(
        (x) => x.productId === productId
      );
      if (indexProductSelected !== -1) {
        this.listProductInOrder.push(
          this.listProductSecondary[indexProductSelected]
        );
      }
      receiptDetails.removeAt(index);
      this.btnAddReturn.nativeElement.disabled = false;
    } else {
      const productId = receiptDetails.at(index).get('productId')?.value;
      const indexProductSelected = this.listProductSecondary.findIndex(
        (x) => x.productId === productId
      );
      if (indexProductSelected !== -1) {
        this.listProductInOrder.push(
          this.listProductSecondary[indexProductSelected]
        );
      }
      receiptDetails.at(0).reset();
    }
  }

  getProductItem(index: number): FormControl {
    const receiptDetailsFormArray = this.returnForm.get(
      'returnProductDetails'
    ) as FormArray;
    const receiptDetailsGroup = receiptDetailsFormArray.at(index) as FormGroup;
    const productIdControl = receiptDetailsGroup.get(
      'productId'
    ) as FormControl;

    productIdControl.valueChanges.pipe().subscribe((productId: any) => {
      this.listProductInOrder.forEach((product: any) => {
        if (product.productId === productId) {
          // Lấy thằng cuối trong listProductSelected ném vào listProductInOrder và xóa khỏi listProductSelected
          if (this.listProductSelected.length > 0) {
            this.listProductInOrder.push(
              this.listProductSelected[this.listProductSelected.length - 1]
            );
            this.listProductSelected.pop();
          }

          // xóa sản phẩm đã chọn khỏi listProductInOrder
          const index = this.listProductInOrder.findIndex(
            (x) => x.productId === productId
          );
          this.listProductInOrder.splice(index, 1);
          this.listProductSelected.push(product); ////

          if (this.listProductInOrder.length === 0) {
            this.btnAddReturn.nativeElement.disabled = true;
          } else this.btnAddReturn.nativeElement.disabled = false;
          receiptDetailsGroup.get('price')?.setValue(product.productPrice);
          receiptDetailsGroup.get('quantity')?.setValue(1);
        }
      });
    });
    return productIdControl;
  }

  // kiểm tra số lượng nhập với số lượng đã mua
  checkQuantity(index: number): boolean {
    const receiptDetailsFormArray = this.returnForm.get(
      'returnProductDetails'
    ) as FormArray;
    const receiptDetailsGroup = receiptDetailsFormArray.at(index) as FormGroup;
    const productIdControl = receiptDetailsGroup.get(
      'productId'
    ) as FormControl;

    let quantity = 0;
    this.order?.orderDetailsResponses.forEach((product: any) => {
      if (product.id === productIdControl.value) {
        quantity = product.quantity;
      }
    });

    if (receiptDetailsGroup.get('quantity')?.value > quantity) {
      this.returnForm
        .get('returnProductDetails')
        ?.setErrors({ quantity: true });
      return true;
    }
    return false;
  }

  onKeyDown(event: KeyboardEvent, index: number): void {
    const input = event.target as HTMLInputElement;
    const isBackspaceOrDelete =
      event.key === 'Backspace' || event.key === 'Delete';
    const hasSelection = input.selectionStart !== input.selectionEnd;
    if (
      event.keyCode === 38 ||
      event.keyCode === 40 ||
      event.key === '-' ||
      (this.checkQuantity(index) && !isBackspaceOrDelete && !hasSelection)
    ) {
      event.preventDefault(); // Ngăn chặn hành động mặc định của sự kiện keydown
      console.log('Số lượng không hợp lệ');

      // chặn nhập ký tự --
      if (event.key === '-') {
        event.preventDefault();
      }
    }
  }
}
