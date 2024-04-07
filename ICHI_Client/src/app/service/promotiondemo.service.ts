import { Injectable } from '@angular/core';
import { Environment } from '../environment/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { ApiResponse } from '../response/api.response';
import { Observable } from 'rxjs';
import { UpdatePromotionDTO } from '../dtos/update.user.dto copy';

@Injectable({
  providedIn: 'root',
})
export class PromotiondemoService {
  private apiBaseUrl = Environment.apiBaseUrl;

  constructor(private http: HttpClient) {}
  getPromotiom(
    PageNumber: number,
    PageSize: number,
    SortDirection: string,
    SortBy: string,
    Search: string
  ): Observable<ApiResponse> {
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
    // console.log(params);
    // return this.apiService.callApi<PromotionModel>(
    //   '/Promotion/FindAllPaged',
    //   'get',
    //   params
    // );
    return this.http.get<ApiResponse>(
      `${Environment.apiBaseUrl}/Promotion/FindAllPaged`,
      {
        params,
      }
    );
  }

  
  getDetailCategory(id: number): Observable<ApiResponse> {
    return this.http.get<ApiResponse>(`${this.apiBaseUrl}/categories/${id}`);
  }
  deleteCategory(id: number): Observable<ApiResponse> {
    debugger;
    return this.http.delete<ApiResponse>(`${this.apiBaseUrl}/categories/${id}`);
  }
  updateCategory(
    id: number,
    updatedCategory: UpdatePromotionDTO
  ): Observable<ApiResponse> {
    return this.http.put<ApiResponse>(
      `${this.apiBaseUrl}/categories/${id}`,
      updatedCategory
    );
  }
  insertCategory(
    insertCategoryDTO: UpdatePromotionDTO
  ): Observable<ApiResponse> {
    // Add a new category
    return this.http.post<ApiResponse>(
      `${this.apiBaseUrl}/categories`,
      insertCategoryDTO
    );
  }
}
