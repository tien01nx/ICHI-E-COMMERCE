import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ApiResponse } from '../models/api.response.model';
import { Observable, tap } from 'rxjs';
import { ApiServiceService } from './api.service.service';
import { InsertSupplierDTO } from '../dtos/insert.supplier.dto';
import { CategoryProduct } from '../models/category.product';

@Injectable({
  providedIn: 'root',
})
export class CategoryService {
  constructor(private apiService: ApiServiceService) {}

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
    return this.apiService.callApi<CategoryProduct>(
      '/CategoryProduct/FindAllPaged',
      'get'
    );
  }

  create(category: InsertSupplierDTO) {
    debugger;
    return this.apiService.callApi<CategoryProduct>(
      '/CategoryProduct/Create-Category',
      'post',
      null,
      category
    );
  }

  update(category: CategoryProduct) {
    debugger;
    return this.apiService.callApi<CategoryProduct>(
      '/CategoryProduct/Update-Category',
      'post',
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
