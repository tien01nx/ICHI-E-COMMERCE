import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';
import { ApiResponse } from '../models/api.response.model';
import { CategoryProduct } from '../models/category.product';
import { ApiServiceService } from './api.service.service';
import { InsertCategoryDTO } from '../dtos/insert.category.dto';
import { PaginationParams } from '../models/PaginationParams';

@Injectable({
  providedIn: 'root',
})
export class CategoryService {
  constructor(private apiService: ApiServiceService) {}

  findAll(
    PageNumber: number,
    PageSize: number,
    SortDirection: string,
    SortBy: string,
    Search: string
  ): Observable<ApiResponse<CategoryProduct[]>> {
    const requestBody = {
      PageNumber: PageNumber,
      PageSize: PageSize,
      Search: Search,
      SortDirection: SortDirection,
      SortBy: SortBy,
    };
    return this.apiService.callApi<CategoryProduct[]>(
      '/CategoryProduct/FindAllPaged',
      'post',
      null,
      requestBody // Đối tượng HttpParams được truyền vào đây
    );
  }

  UpSertCategory(
    categoryData: InsertCategoryDTO
  ): Observable<ApiResponse<any>> {
    if (categoryData.id) {
      return this.apiService.callApi<ApiResponse<any>>(
        `/CategoryProduct/Update/${categoryData.id}`,
        'PUT',
        null,
        categoryData
      );
    } else {
      return this.apiService.callApi<ApiResponse<any>>(
        '/CategoryProduct/Create',
        'POST',
        null,
        categoryData
      );
    }
  }

  deleteOne(id: number): Observable<ApiResponse<any>> {
    return this.apiService.callApi<ApiResponse<any>>(
      `/CategoryProduct/delete/${id}`,
      'PUT',
      null
    );
  }

  findById(id: number): Observable<ApiResponse<CategoryProduct>> {
    return this.apiService.callApi<CategoryProduct>(
      `/CategoryProduct/${id}`,
      'GET',
      null
    );
  }
}
