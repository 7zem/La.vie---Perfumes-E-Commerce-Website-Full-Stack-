import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

export type OrderStatus = 'Pending' | 'Processing' | 'Shipped' | 'Delivered' | 'Cancelled' | 'Refunded' | 'Completed' | 'Paid';

export interface OrderSummaryDto {
  orderId: number;
  orderNumber?: string;
  orderDate: string;
  totalAmount: number;
  status: OrderStatus;
  paymentMethod?: string;
}

export interface OrderItemDto {
  productId: number;
  // Support both backend naming (productName, price) and legacy (name, unitPrice)
  productName?: string;
  name?: string;
  quantity: number;
  price?: number;
  unitPrice?: number;
}

export interface ShippingInfoDto {
  firstName: string;
  lastName: string;
  address: string;
  city: string;
  state: string;
  postalCode: string;
  country: string;
  phoneNumber: string;
  email: string;
  shippingMethod?: string;
}

export interface OrderDetailsDto extends OrderSummaryDto {
  items?: OrderItemDto[];
  shippingInfo?: ShippingInfoDto;
  notes?: string;
}

@Injectable({ providedIn: 'root' })
export class OrderService {
  private http = inject(HttpClient);
  private apiUrl = environment.apiBaseUrl + '/Order';

  getByStatus(status: OrderStatus): Observable<OrderSummaryDto[]> {
    return this.http.get<OrderSummaryDto[]>(`${this.apiUrl}/status/${status}`);
  }

  getPending(): Observable<OrderSummaryDto[]> { return this.http.get<OrderSummaryDto[]>(`${this.apiUrl}/pending`); }
  getCompleted(): Observable<OrderSummaryDto[]> { return this.http.get<OrderSummaryDto[]>(`${this.apiUrl}/completed`); }

  getById(id: number): Observable<OrderDetailsDto> {
    return this.http.get<OrderDetailsDto>(`${this.apiUrl}/id/${id}`);
  }

  getByNumber(orderNumber: string): Observable<OrderDetailsDto> {
    return this.http.get<OrderDetailsDto>(`${this.apiUrl}/number/${encodeURIComponent(orderNumber)}`);
  }

  getByDateRange(start: string, end: string): Observable<OrderSummaryDto[]> {
    const params = new URLSearchParams({ start, end }).toString();
    return this.http.get<OrderSummaryDto[]>(`${this.apiUrl}/date-range?${params}`);
  }

  updateStatus(orderId: number, status: OrderStatus): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.put(`${this.apiUrl}/${orderId}/status`, JSON.stringify(status), { headers });
  }

  updateShipping(orderId: number, dto: ShippingInfoDto): Observable<any> {
    return this.http.put(`${this.apiUrl}/${orderId}/shipping`, dto);
  }

  create(body: any): Observable<any> {
    return this.http.post(this.apiUrl, body);
  }
}


