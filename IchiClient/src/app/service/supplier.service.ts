import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { SupplierModel } from '../models/supplier.model';
import { ApiResponse } from '../models/api.response.model';
import { Observable, tap } from 'rxjs';
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

  findAllByName(
    PageNumber: number,
    PageSize: number,
    SortDirection: string,
    SortBy: string,
    Search: string
  ): Observable<ApiResponse<SupplierModel>> {
    let params = new HttpParams()
    if (PageNumber && PageNumber.toString().trim() !== '') {
      params = params.set('PageNumber', PageNumber.toString());
    }
    if (PageSize && PageSize.toString().trim() !== '') {
      params = params.set('PageSize', PageSize.toString());
    }
    if (SortDirection && SortDirection.trim() !== '') {
      params = params.set('SortDirection', SortDirection);
    }
    if (SortBy && SortBy.trim() !== '') {
      params = params.set('SortBy', SortBy);
    }
    if (Search && Search.trim() !== '') {
      params = params.set('Search', Search);
    }
    console.log(params);
    return this.apiService.callApi<SupplierModel>(
      '/Supplier/FindAllPaged',
      'get',
      params
    );
  }

  // findAllByName(
  //   PageNumber: number,
  //   PageSize: number,
  //   SortDirection: string,
  //   SortBy: string,
  //   Search: string
  // ): Observable<ApiResponse<SupplierModel>> {
  //   let params = new HttpParams();

  //   const paramConfig = {
  //     PageNumber: PageNumber.toString(),
  //     PageSize: PageSize.toString(),
  //     SortDirection: SortDirection,
  //     SortBy: SortBy,
  //     Search: Search,
  //   };

  //   // Duyệt qua đối tượng config và chỉ thêm những tham số có giá trị
  //   Object.entries(paramConfig).forEach(([key, value]) => {
  //     if (value && value.trim() !== '') {
  //       params = params.set(key, value);
  //     }
  //   });
  //   const finalUrl = `$${params.toString()}`;

  //   console.log(finalUrl);
  //   return this.apiService
  //     .callApi<SupplierModel>('/Supplier/FindAllPaged', 'get', params)
  //     .pipe(
  //       tap((data) => console.log('Supplier Data:', data)) // Log ra dữ liệu nhận được
  //     );
  // }

  findAll() {
    return this.apiService.callApi<SupplierModel>(
      '/Supplier/FindAllPaged',
      'get'
    );
  }

  create(supplier: SupplierModel) {
    return this.apiService.callApi<SupplierModel>(
      '/Supplier/Create',
      'post',
      null,
      supplier
    );
  }

  update(supplier: SupplierModel) {
    return this.apiService.callApi<SupplierModel>(
      '/Supplier/Update',
      'put',
      null,
      supplier
    );
  }

  delete(id: number) {
    return this.apiService.callApi<SupplierModel[]>(
      '/Supplier/' + id,
      'delete'
    );
  }
}
