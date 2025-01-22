import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, map, of } from 'rxjs';
import { environment } from '../../environments/environment';


@Injectable({
  providedIn: 'root'
})
export class SystemStatusService {
  constructor(private http: HttpClient) {}

  checkWebStatus(): Observable<boolean> {
    return this.http.get(`${environment.apiUrl}/status`, {
      observe: 'response',
      responseType: 'text'
    }).pipe(
      map(response => response.status === 200),
      catchError(() => of(false))
    );  }

  checkWorkerStatus(): Observable<boolean> {
    return this.http.get(`${environment.workerApiUrl}/status`, {
      observe: 'response',
      responseType: 'text'
    }).pipe(
      map(response => response.status === 200),
      catchError(() => of(false))
    );
  }
}
