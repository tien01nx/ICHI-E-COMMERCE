import { Injectable } from '@angular/core';
import { ApiServiceService } from './api.service.service';
import { CustomerModel } from '../models/customer.model';
import { ApiResponse } from '../models/api.response.model';
import { HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { InsertCustomerDTO } from '../dtos/insert.customer.dto';

@Injectable({
  providedIn: 'root',
})
export class CustomerService {
  constructor(private apiService: ApiServiceService) {}

  findAllByName(
    PageNumber: number,
    PageSize: number,
    SortDirection: string,
    SortBy: string,
    Search: string
  ): Observable<ApiResponse<CustomerModel>> {
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
    return this.apiService.callApi<CustomerModel>(
      '/Customer/FindAllPaged',
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
  // ): Observable<ApiResponse<CustomerModel>> {
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
  //     .callApi<CustomerModel>('/Supplier/FindAllPaged', 'get', params)
  //     .pipe(
  //       tap((data) => console.log('Supplier Data:', data)) // Log ra dữ liệu nhận được
  //     );
  // }

  findAll() {
    return this.apiService.callApi<CustomerModel>(
      '/Customer/FindAllPaged',
      'get'
    );
  }

  create(customer: any) {
    return this.apiService.callApi<CustomerModel>(
      '/Customer/Create-Customer',
      'post',
      null,
      customer
    );
  }

  update(supplier: CustomerModel) {
    return this.apiService.callApi<CustomerModel>(
      '/Customer/Update',
      'put',
      null,
      supplier
    );
  }

  delete(id: number) {
    return this.apiService.callApi<CustomerModel[]>(
      '/Customer/' + id,
      'delete'
    );
  }
}
