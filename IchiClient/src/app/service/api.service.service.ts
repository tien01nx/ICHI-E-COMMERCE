import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';
import { ApiResponse } from '../models/api.response.model';
import { Environment } from '../environment/environment';

@Injectable({
  providedIn: 'root',
})
export class ApiServiceService {
  private Token =
    'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjE4NWZlNjJiLTFhNTUtNDkyNy1iNWYwLTkyNzE1NTg1ODQ3MCIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJBZG1pbiIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6WyJBZG1pbiIsIlVzZXIiXSwiZXhwIjoxNzA5MTM1NDUxLCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo3Mjg3LyIsImF1ZCI6Imh0dHBzOi8vbG9jYWxob3N0OjcyODcvIn0._vj7Lw-AzeoxygvXqE_qdRNKJwQRm_qYAeLy7ltBXJ0';
  constructor(private http: HttpClient) {}
  private static handleError(error: Response | any) {
    console.error('ApiService::handleError', error);
    return throwError(error);
  }

  callApi<T>(
    actionAPI: string,
    method: string,
    params?: any,
    bodyInput?: any
  ): Observable<ApiResponse<T>> {
    const headers = new HttpHeaders({
      Authorization: `Bearer ${this.Token}`,
      'Content-Type': 'application/json',
    });

    let options = { headers: headers, params: params };

    switch (method.toLowerCase()) {
      case 'get':
        return this.http
          .get<ApiResponse<T>>(`${Environment.apiBaseUrl}${actionAPI}`, options)
          .pipe(catchError(ApiServiceService.handleError));
      case 'post':
        return this.http
          .post<ApiResponse<T>>(
            `${Environment.apiBaseUrl}${actionAPI}`,
            bodyInput,
            options
          )
          .pipe(catchError(ApiServiceService.handleError));
      case 'put':
        return this.http
          .post<ApiResponse<T>>(
            `${Environment.apiBaseUrl}${actionAPI}`,
            bodyInput,
            options
          )
          .pipe(catchError(ApiServiceService.handleError));
      case 'delete':
        return this.http
          .delete<ApiResponse<T>>(
            `${Environment.apiBaseUrl}${actionAPI}`,
            options
          )
          .pipe(catchError(ApiServiceService.handleError));
      default:
        throw new Error(`Unsupported method: ${method}`);
    }
  }

  refreshToken(): Observable<boolean> {
    return new Observable<boolean>((observer) => {
      observer.next(true);
      observer.complete();
    });
  }
}
