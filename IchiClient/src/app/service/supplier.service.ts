import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { SupplierModel } from '../models/supplier.model';
import { ApiResponse } from '../models/api.response.model';
import { Observable } from 'rxjs';
import { ApiServiceService } from './api.service.service';

@Injectable({
  providedIn: 'root',
})
export class SupplierService {
  constructor(private apiService: ApiServiceService) {}

  // truyền đối tượng SupplierModel
  // findAll(
  //   PageNumber: number,
  //   PageSize: number,
  //   SortDirection: string,
  //   SortBy: string,
  //   Search: string
  // ): Observable<ApiResponse<SupplierModel[]>> {
  //   const params = {
  //     PageNumber: PageNumber,
  //     PageSize: PageSize,
  //     Search: Search,
  //     SortDirection: SortDirection,
  //     SortBy: SortBy,
  //   };
  //   return this.apiService.callApi<SupplierModel[]>(
  //     '/Supplier/FindAllPaged',
  //     'post',
  //     null,
  //     params
  //   );
  // }

  findAll(
    PageNumber: number,
    PageSize: number,
    SortDirection: string,
    SortBy: string,
    Search: string
  ): Observable<ApiResponse<SupplierModel[]>> {
    const params = new HttpParams()
      .set('PageNumber', PageNumber.toString())
      .set('PageSize', PageSize.toString())
      .set('Search', Search)
      .set('SortDirection', SortDirection)
      .set('SortBy', SortBy);

    return this.apiService.callApi<SupplierModel[]>(
      '/Supplier/FindAllPaged',
      'get',
      params
    );
  }

  // findAll() {
  //   return this.http.get(this.apiSupplierUrl);
  // }

  create(supplierDto: SupplierDto) {
    return this.http.post(
      this.apiSupplierAdminUrl,
      supplierDto,
      this.apiConfigUrl
    );
  }

  update(supplierDto: SupplierDto) {
    return this.http.put(
      this.apiSupplierAdminUrl,
      supplierDto,
      this.apiConfigUrl
    );
  }

  delete(id: number) {
    return this.http.delete(`${this.apiSupplierAdminUrl}/${id}`);
  }
}
