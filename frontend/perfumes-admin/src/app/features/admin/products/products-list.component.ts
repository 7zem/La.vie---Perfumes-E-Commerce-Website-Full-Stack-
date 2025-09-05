import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { ProductService, ProductDto } from '../../../core/services/product.service';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-products-list',
  standalone: true,
  imports: [CommonModule, RouterLink],
  template: `
    <div class="header">
      <h3>Products</h3>
      <a routerLink="/dashboard/products/new" class="btn">Add Product</a>
    </div>

    <div class="grid" *ngIf="products?.length; else empty">
      <div class="card" *ngFor="let p of products">
        <div class="thumb" [class.noimg]="!p.imageUrl">
          <img *ngIf="p.imageUrl" [src]="apiBase + p.imageUrl" [alt]="p.name" />
          <div class="badge" title="In stock">{{ p.stock }}</div>
        </div>
        <div class="info">
          <h4 class="name" title="{{ p.name }}">{{ p.name }}</h4>
          <div class="chips">
            <span class="chip" *ngIf="p.brandName">{{ p.brandName }}</span>
            <span class="chip alt" *ngIf="p.categoryName">{{ p.categoryName }}</span>
          </div>
          <div class="meta">
            <span class="price">{{ p.price | currency:'EGP':'symbol':'1.2-2' }}</span>
            <span class="stock">Stock: {{ p.stock }}</span>
            <div class="cta">
              <a class="btn-outline sm" [routerLink]="['/dashboard/products', p.productId]">Edit</a>
              <button class="btn-outline sm danger" (click)="remove(p)">Delete</button>
            </div>
          </div>
        </div>
      </div>
    </div>

    <ng-template #empty>
      <p>No products found.</p>
    </ng-template>
  `,
  styles: [`
    .header{display:flex;justify-content:space-between;align-items:center;margin-bottom:16px}
    .btn{background:var(--primary);color:#fff;padding:8px 12px;border-radius:10px;text-decoration:none}
    .grid{display:grid;grid-template-columns:repeat(auto-fill,minmax(260px,1fr));gap:18px}
    .card{background:var(--panel);border:1px solid var(--border);border-radius:14px;overflow:hidden;display:flex;flex-direction:column;transition:transform .2s ease, box-shadow .2s ease;border-inline:1px solid var(--border)}
    .card:hover{transform:translateY(-4px);box-shadow:0 10px 30px rgba(0,0,0,.35)}
    .thumb{position:relative;background:#0e0e18;aspect-ratio:4/3}
    .thumb.noimg{display:grid;place-items:center;color:#94a3b8}
    .thumb img{width:100%;height:100%;object-fit:cover;display:block}
    .badge{position:absolute;top:8px;right:8px;background:rgba(0,0,0,.55);backdrop-filter: blur(6px);color:#fff;border:1px solid var(--border);border-radius:999px;padding:2px 8px;font-size:12px}
    .info{padding:12px}
    .name{margin:0 0 8px;font-size:16px;line-height:1.3;white-space:nowrap;overflow:hidden;text-overflow:ellipsis}
    .chips{display:flex;gap:6px;margin:4px 0 10px;flex-wrap:wrap}
    .chip{border:1px solid var(--border);background:rgba(255,255,255,.04);color:var(--text);border-radius:9999px;padding:3px 10px;font-size:12px}
    .chip.alt{background:rgba(0,180,216,.08)}
    .meta{display:flex;justify-content:space-between;align-items:center;gap:10px;font-size:13px}
    .price{font-weight:700;color:#8ab4ff}
    .stock{color:var(--muted);white-space:nowrap}
    .cta{display:flex;gap:8px}
    .btn-outline.sm{padding:6px 10px;border-radius:8px}
    .btn-outline.sm.danger{border-color:#ef4444;color:#ef4444}
  `]
})
export class ProductsListComponent {
  private api = inject(ProductService);
  products: ProductDto[] = [];
  apiBase = environment.apiBaseUrl.replace('/api','');

  ngOnInit(){
    this.load();
  }

  load(){
    this.api.getAll().subscribe(res => {
      this.products = res;
    });
  }

  remove(p: ProductDto){
    if(!confirm(`Delete ${p.name}?`)) return;
    this.api.delete(p.productId).subscribe(() => this.load());
  }
}



