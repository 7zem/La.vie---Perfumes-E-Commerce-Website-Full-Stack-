import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { CartItemDto, CartService } from '../../core/services/cart.service';
import { ProductService, ProductDto } from '../../core/services/product.service';
import { forkJoin } from 'rxjs';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CommonModule, RouterLink],
  template: `
    <div class="cart">
      <div class="cart-header">
        <h2>Your Cart</h2>
        <a routerLink="/store" class="link">Continue shopping</a>
      </div>

      <div *ngIf="items?.length; else empty" class="cart-grid">
        <section class="cart-items card">
          <div class="item" *ngFor="let it of items">
            <div class="thumb lg">
              <img [src]="imageUrl(it.imageUrl)" [alt]="it.name" loading="lazy" />
            </div>
            <div class="details">
              <div class="name" [title]="it.name">{{it.name}}</div>
              <div class="unit">Unit price</div>
              <div class="price">{{it.price | currency:'EGP'}}</div>
            </div>
            <div class="qty">
              <button class="qty-btn" (click)="dec(it)" aria-label="decrease">−</button>
              <span class="qty-val">{{it.quantity}}</span>
              <button class="qty-btn" (click)="inc(it)" aria-label="increase">+</button>
            </div>
            <div class="subtotal">
              <span>Subtotal</span>
              <strong>{{it.price*it.quantity | currency:'EGP'}}</strong>
            </div>
            <button class="remove" (click)="remove(it)">Remove</button>
          </div>
        </section>

        <aside class="summary card">
          <h3>Order Summary</h3>
          <div class="row"><span>Subtotal</span><span>{{ subtotal | currency:'EGP' }}</span></div>
          <div class="row"><span>Shipping</span><span>{{ shipping | currency:'EGP' }}</span></div>
          <div class="row total"><span>Total</span><span>{{ total | currency:'EGP' }}</span></div>
          <a class="btn block" routerLink="/store/checkout">Proceed to checkout</a>
        </aside>
      </div>

      <ng-template #empty>
        <div class="empty card">
          <p>Your cart is empty.</p>
          <a routerLink="/store" class="btn">Browse products</a>
        </div>
      </ng-template>
    </div>
  `,
  styles: [`
    .cart-header{display:flex;justify-content:space-between;align-items:center;margin-bottom:16px}
    .link{color:#8ab4ff;text-decoration:none}
    .cart-grid{display:grid;grid-template-columns:1fr 340px;gap:18px}
    .card{background:var(--panel);border:1px solid var(--border);border-radius:14px;padding:14px}
    .cart-items{display:grid;gap:10px}

    .item{display:grid;grid-template-columns:96px 1fr auto auto auto;gap:14px;align-items:center;padding:12px;border-radius:12px;border:1px solid var(--border);background:linear-gradient(180deg, rgba(255,255,255,.02), rgba(255,255,255,0))}
    .thumb{width:96px;height:96px;border-radius:10px;overflow:hidden;background:#0e0e18;display:grid;place-items:center}
    .thumb.lg{aspect-ratio:1/1}
    .thumb img{width:100%;height:100%;object-fit:cover;transition:transform .35s ease}
    .item:hover .thumb img{transform:scale(1.05)}

    .details{min-width:0}
    .name{font-weight:700;white-space:nowrap;overflow:hidden;text-overflow:ellipsis;margin-bottom:4px}
    .unit{opacity:.8;font-size:12px}
    .price{font-weight:800}

    .qty{display:flex;align-items:center;gap:10px}
    .qty-btn{width:30px;height:30px;border:1px solid var(--border);background:transparent;color:var(--text);border-radius:8px;cursor:pointer}
    .qty-val{min-width:18px;text-align:center}
    .subtotal{display:flex;flex-direction:column;align-items:flex-end;gap:4px}
    .remove{background:transparent;border:none;color:#ef4444;cursor:pointer}

    .summary{position:sticky;top:16px;height:fit-content}
    .summary .row{display:flex;justify-content:space-between;margin:8px 0}
    .summary .total{font-weight:900}
    .btn{background:var(--primary);color:#fff;border:none;border-radius:10px;padding:12px 14px;text-align:center;text-decoration:none;display:inline-block}
    .btn.block{width:100%}
    .empty{text-align:center}
    @media (max-width: 900px){.cart-grid{grid-template-columns:1fr}}
  `]
})
export class CartComponent implements OnInit {
  private cart = inject(CartService);
  private productsApi = inject(ProductService);
  items: CartItemDto[] = [];
  apiBase = environment.apiBaseUrl.replace('/api','');
  ngOnInit(){ this.load(); }
  load(){
    forkJoin([
      this.cart.getCart(),
      this.productsApi.getAllNoCacheBust()
    ]).subscribe(([cartItems, products]) => {
      const map = new Map<number, ProductDto>(products.map(p => [p.productId, p]));
      this.items = cartItems.map(it => {
        const p = map.get(it.productId);
        return {
          ...it,
          name: it.name || p?.name || '',
          price: (it.price && it.price > 0) ? it.price : (p?.price ?? 0),
          imageUrl: it.imageUrl || p?.imageUrl
        } as CartItemDto;
      });
    });
  }
  inc(it: CartItemDto){ this.cart.increase(it.cartId).subscribe(() => this.load()); }
  dec(it: CartItemDto){
    if (it.quantity <= 1) { this.remove(it); return; }
    this.cart.decrease(it.cartId).subscribe(() => this.load());
  }
  remove(it: CartItemDto){ this.cart.remove(it.cartId).subscribe(() => this.load()); }
  get subtotal(){ return this.items.reduce((s,i)=> s + i.price * i.quantity, 0); }
  get shipping(){ return this.items.length ? 50 : 0; }
  get total(){ return this.subtotal + this.shipping; }
  imageUrl(url?: string){
    if (!url) return 'https://via.placeholder.com/64?text=%20';
    return `${this.apiBase}${url}`;
  }
}


