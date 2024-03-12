import { Injectable } from '@angular/core';
import { ApiServiceService } from './api.service.service';
import { ApiResponse } from '../models/api.response.model';
import { Observable } from 'rxjs';
import { ProductModel } from '../models/product.model';
import { HttpParams } from '@angular/common/http';
import { InsertProductDTO } from '../dtos/insert.product.dto';
import { ProductDTO } from '../dtos/product.dto';

@Injectable({
  providedIn: 'root',
})
export class ProductsService {
  constructor(private apiService: ApiServiceService) {}

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

  create(product: InsertProductDTO, files: File[]) {
    const formData = new FormData();
    formData.append('ProductName', product.productName);
    formData.append('Description', product.description);
    formData.append('CategoryProductID', product.categoryProductID.toString());
    formData.append('SuggestedPrice', product.suggestedPrice.toString());
    formData.append('SellingPrice', product.sellingPrice.toString());
    formData.append('Notes', product.notes.toString());

    for (let i = 0; i < files.length; i++) {
      formData.append('files', files[i]);
    }
    console.log(product, files);
    return this.apiService.callApi<ProductModel>(
      '/Product/Create-Product',
      'post',
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
      '/Product/Delete-Image/' + id + '/' + imageName,
      'delete'
    );
  }
}
