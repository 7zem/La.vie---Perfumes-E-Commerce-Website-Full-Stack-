import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { CategoryDto, CategoryService } from '../../../core/services/category.service';

@Component({
  selector: 'app-categories-list',
  standalone: true,
  imports: [CommonModule, RouterLink],
  template: `
    <div class="page-header">
      <h2 class="title">Categories</h2>
      <a class="btn primary" routerLink="/dashboard/categories/new">Add category</a>
    </div>

    <div class="grid" *ngIf="categories?.length; else empty">
      <div class="card" *ngFor="let c of categories">
        <div class="info">
          <h4 class="name">{{ c.name }}</h4>
          <p class="muted" *ngIf="c.description">{{ c.description }}</p>
        </div>
        <div class="row-actions">
          <a class="btn sm" [routerLink]="['/dashboard/categories', c.categoryId]">Edit</a>
          <button class="btn sm danger" (click)="remove(c)">Delete</button>
        </div>
      </div>
    </div>
    <ng-template #empty>
      <div class="empty-card">No categories found.</div>
    </ng-template>
  `,
  styles: [`
    :host { display:block; padding:16px 0; }
    :host {
      background:
        radial-gradient(1200px 400px at 0% -10%, rgba(99,102,241,0.06), transparent 50%),
        radial-gradient(1000px 400px at 100% 0%, rgba(16,185,129,0.06), transparent 50%);
    }

    .page-header { display:flex; justify-content:space-between; align-items:flex-end; gap:12px; margin-bottom:16px; }
    .title { margin:0; font-size:22px; color: var(--text); font-weight:800; letter-spacing:-0.01em; }

    .btn { display:inline-flex; align-items:center; gap:8px; border-radius:10px; border:1px solid rgba(17,24,39,0.30); background: rgba(255,255,255,0.65); padding:8px 12px; font-size:14px; cursor:pointer; -webkit-backdrop-filter: blur(8px) saturate(160%); backdrop-filter: blur(8px) saturate(160%); transition: transform .05s ease, box-shadow .2s ease, background .2s ease; color:#0f172a; text-decoration:none }
    .btn:hover { background: rgba(255,255,255,0.85); box-shadow: 0 6px 16px rgba(0,0,0,0.08); }
    .btn:active { transform: translateY(1px); }
    .btn.primary { color:#fff; border-color:transparent; background: linear-gradient(135deg, rgba(2,6,23,0.95), rgba(30,41,59,0.95)); box-shadow: 0 8px 22px rgba(2,6,23,0.25); }
    .btn.danger { color:#b91c1c; border-color: rgba(239,68,68,0.35); background: rgba(239,68,68,0.10); }
    .btn.sm { padding:6px 10px; font-size:12px; }

    .grid { display:grid; grid-template-columns: repeat(auto-fill, minmax(260px, 1fr)); gap:16px; }

    .card { display:flex; flex-direction:column; border-radius:16px; overflow:hidden; border: 1px solid rgba(17,24,39,0.28); background: rgba(255,255,255,0.30); -webkit-backdrop-filter: blur(14px) saturate(160%); backdrop-filter: blur(14px) saturate(160%); box-shadow: 0 8px 30px rgba(31, 38, 135, 0.10), inset 0 1px 0 rgba(255,255,255,0.25); }
    .info { padding:12px; }
    .name { margin:0 0 6px; font-size:16px; color:#0f172a; font-weight:700; }
    .muted { margin:0; color:#374151; font-size:13px; opacity:0.8; }
    .row-actions { display:flex; justify-content:flex-end; gap:8px; padding:10px 12px; border-top:1px solid rgba(17,24,39,0.20); }

    .empty-card { border-radius:16px; padding:16px; text-align:center; color:#475569; border:1px dashed rgba(17,24,39,0.28); background: rgba(255,255,255,0.30); -webkit-backdrop-filter: blur(8px); backdrop-filter: blur(8px); }
  `]
})
export class CategoriesListComponent {
  private api = inject(CategoryService);
  categories: CategoryDto[] = [];

  ngOnInit(){ this.load(); }
  load(){ this.api.getAll().subscribe(res => this.categories = res); }
  remove(c: CategoryDto){ if(confirm(`Delete ${c.name}?`)) this.api.delete(c.categoryId).subscribe(() => this.load()); }
}


