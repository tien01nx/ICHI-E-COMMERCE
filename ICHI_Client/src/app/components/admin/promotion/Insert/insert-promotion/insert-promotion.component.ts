import { Component, OnInit } from '@angular/core';
import { Environment } from '../../../../../environment/environment';
import { Utils } from '../../../../../Utils.ts/utils';
import { ProductModel } from '../../../../../models/product.model';
import { SupplierModel } from '../../../../../models/supplier.model';
import { ProductDTO } from '../../../../../dtos/product.dto';
import {
  AbstractControl,
  FormArray,
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { Title } from '@angular/platform-browser';
import { ProductsService } from '../../../../../service/products.service';
import { SupplierService } from '../../../../../service/supplier.service';
import { InventorryReceiptsService } from '../../../../../service/inventory.receipts.service';
import { ActivatedRoute, Router } from '@angular/router';
import { TokenService } from '../../../../../service/token.service';
import { ToastrService } from 'ngx-toastr';
import Swal from 'sweetalert2';
import { PromotionService } from '../../../../../service/promotion.service';
import { ApiResponse } from '../../../../../models/api.response.model';
import { InsertPromotion } from '../../../../../dtos/Insert.Promotion.dto.';
import { DatePipe } from '@angular/common';
import { PromotionDetails } from '../../../../../models/PromotionDetails.model';

@Component({
  selector: 'app-insert-promotion',
  templateUrl: './insert-promotion.component.html',
  styleUrl: './insert-promotion.component.css',
})
export class InsertPromotionComponent implements OnInit {
  protected readonly Environment = Environment;
  protected readonly Utils = Utils;
  product: ProductModel | undefined = undefined;
  titleString: string = '';
  suppliers: SupplierModel[] = [];
  products: ProductDTO[] = [];
  productsRoot: ProductDTO[] = [];
  originalProducts: ProductDTO[] = [];
  totalMoney: number = 0;
  isDisplayNone: boolean = false;
  btnSave: string = '';
  employeeName: string = '';
  promotionForm: FormGroup = new FormGroup({
    id: new FormControl('0'),
    promotionName: new FormControl('', [
      Validators.required,
      Validators.maxLength(50),
    ]),
    startTime: new FormControl(Date, [Validators.required]),
    endTime: new FormControl(Date, [Validators.required]),
    discount: new FormControl('', [Validators.required]),

    isActive: new FormControl(true),
    isDeleted: new FormControl(false),
    promotionDetails: new FormArray([
      new FormGroup({
        id: new FormControl(0),
        promotionId: new FormControl(0),
        productId: new FormControl(null, [Validators.required]),
        quantity: new FormControl('', [
          Validators.required,
          Validators.min(1),
          Validators.max(1000),
        ]),
        usedCodesCount: new FormControl(0),
      }),
    ]),
  });

  get promotionDetails() {
    return this.promotionForm.get('promotionDetails') as FormArray;
  }

  constructor(
    private title: Title,
    private productService: ProductsService,
    private supplierService: SupplierService,
    private promotionService: PromotionService,
    private activatedRoute: ActivatedRoute,
    private tokenService: TokenService,
    private router: Router,
    private toastr: ToastrService,
    private fb: FormBuilder,
    private datePipe: DatePipe
  ) {
    this.promotionForm = this.fb.group({
      id: new FormControl('0'),
      promotionName: new FormControl('', [
        Validators.required,
        Validators.maxLength(50),
      ]),
      startTime: new FormControl('', [Validators.required]),
      endTime: new FormControl('', [Validators.required]),
      discount: new FormControl('', [
        Validators.required,
        Validators.min(1),
        Validators.max(100),
      ]),
      isActive: new FormControl(true),
      isDeleted: new FormControl(false),
      promotionDetails: this.fb.array([this.createReceiptDetail()]),
    });
  }

  ngOnInit(): void {
    if (this.activatedRoute.snapshot.params['id'] === undefined) {
      this.titleString = 'Thêm sản phẩm';
      this.btnSave = 'Thêm mới';
    } else {
      this.titleString = 'Cập nhật sản phẩm';
      this.btnSave = 'Cập nhật';
      this.findProductById(this.activatedRoute.snapshot.params['id']);
    }

    this.title.setTitle(this.titleString);
    this.getDatacombobox();
    this.promotionForm
      .get('employeeId')
      ?.setValue(this.tokenService.getUserEmail());
  }

  getDatacombobox() {
    this.supplierService.findAll().subscribe((data: any) => {
      this.suppliers = data.data.items;
      console.log(data.data.items);
    });

    this.productService.findAll().subscribe((data: any) => {
      this.products = data.data.items;
      this.productsRoot = data.data.items;
      this.originalProducts = data.data.items.map((item: ProductDTO) => ({
        ...item,
      }));
      console.log(data.data.items);
    });
  }

  onSubmit() {
    debugger;
    console.log(this.promotionForm.value);
    if (this.promotionForm.invalid) {
      const formErrors = this.promotionForm.errors;
      console.log(formErrors);
      return;
    }
    if (this.activatedRoute.snapshot.params['id'] === undefined) {
      this.createProduct();
    } else {
      this.updateProduct();
    }
  }

  findProductById(id: number) {
    this.promotionService.findById<InsertPromotion>(id).subscribe({
      next: (response: ApiResponse<InsertPromotion>) => {
        const data = response.data;
        if (data === null) {
          this.toastr.error('Không tìm thấy sản phẩm', 'Thất bại');
          this.router.navigate(['/admin/promotion']);
        }
        this.isDisplayNone = true;

        this.promotionForm.get('id')?.setValue(data.id);
        this.promotionForm.get('promotionName')?.setValue(data.promotionName);
        this.promotionForm
          .get('startTime')
          ?.setValue(this.datePipe.transform(data.startTime, 'yyyy-MM-dd'));
        this.promotionForm
          .get('endTime')
          ?.setValue(this.datePipe.transform(data.endTime, 'yyyy-MM-dd'));
        this.promotionForm.get('discount')?.setValue(data.discount);
        this.promotionForm.get('quantity')?.setValue(data.quantity);
        this.promotionForm.get('isActive')?.setValue(data.isActive);
        this.promotionForm.get('isDeleted')?.setValue(data.isDeleted);

        // Clear existing controls in promotionDetails FormArray
        while (this.promotionDetails.length !== 0) {
          this.promotionDetails.removeAt(0);
        }

        debugger;

        // Iterate through promotionDetails in response data and add each detail to FormArray
        data.promotionDetails.forEach((detail: PromotionDetails) => {
          const detailFormGroup = new FormGroup({
            id: new FormControl(detail.id),
            promotionId: new FormControl(detail.promotionId),
            productId: new FormControl(detail.productId), // Assuming product is available in detail
            quantity: new FormControl(detail.quantity, [
              Validators.required,
              Validators.min(1),
              Validators.max(1000),
            ]),
            usedCodesCount: new FormControl(detail.usedCodesCount),
          });

          this.promotionDetails.push(detailFormGroup);
        });
      },
      error: (err: any) => {
        this.toastr.error(err.error, 'Thất bại');
      },
    });
  }

  createReceiptDetail(): FormGroup {
    return this.fb.group({
      id: [0],
      promotionId: [0],
      productId: ['', Validators.required],
      quantity: new FormControl('', [
        Validators.required,
        Validators.min(1),
        Validators.max(1000),
      ]),
      usedCodesCount: new FormControl(0),
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
    const allFieldsFilled = this.promotionDetails.controls.every((control) => {
      const productId = control.get('productId')?.value;
      return productId;
    });

    // trường hợp có id trên url thực hiện cập nhật danh sách sản phẩm
    if (this.activatedRoute.snapshot.params['id'] !== undefined)
      this.products = this.productsRoot.filter((product) =>
        this.promotionDetails.value.every(
          (detail: { productId: any }) =>
            detail.productId !== product.product.id
        )
      );

    if (allFieldsFilled) {
      this.promotionDetails.push(this.createReceiptDetail());
      this.updateOriginalProducts();
    } else {
      this.toastr.error('Vui lòng điền đầy đủ thông tin sản phẩm', 'Thông báo');
    }
  }

  createProduct() {
    this.promotionService.create(this.promotionForm.value).subscribe({
      next: (respon: any) => {
        if (respon.message === 'Tạo mới chương trình khuyến mãi thành công') {
          this.promotionForm.reset();
          this.toastr.success(respon.message, 'Thành công');
          this.router.navigateByUrl('/admin/promotion');
        } else {
          const regex = /Id:\s*([\d,]+)\s*:/;
          const match = respon.message.match(regex);

          if (match) {
            this.toastr.error(
              'Sản phẩm đã tồn tại trong chương trình khuyến mãi khác',
              'Thất bại'
            );
            const numbersString = match[1];
            const invalidProductIds = numbersString.split(',').map(Number);
            console.log(invalidProductIds);

            this.promotionDetails.controls.forEach((control, index) => {
              const productIdControl = control.get('productId');
              if (productIdControl && productIdControl.value) {
                const productId = productIdControl.value;
                if (invalidProductIds.includes(productId)) {
                  productIdControl.setErrors({ invalid: true });
                }
              }
            });
          } else {
            console.log('Không tìm thấy chuỗi số.');
          }
        }
      },
      error: (err: any) => {
        this.toastr.error(err.error, 'Thất bại');
      },
    });
  }

  updateProduct() {
    debugger;
    this.promotionService.update(this.promotionForm.value).subscribe({
      next: (response: any) => {
        if (
          response.message === 'Cập nhật chương trình khuyến mãi thành công'
        ) {
          this.toastr.success('Cập nhật sản phẩm thành công');
          this.router.navigateByUrl('/admin/promotion');
        } else {
          this.toastr.error(
            'Sản phẩm đã tồn tại trong chương trình khuyến mãi khác',
            'Thất bại'
          );
          const regex = /Id:\s*([\d,]+)\s*:/;
          const match = response.message.match(regex);

          if (match) {
            const numbersString = match[1];
            console.log(numbersString);
          } else {
            console.log('Không tìm thấy chuỗi số.');
          }
        }
      },
      error: (err: any) => {
        this.toastr.error(err.error, 'Thất bại');
      },
    });
  }

  deleteProduct(id: number) {
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
        this.productService.deleteProductDetails(id).subscribe({
          next: () => {
            this.toastr.success('Xóa sản phẩm thành công');
            this.router.navigateByUrl('/admin/products');
          },
          error: (err: any) => {
            this.toastr.error(err.error, 'Thất bại');
          },
        });
      }
    });
  }

  removeProduct(index: number) {
    if (this.promotionDetails.length > 1) {
      this.promotionDetails.removeAt(index);
      this.products = this.productsRoot.filter((product) =>
        this.promotionDetails.value.every(
          (detail: { productId: any }) =>
            detail.productId !== product.product.id
        )
      );
      console.log('sản phẩm', this.products);
    }
  }

  updateOriginalProducts() {
    this.originalProducts = this.products.map((item) => ({ ...item }));
    console.log(this.originalProducts);
  }

  onProductSelection(event: any) {
    // Xử lý sự kiện khi người dùng chọn một sản phẩm từ ng-select
    const unselectedProducts = this.originalProducts.filter((product) =>
      this.promotionForm.value.promotionDetails.every(
        (detail: { productId: any }) => detail.productId !== product.product.id
      )
    );
    console.log('chưa chọn', unselectedProducts);
    // Cập nhật danh sách sản phẩm chỉ hiển thị các sản phẩm chưa được chọn
    this.products = unselectedProducts;
  }

  onKeyDown(event: any, type: boolean) {
    const input = event.target as HTMLInputElement;
    const isBackspaceOrDelete =
      event.key === 'Backspace' || event.key === 'Delete';
    const hasSelection = input.selectionStart !== input.selectionEnd;

    if (type) {
      // Kiểm tra nếu số lượng ký tự vượt quá maxLength
      if (input.value.length >= 9 && !isBackspaceOrDelete && !hasSelection) {
        // Ngăn chặn sự kiện và không cho phép nhập
        event.preventDefault();
      }
    } else {
      // Kiểm tra nếu số lượng ký tự vượt quá maxLength
      if (input.value.length >= 3 && !isBackspaceOrDelete && !hasSelection) {
        // Ngăn chặn sự kiện và không cho phép nhập
        event.preventDefault();
      }
    }
  }
}
