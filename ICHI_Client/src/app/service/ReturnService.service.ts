import { Injectable } from '@angular/core';
import { Environment } from '../environment/environment';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { TokenService } from './token.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ReturnProductService {
  baseUrl = Environment.apiBaseUrl;

  constructor(private http: HttpClient, private tokenService: TokenService) {}

  findAll(
    page: number,
    size: number,
    sortDir: string,
    sortBy: string,
    search: string,
    status: string
  ): Observable<any> {
    const params = new HttpParams()
      .set('search', search)
      .set('size', size.toString())
      .set('page', page.toString())
      .set('sort-direction', sortDir)
      .set('sort-by', sortBy)
      .set('status', status);

    return this.http.get(`${this.baseUrl}/ProductReturn/FindAllPaged`, {
      params,
    });
  }

  findById(id: number): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/${id}`);
  }

  create(returnProduct: any) {
    debugger;
    return this.http.post(
      `${this.baseUrl}/ProductReturn/Create`,
      returnProduct
    );
  }

  // updateReturnStatus(returnProduct: any): Observable<any> {
  //   return this.http.put<any>(this.baseUrl, returnProduct);
  // }

  // For customer
  findAllByCustomer(
    email: string,
    pageSize: number,
    pageNumber: number,
    sortDir: string,
    sortBy: string
  ) {
    let queryParams = new HttpParams();
    queryParams = queryParams.append('email', email);
    queryParams = queryParams.append('page-size', pageSize);
    queryParams = queryParams.append('page-number', pageNumber);
    queryParams = queryParams.append('sort-direction', sortDir);
    queryParams = queryParams.append('sort-by', sortBy);
    return this.http.get(`${this.baseUrl}/customer`, { params: queryParams });
  }

  getTotalsByUserLogin(): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/customer/totals`);
  }

  findByIdWithClient(id: number) {
    return this.http.get(`${this.baseUrl}/${id}`);
  }
}
