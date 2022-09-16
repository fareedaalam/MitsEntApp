import { HttpClient, HttpParams } from "@angular/common/http";
import { map } from "rxjs";
import { PaginatedResult } from "../_models/pagination";

export function getPaginatedResult<T>(url, params,http: HttpClient) {
    const paginationResult: PaginatedResult<T> = new PaginatedResult<T>();
    return http.get<T>(url, { observe: 'response', params }).pipe(
      map(response => {
        paginationResult.result = response.body;
        if (response.headers.get('Pagination') !== null) {
          paginationResult.pagination = JSON.parse(response.headers.get('Pagination'));
        }
        return paginationResult;
      })
    );
  }

  export function getPaginationHeaders(pangeNumber: number, pageSize: number) {
    let params = new HttpParams();
    params = params.append('pageNumber', pangeNumber.toString());
    params = params.append('pageSize', pageSize.toString());
    return params;
  }

