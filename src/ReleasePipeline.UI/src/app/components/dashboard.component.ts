import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService, Deployment, HealthStatus, ReleaseInfo } from '../services/api.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  template: `
    <main class="container">

      <div *ngIf="error" class="error-banner">⚠ {{ error }}</div>

      <!-- Health -->
      <section class="card">
        <h2>Health</h2>
        <ng-container *ngIf="health; else loadingHealth">
          <span [class.badge-success]="health.status === 'healthy'"
                [class.badge-danger]="health.status !== 'healthy'"
                class="badge">{{ health.status }}</span>
          <span style="margin-left: 0.5rem; font-size: 0.8rem; color: var(--muted);">
            {{ health.timestamp | date:'medium' }}
          </span>
        </ng-container>
        <ng-template #loadingHealth><span class="loading">Checking...</span></ng-template>
      </section>

      <!-- Release Info -->
      <section class="card">
        <h2>Release Info</h2>
        <ng-container *ngIf="releaseInfo; else loadingRelease">
          <div class="info-grid">
            <div class="info-item">
              <span class="info-label">Service</span>
              <span class="info-value">{{ releaseInfo.service }}</span>
            </div>
            <div class="info-item">
              <span class="info-label">Version</span>
              <span class="info-value">{{ releaseInfo.informationalVersion }}</span>
            </div>
            <div class="info-item">
              <span class="info-label">Environment</span>
              <span class="info-value">{{ releaseInfo.environment }}</span>
            </div>
            <div class="info-item">
              <span class="info-label">Git SHA</span>
              <span class="info-value">{{ releaseInfo.gitSha }}</span>
            </div>
            <div class="info-item">
              <span class="info-label">Build Time</span>
              <span class="info-value">{{ releaseInfo.buildTime | date:'medium' }}</span>
            </div>
            <div class="info-item">
              <span class="info-label">Deployed At</span>
              <span class="info-value">{{ releaseInfo.deployedAt || '—' }}</span>
            </div>
          </div>
        </ng-container>
        <ng-template #loadingRelease><span class="loading">Loading...</span></ng-template>
      </section>

      <!-- Deployments -->
      <section class="card">
        <h2>Deployments</h2>
        <ng-container *ngIf="deployments; else loadingDeployments">
          <table *ngIf="deployments.length; else noDeployments">
            <thead>
              <tr>
                <th>ID</th>
                <th>Environment</th>
                <th>Status</th>
                <th>Deployed At</th>
              </tr>
            </thead>
            <tbody>
              <tr *ngFor="let d of deployments">
                <td>{{ d.id }}</td>
                <td>{{ d.environment }}</td>
                <td>
                  <span [class.badge-success]="d.status === 'success'"
                        [class.badge-danger]="d.status !== 'success'"
                        class="badge">{{ d.status }}</span>
                </td>
                <td>{{ d.deployedAt | date:'medium' }}</td>
              </tr>
            </tbody>
          </table>
          <ng-template #noDeployments>
            <span class="loading">No deployments recorded yet.</span>
          </ng-template>
        </ng-container>
        <ng-template #loadingDeployments><span class="loading">Loading...</span></ng-template>
      </section>

    </main>
  `,
})
export class DashboardComponent implements OnInit {
  health?: HealthStatus;
  releaseInfo?: ReleaseInfo;
  deployments?: Deployment[];
  error?: string;

  constructor(private api: ApiService) {}

  ngOnInit(): void {
    this.api.getHealth().subscribe({
      next: (h) => (this.health = h),
      error: (e) => (this.error = `Health check failed: ${e.message}`),
    });

    this.api.getReleaseInfo().subscribe({
      next: (r) => (this.releaseInfo = r),
      error: (e) => {
        if (!this.error) this.error = `Failed to load release info: ${e.message}`;
      },
    });

    this.api.getDeployments().subscribe({
      next: (d) => (this.deployments = d),
      error: (e) => {
        if (!this.error) this.error = `Failed to load deployments: ${e.message}`;
      },
    });
  }
}
