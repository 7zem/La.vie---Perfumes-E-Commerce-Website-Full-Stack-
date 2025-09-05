import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ProductService, ProductDto } from '../../core/services/product.service';
import { CartService } from '../../core/services/cart.service';
import { WishlistService } from '../../core/services/wishlist.service';
import { switchMap } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-shop',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="toolbar">
      <h2>Shop</h2>
      <div class="filters">
        <input type="text" placeholder="Search products" [(ngModel)]="q" (keyup.enter)="search()" />
        <button class="btn" (click)="search()">Search</button>
      </div>
    </div>
    <div class="grid" *ngIf="!loading && products?.length; else state">
      <div class="card" *ngFor="let p of products">
        <div class="thumb">
          <img [src]="apiBase + (p.imageUrl||'')" [alt]="p.name" loading="lazy" />
          <button class="wish" [class.active]="isWished(p.productId)" [title]="isWished(p.productId) ? 'Remove from wishlist' : 'Add to wishlist'" (click)="toggleWish(p, $event)">❤</button>
          <span class="brand-chip" *ngIf="p.brandName">{{ p.brandName }}</span>
          <span class="price-badge">{{ p.price | currency:'EGP' }}</span>
        </div>
        <div class="info">
          <h4 class="name" [title]="p.name">{{ p.name }}</h4>
          <p class="desc" *ngIf="p.description">{{ p.description }}</p>
          <div class="meta">
            <button class="btn add-btn" (click)="add(p)">Add to cart</button>
          </div>
        </div>
      </div>
    </div>
    <ng-template #state>
      <div *ngIf="loading" class="skeleton-grid">
        <div class="skeleton-card" *ngFor="let i of skel"></div>
      </div>
      <div *ngIf="!loading && (!products || products.length===0)"><p>No products found.</p></div>
    </ng-template>
  `,
  styles: [`
    .toolbar{display:flex;justify-content:space-between;align-items:center;margin-bottom:16px}
    .filters{display:flex;gap:8px;align-items:center}
    .filters input{padding:10px 12px;border:1px solid var(--border);background:var(--panel);color:var(--text);border-radius:10px;min-width:240px}

    .grid{display:grid;grid-template-columns:repeat(auto-fill,minmax(240px,1fr));gap:18px}

    .card{background:linear-gradient(180deg, rgba(255,255,255,.02), rgba(255,255,255,.00));
          border:1px solid var(--border);border-radius:16px;overflow:hidden;transition:transform .22s ease, box-shadow .22s ease, border-color .22s ease}
    .card:hover{transform:translateY(-4px);box-shadow:0 10px 30px rgba(0,0,0,.35);border-color:rgba(255,255,255,.14)}

    .thumb{position:relative;background:#0e0e18;aspect-ratio:4/5;overflow:hidden}
    .thumb img{width:100%;height:100%;object-fit:cover;transition:transform .4s ease}
    .card:hover .thumb img{transform:scale(1.06)}
    .wish{position:absolute;right:10px;top:10px;background:rgba(255,255,255,.85);border:none;border-radius:999px;padding:6px 8px;cursor:pointer;transition:transform .2s ease, background .2s ease}
    .wish:hover{transform:scale(1.05)}
    .wish.active{background:#ef4444;color:#fff}

    .brand-chip{position:absolute;left:10px;top:10px;font-size:12px;padding:4px 8px;border-radius:999px;border:1px solid var(--border);background:rgba(255,255,255,.06)}
    .price-badge{position:absolute;right:10px;bottom:10px;font-weight:700;padding:6px 10px;border-radius:10px;background:var(--primary);color:#fff}

    .info{padding:12px}
    .name{margin:0 0 6px 0;font-size:15px;line-height:1.3;display:-webkit-box;-webkit-line-clamp:2;-webkit-box-orient:vertical;overflow:hidden}
    .desc{margin:0 0 10px 0;color:var(--muted);font-size:13px;line-height:1.4;display:-webkit-box;-webkit-line-clamp:2;-webkit-box-orient:vertical;overflow:hidden}
    .meta{display:flex;align-items:center;gap:10px}
    .add-btn{width:100%}

    .skeleton-grid{display:grid;grid-template-columns:repeat(auto-fill,minmax(240px,1fr));gap:18px}
    .skeleton-card{height:280px;border-radius:16px;border:1px solid var(--border);background:linear-gradient(90deg, rgba(255,255,255,.04), rgba(255,255,255,.08), rgba(255,255,255,.04));background-size:200% 100%;animation: shimmer 1.2s infinite}
    @keyframes shimmer{0%{background-position:-200px 0}100%{background-position:200px 0}}
  `]
})
export class ShopComponent implements OnInit {
  private productsApi = inject(ProductService);
  private cart = inject(CartService);
  private wish = inject(WishlistService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  all: ProductDto[] = [];
  products: ProductDto[] = [];
  apiBase = environment.apiBaseUrl.replace('/api','');
  loading = false;
  q = '';
  skel = Array.from({length:6});
  private wished = new Set<number>();

  ngOnInit(){
    this.route.queryParamMap.subscribe(params => {
      this.q = (params.get('q') || '').trim();
      if (this.all.length) this.applyFilter(this.q);
      else this.load();
    });
  }

  load(){
    this.loading = true;
    this.productsApi.getAll().subscribe(res => {
      this.all = res || [];
      this.applyFilter(this.q);
      this.loading = false;
      const token = localStorage.getItem('token');
      if (token) {
        this.wish.get().subscribe(items => {
          this.wished = new Set((items||[]).map(i => i.productId));
        });
      } else {
        this.wished.clear();
      }
    }, _ => this.loading = false);
  }

  search(){
    const term = this.q.trim();
    this.router.navigate(['/store'], { queryParams: term ? { q: term } : {} });
  }

  add(p: ProductDto){
    const token = localStorage.getItem('token');
    if (!token) { this.router.navigate(['/login'], { queryParams: { returnUrl: '/store' } }); return; }
    this.cart.add(p.productId, 1).subscribe({
      next: () => this.cart.refreshCount(),
      error: () => this.router.navigate(['/login'], { queryParams: { returnUrl: '/store' } })
    });
  }

  toggleWish(p: ProductDto, ev: MouseEvent){
    ev.stopPropagation();
    const token = localStorage.getItem('token');
    if (!token) { this.router.navigate(['/login'], { queryParams: { returnUrl: '/store' } }); return; }
    this.wish.exists(p.productId)
      .pipe(switchMap(r => r?.exists ? this.wish.remove(p.productId) : this.wish.add(p.productId)))
      .subscribe(() => { if (this.wished.has(p.productId)) this.wished.delete(p.productId); else this.wished.add(p.productId); this.wish.refreshCount(); });
  }

  isWished(productId: number){ return this.wished.has(productId); }

  private applyFilter(term: string){
    const t = (term || '').toLowerCase();
    if (!t) { this.products = [...this.all]; return; }
    this.products = this.all.filter(p => (p.name||'').toLowerCase().includes(t) || (p.brandName||'').toLowerCase().includes(t));
  }
}


