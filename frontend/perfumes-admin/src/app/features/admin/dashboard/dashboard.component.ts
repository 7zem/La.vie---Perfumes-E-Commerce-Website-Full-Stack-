import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="dashboard">
      <h2>Admin Dashboard</h2>
      <p>Welcome to the admin panel.</p>
    </div>
  `,
  styles: [`
    .dashboard{ padding:16px }
  `]
})
export class DashboardComponent {}


