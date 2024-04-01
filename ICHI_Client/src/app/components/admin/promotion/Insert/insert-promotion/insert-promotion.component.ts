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
import { InsertPromotion } from '../../../../../dtos/Insert.Promotion.dto.';
import { PromotionModel } from '../../../../../models/promotion.model';
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
      Validators.pattern(Utils.textPattern),
    ]),
    startTime: new FormControl('', [Validators.required]),
    endTime: new FormControl('', [Validators.required]),
    discount: new FormControl('', [Validators.required]),
    quantity: new FormControl('', [Validators.required]),
    isActive: new FormControl(true),
    isDeleted: new FormControl(false),
    promotionDetails: new FormArray([
      new FormGroup({
        id: new FormControl(0),
        promotionId: new FormControl(0),
        productId: new FormControl(null, [Validators.required]),
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
    private inventoryService: InventorryReceiptsService,
    private activatedRoute: ActivatedRoute,
    private tokenService: TokenService,
    private router: Router,
    private toastr: ToastrService,
    private fb: FormBuilder
  ) {
    this.promotionForm = this.fb.group({
      id: new FormControl('0'),
      promotionName: new FormControl('', [
        Validators.required,
        Validators.maxLength(50),
        Validators.pattern(Utils.textPattern),
      ]),
      startDate: new FormControl('', [Validators.required]),
      endDate: new FormControl('', [Validators.required]),
      discount: new FormControl('', [
        Validators.required,
        Validators.min(1),
        Validators.max(100),
      ]),
      quantity: new FormControl('', [
        Validators.required,
        Validators.min(1),
        Validators.max(1000),
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
      // thumbnailFileControl?.setValidators([Validators.required]);
    } else {
      this.titleString = 'Cập nhật sản phẩm';
      this.btnSave = 'Cập nhật';
      this.findProductById(this.activatedRoute.snapshot.params['id']);
    }

    const promotionDetailsArray = this.promotionForm.get(
      'promotionDetails'
    ) as FormArray;

    promotionDetailsArray.valueChanges.subscribe(() => {
      this.totalMoney = this.getTotalMoney();
    });
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

  addProductDetails() {
    const productDetails = this.promotionForm.get(
      'productDetails'
    ) as FormArray;
    productDetails.push(
      new FormGroup({
        id: new FormControl(null),
        color: new FormControl('', [Validators.required]),
        quantity: new FormControl(0),
      })
    );
  }

  removeProductDetails(index: number) {
    const productDetails = this.promotionForm.get(
      'productDetails'
    ) as FormArray;

    if (productDetails.at(index).get('id')?.value !== null) {
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
          this.productService
            .deleteProductDetails(productDetails.at(index).get('id')?.value)
            .subscribe({
              next: () => {
                this.toastr.success('Xóa chi tiết sản phẩm thành công');
                productDetails.removeAt(index);
              },
              error: (err: any) => {
                this.toastr.error(err.error, 'Thất bại');
              },
            });
        }
      });
    } else {
      productDetails.removeAt(index);
    }
  }

  onSubmit() {
    debugger;
    console.log(this.promotionForm.value);
    if (this.promotionForm.invalid) {
      const formErrors = this.promotionForm.errors;

      // Log các lỗi để kiểm tra
      console.log(formErrors);

      // Trả về để dừng tiến trình nếu form không hợp lệ
      return;
    }
    if (this.activatedRoute.snapshot.params['id'] === undefined) {
      this.createProduct();
    } else {
      this.updateProduct();
    }
  }

  findProductById(id: number) {
    this.inventoryService.findById(id).subscribe({
      next: (response: any) => {
        const data = response.data;
        if (data === null) {
          this.toastr.error('Không tìm thấy sản phẩm', 'Thất bại');
          this.router.navigate(['/admin/inventory_receipts']);
        }
        this.isDisplayNone = true;
        this.employeeName = data.fullName;
        // Set values for top-level controls
        this.promotionForm.get('id')?.setValue(data.id);
        this.promotionForm.get('notes')?.setValue(data.notes);
        this.promotionForm.get('supplierId')?.setValue(data.supplierId);
        this.promotionForm.get('employeeId')?.setValue(data.employeeId);

        // Clear existing controls in promotionDetails FormArray
        while (this.promotionDetails.length !== 0) {
          this.promotionDetails.removeAt(0);
        }

        // Iterate through promotionDetails in response data and add each detail to FormArray
        data.promotionDetails.forEach((detail: any) => {
          const detailFormGroup = new FormGroup({
            id: new FormControl(detail.id),
            inventoryReceiptId: new FormControl(detail.inventoryReceiptId),
            price: new FormControl(detail.price),
            total: new FormControl(detail.total),
            productId: new FormControl(detail.product?.id), // Assuming product is available in detail
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

    if (allFieldsFilled) {
      // Lọc ra các sản phẩm chưa được chọn
      // const unselectedProducts = this.originalProducts.filter((product) =>
      //   this.promotionForm.value.promotionDetails.every(
      //     (detail: { productId: any }) =>
      //       detail.productId !== product.product.id
      //   )
      // );
      // // Cập nhật danh sách sản phẩm chỉ hiển thị các sản phẩm chưa được chọn
      // this.products = unselectedProducts;

      this.promotionDetails.push(this.createReceiptDetail());
      this.updateOriginalProducts();
    } else {
      this.toastr.error('Vui lòng điền đầy đủ thông tin sản phẩm', 'Thông báo');
    }
  }

  getTotalMoney(): number {
    const promotionDetails = this.promotionForm.get(
      'promotionDetails'
    ) as FormArray;
    let total = 0;

    promotionDetails.controls.forEach((control: AbstractControl<any, any>) => {
      const price = (control.get('price') as FormControl)?.value || 0;
      const quantity = (control.get('total') as FormControl)?.value || 0;
      total += price * quantity;
    });

    return total;
  }
  convertToDTO(form: FormGroup): InsertPromotion {
    const promotion: PromotionModel = {
      id: form.get('id')?.value,
      promotionName: form.get('promotionName')?.value,
      startTime: form.get('startTime')?.value,
      endTime: form.get('endTime')?.value,
      discount: form.get('discount')?.value,
      quantity: form.get('quantity')?.value,
      isActive: form.get('isActive')?.value,
      isDeleted: form.get('isDeleted')?.value,
      createBy: this.tokenService.getUserEmail(),
      createDate: new Date(),
      modifiedBy: this.tokenService.getUserEmail(),
      modifiedDate: new Date(),
    };

    const promotionDetails: PromotionDetails[] = [];
    const promotionDetailsFormArray = form.get('promotionDetails') as FormArray;
    promotionDetailsFormArray.controls.forEach((control) => {
      const promotionDetail: PromotionDetails = {
        id: control.get('id')?.value,
        promotionId: control.get('promotionId')?.value,
        productId: control.get('productId')?.value,
        createBy: this.tokenService.getUserEmail(),
        createDate: new Date(),
        modifiedBy: this.tokenService.getUserEmail(),
        modifiedDate: new Date(),
      };
      promotionDetails.push(promotionDetail);
    });

    return new InsertPromotion(promotion, promotionDetails);
  }

  createProduct() {
    // chuyển dữ liệu từ form sang model
    console.log('data', this.convertToDTO(this.promotionForm.value));
    this.inventoryService
      .create(this.convertToDTO(this.promotionForm.value))
      .subscribe({
        next: (respon: any) => {
          this.toastr.success(respon.message, 'Thành công');
          // this.router.navigateByUrl('/admin/products');
        },
        error: (err: any) => {
          this.toastr.error(err.error, 'Thất bại');
        },
      });
  }

  updateProduct() {
    // debugger;
    // // this.promotionForm.value.id = 0;
    // if (this.promotionForm.value.productImages === null) {
    //   this.toastr.error('Chưa chọn ảnh sản phẩm', 'Thất bại');
    //   return;
    // }
    // this.productService
    //   .update(this.promotionForm.value, this.selectedImageProductFiles)
    //   .subscribe({
    //     next: () => {
    //       this.toastr.success('Cập nhật sản phẩm thành công');
    //       this.router.navigateByUrl('/admin/product');
    //     },
    //     error: (err: any) => {
    //       this.toastr.error(err.error, 'Thất bại');
    //     },
    //   });
  }

  deleteImageProduct(imageName: string) {
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
      }
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
      // cập nhật lại danh sách sản phẩm products
      // productsRoot là data gốc ban đầu
      // products = productsRoot - các sản phẩm đang chọn trong promotionDetails
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
  // chuyển
}
