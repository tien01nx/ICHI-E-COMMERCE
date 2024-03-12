import { Component, OnInit } from '@angular/core';
import { ProductsService } from '../../../service/products.service';
import { ProductDTO } from '../../../dtos/product.dto';
import { ProductModel } from '../../../models/product.model';
import { EditorModule } from '@tinymce/tinymce-angular';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';

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
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
})
export class HomeComponent implements OnInit {
  ngOnInit(): void {
    this.getDataProduct();
  }
  products: any;
  constructor(private productService: ProductsService) {}

  getDataProduct() {
    this.productService.findAllByName(1, 10, 'ASC', 'id', '').subscribe({
      next: (response: any) => {
        this.products = response.data.items;
        console.log(response);
      },
      error: (error: any) => {
        console.log(error);
      },
    });
  }
}
