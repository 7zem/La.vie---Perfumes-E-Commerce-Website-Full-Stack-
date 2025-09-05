import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, map, tap, of, throwError } from 'rxjs';
import { environment } from '../../../environments/environment';

export interface CartItemDto {
  cartId: number;
  productId: number;
  name: string;
  imageUrl?: string;
  price: number;
  quantity: number;
}

@Injectable({ providedIn: 'root' })
export class CartService {
  private http = inject(HttpClient);
  private apiUrl = environment.apiBaseUrl + '/Cart';
  private countSubject = new BehaviorSubject<number>(0);
  readonly count$ = this.countSubject.asObservable();

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

  getCart(userId?: number): Observable<CartItemDto[]> {
    const uid = userId ?? this.getUserIdFromToken() ?? undefined;
    if (!uid) { this.countSubject.next(0); return of([]); }
    const params: any = { userId: uid };
    return this.http.get<any[]>(this.apiUrl, { params }).pipe(
      map(items => (items || []).map(it => ({
        cartId: it.cartId ?? it.CartId ?? 0,
        productId: it.productId ?? it.ProductId ?? 0,
        name: it.name ?? it.productName ?? it.ProductName ?? '',
        imageUrl: it.imageUrl ?? it.productImageUrl ?? it.ProductImageUrl ?? undefined,
        price: Number(it.price ?? it.Price ?? 0),
        quantity: Number(it.quantity ?? it.Quantity ?? 0),
      }) as CartItemDto)),
      tap(() => this.refreshCount())
    );
  }

  add(productId: number, quantity = 1, userId?: number): Observable<any> {
    const uid = userId ?? this.getUserIdFromToken() ?? undefined;
    if (!uid) return throwError(() => new Error('Not authenticated'));
    const body: any = { productId, quantity, userId: uid };
    return this.http.post(this.apiUrl, body).pipe(tap(() => this.refreshCount()));
  }

  increase(cartId: number){ return this.http.put(`${this.apiUrl}/${cartId}/increase`, {}).pipe(tap(() => this.refreshCount())); }
  decrease(cartId: number){ return this.http.put(`${this.apiUrl}/${cartId}/decrease`, {}).pipe(tap(() => this.refreshCount())); }
  updateQuantity(cartId: number, quantity: number){ return this.http.put(`${this.apiUrl}/${cartId}/quantity`, {}, { params: { quantity } as any }).pipe(tap(() => this.refreshCount())); }
  remove(cartId: number){ return this.http.delete(`${this.apiUrl}/${cartId}`).pipe(tap(() => this.refreshCount())); }
  clear(userId?: number): Observable<any> {
    const uid = userId ?? this.getUserIdFromToken() ?? undefined;
    if (!uid) { this.countSubject.next(0); return of(null); }
    const params: any = { userId: uid };
    return this.http.delete(`${this.apiUrl}/clear`, { params }).pipe(tap(() => this.refreshCount()));
  }

  refreshCount(userId?: number){
    const uid = userId ?? this.getUserIdFromToken() ?? undefined;
    if (!uid) { this.countSubject.next(0); return; }
    const params: any = { userId: uid };
    this.http.get<{count:number}>(`${this.apiUrl}/count`, { params }).subscribe(r => this.countSubject.next(r.count), () => {});
  }

  constructor(){
    // initialize count on service creation
    Promise.resolve().then(() => this.refreshCount());
  }
}


