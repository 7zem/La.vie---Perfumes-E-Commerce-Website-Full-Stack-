import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterOutlet } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-admin-layout',
  standalone: true,
  imports: [CommonModule, RouterLink, RouterOutlet],
  template: `
    <div class="layout admin-theme">
      <aside class="sidebar">
        <div class="logo">Perfumes Admin</div>
        <nav class="menu">
          <a routerLink="/dashboard" routerLinkActive="active">Dashboard</a>
          <a routerLink="/dashboard/products" routerLinkActive="active">Products</a>
          <a routerLink="/dashboard/brands" routerLinkActive="active">Brands</a>
          <a routerLink="/dashboard/categories" routerLinkActive="active">Categories</a>
          <a routerLink="/dashboard/users" routerLinkActive="active">Users</a>
          <a routerLink="/dashboard/orders" routerLinkActive="active">Orders</a>
        </nav>
        <button class="logout" (click)="logout()">Logout</button>
      </aside>
      <div class="main">
        <header class="topbar">
          <div class="title">Control Panel</div>
        </header>
        <main class="content">
          <router-outlet />
        </main>
      </div>
    </div>
  `,
  styles: [`
    /* Admin-wide theme variables using the provided palette */
    .admin-theme{
      --primary: #3B38A0;
      --primary-2: #7A85C1;
      --primary-grad: linear-gradient(135deg, #1A2A80, #3B38A0 40%, #7A85C1 75%, #B2B0E8 100%);
      --border: rgba(255,255,255,0.14);
    }

    .layout{display:grid;grid-template-columns:260px 1fr;min-height:100vh;
      background:
        radial-gradient(1000px 400px at 0% -10%, rgba(26,42,128,0.25), transparent 50%),
        radial-gradient(1000px 400px at 100% 0%, rgba(123,133,193,0.22), transparent 50%),
        linear-gradient(160deg, rgba(26,42,128,0.20), rgba(58,56,160,0.15) 40%, rgba(122,133,193,0.12) 70%, rgba(178,176,232,0.10));
    }
    .sidebar{
      background: rgba(10,10,18,0.65);
      border-right:1px solid var(--border);
      padding:16px;display:flex;flex-direction:column;gap:12px;
      -webkit-backdrop-filter: blur(12px) saturate(140%);
      backdrop-filter: blur(12px) saturate(140%);
    }
    .logo{font-weight:800; letter-spacing:-0.01em; color: var(--text)}
    .menu{display:flex;flex-direction:column;gap:6px}
    .menu a{color:var(--muted);text-decoration:none;padding:8px 10px;border-radius:10px}
    .menu a.active,.menu a:hover{background:rgba(255,255,255,.08);color:var(--text)}
    .logout{margin-top:auto;background:transparent;border:1px solid var(--border);color:var(--text);border-radius:10px;padding:8px 10px;cursor:pointer}
    .topbar{
      display:flex;align-items:center;justify-content:space-between;padding:12px 16px;border-bottom:1px solid var(--border);
      background: rgba(255,255,255,0.04);
      -webkit-backdrop-filter: blur(10px);
      backdrop-filter: blur(10px);
    }
    .topbar .title{font-weight:800; letter-spacing:-0.01em; background: var(--primary-grad); -webkit-background-clip:text; background-clip:text; color: transparent}
    .content{padding:16px}
    @media (max-width: 900px){.layout{grid-template-columns:1fr}.sidebar{position:fixed;inset:0 60% 0 0;z-index:20;transform:translateX(-100%)} }
  `]
})
export class AdminLayoutComponent {
  private auth = inject(AuthService);
  logout(){ this.auth.logout(); }
}


