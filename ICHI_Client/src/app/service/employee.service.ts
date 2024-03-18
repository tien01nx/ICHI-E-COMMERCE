import { Environment } from './../environment/environment';
import { Injectable } from '@angular/core';
import { ApiServiceService } from './api.service.service';
import { ApiResponse } from '../models/api.response.model';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { EmployeeModel } from '../models/employee.model';

@Injectable({
  providedIn: 'root',
})
export class EmployeeService {
  constructor(
    private apiService: ApiServiceService,
    private http: HttpClient
  ) {}
  Environment = Environment;
  findAllByName(
    PageNumber: number,
    PageSize: number,
    SortDirection: string,
    SortBy: string,
    Search: string
  ): Observable<ApiResponse<EmployeeModel>> {
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
    return this.apiService.callApi<EmployeeModel>(
      '/Employee/FindAllPaged',
      'get',
      params
    );
  }

  findAll() {
    return this.apiService.callApi<EmployeeModel>(
      '/Employee/FindAllPaged',
      'get'
    );
  }

  create(Employee: any) {
    return this.apiService.callApi<EmployeeModel>(
      '/Employee/Create',
      'post',
      null,
      Employee
    );
  }

  update(Employee: EmployeeModel) {
    return this.apiService.callApi<EmployeeModel>(
      '/Employee/Update',
      'put',
      null,
      Employee
    );
  }

  delete(id: number) {
    return this.apiService.callApi<EmployeeModel[]>(
      '/Employee/Delete?id=' + id,
      'delete'
    );
  }
  private apiProductAdminUrl = `${Environment.apiBaseUrl}/Employee`;
  UpdateImage(Employee: EmployeeModel, files: File | null) {
    const formData = new FormData();
    formData.append('id', Employee.id.toString());
    formData.append('fullname', Employee.fullName);
    formData.append('phoneNumber', Employee.phoneNumber.toString());
    formData.append('gender', Employee.gender.toString());
    formData.append('birthday', Employee.birthday.toString());
    formData.append('address', Employee.address);
    formData.append('userId', Employee.userId.toString());
    if (files) {
      formData.append('file', files);
    }
    return this.http.put(this.apiProductAdminUrl, formData);
    // return this.apiService.callApi<EmployeeModel>(
    //   '/Employee', // URL của API
    //   'put', // Method là 'put'
    //   null, // Không có params
    //   formData, // Dữ liệu form data
    //   'multipart/form-data' // ContentType là 'multipart/form-data'
    // );
  }
}
