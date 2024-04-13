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
  ngOnInit(): void {
    this.getDataProduct();
    this.getCategoryProduct();
  }
  products: ProductDTO[] = [];
  CategoryProduct: CategoryProduct[] = [];
  constructor(
    private productService: ProductsService,
    private categortService: CategoryService,
    private router: Router
  ) {}

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
    breakpoints: {
      450: { slidesPerView: 1, spaceBetween: 16 },
      768: { slidesPerView: 1, spaceBetween: 16 },
      992: { slidesPerView: 5, spaceBetween: 16 },
      1200: { slidesPerView: 6, spaceBetween: 16 },
    },
  };

  getDataProduct() {
    this.productService.findAllByName(1, 10, 'ASC', 'id', '').subscribe({
      next: (response: any) => {
        this.products = response.data.items;
        console.log(this.products);
      },
      error: (error: any) => {
        console.log(error);
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
