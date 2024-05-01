import { Injectable } from '@angular/core';
import { ApiServiceService } from './api.service.service';
import { ApiResponse } from '../models/api.response.model';
import { Observable, from } from 'rxjs';
import { ProductModel } from '../models/product.model';
import { HttpClient, HttpParams } from '@angular/common/http';
import { InsertProductDTO } from '../dtos/insert.product.dto';
import { ProductDTO } from '../dtos/product.dto';
import { Environment } from '../environment/environment';

@Injectable({
  providedIn: 'root',
})
export class ProductsService {
  baseUrl = Environment.apiBaseUrl;
  constructor(
    private apiService: ApiServiceService,
    private http: HttpClient
  ) {}

  ProductTopFive(dateTime: string) {
    return this.http.get(
      this.baseUrl + '/Product/ProductTopFive?dateTime=' + dateTime
    );
  }

  findAllProduct() {
    return this.http.get(this.baseUrl + '/Product/FindAll');
  }

  searchProductName(name: string) {
    return this.http.get(this.baseUrl + '/Product/Search/' + name);
  }

  // this.baseUrl + '/TrxTransaction/GetMonneyTotalByMonth?year=' + id

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

  findProductToCategory(
    categoryName: string,
    colors: string[],
    categories: string[],
    trademarks: string[],
    priceMin: number,
    priceMax: number,
    pageSize: number,
    pageNumber: number
  ) {
    let params = new HttpParams();
    if (categoryName && categoryName.trim() !== '') {
      params = params.set('category-name', categoryName);
    }
    if (colors && colors.length > 0) {
      params = params.set('colors', colors.join(','));
    }
    if (categories && categories.length > 0) {
      params = params.set('category-parent', categories.join(','));
    }

    if (trademarks && trademarks.length > 0) {
      params = params.set('trademark-name', trademarks.join(','));
    }
    if (priceMin && priceMin.toString().trim() !== '') {
      params = params.set('price-min', priceMin.toString());
    }
    if (priceMax && priceMax.toString().trim() !== '') {
      params = params.set('price-max', priceMax.toString());
    }
    if (pageSize && pageSize.toString().trim() !== '') {
      params = params.set('page-size', pageSize.toString());
    }
    if (pageNumber && pageNumber.toString().trim() !== '') {
      params = params.set('page-number', pageNumber.toString());
    }
    console.log(params);

    return this.http.get(this.baseUrl + '/Product/FindProductInCategoryName', {
      params: params,
    });
  }
  findAll() {
    return this.apiService.callApi<ProductModel>(
      '/Product/FindAllPaged',
      'get'
    );
  }

  findById(id: number) {
    return this.apiService.callApi<ProductDTO>(
      '/Product/GetProductById/' + id,
      'get'
    );
  }

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
    return this.http.delete(this.baseUrl + '/Product/' + id);
  }

  deleteImage(id: number, imageName: string) {
    return this.http.delete(
      this.baseUrl +
        '/Product/Delete-Image?productId=' +
        id +
        '&imageName=' +
        imageName
    );
  }
}
