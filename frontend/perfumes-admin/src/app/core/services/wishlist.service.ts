import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, map, tap, of, throwError } from 'rxjs';
import { environment } from '../../../environments/environment';

export interface WishlistItemDto {
  productId: number;
  productName: string;
  price: number;
  imageUrl?: string;
  addedDate?: string;
}

@Injectable({ providedIn: 'root' })
export class WishlistService {
  private http = inject(HttpClient);
  private apiUrl = environment.apiBaseUrl + '/Wishlist';
  private countSubject = new BehaviorSubject<number>(0);
  readonly count$ = this.countSubject.asObservable();

  get(): Observable<WishlistItemDto[]> {
    const token = localStorage.getItem('token');
    if (!token) { this.countSubject.next(0); return of([]); }
    return this.http.get<any[]>(this.apiUrl).pipe(
      map(items => (items || []).map(it => ({
        productId: it.productId ?? it.ProductId ?? 0,
        productName: it.productName ?? it.ProductName ?? '',
        price: Number(it.price ?? it.Price ?? 0),
        imageUrl: it.imageUrl ?? it.ImageUrl ?? undefined,
        addedDate: it.addedDate ?? it.AddedDate
      }) as WishlistItemDto)),
      tap(() => this.refreshCount())
    );
  }

  add(productId: number){
    const token = localStorage.getItem('token');
    if (!token) return throwError(() => new Error('Not authenticated'));
    return this.http.post(this.apiUrl, null, { params: { productId } as any }).pipe(tap(() => this.refreshCount()));
  }
  remove(productId: number){
    const token = localStorage.getItem('token');
    if (!token) return throwError(() => new Error('Not authenticated'));
    return this.http.delete(this.apiUrl, { params: { productId } as any }).pipe(tap(() => this.refreshCount()));
  }
  exists(productId: number){
    const token = localStorage.getItem('token');
    if (!token) return of({ exists: false });
    return this.http.get<{exists:boolean}>(`${this.apiUrl}/exists`, { params: { productId } as any });
  }
  count(){
    const token = localStorage.getItem('token');
    if (!token) return of({ count: 0 });
    return this.http.get<{count:number}>(`${this.apiUrl}/count`);
  }

  refreshCount(){
    const token = localStorage.getItem('token');
    if (!token) { this.countSubject.next(0); return; }
    this.count().subscribe((r: { count: number }) => this.countSubject.next(r.count), () => {});
  }

  constructor(){ Promise.resolve().then(() => this.refreshCount()); }
}


