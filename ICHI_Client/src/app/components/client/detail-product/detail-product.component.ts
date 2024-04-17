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
import { SwiperOptions } from 'swiper';
import { Utils } from '../../../Utils.ts/utils';

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
  image: string = '';

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
  ngAfterViewInit(): void {}

  ngOnInit(): void {
    this.findProductById(this.activatedRoute.snapshot.params['id']);
    this.cartService.getCartItemCount(this.tokenService.getUserEmail());
  }
  findProductById(id: number) {
    this.productService.findById(id).subscribe({
      next: (respon: any) => {
        console.log('data product', respon.data);
        this.productdto = respon.data;
        this.category = respon.data.categoryProduct;
        this.image = respon.data?.productImages[0].imagePath;
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
    // nếu  số lượng thêm vào giỏ lớn hơn số lương sản phẩm thì thông báo lỗi
    if (this.quantity >= this.productdto.product.quantity) {
      this.toastr.warning('Số lượng sản phẩm trong kho không đủ', 'Thông báo');
      this.quantity = this.productdto.product.quantity;
      return;
    }
    if (this.cart) {
      this.quantity++;
      console.log('quantity', this.quantity);
      return;
    }
  }
  changeQuantity(envent: any) {
    this.quantity = envent.target.value;
    if (this.quantity >= this.productdto.product.quantity) {
      // this.toastr = 'Số lượng sản phẩm trong kho không đủ';
      this.toastr.warning('Số lượng sản phẩm trong kho không đủ', 'Thông báo');
      return;
    }
    // nếu số lượng nhập vào nhỏ hơn 1 thì thông báo lỗi
    if (this.quantity < 1) {
      this.toastr.warning('Số lượng sản phẩm phải lớn hơn 0', 'Thông báo');
      this.quantity = 1;
      return;
    }
  }

  // Hàm giảm số lượng khi người dùng nhấn nút minus
  decreaseQuantity() {
    if (this.quantity > 1) {
      this.quantity--;
    }
  }
  onSubmit() {
    if (this.quantity > this.productdto.product.quantity) {
      this.toastr.warning('Số lượng sản phẩm trong kho không đủ', 'Thông báo');
      this.quantity = this.productdto.product.quantity;
      return;
    }
    if (this.quantity < 1) {
      this.toastr.warning('Số lượng sản phẩm phải lớn hơn 0', 'Thông báo');
      this.quantity = 1;
      return;
    }
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

  // Hàm mua ngay sản phẩm lấy thông tin sản phẩm và số lượng và lưu vào local storage và chuyển hướng đến trang thanh toán
  buyNow() {
    this.cart.productId = this.productdto.product.id;
    this.cart.price = this.productdto.product.price;
    this.cart.quantity = this.quantity;
    localStorage.setItem(Utils.cartBuyNow, JSON.stringify(this.cart));
  }

  // Swiper
  swiperThumbsConfig: SwiperOptions = {
    slidesPerView: 1, // Số lượng slide hiển thị trên một lần trượt
    spaceBetween: 20, // Khoảng cách giữa các slide
    speed: 500, // Tốc độ chuyển slide (milliseconds)
    autoplay: {
      // Tự động chuyển slide
      delay: 3000, // Thời gian delay giữa các slide (milliseconds)
      disableOnInteraction: false, // Tạm dừng tự động chuyển slide khi người dùng tương tác
    },
    navigation: {
      // Hiển thị nút điều hướng
      nextEl: '.swiper-button-next', // Nút next
      prevEl: '.swiper-button-prev', // Nút prev
    },
    breakpoints: {
      450: { slidesPerView: 2, spaceBetween: 16 },
      768: { slidesPerView: 3, spaceBetween: 16 },
      992: { slidesPerView: 4, spaceBetween: 16 },
      1200: { slidesPerView: 5, spaceBetween: 16 },
    },
  };

  swiperImageConfig: SwiperOptions = {
    slidesPerView: 1, // Số lượng slide hiển thị trên một lần trượt
    spaceBetween: 16, // Khoảng cách giữa các slide
    navigation: {
      // Hiển thị nút điều hướng
      nextEl: '.swiper-next', // Nút next
      prevEl: '.swiper-prev', // Nút prev
    },
    breakpoints: {
      200: { slidesPerView: 3, spaceBetween: 16 },
      450: { slidesPerView: 4, spaceBetween: 16 },
      768: { slidesPerView: 4, spaceBetween: 16 },
      992: { slidesPerView: 4, spaceBetween: 16 },
      1200: { slidesPerView: 4, spaceBetween: 16 },
    },
  };
  chooseImage(image: any) {
    this.image = image.imagePath;
  }
}
