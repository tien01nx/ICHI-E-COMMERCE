import { Injectable } from '@angular/core';
import { TrademarkModel } from '../models/trademark.model';
import { ApiResponse } from '../models/api.response.model';
import { HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiServiceService } from './api.service.service';
import { InsertTrademarkDTO } from '../dtos/insert.trademark.dto';

@Injectable({
  providedIn: 'root',
})
export class TrademarkService {
  constructor(private apiService: ApiServiceService) {}

  // truyền đối tượng TrademarkModel
  // findAll(
  //   PageNumber: number,
  //   PageSize: number,
  //   SortDirection: string,
  //   SortBy: string,
  //   Search: string
  // ): Observable<ApiResponse<TrademarkModel[]>> {
  //   const params = {
  //     PageNumber: PageNumber,
  //     PageSize: PageSize,
  //     Search: Search,
  //     SortDirection: SortDirection,
  //     SortBy: SortBy,
  //   };
  //   return this.apiService.callApi<TrademarkModel[]>(
  //     '/Trademark/FindAllPaged',
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
  ): Observable<ApiResponse<TrademarkModel>> {
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
    return this.apiService.callApi<TrademarkModel>(
      '/Trademark/FindAllPaged',
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
  // ): Observable<ApiResponse<TrademarkModel>> {
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
  //     .callApi<TrademarkModel>('/Trademark/FindAllPaged', 'get', params)
  //     .pipe(
  //       tap((data) => console.log('Trademark Data:', data)) // Log ra dữ liệu nhận được
  //     );
  // }

  findAll() {
    return this.apiService.callApi<TrademarkModel>(
      '/Trademark/FindAllPaged',
      'get'
    );
  }

  create(Trademark: InsertTrademarkDTO) {
    debugger;
    return this.apiService.callApi<TrademarkModel>(
      '/Trademark/Create',
      'post',
      null,
      Trademark
    );
  }

  update(Trademark: TrademarkModel) {
    debugger;
    return this.apiService.callApi<TrademarkModel>(
      '/Trademark/Update',
      'put',
      null,
      Trademark
    );
  }

  delete(id: number) {
    return this.apiService.callApi<TrademarkModel[]>(
      '/Trademark/Delete?id' + id,
      'delete'
    );
  }
}
