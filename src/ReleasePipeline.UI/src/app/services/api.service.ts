import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface HealthStatus {
  status: string;
  timestamp: string;
}

export interface ReleaseInfo {
  service: string;
  version: string;
  informationalVersion: string;
  environment: string;
  buildTime: string;
  gitSha: string;
  deployedAt: string;
}

export interface Deployment {
  id: number;
  environment: string;
  status: string;
  deployedAt: string;
}

@Injectable({ providedIn: 'root' })
export class ApiService {
  constructor(private http: HttpClient) {}

  getHealth(): Observable<HealthStatus> {
    return this.http.get<HealthStatus>('/health');
  }

  getReleaseInfo(): Observable<ReleaseInfo> {
    return this.http.get<ReleaseInfo>('/api/release-info');
  }

  getDeployments(): Observable<Deployment[]> {
    return this.http.get<Deployment[]>('/api/deployments');
  }
}
