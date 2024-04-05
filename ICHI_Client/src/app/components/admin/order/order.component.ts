import { ActivatedRoute, Router } from '@angular/router';
import { AfterViewInit, Component, OnInit } from '@angular/core';
import { Utils } from '../../../Utils.ts/utils';
import { Environment } from '../../../environment/environment';
import { ProductModel } from '../../../models/product.model';
import { CategoryProduct } from '../../../models/category.product';
import { TrademarkModel } from '../../../models/trademark.model';
import { ProductImage } from '../../../models/product.image';
import { FormArray, FormControl, FormGroup, Validators } from '@angular/forms';
import { Title } from '@angular/platform-browser';
import { ProductsService } from '../../../service/products.service';
import { CategoryService } from '../../../service/category-product.service';
import { TrademarkService } from '../../../service/trademark.service';
import { ToastrService } from 'ngx-toastr';
import Swal from 'sweetalert2';
import { ProductDTO } from '../../../dtos/product.dto';

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrl: './order.component.css',
})
export class OrderComponent implements OnInit {
  protected readonly Environment = Environment;
  protected readonly Utils = Utils;
  product: ProductModel | undefined = undefined;
  titleString: string = '';
  selectedImageUrl: string = '';
  selectedImageFile: File = new File([''], 'filename');
  selectedImageProductUrl: string[] = [];
  selectedImageProductFiles: File[] = [];
  categories: CategoryProduct[] = [];
  trademarks: TrademarkModel[] = [];
  productImage: ProductImage[] = [];

  productdtos: ProductDTO[] = [];
  products: ProductModel[] = [];

  color: any;
  selectedItem: any; // Biến để lưu trữ giá trị được chọn

  isDisplayNone: boolean = false;
  btnSave: string = '';
  productForm: FormGroup = new FormGroup({
    id: new FormControl(0),
    trademarkId: new FormControl(null, [Validators.required]),
    categoryId: new FormControl(null, [Validators.required]),
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
    // isActive: new FormControl('false', [Validators.required]),
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
    this.productService.findAll().subscribe((data: any) => {
      this.productdtos = data.data.items;
      this.products = this.productdtos.map((product) => product.product);
      console.log('products', this.products);
    });

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
      this.categories = data.data;
      console.log(data.data);
    });
    this.trademarkService.findAll().subscribe((data: any) => {
      this.trademarks = data.data.items;
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
    this.createProduct();
    // if (this.activatedRoute.snapshot.params['id'] === undefined) {
    //   this.createProduct();
    // } else {
    //   this.updateProduct();
    // }
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
      next: (respon: any) => {
        this.productForm.get('id')?.setValue(respon.data.product.id);
        this.productForm
          .get('productName')
          ?.setValue(respon.data.product.productName);
        this.productForm
          .get('description')
          ?.setValue(respon.data.product.description);
        this.productForm.get('price')?.setValue(respon.data.product.price);
        this.productForm
          .get('priorityLevel')
          ?.setValue(respon.data.product.priorityLevel);
        this.productForm.get('notes')?.setValue(respon.data.product.notes);
        this.productForm.get('color')?.setValue(respon.data.product.color);
        this.productForm
          .get('quantity')
          ?.setValue(respon.data.product.quantity);
        this.productForm
          .get('categoryId')
          ?.setValue(respon.data.product.categoryId);
        this.productForm
          .get('trademarkId')
          ?.setValue(respon.data.product.trademarkId);

        this.productForm
          .get('isActive')
          ?.setValue(respon.data.product.isActive);
        this.productImage = respon.data.productImages;
        console.log('image', this.productImage);

        this.selectedImageUrl =
          Environment.apiBaseUrl + '/images/' + respon.data.product.thumbnail;
        this.selectedImageFile = new File([''], 'filename');
        respon.data.productImages.forEach((productImage: ProductImage) => {
          this.selectedImageProductUrl.push(productImage.imagePath);
        });
        console.log(this.selectedImageProductUrl);
      },
      error: (err: any) => {
        this.toastr.error(err.error, 'Thất bại');
      },
    });
  }

  createProduct() {
    debugger;
    if (
      this.selectedImageProductFiles.length === 0 &&
      this.productImage.length === 0
    ) {
      this.toastr.error('Chưa chọn ảnh sản phẩm', 'Thất bại');
      return;
    }

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

  updateProduct() {
    debugger;
    // this.productForm.value.id = 0;
    if (this.productForm.value.productImages === null) {
      this.toastr.error('Chưa chọn ảnh sản phẩm', 'Thất bại');
      return;
    }
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
              this.productImage = this.productImage.filter(
                (image) => image.imageName !== imageName
              );

              this.toastr.success('Xóa ảnh thành công');
            },
            error: (err: any) => {
              this.toastr.error(err.error, 'Thất bại');
            },
          });
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

  // items = [
  //   { id: 1, name: 'Item 1', imageURL: 'https://example.com/image1.jpg' },
  //   { id: 2, name: 'Item 2', imageURL: 'https://example.com/image2.jpg' },
  //   { id: 3, name: 'Item 3', imageURL: '' }, // No image URL for this item
  // ];
  items = [
    {
      trademarkId: 5,
      categoryId: 3,
      color: 'Xanh',
      producName: 'Bút 12',
      description: '<p>axx</p>',
      price: 8000,
      priorityLevel: 0,
      quantity: 10,
      notes: '',
      isActive: true,
      isDeleted: false,
      discount: 20,
      image:
        Environment.apiBaseRoot +
        '\\images\\products\\product-17\\d869afa2-2e51-419a-8148-be1665fdeb2a.jpg',
      id: 17,
      createDate: '2024-04-04T15:48:58.8030916',
      createBy: 'Admin',
      modifiedDate: '2024-04-04T15:48:58.8030924',
      modifiedBy: 'Admin',
    },
    {
      trademarkId: 7,
      categoryId: 2,
      color: 'Xanh',
      producName: 'Sản phẩm demo',
      description: '<p>www</p>',
      price: 9000,
      priorityLevel: 0,
      quantity: 0,
      notes: '',
      isActive: true,
      isDeleted: false,
      discount: 20,
      image:
        Environment.apiBaseRoot +
        '\\images\\products\\product-17\\d869afa2-2e51-419a-8148-be1665fdeb2a.jpg',

      id: 16,
      createDate: '2024-04-04T14:54:17.4795909',
      createBy: 'Admin',
      modifiedDate: '2024-04-04T14:54:17.4795916',
      modifiedBy: 'Admin',
    },
    // Các mục sản phẩm khác
  ];
}
