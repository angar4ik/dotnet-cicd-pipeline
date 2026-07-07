import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  template: `
    <header>
      <h1>🔄 Release Pipeline</h1>
    </header>
    <router-outlet />
  `,
})
export class AppComponent {}
