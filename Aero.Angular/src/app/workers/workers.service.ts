import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Worker, WorkerLaunchRequest, WorkerLaunchResponse } from './workers.model';

@Injectable({
  providedIn: 'root'
})
export class WorkerService {

  constructor(private http: HttpClient) {}

  getWorkers(): Observable<Worker[]> {
    return this.http.get<Worker[]>(`${environment.workerApiUrl}/workers`);
  }

  launchWorker(request: WorkerLaunchRequest): Observable<WorkerLaunchResponse> {
    return this.http.post<WorkerLaunchResponse>(`${environment.workerApiUrl}/workers`, request);
  }
}
