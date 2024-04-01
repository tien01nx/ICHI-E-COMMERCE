import { PromotionModel } from './../models/promotion.model';
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ApiResponse } from '../models/api.response.model';
import { Observable, tap } from 'rxjs';
import { ApiServiceService } from './api.service.service';
import { InsertSupplierDTO } from '../dtos/insert.supplier.dto';
import { Environment } from '../environment/environment';

@Injectable({
  providedIn: 'root',
})
export class PromotionService {
  baseUrl = Environment.apiBaseUrl;
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
  ): Observable<ApiResponse<PromotionModel>> {
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
    return this.apiService.callApi<PromotionModel>(
      '/Promotion/FindAllPaged',
      'get',
      params
    );
  }
  findAll() {
    return this.apiService.callApi<PromotionModel>(
      '/Promotion/FindAllPaged',
      'get'
    );
  }

  findById<T>(id: number): Observable<ApiResponse<T>> {
    return this.http.get<ApiResponse<T>>(this.baseUrl + '/Promotion/' + id);
  }

  create(data: any) {
    return this.http.post('https://localhost:7150/api/Promotion/Create', data);
  }
  update(data: any) {
    return this.http.put('https://localhost:7150/api/Promotion/Update', data);
  }
  delete(id: number) {
    return this.apiService.callApi<PromotionModel[]>(
      '/Promotion/' + id,
      'delete'
    );
  }

  updateStatus(id: number, status: boolean) {
    return this.apiService.callApi<PromotionModel[]>(
      '/Promotion/' + id,
      'delete'
    );
  }
}
