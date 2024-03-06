import { Injectable } from '@angular/core';
import { ApiServiceService } from './api.service.service';
import { Observable } from 'rxjs';
import { EmployeeModel } from '../models/employee.model';
import { ApiResponse } from '../models/api.response.model';
import { HttpParams } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class EmployeeService {
  constructor(private apiService: ApiServiceService) {}

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
      '/Employee/Create-Employee',
      'post',
      null,
      Employee
    );
  }

  update(supplier: EmployeeModel) {
    return this.apiService.callApi<EmployeeModel>(
      '/Employee/Update',
      'put',
      null,
      supplier
    );
  }

  delete(id: number) {
    return this.apiService.callApi<EmployeeModel[]>(
      '/Employee/' + id,
      'delete'
    );
  }
}
