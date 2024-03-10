import { UserDTO } from './../dtos/user.dto';
import { Injectable } from '@angular/core';
import { ApiServiceService } from './api.service.service';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Environment } from '../environment/environment';
import { ApiResponse } from '../models/api.response.model';
import { Observable } from 'rxjs';
import { UserModel } from '../models/user.model';
import { UpdateUserDTO } from '../dtos/update.user.dto';

@Injectable({
  providedIn: 'root',
})
export class UserService {
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
  ): Observable<ApiResponse<UserModel>> {
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
    return this.apiService.callApi<UserModel>(
      '/Auth/FindAllPaged',
      'get',
      params
    );
  }

  findAll() {
    return this.apiService.callApi<UserModel>('/Auth/FindAllPaged', 'get');
  }

  create(User: any) {
    return this.apiService.callApi<UserModel>(
      '/Auth/Create-User',
      'post',
      null,
      User
    );
  }

  update(User: UserModel) {
    return this.apiService.callApi<UserModel>(
      '/Auth/Update',
      'put',
      null,
      User
    );
  }

  delete(id: number, status: boolean) {
    return this.apiService.callApi<UserModel[]>(
      // '/Auth/' + id + '?status=' + status,
      '/Auth/' + id + '?status=' + status,
      'delete'
    );
  }
  UpdateImage(user: UpdateUserDTO) {
    return this.apiService.callApi<UpdateUserDTO>(
      '/Users/Update-User',
      'put',
      null,
      user
    );
  }
}
