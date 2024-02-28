import { Injectable } from '@angular/core';
import { ApiServiceService } from './api.service.service';
import { ApiResponse } from '../models/api.response.model';
import { Observable } from 'rxjs';
import { ProductModel } from '../models/product.model';
import { HttpParams } from '@angular/common/http';
import { InsertProductDTO } from '../dtos/insert.product.dto';

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

  findAll() {
    return this.apiService.callApi<ProductModel>(
      '/Product/FindAllPaged',
      'get'
    );
  }

  create(supplier: InsertProductDTO) {
    debugger;
    return this.apiService.callApi<ProductModel>(
      '/Product/Create-Product',
      'post',
      null,
      supplier
    );
  }

  update(supplier: ProductModel) {
    return this.apiService.callApi<ProductModel>(
      '/Product/Update',
      'put',
      null,
      supplier
    );
  }

  delete(id: number) {
    return this.apiService.callApi<ProductModel[]>('/Supplier/' + id, 'delete');
  }
}
