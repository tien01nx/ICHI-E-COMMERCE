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
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { SwiperConfigInterface } from 'ngx-swiper-wrapper';
import { SwiperOptions } from 'swiper/types/swiper-options';
import { SwiperContainer } from 'swiper/element/swiper-element';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
})
export class HomeComponent implements OnInit, AfterViewInit {
  Environment = Environment;
  ngOnInit(): void {
    this.getDataProduct();
  }
  products: ProductDTO[] = [];
  constructor(private productService: ProductsService) {}

  @ViewChild('swiper') swiper!: ElementRef<SwiperContainer>;

  index = 0;

  // Swiper
  swiperThumbsConfig: SwiperOptions = {
    slidesPerView: 1,
    spaceBetween: 16,
    breakpoints: {
      450: { slidesPerView: 2, spaceBetween: 16 },
      768: { slidesPerView: 3, spaceBetween: 20 },
      1200: { slidesPerView: 4, spaceBetween: 16 },
    },
    freeMode: true,
    watchSlidesProgress: true,
  };

  ngAfterViewInit() {
    this.swiper.nativeElement.swiper.activeIndex = this.index;
  }

  slideChange(swiper: any) {
    this.index = swiper.detail[0].activeIndex;
  }
  // End Swiper

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
  swiperConfig: SwiperConfigInterface = {
    direction: 'horizontal',
    slidesPerView: 1,
    spaceBetween: 10,
    pagination: {
      el: '.swiper-pagination',
      clickable: true,
    },
  };
}
