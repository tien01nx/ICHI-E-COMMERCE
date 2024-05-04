import { CategoryProduct } from './../../../models/category.product';
import { Environment } from './../../../environment/environment';
import {
  AfterViewInit,
  CUSTOM_ELEMENTS_SCHEMA,
  Component,
  ElementRef,
  OnInit,
  ViewChild,
} from '@angular/core';
import { ProductsService } from '../../../service/products.service';
import { ProductDTO } from '../../../dtos/product.dto';
import { ProductModel } from '../../../models/product.model';
import { EditorModule } from '@tinymce/tinymce-angular';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { SwiperConfigInterface } from 'ngx-swiper-wrapper';
import { SwiperOptions } from 'swiper/types/swiper-options';
import { SwiperContainer } from 'swiper/element/swiper-element';
import { CategoryService } from '../../../service/category-product.service';
import { Utils } from '../../../Utils.ts/utils';
import { PaginationDTO } from '../../../dtos/pagination.dto';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
})
export class HomeComponent implements OnInit {
  Environment = Environment;
  date: any;
  dataProductNew: ProductDTO[] = [];
  ngOnInit(): void {
    this.getDataProduct();
    this.getCategoryProduct();
    this.date = new Date().toDateString();
    this.productTopFive(this.date);
  }
  products: ProductDTO[] = [];
  productdto: ProductDTO[] = [];

  CategoryProduct: CategoryProduct[] = [];
  constructor(
    private productService: ProductsService,
    private categortService: CategoryService,
    private router: Router
  ) {}

  banners: string[] = [
    '../../../assets/img/banner/home1.png',
    '../../../assets/img/banner/home2.png',
    '../../../assets/img/banner/home3.png',
  ];

  // Swiper
  swiperNewestConfig: SwiperOptions = {
    slidesPerView: 1,
    spaceBetween: 20,
    speed: 500,
    navigation: {
      // Hiển thị nút điều hướng
      nextEl: '.swiper-button-next', // Nút next
      prevEl: '.swiper-button-prev', // Nút prev
    },
    allowTouchMove: false,
    breakpoints: {
      450: { slidesPerView: 1, spaceBetween: 16 },
      768: { slidesPerView: 1, spaceBetween: 16 },
      992: { slidesPerView: 5, spaceBetween: 16 },
      1200: { slidesPerView: 6, spaceBetween: 16 },
    },
  };

  // Swiper
  swiperNewestConfig1: SwiperOptions = {
    slidesPerView: 1,
    spaceBetween: 20,
    speed: 500,
    navigation: {
      // Hiển thị nút điều hướng
      nextEl: '.swiper-button-next1', // Nút next
      prevEl: '.swiper-button-prev1', // Nút prev
    },
    allowTouchMove: false,
    breakpoints: {
      450: { slidesPerView: 1, spaceBetween: 16 },
      768: { slidesPerView: 1, spaceBetween: 16 },
      992: { slidesPerView: 5, spaceBetween: 16 },
      1200: { slidesPerView: 6, spaceBetween: 16 },
    },
  };

  // Swiper
  swiperNewestConfig2: SwiperOptions = {
    slidesPerView: 1,
    spaceBetween: 20,
    speed: 500,
    navigation: {
      // Hiển thị nút điều hướng
      nextEl: '.swiper-button-next2', // Nút next
      prevEl: '.swiper-button-prev2', // Nút prev
    },
    allowTouchMove: false,
    breakpoints: {
      450: { slidesPerView: 1, spaceBetween: 16 },
      768: { slidesPerView: 1, spaceBetween: 16 },
      992: { slidesPerView: 5, spaceBetween: 16 },
      1200: { slidesPerView: 6, spaceBetween: 16 },
    },
  };

  swiperBannerConfig: SwiperOptions = {
    slidesPerView: 1, // Số lượng slide hiển thị trên một lần trượt
    spaceBetween: 20, // Khoảng cách giữa các slide
    speed: 500, // Tốc độ chuyển slide (milliseconds)
    autoplay: {
      // Tự động chuyển slide
      delay: 4000, // Thời gian delay giữa các slide (milliseconds)
    },
    allowTouchMove: false, // Cho phép chạm để chuyển slide
    loop: true,
  };

  getDataProduct() {
    this.productService.findAllByName(1, 10000, 'ASC', 'id', '').subscribe({
      next: (response: any) => {
        this.products = response.data.items;

        // chỉ lấy data từ response và với điều kiện là response.data.items.product.discount > 0
        this.products = this.products.filter(
          (item: any) => item.product.discount > 0
        );
        
        this.dataProductNew = [...this.products].sort((a, b) => {
          return (
            new Date(b.product.createDate).getTime() -
            new Date(a.product.createDate).getTime()
          );
        });
        // lấy ra cac sản phẩm giảm dần theo thời gian  createDate  response.data.items.product.createDate

        console.log('promotionProduct:', this.products);
        console.log('productNew :', this.dataProductNew);
        // [0].product.discount;
      },
      error: (error: any) => {
        console.log(error);
      },
    });
  }

  productTopFive(dateTime: string) {
    this.productService.ProductTopFive(dateTime).subscribe({
      next: (res: any) => {
        console.log('data List:', res.data);
        this.productdto = res.data;
      },
      error: (err) => {
        console.log(err);
      },
    });
  }

  getCategoryProduct() {
    this.categortService.findAll().subscribe({
      next: (response: any) => {
        console.log(response.data);
        // lấy dữ liệu từ response và gán vào mảng CategoryProduct với điều kiện là response.data.items.CategoryLevel ===1

        this.CategoryProduct = response.data.filter(
          (item: any) => item.categoryLevel === 2
        );
        console.log(this.CategoryProduct);
      },
      error: (error: any) => {
        console.log(error);
      },
    });
  }

  productFilter(categoryName: string) {
    this.router.navigate(['/product_filter/' + categoryName]);
  }

  getImageUrl(categoryName: string): string {
    const category = Utils.categories.find((cat) => cat.name === categoryName);
    return category ? category.image : '';
  }
}
