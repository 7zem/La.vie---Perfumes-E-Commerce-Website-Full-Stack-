import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { OrderDetailsDto, OrderService, OrderStatus } from '../../../core/services/order.service';

@Component({
  selector: 'app-order-detail',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  template: `
    <div class="page-header">
      <a routerLink="/dashboard/orders" class="btn ghost">← Back</a>
      <h2 class="title">Order details</h2>
      <div></div>
    </div>

    <div *ngIf="loading" class="card">Loading order...</div>
    <div *ngIf="!loading && error" class="card alert">{{ error }}</div>

    <div class="grid grid-2" *ngIf="!loading && order as o">
      <div class="card">
        <h3>Order #{{ o.orderNumber || o.orderId }}</h3>
        <p>Date: {{ o.orderDate | date: 'short' }}</p>
        <p>Total: {{ o.totalAmount | currency: 'USD' }}</p>
        <p>Status: <span class="chip">{{ o.status }}</span></p>
        <p>Payment: <span class="chip">{{ formatPaymentMethod(o.paymentMethod) }}</span></p>

        <label>Change Status
          <select [(ngModel)]="status" class="control">
            <option value="Pending">Pending</option>
            <option value="Processing">Processing</option>
            <option value="Shipped">Shipped</option>
            <option value="Delivered">Delivered</option>
            <option value="Cancelled">Cancelled</option>
            <option value="Refunded">Refunded</option>
            <option value="Completed">Completed</option>
            <option value="Paid">Paid</option>
          </select>
        </label>
        <button class="btn primary" (click)="saveStatus()">Save Status</button>
      </div>

      <div class="card">
        <h3>Shipping</h3>
        <div *ngIf="o.shippingInfo as s; else noShip">
          <div class="grid grid-2">
            <p><strong>Name:</strong> {{ s.firstName }} {{ s.lastName }}</p>
            <p><strong>Phone:</strong> {{ s.phoneNumber }}</p>
            <p><strong>Email:</strong> {{ s.email }}</p>
            <p><strong>Method:</strong> {{ s.shippingMethod || 'Standard' }}</p>
            <p class="col-span"><strong>Address:</strong> {{ s.address }}, {{ s.city }} {{ s.postalCode }}, {{ s.state }}, {{ s.country }}</p>
          </div>
        </div>
        <ng-template #noShip>
          <p class="muted">No shipping info.</p>
        </ng-template>
      </div>

      <div class="card" style="grid-column:1/-1">
        <h3>Items</h3>
        <table class="table" *ngIf="o.items?.length; else noitems">
          <thead>
            <tr><th>Name</th><th>Qty</th><th>Price</th><th>Subtotal</th></tr>
          </thead>
          <tbody>
            <tr *ngFor="let it of o.items">
              <td>{{ it.productName || it.name }}</td>
              <td>{{ it.quantity }}</td>
              <td>{{ (it.price ?? it.unitPrice ?? 0) | currency: 'USD' }}</td>
              <td>{{ (it.quantity * (it.price ?? it.unitPrice ?? 0)) | currency: 'USD' }}</td>
            </tr>
          </tbody>
        </table>
        <ng-template #noitems><p>No items.</p></ng-template>
      </div>
    </div>
  `,
  styles: [`
    .control{padding:8px;border:1px solid var(--border);border-radius:10px;background:rgba(255,255,255,0.10);color:var(--text)}
  `]
})
export class OrderDetailComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private api = inject(OrderService);

  orderId!: number;
  order: OrderDetailsDto | null = null;
  status: OrderStatus = 'Pending';
  loading = true;
  error = '';

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.loading = true;
      this.error = '';
      this.orderId = +(params.get('id') || 0);
      if (!this.orderId) { this.error = 'Invalid order id.'; this.loading = false; return; }

      // Fetch combined OrderDetails + ShippingInfo
      this.api.getById(this.orderId).subscribe({
        next: (o) => {
          this.order = o;
          this.status = (o.status as OrderStatus) || 'Pending';
          this.loading = false;
        },
        error: (err) => {
          const status = err?.status;
          const msg = err?.error?.message;
          this.error = msg || (status === 404 ? 'Order not found.' : 'Order not found or could not be loaded.');
          this.loading = false;
        }
      });
    });
  }

  saveStatus() {
    if (!this.orderId) return;
    this.api.updateStatus(this.orderId, this.status).subscribe(() => {
      // Refresh details to reflect latest status
      this.ngOnInit();
    });
  }

  formatPaymentMethod(pm?: string): string {
    const v = (pm || '').trim().toLowerCase();
    if (!v) return 'N/A';
    if (v === 'cod' || v === 'cashondelivery' || v === 'cash on delivery' || v === 'cash_on_delivery' || v === 'cash') {
      return 'COD';
    }
    if (v === 'paymob' || v === 'online' || v === 'card') {
      return 'Paymob';
    }
    return pm as string;
  }
}


