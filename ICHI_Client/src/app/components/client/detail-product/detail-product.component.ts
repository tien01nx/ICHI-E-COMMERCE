import { CategoryProduct } from './../../../models/category.product';
import { Environment } from './../../../environment/environment';
import { AfterViewInit, Component, OnInit } from '@angular/core';
import { ClientFooterComponent } from '../client-footer/client-footer.component';
import { ClientMenuComponent } from '../client-menu/client-menu.component';
import { ClientHeaderComponent } from '../client-header/client-header.component';
import { ProductDTO } from '../../../dtos/product.dto';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductsService } from '../../../service/products.service';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';
import { NgxDropzoneModule } from 'ngx-dropzone';
import { InsertCartDTO } from '../../../dtos/insert.cart.dto';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { TokenService } from '../../../service/token.service';
import { TrxTransactionService } from '../../../service/trx-transaction.service';
import { ToastrService } from 'ngx-toastr';
import { CategoryService } from '../../../service/category-product.service';

@Component({
  selector: 'app-detail-product',
  templateUrl: './detail-product.component.html',
  styleUrl: './detail-product.component.css',
})
export class DetailProductComponent implements OnInit, AfterViewInit {
  protected readonly Environment = Environment;
  productdto: any;
  errorMessage: string = '';
  cart: InsertCartDTO = new InsertCartDTO('', 0, 0, 1);
  quantity: number = 1;
  category!: CategoryProduct;
  categories: CategoryProduct[] = [];
  constructor(
    private activatedRoute: ActivatedRoute,
    private productService: ProductsService,
    private categoryService: CategoryService,
    private tokenService: TokenService,
    private toastr: ToastrService,
    private router: Router,
    private cartService: TrxTransactionService,
    private sanitizer: DomSanitizer
  ) {}
  ngAfterViewInit(): void {
    // this.ratingInit();
    console.log('object', this.category);
  }

  ngOnInit(): void {
    this.findProductById(this.activatedRoute.snapshot.params['id']);
    this.cartService.getCartItemCount(this.tokenService.getUserEmail());
  }
  findProductById(id: number) {
    this.productService.findById(id).subscribe({
      next: (respon: any) => {
        this.productdto = respon.data;
        this.category = respon.data.categoryProduct;
        this.getParent();
      },
    });
  }

  getParent() {
    this.categoryService.getParentId(this.category).subscribe({
      next: (respon: any) => {
        this.categories = respon.data;
        console.log('list', this.categories);
      },
    });
  }
  getHtmlProductDescription(): SafeHtml {
    let description = this.productdto?.product?.description;

    if (!description || description.trim() === '') {
      return this.sanitizer.bypassSecurityTrustHtml(
        'Chưa có thông tin mô tả sản phẩm.'
      );
    }

    // Thực hiện sự san phẳng cho HTML
    let sanitizedDescription =
      this.sanitizer.bypassSecurityTrustHtml(description);

    // Tạo CSS cho việc kiểm soát kích thước hình ảnh
    let style = this.sanitizer.bypassSecurityTrustStyle(`
        .custom-html img {
            max-width: 100%; /* Đảm bảo hình ảnh không vượt quá kích thước của phần tử cha */
            height: auto; /* Giữ tỷ lệ khung hình */
            display: block; /* Ngăn chặn hình ảnh làm hỏng bố cục */
            margin: 0 auto; /* Căn giữa hình ảnh */
        }
    `);

    // Combine HTML content and CSS styles
    let combinedContent = `${style}${sanitizedDescription}`;

    return this.sanitizer.bypassSecurityTrustHtml(combinedContent);
  }

  // Hàm tăng số lượng khi người dùng nhấn nút plus
  increaseQuantity() {
    if (this.cart) {
      this.quantity++;
    }
  }

  // Hàm giảm số lượng khi người dùng nhấn nút minus
  decreaseQuantity() {
    if (this.quantity > 1) {
      this.quantity--;
    }
  }
  onSubmit() {
    this.cart.productId = this.productdto.product.id;
    this.cart.price = this.productdto.product.price;
    this.cart.userId = this.tokenService.getUserEmail();
    this.cart.quantity = this.quantity;
    this.cartService.AddToCart(this.cart).subscribe({
      next: (response: any) => {
        if (response.code === 200) {
          // Đăng nhập thành công
          if (response.message === 'Thêm vào giỏ hàng thành công') {
            this.toastr.success(response.message, 'Thông báo');
            this.router.navigate(['/']);
          } else {
            this.errorMessage = response.message;
          }
        } else {
          this.errorMessage = response.message;
          // this.isDisplayNone = false;
        }
      },
      error: (error: any) => {
        this.errorMessage = error.error;
        // this.isDisplayNone = false;
      },
    });
  }

  productCategory(category: CategoryProduct) {
    this.router.navigate(['/product_filter', category.categoryName]);
  }
}
