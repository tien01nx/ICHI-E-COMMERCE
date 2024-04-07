import { Environment } from '../environment/environment';
import { Injectable } from '@angular/core';
import { ApiServiceService } from './api.service.service';
import { CustomerModel } from '../models/customer.model';
import { ApiResponse } from '../models/api.response.model';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { InsertCustomerDTO } from '../dtos/insert.customer.dto';
import { InventoryReceiptDetailModel } from '../models/inventory.receipts.detail';
import { InsertInvertoryReceiptsDTO } from '../dtos/insert.inventory.receipts.dto';

@Injectable({
  providedIn: 'root',
})
export class InventorryReceiptsService {
  baseUrl = Environment.apiBaseUrl;
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
  ): Observable<ApiResponse<InventoryReceiptDetailModel>> {
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
    return this.apiService.callApi<InventoryReceiptDetailModel>(
      '/InventoryReceipt/FindAllPaged',
      'get',
      params
    );
  }

  findAll() {
    return this.http.get(this.baseUrl + '/InventoryReceipt/FindAll');
  }

  create(data: any) {
    return this.http.post(
      'https://localhost:7150/api/InventoryReceipt/Create',
      data
    );
  }

  update(data: any) {
    return this.http.post(
      'https://localhost:7150/api/InventoryReceipt/Update',
      data
    );
  }

  findById(id: number) {
    return this.http.get(this.baseUrl + '/InventoryReceipt/' + id);
  }
}
