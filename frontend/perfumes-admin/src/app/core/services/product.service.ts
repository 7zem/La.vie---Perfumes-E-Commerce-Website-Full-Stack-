import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';

export interface ProductDto {
  productId: number;
  name: string;
  description?: string;
  sku?: string;
  price: number;
  stock: number;
  imageUrl?: string;
  brandId?: number;
  brandName?: string;
  categoryId?: number;
  categoryName?: string;
  isActive: boolean;
}

export interface BrandDto { brandId: number; name: string }
export interface CategoryDto { categoryId: number; name: string }

@Injectable({ providedIn: 'root' })
export class ProductService {
  private http = inject(HttpClient);
  private base = `${environment.apiBaseUrl}`;

  // Products
  getAll() {
    return this.http.get<ProductDto[]>(`${this.base}/Product`, { params: { _ts: Date.now().toString() } });
  }
  getAllNoCacheBust() { return this.getAll(); }
  getById(id: number) {
    return this.http.get<ProductDto>(`${this.base}/Product/${id}`);
  }
  create(form: FormData) {
    return this.http.post<ProductDto>(`${this.base}/Product`, form);
  }
  update(id: number, form: FormData) {
    return this.http.put<void>(`${this.base}/Product/${id}`, form);
  }
  delete(id: number) {
    return this.http.delete<void>(`${this.base}/Product/${id}`);
  }

  // Lookups
  getBrands() {
    return this.http.get<BrandDto[]>(`${this.base}/Brand`);
  }
  getCategories() {
    return this.http.get<CategoryDto[]>(`${this.base}/Category`);
  }
}



