import { Environment } from './../../../environment/environment';
import { CUSTOM_ELEMENTS_SCHEMA, Component, OnInit } from '@angular/core';
import { ProductsService } from '../../../service/products.service';
import { ProductDTO } from '../../../dtos/product.dto';
import { ProductModel } from '../../../models/product.model';
import { EditorModule } from '@tinymce/tinymce-angular';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { SwiperConfigInterface } from 'ngx-swiper-wrapper';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    EditorModule,
    FormsModule,
    ReactiveFormsModule,
    CommonModule,
    RouterLink,
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
})
export class HomeComponent implements OnInit {
  Environment = Environment;
  ngOnInit(): void {
    this.getDataProduct();
  }
  products: any;
  constructor(private productService: ProductsService) {}

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
