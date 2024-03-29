import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ApiResponse } from '../models/api.response.model';
import { Observable, tap } from 'rxjs';
import { ApiServiceService } from './api.service.service';
import { InsertSupplierDTO } from '../dtos/insert.supplier.dto';
import { CategoryProduct } from '../models/category.product';
import { Environment } from '../environment/environment';

@Injectable({
  providedIn: 'root',
})
export class CategoryService {
  apiBaseUrl = Environment.apiBaseUrl;
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
  ): Observable<ApiResponse<CategoryProduct>> {
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
    return this.apiService.callApi<CategoryProduct>(
      '/CategoryProduct/FindAllPaged',
      'get',
      params
    );
  }
  findAll() {
    // return this.http.get(this.apiBaseUrl + '/CategoryProduct/FindAllPaged', {params: });
    return this.apiService.callApi<CategoryProduct>(
      '/CategoryProduct/FindAll',
      'get'
    );
  }

  // data theo parentId
  findAllByParentId(categoryName: string) {
    return this.http.get(
      this.apiBaseUrl +
        '/CategoryProduct/GetCategoryByProduct?categoryname=' +
        categoryName
    );
  }

  create(category: InsertSupplierDTO) {
    return this.apiService.callApi<CategoryProduct>(
      '/CategoryProduct/Create',
      'post',
      null,
      category
    );
  }

  update(category: CategoryProduct) {
    return this.apiService.callApi<CategoryProduct>(
      '/CategoryProduct/Update',
      'put',
      null,
      category
    );
  }

  delete(id: number) {
    return this.apiService.callApi<CategoryProduct[]>(
      '/CategoryProduct/' + id,
      'delete'
    );
  }
}
