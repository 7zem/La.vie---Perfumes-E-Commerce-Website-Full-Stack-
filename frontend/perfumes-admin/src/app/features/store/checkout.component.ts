import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { OrderService, ShippingInfoDto } from '../../core/services/order.service';
import { CartItemDto, CartService } from '../../core/services/cart.service';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-checkout',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  template: `
    <div class="checkout">
      <div class="checkout-header">
        <h2>Checkout</h2>
        <a class="link" routerLink="/store/cart">Back to cart</a>
      </div>
      <div class="checkout-grid">
        <section class="card">
          <h3>Shipping information</h3>
          <form [formGroup]="form" (ngSubmit)="submit()" class="form">
            <div class="grid-2">
              <label>First name<input formControlName="firstName" /></label>
              <label>Last name<input formControlName="lastName" /></label>
              <label>Phone<input formControlName="phone" /></label>
              <label>Email<input formControlName="email" /></label>
              <label>City<input formControlName="city" /></label>
              <label>Postal code<input formControlName="postalCode" /></label>
            </div>
            <label>Address<input formControlName="address" /></label>
            <label>Notes<textarea formControlName="notes"></textarea></label>
            <div class="pay-section">
              <h4>Select payment method</h4>
              <div class="pay-grid">
                <label class="pay-card" [class.active]="form.value.paymentMethod==='COD'">
                  <input type="radio" formControlName="paymentMethod" value="COD" />
                  <div class="icon">💵</div>
                  <div class="title">Cash on delivery</div>
                  <div class="desc">Pay when you receive your order.</div>
                </label>
                <label class="pay-card" [class.active]="form.value.paymentMethod==='Paymob'">
                  <input type="radio" formControlName="paymentMethod" value="Paymob" />
                  <div class="icon">💳</div>
                  <div class="title">Pay online (Paymob)</div>
                  <div class="desc">Secure online payment gateway.</div>
                </label>
              </div>
            </div>
            <button class="btn" type="submit">Place order</button>
          </form>
        </section>
        <aside class="summary card">
          <h3>Order Summary</h3>
          <div class="row"><span>Subtotal</span><span>{{ subtotal | currency:'EGP' }}</span></div>
          <div class="row"><span>Shipping</span><span>{{ shipping | currency:'EGP' }}</span></div>
          <div class="row total"><span>Total</span><span>{{ total | currency:'EGP' }}</span></div>
          <p class="muted">Payment method: Cash on delivery</p>
        </aside>
      </div>
    </div>
  `,
  styles: [`
    .checkout-header{display:flex;justify-content:space-between;align-items:center;margin-bottom:12px}
    .checkout-grid{display:grid;grid-template-columns:1fr 320px;gap:16px}
    .card{background:var(--panel);border:1px solid var(--border);border-radius:12px;padding:12px}
    .form{display:grid;gap:10px}
    label{display:grid;gap:6px}
    input,textarea{padding:10px;border:1px solid var(--border);background:var(--bg);color:var(--text);border-radius:8px}
    .summary{position:sticky;top:16px;height:fit-content}
    .summary .row{display:flex;justify-content:space-between;margin:6px 0}
    .summary .total{font-weight:800}
    .muted{opacity:.8}
    .btn{background:var(--primary);color:#fff;border:none;border-radius:10px;padding:10px 12px}
    .pay-section{display:grid;gap:10px}
    .pay-grid{display:grid;grid-template-columns:repeat(2,minmax(0,1fr));gap:10px}
    .pay-card{position:relative;display:grid;gap:6px;padding:12px;border:1px solid var(--border);border-radius:12px;background:var(--panel);cursor:pointer;user-select:none}
    .pay-card:hover{border-color:rgba(0,0,0,.15)}
    .pay-card.active{outline:2px solid var(--primary);border-color:transparent}
    .pay-card input{position:absolute;opacity:0;pointer-events:none}
    .pay-card .icon{font-size:20px}
    .pay-card .title{font-weight:700}
    .pay-card .desc{color:var(--muted);font-size:12px}
    @media (max-width: 900px){.checkout-grid{grid-template-columns:1fr}}
  `]
})
export class CheckoutComponent implements OnInit {
  private fb = inject(FormBuilder);
  private orders = inject(OrderService);
  private cart = inject(CartService);

  private getUserIdFromToken(): number | null {
    const token = localStorage.getItem('token');
    if (!token) return null;
    try {
      const payload = JSON.parse(atob(token.split('.')[1] || ''));
      const id =
        payload['UserId'] ||
        payload['userId'] ||
        payload['nameid'] ||
        payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
      return id ? Number(id) : null;
    } catch {
      return null;
    }
  }

  form = this.fb.group({
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    phone: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    city: [''],
    postalCode: [''],
    address: ['', Validators.required],
    notes: [''],
    paymentMethod: ['COD', Validators.required]
  });
  items: CartItemDto[] = [];

  ngOnInit(){
    this.cart.getCart().subscribe(items => { this.items = items; });
  }

  submit(){
    if (this.form.invalid) return;
    const v = this.form.getRawValue();
    const userId = this.getUserIdFromToken();
    const shipping: ShippingInfoDto = {
      firstName: v.firstName || '',
      lastName: v.lastName || '',
      phoneNumber: v.phone || '',
      email: v.email || '',
      city: v.city || '',
      postalCode: v.postalCode || '',
      address: v.address || '',
      state: '', country: '', shippingMethod: 'Standard'
    } as any;
    const body = {
      userId: userId,
      visitorId: userId ? null : localStorage.getItem('visitorId'),
      items: this.items.map(it => ({ productId: it.productId, quantity: it.quantity })),
      shippingInfo: shipping,
      paymentMethod: v.paymentMethod || 'COD',
      couponCode: null,
      notes: v.notes
    } as any;
    this.orders.create(body).subscribe({
      next: (res) => {
        const redirect = res?.redirect || res;
        const isPaymob = (v.paymentMethod || 'COD') === 'Paymob';
        if (isPaymob && typeof redirect === 'string' && redirect.startsWith('http')) {
          window.location.href = redirect;
          return;
        }
        alert('Order placed!');
        this.cart.clear(userId || undefined)
          .subscribe(() => this.cart.refreshCount(userId || undefined));
      },
      error: (err) => {
        const msg = err?.error?.message || err?.error?.Message || err?.message || 'Failed to place order.';
        alert(msg);
      }
    });
  }

  get subtotal(){ return this.items.reduce((s,i)=> s + i.price * i.quantity, 0); }
  get shipping(){ return this.items.length ? 50 : 0; }
  get total(){ return this.subtotal + this.shipping; }
}


