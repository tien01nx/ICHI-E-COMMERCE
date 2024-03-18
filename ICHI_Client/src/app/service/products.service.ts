import { Injectable } from '@angular/core';
import { ApiServiceService } from './api.service.service';
import { ApiResponse } from '../models/api.response.model';
import { Observable, from } from 'rxjs';
import { ProductModel } from '../models/product.model';
import { HttpClient, HttpParams } from '@angular/common/http';
import { InsertProductDTO } from '../dtos/insert.product.dto';
import { ProductDTO } from '../dtos/product.dto';

@Injectable({
  providedIn: 'root',
})
export class ProductsService {
  constructor(
    private apiService: ApiServiceService,
    private http: HttpClient
  ) {}

  findAllByName(
    PageNumber: number,
    PageSize: number,
    SortDirection: string,
    SortBy: string,
    Search: string
  ): Observable<ApiResponse<ProductModel>> {
    let params = new HttpParams();
    if (PageNumber && PageNumber.toString().trim() !== '') {
      params = params.set('page-number', PageNumber.toString());
    }
    if (PageSize && PageSize.toString().trim() !== '') {
      params = params.set('page-size', PageSize.toString());
    }
    if (SortDirection && SortDirection.trim() !== '') {
      params = params.set('sort-direction', SortDirection);
    }
    if (SortBy && SortBy.trim() !== '') {
      params = params.set('sort-by', SortBy);
    }
    if (Search && Search.trim() !== '') {
      params = params.set('search', Search);
    }
    console.log(params);
    return this.apiService.callApi<ProductModel>(
      '/Product/FindAllPaged',
      'get',
      params
    );
  }

  // findAll() {
  //   return this.apiService.callApi<ProductModel>(
  //     '/Product/FindAllPaged',
  //     'get'
  //   );
  // }

  findById(id: number) {
    return this.apiService.callApi<ProductDTO>(
      '/Product/GetProductById/' + id,
      'get'
    );
  }

  //   public int TrademarkId { get; set; }
  // [ForeignKey("TrademarkId")]
  // [ValidateNever]
  // public Trademark? Trademark { get; set; }
  // [Required]
  // public int CategoryId { get; set; }
  // [ForeignKey("CategoryId")]
  // [ValidateNever]
  // public Category? Category { get; set; }
  // [Required]
  // [StringLength(255)]

  // public string Color { get; set; } = string.Empty;
  // public string ProductName { get; set; } = string.Empty;
  // [Required]
  // public string Description { get; set; } = string.Empty;
  // [Required]
  // public decimal Price { get; set; } = 0;
  // public string Image { get; set; } = string.Empty;
  // public int PriorityLevel { get; set; } = 0;
  // public string Notes { get; set; } = string.Empty;
  // public bool isActive { get; set; } = false;
  // public bool isDeleted { get; set; } = false;

  create(product: InsertProductDTO, files: File[]) {
    const formData = new FormData();
    formData.append('Id', product.id.toString());
    formData.append('TrademarkId', product.trademarkId.toString());
    formData.append('CategoryId', product.categoryId.toString());
    formData.append('Color', product.color);
    formData.append('ProductName', product.productName);
    formData.append('Description', product.description);
    formData.append('Price', product.price.toString());
    formData.append('PriorityLevel', product.priorityLevel.toString());
    formData.append('Quantity', product.quantity.toString());
    formData.append('isActive', product.isActive.toString());
    if (
      product.notes != null &&
      product.notes != undefined &&
      product.notes != ''
    ) {
      formData.append('Notes', product.notes);
    }

    for (let i = 0; i < files.length; i++) {
      formData.append('files', files[i]);
    }
    console.log(product, files);
    return this.http.post(
      'https://localhost:7150/api/Product/Upsert',
      formData
    );
  }

  update(product: ProductModel, files: File[]) {
    return this.apiService.callApi<ProductModel>(
      '/Product/Update',
      'put',
      null
    );
  }

  deleteProductDetails(id: number) {
    return this.apiService.callApi<ProductModel[]>('/Product/' + id, 'delete');
  }

  deleteImage(id: number, imageName: string) {
    return this.apiService.callApi<string>(
      'Product/Delete-Image?productId=' + id + '&imageName=' + imageName,
      'delete'
    );
  }
}
