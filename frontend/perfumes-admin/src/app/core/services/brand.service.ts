import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { environment } from '../../../environments/environment';

export interface BrandDto {
  brandId: number;
  name: string;
  description?: string;
  logoUrl?: string;
  isActive: boolean;
}

@Injectable({ providedIn: 'root' })
export class BrandService {
  private http = inject(HttpClient);
  private base = `${environment.apiBaseUrl}/Brand`;

  getAll() { return this.http.get<BrandDto[]>(this.base, { params: { _ts: Date.now() } as any }); }
  getById(id: number) { return this.http.get<BrandDto>(`${this.base}/${id}`); }
  create(fd: FormData) { return this.http.post<BrandDto>(this.base, fd); }
  update(id: number, fd: FormData) { return this.http.put<void>(`${this.base}/${id}`, fd); }
  delete(id: number) { return this.http.delete<void>(`${this.base}/${id}`); }
}


