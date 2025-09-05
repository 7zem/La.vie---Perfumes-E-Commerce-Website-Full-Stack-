import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { environment } from '../../../environments/environment';

export interface CategoryDto {
  categoryId: number;
  name: string;
  description?: string;
  parentCategoryId?: number | null;
  isActive: boolean;
}

@Injectable({ providedIn: 'root' })
export class CategoryService {
  private http = inject(HttpClient);
  private base = `${environment.apiBaseUrl}/Category`;

  getAll() { return this.http.get<CategoryDto[]>(this.base, { params: { _ts: Date.now() } as any }); }
  getById(id: number) { return this.http.get<CategoryDto>(`${this.base}/${id}`); }
  create(body: any) { return this.http.post<CategoryDto>(this.base, body); }
  update(id: number, body: any) { return this.http.put<void>(`${this.base}/${id}`, body); }
  delete(id: number) { return this.http.delete<void>(`${this.base}/${id}`); }
}


