import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink, RouterOutlet } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CartService } from '../../core/services/cart.service';
import { AuthService } from '../../core/services/auth.service';
import { WishlistService } from '../../core/services/wishlist.service';

@Component({
  selector: 'app-store-layout',
  standalone: true,
  imports: [CommonModule, RouterLink, RouterOutlet, FormsModule],
  template: `
    <div class="store-theme">
      <div class="promo">FREE shipping on orders above 2500 EGP</div>
      <header class="mz-header">
        <div class="container head">
          <a routerLink="/store" class="logo">
            <img src="/logo.jpg" alt="La.vie" />
            <span>La.vie</span>
          </a>
          <div class="search">
            <input [(ngModel)]="q" type="text" placeholder="Search products ..." (keyup.enter)="goSearch()" />
          </div>
          <nav class="icons">
          <a *ngIf="!isLoggedIn" routerLink="/login" class="icon" title="Account">👤</a>
          <button *ngIf="isLoggedIn" class="icon btn-link" title="Logout" (click)="logout()">🚪</button>
          <a routerLink="/store/wishlist" class="icon" title="Wishlist">❤ <span class="badge" *ngIf="(wish.count$|async) as w">{{w}}</span></a>
          <a routerLink="/store/cart" class="icon cart" title="Cart">🛒 <span class="badge" *ngIf="(cart.count$|async) as c">{{c}}</span></a>
          </nav>
        </div>
        <div class="container navcats">
          <a routerLink="/store" class="cat active">Perfumes</a>
        </div>
      </header>
      <main class="container content"><router-outlet /></main>
      <footer class="mz-footer">
        <div class="container">
          <p>© {{year}} La.vie. All rights reserved.</p>
        </div>
      </footer>
    </div>
  `,
  styles: [`
    .store-theme{--bg:#ffffff;--panel:#ffffff;--border:#e5e7eb;--muted:#6b7280;--text:#111827;--primary:#0ea5a2;background:var(--bg);color:var(--text)}
    .container{max-width:1200px;margin:0 auto;padding:0 16px}
    .promo{background:#0ea5a2;color:#fff;text-align:center;font-size:13px;padding:6px 0}
    .mz-header{position:sticky;top:0;z-index:10;background:#ffffff;border-bottom:1px solid #e5e7eb}
    .head{display:flex;align-items:center;gap:16px;justify-content:space-between;padding:12px 0}
    .logo{display:flex;align-items:center;gap:10px;text-decoration:none}
    .logo img{height:44px;display:block}
    .logo span{font-weight:800;font-size:24px;color:#0ea5a2}
    .search input{width:480px;max-width:50vw;padding:10px 12px;border:1px solid #e5e7eb;border-radius:999px;background:#f9fafb}
    .icons{display:flex;gap:14px;align-items:center}
     .icon{color:#111827;text-decoration:none;font-size:18px}
     .btn-link{background:transparent;border:none;cursor:pointer}
    .icon.cart{position:relative}
    .badge{margin-left:6px;background:#0ea5a2;color:#fff;border-radius:999px;padding:2px 8px;font-size:12px}
    .navcats{display:flex;gap:18px;align-items:center;padding:8px 0}
    .cat{color:#111827;text-decoration:none;padding:8px 4px;border-bottom:2px solid transparent}
    .cat.active{border-color:#0ea5a2}
    .content{padding:20px 0}
    .mz-footer{border-top:1px solid #e5e7eb;background:#ffffff}
    .mz-footer p{margin:0;padding:14px 0;color:#6b7280;text-align:center}
  `]
})
export class StoreLayoutComponent {
  cart = inject(CartService);
  wish = inject(WishlistService);
  private auth = inject(AuthService);
  year = new Date().getFullYear();
  q = '';
  private router = inject(Router);
  goSearch(){
    const term = (this.q || '').trim();
    this.router.navigate(['/store'], { queryParams: term ? { q: term } : {} });
  }
  get isLoggedIn(){ return !!localStorage.getItem('token'); }
  logout(){ this.auth.logout(); }
}


