import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { WishlistItemDto, WishlistService } from '../../core/services/wishlist.service';
import { CartService } from '../../core/services/cart.service';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-wishlist',
  standalone: true,
  imports: [CommonModule, RouterLink],
  template: `
    <div class="wishlist">
      <div class="header">
        <h2>My Wishlist</h2>
        <a routerLink="/store" class="link">Continue shopping</a>
      </div>
      <div *ngIf="items?.length; else empty" class="grid">
        <div class="card" *ngFor="let it of items">
          <div class="thumb"><img [src]="imageUrl(it.imageUrl)" [alt]="it.productName" loading="lazy" /></div>
          <div class="info">
            <div class="name" [title]="it.productName">{{it.productName}}</div>
            <div class="price">{{it.price | currency:'EGP'}}</div>
            <div class="actions">
              <button class="btn" (click)="addToCart(it.productId)">Add to cart</button>
              <button class="btn outline" (click)="remove(it.productId)">Remove</button>
            </div>
          </div>
        </div>
      </div>
      <ng-template #empty>
        <div class="empty card">
          <p>Your wishlist is empty.</p>
          <a routerLink="/store" class="btn">Browse products</a>
        </div>
      </ng-template>
    </div>
  `,
  styles: [`
    .header{display:flex;justify-content:space-between;align-items:center;margin-bottom:16px}
    .grid{display:grid;grid-template-columns:repeat(auto-fill,minmax(240px,1fr));gap:18px}
    .card{display:flex;gap:12px;border:1px solid var(--border);border-radius:14px;overflow:hidden;background:var(--panel)}
    .thumb{width:120px;height:140px;flex:0 0 120px;background:#0e0e18;display:grid;place-items:center}
    .thumb img{width:100%;height:100%;object-fit:cover}
    .info{padding:12px;display:flex;flex-direction:column;gap:8px;min-width:0}
    .name{font-weight:700;white-space:nowrap;overflow:hidden;text-overflow:ellipsis}
    .actions{display:flex;gap:8px;flex-wrap:wrap}
    .btn{background:var(--primary);color:#fff;border:none;border-radius:10px;padding:8px 12px;text-decoration:none}
    .btn.outline{background:transparent;border:1px solid var(--border);color:var(--text)}
    .empty{text-align:center;padding:20px;border:1px solid var(--border);border-radius:12px}
  `]
})
export class WishlistComponent implements OnInit {
  private api = inject(WishlistService);
  private cart = inject(CartService);
  items: WishlistItemDto[] = [];
  apiBase = environment.apiBaseUrl.replace('/api','');
  ngOnInit(){ this.load(); }
  load(){ this.api.get().subscribe(items => this.items = items); }
  remove(productId: number){ this.api.remove(productId).subscribe(() => this.load()); }
  addToCart(productId: number){
    const token = localStorage.getItem('token');
    if (!token) return;
    this.cart.add(productId, 1).subscribe(() => this.cart.refreshCount());
  }
  imageUrl(url?: string){ return url ? `${this.apiBase}${url}` : 'https://via.placeholder.com/120?text=%20'; }
}


