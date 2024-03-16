import { CategoryProduct } from './../../../models/category.product';
import { Environment } from './../../../environment/environment';
import { AfterViewInit, Component, OnInit } from '@angular/core';
import { ClientFooterComponent } from '../client-footer/client-footer.component';
import { ClientMenuComponent } from '../client-menu/client-menu.component';
import { ClientHeaderComponent } from '../client-header/client-header.component';
import { ProductDTO } from '../../../dtos/product.dto';
import { ActivatedRoute } from '@angular/router';
import { ProductsService } from '../../../service/products.service';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';
import { NgxDropzoneModule } from 'ngx-dropzone';

@Component({
  selector: 'app-detail-product',
  standalone: true,
  templateUrl: './detail-product.component.html',
  styleUrl: './detail-product.component.css',
  imports: [
    ClientFooterComponent,
    ClientMenuComponent,
    ClientHeaderComponent,
    CommonModule,
    NgxDropzoneModule,
  ],
})
export class DetailProductComponent implements OnInit, AfterViewInit {
  protected readonly Environment = Environment;
  productdto: any;
  constructor(
    private activatedRoute: ActivatedRoute,
    private productService: ProductsService,
    private sanitizer: DomSanitizer
  ) {}
  ngAfterViewInit(): void {
    // this.ratingInit();
  }

  ngOnInit(): void {
    this.findProductById(this.activatedRoute.snapshot.params['id']);
  }
  findProductById(id: number) {
    this.productService.findById(id).subscribe({
      next: (respon: any) => {
        this.productdto = respon.data;
        console.log(this.productdto);
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
}
