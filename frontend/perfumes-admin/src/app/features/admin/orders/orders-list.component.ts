import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { OrderService, OrderSummaryDto, OrderStatus, OrderDetailsDto } from '../../../core/services/order.service';
import { forkJoin, of } from 'rxjs';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-orders-list',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  template: `
    <div class="page-header">
      <h2 class="title">Orders</h2>
      <div class="filters">
        <input class="control" type="text" placeholder="Search by order number" [(ngModel)]="query" (keyup.enter)="searchByNumber()" />
        <select class="control" [(ngModel)]="status" (change)="load()">
          <option value="All">All</option>
          <option value="Pending">Pending</option>
          <option value="Processing">Processing</option>
          <option value="Shipped">Shipped</option>
          <option value="Delivered">Delivered</option>
          <option value="Cancelled">Cancelled</option>
          <option value="Refunded">Refunded</option>
          <option value="Completed">Completed</option>
          <option value="Paid">Paid</option>
        </select>
        <input class="control" type="date" [(ngModel)]="start" />
        <input class="control" type="date" [(ngModel)]="end" />
        <button class="btn primary" (click)="filterByDate()">Apply</button>
      </div>
    </div>

    <div class="grid grid-3" *ngIf="!loading && orders?.length; else state">
      <div class="card" *ngFor="let o of orders">
        <div class="row"><span class="chip">#{{o.orderNumber || o.orderId}}</span><span class="muted">{{o.orderDate | date:'short'}}</span></div>
        <div class="row"><strong>{{o.totalAmount | currency:'USD'}}</strong><span class="chip" [class]="o.status.toLowerCase()">{{o.status}}</span></div>
        <a class="btn sm" [routerLink]="['/dashboard/orders', o.orderId]">View Details</a>
      </div>
    </div>

    <ng-template #state>
      <div *ngIf="loading" class="skeleton-grid">
        <div class="skeleton-card" *ngFor="let i of skel"> </div>
      </div>
      <div *ngIf="!loading && (!orders || orders.length===0)" class="empty">
        <p>No orders found for the current filters.</p>
      </div>
    </ng-template>

    <div class="card" *ngIf="loadingDetail">Loading order details...</div>
    <div class="card alert" *ngIf="!loadingDetail && errorDetail">{{errorDetail}}</div>

    <div class="card" *ngIf="!loadingDetail && selected as o">
      <div class="row" style="justify-content:space-between;align-items:center">
        <h3 style="margin:0">Order #{{o.orderNumber || o.orderId}}</h3>
        <button class="btn ghost" (click)="closeDetail()">Close</button>
      </div>
      <p>Date: {{o.orderDate | date:'short'}} | Status: <span class="chip">{{o.status}}</span> | Total: {{o.totalAmount | currency:'USD'}}</p>
      <h4>Items</h4>
      <table class="table" *ngIf="o.items?.length; else noItems">
        <thead><tr><th>Name</th><th>Qty</th><th>Price</th><th>Subtotal</th></tr></thead>
        <tbody>
          <tr *ngFor="let it of o.items">
            <td>{{it.productName || it.name}}</td>
            <td>{{it.quantity}}</td>
            <td>{{(it.price ?? it.unitPrice ?? 0) | currency:'USD'}}</td>
            <td>{{(it.quantity * (it.price ?? it.unitPrice ?? 0)) | currency:'USD'}}</td>
          </tr>
        </tbody>
      </table>
      <ng-template #noItems><p>No items.</p></ng-template>
    </div>
  `,
  styles: [`
    .filters{display:flex;gap:8px;align-items:center}
    .filters .control{min-width:160px}
    .row{display:flex;justify-content:space-between;align-items:center;margin:6px 0}
    .chip.pending{background:rgba(250,204,21,.10);border-color:rgba(250,204,21,.35);color:#fde68a}
    .chip.processing{background:rgba(91,124,250,.12);border-color:rgba(91,124,250,.35);color:#c7d2fe}
    .chip.shipped{background:rgba(16,185,129,.12);border-color:rgba(16,185,129,.35);color:#bbf7d0}
    .chip.delivered{background:rgba(34,197,94,.12);border-color:rgba(34,197,94,.35);color:#bbf7d0}
    .chip.cancelled{background:rgba(239,68,68,.12);border-color:rgba(239,68,68,.35);color:#fecaca}
    .chip.refunded{background:rgba(14,165,233,.12);border-color:rgba(14,165,233,.35);color:#bae6fd}
    .chip.completed{background:rgba(52,211,153,.12);border-color:rgba(52,211,153,.35);color:#bbf7d0}
    .chip.paid{background:rgba(99,102,241,.12);border-color:rgba(99,102,241,.35);color:#c7d2fe}
    .skeleton-grid{display:grid;grid-template-columns:repeat(3,minmax(0,1fr));gap:12px}
    .skeleton-card{height:120px;border-radius:12px;background:linear-gradient(90deg, rgba(255,255,255,.04), rgba(255,255,255,.08), rgba(255,255,255,.04));animation: shimmer 1.2s infinite;}
    @keyframes shimmer{0%{background-position:-200px 0}100%{background-position:200px 0}}
    .empty{opacity:.8;text-align:center;margin-top:24px}
  `]
})
export class OrdersListComponent implements OnInit {
  private api = inject(OrderService);
  orders: OrderSummaryDto[] = [];
  status: OrderStatus | 'All' = 'Pending';
  query = '';
  start = '';
  end = '';
  loading = false;
  skel = Array.from({length:6});

  selected: OrderDetailsDto | null = null;
  loadingDetail = false;
  errorDetail = '';

  ngOnInit(){ this.load(); }
  load(){
    this.loading = true;
    if (this.status === 'All') {
      this.loadAllStatuses();
      return;
    }
    this.api.getByStatus(this.status as OrderStatus).subscribe(res => { this.orders = res; this.loading = false; }, _ => this.loading = false);
  }

  private loadAllStatuses(){
    const statuses: OrderStatus[] = ['Pending','Processing','Shipped','Delivered','Cancelled','Refunded','Completed','Paid'];
    forkJoin(statuses.map(s => this.api.getByStatus(s))).pipe(
      map(results => {
        const merged = results.flat();
        const byId = new Map<number, OrderSummaryDto>();
        for (const o of merged) byId.set(o.orderId, o);
        return Array.from(byId.values()).sort((a,b) => new Date(b.orderDate).getTime() - new Date(a.orderDate).getTime());
      })
    ).subscribe({
      next: res => { this.orders = res; this.loading = false; },
      error: _ => { this.orders = []; this.loading = false; }
    });
  }

  searchByNumber(){
    const q = this.query.trim();
    if (!q) { this.load(); return; }
    this.loading = true;
    this.api.getByNumber(q).subscribe(o => {
      this.orders = o ? [{
        orderId: o.orderId,
        orderNumber: o.orderNumber,
        orderDate: o.orderDate,
        totalAmount: o.totalAmount,
        status: o.status as OrderStatus,
        paymentMethod: o.paymentMethod
      }] : [];
      this.loading = false;
    }, _ => { this.orders = []; this.loading = false; });
  }

  filterByDate(){
    if (!this.start || !this.end) { this.load(); return; }
    this.loading = true;
    this.api.getByDateRange(this.start, this.end).subscribe(res => { this.orders = res; this.loading = false; }, _ => this.loading = false);
  }

  view(orderId: number){
    this.loadingDetail = true;
    this.errorDetail = '';
    this.api.getById(orderId).subscribe({
      next: (o) => { this.selected = o; this.loadingDetail = false; },
      error: (err) => { this.errorDetail = err?.error?.message || 'Could not load order details.'; this.loadingDetail = false; }
    });
  }

  closeDetail(){ this.selected = null; }
}


