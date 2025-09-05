import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { BrandDto, BrandService } from '../../../core/services/brand.service';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-brands-list',
  standalone: true,
  imports: [CommonModule, RouterLink],
  template: `
    <div class="header">
      <h3>Brands</h3>
      <a class="btn" routerLink="/dashboard/brands/new">Add Brand</a>
    </div>

    <div class="grid" *ngIf="brands?.length; else empty">
      <div class="card" *ngFor="let b of brands">
        <div class="thumb" [class.noimg]="!b.logoUrl">
          <img *ngIf="b.logoUrl" [src]="apiBase + b.logoUrl" [alt]="b.name" />
        </div>
        <div class="info">
          <h4 class="name" title="{{ b.name }}">{{ b.name }}</h4>
          <p class="muted" *ngIf="b.description">{{ b.description }}</p>
          <div class="cta">
            <a class="btn-outline sm" [routerLink]="['/dashboard/brands', b.brandId]">Edit</a>
            <button class="btn-outline sm danger" (click)="remove(b)">Delete</button>
          </div>
        </div>
      </div>
    </div>
    <ng-template #empty><p>No brands found.</p></ng-template>
  `,
  styles: [`
    .header{display:flex;justify-content:space-between;align-items:center;margin-bottom:16px}
    .btn{background:var(--primary);color:#fff;padding:8px 12px;border-radius:10px;text-decoration:none}
    .grid{display:grid;grid-template-columns:repeat(auto-fill,minmax(240px,1fr));gap:18px}
    .card{background:var(--panel);border:1px solid var(--border);border-radius:14px;overflow:hidden;transition:transform .2s ease, box-shadow .2s ease}
    .card:hover{transform:translateY(-4px);box-shadow:0 10px 30px rgba(0,0,0,.35)}
    .thumb{position:relative;background:#0e0e18;height:160px;display:grid;place-items:center}
    .thumb img{max-height:120px;max-width:80%;object-fit:contain;filter: drop-shadow(0 6px 24px rgba(0,0,0,.35))}
    .info{padding:12px;display:grid;gap:8px}
    .name{margin:0;font-size:16px}
    .muted{margin:0;color:var(--muted);font-size:13px;line-height:1.4}
    .cta{display:flex;gap:8px;margin-top:4px}
    .btn-outline.sm{padding:6px 10px;border-radius:8px}
    .btn-outline.sm.danger{border-color:#ef4444;color:#ef4444}
  `]
})
export class BrandsListComponent {
  private api = inject(BrandService);
  brands: BrandDto[] = [];
  apiBase = environment.apiBaseUrl.replace('/api','');

  ngOnInit(){ this.load(); }
  load(){ this.api.getAll().subscribe(res => this.brands = res); }
  remove(b: BrandDto){ if(confirm(`Delete ${b.name}?`)) this.api.delete(b.brandId).subscribe(() => this.load()); }
}


