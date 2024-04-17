import { Environment } from './../environment/environment';
import { Injectable } from '@angular/core';
import { ApiServiceService } from './api.service.service';
import { CustomerModel } from '../models/customer.model';
import { ApiResponse } from '../models/api.response.model';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { InsertCustomerDTO } from '../dtos/insert.customer.dto';

@Injectable({
  providedIn: 'root',
})
export class CustomerService {
  constructor(
    private apiService: ApiServiceService,
    private http: HttpClient
  ) {}
  baseUrl = Environment.apiBaseUrl;
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

  findAll() {
    return this.apiService.callApi<CustomerModel>(
      '/Customer/FindAllPaged',
      'get'
    );
  }

  create(customer: any) {
    return this.apiService.callApi<CustomerModel>(
      '/Customer/Create',
      'post',
      null,
      customer
    );
  }

  update(customer: CustomerModel) {
    return this.apiService.callApi<CustomerModel>(
      '/Customer/Update',
      'put',
      null,
      customer
    );
  }

  delete(id: number) {
    return this.apiService.callApi<CustomerModel[]>(
      '/Customer/delete?=' + id,
      'delete'
    );
  }
  UpdateImage(customer: CustomerModel, files: File | null) {
    const formData = new FormData();
    formData.append('id', customer.id.toString());
    formData.append('fullname', customer.fullName);
    formData.append('phoneNumber', customer.phoneNumber.toString());
    formData.append('gender', customer.gender.toString());
    formData.append('birthday', customer.birthday.toString());
    formData.append('userId', customer.userId.toString());
    formData.append('address', customer.address);
    formData.append('ward', customer.ward);
    formData.append('district', customer.district);
    formData.append('city', customer.city);
    if (files) {
      formData.append('file', files);
    }
    return this.http.put(this.baseUrl + '/Customer/Update', formData);
  }
}
