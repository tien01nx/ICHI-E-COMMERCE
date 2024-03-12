import { Utils } from './../../../../Utils.ts/utils';
import { Component, OnInit } from '@angular/core';
import { EditorModule } from '@tinymce/tinymce-angular';
import { NgxDropzoneModule } from 'ngx-dropzone';
import Swal from 'sweetalert2';
import { ProductsService } from '../../../../service/products.service';
import { CommonModule } from '@angular/common';
import { Environment } from '../../../../environment/environment';
import {
  FormArray,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Title } from '@angular/platform-browser';
import { CategoryService } from '../../../../service/category-product.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { NgSelectModule } from '@ng-select/ng-select';
import { TrademarkService } from '../../../../service/trademark.service';

@Component({
  selector: 'app-insert-admin-product',
  standalone: true,
  imports: [
    EditorModule,
    NgxDropzoneModule,
    CommonModule,
    ReactiveFormsModule,
    NgSelectModule,
  ],
  templateUrl: './insert-admin-product.component.html',
  styleUrl: './insert-admin-product.component.css',
})
export class InsertAdminProductComponent implements OnInit {
  protected readonly Environment = Environment;
  protected readonly Utils = Utils;

  titleString: string = '';
  selectedImageUrl: string = '';
  selectedImageFile: File = new File([''], 'filename');
  selectedImageProductUrl: string[] = [];
  selectedImageProductFiles: File[] = [];
  // categories: CategoryDto[] = [];
  // origins: OriginDto[] = [];
  // brands: BrandDto[] = [];
  // shapes: ShapeDto[] = [];
  // materials: MaterialDto[] = [];

  category: any;
  trademark: any;
  color: any;

  selectedItem: any; // Biến để lưu trữ giá trị được chọn

  isDisplayNone: boolean = false;
  btnSave: string = '';

  productForm: FormGroup = new FormGroup({
    id: new FormControl(null),
    productName: new FormControl('', [
      Validators.required,
      Validators.maxLength(50),
    ]),
    sellingPrice: new FormControl('', [
      Validators.required,
      Validators.min(1),
      Validators.pattern(/^-?\d+\.?\d*$/),
    ]),
    suggestedPrice: new FormControl('', [
      Validators.required,
      Validators.min(1),
      Validators.pattern(/^-?\d+\.?\d*$/),
    ]),
    description: new FormControl('', [Validators.required]),
    imageProductFiles: new FormControl(null, [Validators.required]),
    status: new FormControl('false', [Validators.required]),
    categoryId: new FormControl(null, [Validators.required]),
    // materialId: new FormControl(null, [Validators.required]),
    // originId: new FormControl(null, [Validators.required]),
    // shapeId: new FormControl(null, [Validators.required]),
    // brandId: new FormControl(null, [Validators.required]),
    productDetails: new FormArray(
      [
        new FormGroup({
          id: new FormControl(null),
          color: new FormControl('', [Validators.required]),
          quantity: new FormControl(0),
        }),
      ],
      Validators.required
    ),
  });

  get productDetails() {
    return this.productForm.get('productDetails') as FormArray;
  }

  constructor(
    private title: Title,
    private productService: ProductsService,
    private categoryService: CategoryService,
    private trademarkService: TrademarkService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    // const thumbnailFileControl = this.productForm.get('thumbnailFile');
    const imageProductFilesControl = this.productForm.get('imageProductFiles');
    if (this.activatedRoute.snapshot.params['id'] === undefined) {
      this.titleString = 'Thêm sản phẩm';
      this.btnSave = 'Thêm mới';
      // thumbnailFileControl?.setValidators([Validators.required]);
      imageProductFilesControl?.setValidators([Validators.required]);
    } else {
      this.titleString = 'Cập nhật sản phẩm';
      this.btnSave = 'Cập nhật';
      // thumbnailFileControl?.setValidators([Validators.nullValidator]);
      imageProductFilesControl?.setValidators([Validators.nullValidator]);
      this.findProductById(this.activatedRoute.snapshot.params['id']);
    }
    // thumbnailFileControl?.updateValueAndValidity();
    imageProductFilesControl?.updateValueAndValidity();

    this.title.setTitle(this.titleString);
    this.findAllCategory();
    this.findAllOrigin();
    this.findAllBrand();
    this.findAllShape();
    this.findAllMaterial();
    this.getDatacombobox();
  }

  getDatacombobox() {
    this.categoryService.findAll().subscribe((data: any) => {
      this.category = data.data.items;
      console.log(data.data.items);
    });
    this.trademarkService.findAll().subscribe((data: any) => {
      this.trademark = data.data.items;
      console.log(data.data.items);
    });
    this.color = Utils.createColorList();
  }

  onFileChange(event: any) {
    const file = event.target.files[0];
    this.selectedImageFile = file;
    if (file) {
      const reader = new FileReader();
      reader.onload = (e: any) => {
        this.selectedImageUrl = e.target.result;
      };
      reader.readAsDataURL(file);
    }
  }

  onSelect(event: any) {
    this.selectedImageProductFiles.push(...event.addedFiles);
    this.productForm
      .get('imageProductFiles')
      ?.setValue(this.selectedImageProductFiles);
    console.log(this.selectedImageProductFiles);
  }

  onRemove(event: any) {
    this.selectedImageProductFiles.splice(
      this.selectedImageProductFiles.indexOf(event),
      1
    );
    this.productForm
      .get('imageProductFiles')
      ?.setValue(this.selectedImageProductFiles);
  }

  addProductDetails() {
    const productDetails = this.productForm.get('productDetails') as FormArray;
    productDetails.push(
      new FormGroup({
        id: new FormControl(null),
        color: new FormControl('', [Validators.required]),
        quantity: new FormControl(0),
      })
    );
  }

  removeProductDetails(index: number) {
    const productDetails = this.productForm.get('productDetails') as FormArray;

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
    if (this.productForm.invalid) {
      return;
    }
    if (this.activatedRoute.snapshot.params['id'] === undefined) {
      this.createProduct();
    } else {
      this.updateProduct();
    }
  }

  findAllCategory() {
    // this.categoryService
    //   .findAllByName('', true, 100, 1, 'ASC', 'name')
    //   .subscribe((data: any) => {
    //     this.categories = data.content;
    //   });
  }

  findAllOrigin() {
    // this.originService.findAll().subscribe((data: any) => {
    //   this.origins = data;
    // });
  }

  findAllBrand() {
    // this.brandService.findAll().subscribe((data: any) => {
    //   this.brands = data;
    // });
  }

  findAllShape() {
    // this.shapeService.findAll().subscribe((data: any) => {
    //   this.shapes = data;
    // });
  }

  findAllMaterial() {
    // this.materialService.findAll().subscribe((data: any) => {
    //   this.materials = data;
    // });
  }

  findProductById(id: number) {
    this.productService.findById(id).subscribe({
      next: (data: any) => {
        this.productForm.patchValue(data);
        this.selectedImageUrl =
          Environment.apiBaseUrl + '/images/' + data.thumbnail;
        this.selectedImageFile = new File([''], 'filename');
        this.selectedImageProductFiles = [];
        this.selectedImageProductUrl = data.images.map((image: string) => {
          return image;
        });
        this.productForm
          .get('imageProductFiles')
          ?.setValue(this.selectedImageProductFiles);
        this.productForm.get('status')?.setValue(data.status.toString());
        this.productForm.setControl('productDetails', new FormArray([]));
        data.productDetails.forEach((productDetail: any) => {
          this.productDetails.push(
            new FormGroup({
              id: new FormControl(productDetail.id),
              color: new FormControl(productDetail.color, [
                Validators.required,
              ]),
              quantity: new FormControl(productDetail.quantity),
            })
          );
        });
      },
      error: (err: any) => {
        this.toastr.error(err.error, 'Thất bại');
      },
    });
  }

  createProduct() {
    this.productService
      .create(this.productForm.value, this.selectedImageProductFiles)
      .subscribe({
        next: () => {
          this.toastr.success('Thêm sản phẩm thành công');
          this.router.navigateByUrl('/admin/product');
        },
        error: (err: any) => {
          this.toastr.error(err.error, 'Thất bại');
        },
      });
  }

  updateProduct() {
    this.productService
      .update(this.productForm.value, this.selectedImageProductFiles)
      .subscribe({
        next: () => {
          this.toastr.success('Cập nhật sản phẩm thành công');
          this.router.navigateByUrl('/admin/product');
        },
        error: (err: any) => {
          this.toastr.error(err.error, 'Thất bại');
        },
      });
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
        this.productService
          .deleteImage(this.activatedRoute.snapshot.params['id'], imageName)
          .subscribe({
            next: () => {
              this.toastr.success('Xóa ảnh thành công');
              if (this.selectedImageProductUrl.length === 0) {
                const imageProductFilesControl =
                  this.productForm.get('imageProductFiles');
                imageProductFilesControl?.setValidators([Validators.required]);
                imageProductFilesControl?.updateValueAndValidity();
              }

              this.selectedImageProductUrl =
                this.selectedImageProductUrl.filter((image: string) => {
                  return image !== imageName;
                });
            },
            error: (err: any) => {
              this.toastr.error(err.error, 'Thất bại');
            },
          });
      }
    });
  }
}
