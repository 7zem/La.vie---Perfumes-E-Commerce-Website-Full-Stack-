import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { ProductService } from '../../../core/services/product.service';

@Component({
  selector: 'app-product-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  template: `
    <div class="page-header">
      <div>
        <h2 class="title">{{ isEdit ? 'Edit Product' : 'Add Product' }}</h2>
        <p class="subtitle">Provide product details, categories, inventory and image.</p>
        <span *ngIf="isEdit && productId" class="badge">ID: {{ productId }}</span>
      </div>
      <a class="link" routerLink="/dashboard/products">← Back to products</a>
    </div>

    <form class="form-card" [formGroup]="form" (ngSubmit)="submit()" autocomplete="off" novalidate>
      <div class="content">
        <section class="main">
          <div class="section">
            <h3 class="section-title">Basic information</h3>
            <div class="grid-2">
              <label class="field">
                <span class="label">Name <span class="req">*</span></span>
                <input class="control" formControlName="name" placeholder="e.g., Oud Wood" [class.invalid]="hasError('name')" [disabled]="isSaving" />
                <small class="error" *ngIf="hasError('name')">Name is required</small>
              </label>

              <label class="field">
                <span class="label">Price <span class="req">*</span></span>
                <div class="input-group">
                  <span class="addon">EGP</span>
                  <input class="control with-addon" type="number" min="0" step="0.01" formControlName="price" placeholder="0.00" [class.invalid]="hasError('price')" [disabled]="isSaving" />
                </div>
                <small class="error" *ngIf="hasError('price')">Enter a valid price</small>
              </label>

              <label class="field">
                <span class="label">Stock <span class="req">*</span></span>
                <div class="input-group">
                  <input class="control with-addon" type="number" min="0" step="1" formControlName="stock" placeholder="0" [class.invalid]="hasError('stock')" [disabled]="isSaving" />
                  <span class="addon">units</span>
                </div>
                <small class="error" *ngIf="hasError('stock')">Enter a valid quantity</small>
              </label>

              <label class="field">
                <span class="label">Brand</span>
                <select class="control" formControlName="brandId" [disabled]="isSaving || brandsLoading">
                  <option [ngValue]="null" disabled selected *ngIf="brandsLoading">Loading…</option>
                  <option [ngValue]="null" *ngIf="!brandsLoading">— Select brand —</option>
                  <option *ngFor="let b of brands" [ngValue]="b.brandId">{{ b.name }}</option>
                </select>
              </label>

              <label class="field">
                <span class="label">Category</span>
                <select class="control" formControlName="categoryId" [disabled]="isSaving || categoriesLoading">
                  <option [ngValue]="null" disabled selected *ngIf="categoriesLoading">Loading…</option>
                  <option [ngValue]="null" *ngIf="!categoriesLoading">— Select category —</option>
                  <option *ngFor="let c of categories" [ngValue]="c.categoryId">{{ c.name }}</option>
                </select>
              </label>
            </div>
          </div>

          <div class="section">
            <h3 class="section-title">Description</h3>
            <label class="field">
              <textarea class="control" rows="5" formControlName="description" placeholder="Notes, longevity, sillage, etc." [disabled]="isSaving"></textarea>
            </label>
          </div>
        </section>

        <aside class="side">
          <div class="section">
            <h3 class="section-title">Image</h3>
            <div class="image-uploader">
              <div class="preview" [class.empty]="!imagePreviewUrl">
                <img *ngIf="imagePreviewUrl" [src]="imagePreviewUrl" alt="Preview" />
                <div *ngIf="!imagePreviewUrl" class="placeholder">No image selected</div>
              </div>
              <small class="hint">Recommended: square image, at least 800×800px, JPG/PNG.</small>
              <div class="upload-actions">
                <label class="btn">
                  <input type="file" accept="image/*" (change)="onFile($event)" hidden [disabled]="isSaving" />
                  Choose image
                </label>
                <button type="button" class="btn ghost" (click)="clearImage()" [disabled]="!file || isSaving">Clear</button>
              </div>
            </div>
          </div>
        </aside>
      </div>

      <div class="actions sticky">
        <button type="button" class="btn ghost" (click)="navigateBack()" [disabled]="isSaving">Cancel</button>
        <button *ngIf="!isEdit" type="button" class="btn" (click)="submitAndAddAnother()" [disabled]="form.invalid || isSaving">Save & add another</button>
        <button type="submit" class="btn primary" [disabled]="form.invalid || isSaving">
          {{ isSaving ? (isEdit ? 'Saving…' : 'Creating…') : (isEdit ? 'Update product' : 'Create product') }}
        </button>
      </div>
    </form>
  `,
  styles: [`
    :host { display: block; padding: 16px 0; }
    /* Background gradient to make glass pop */
    :host {
      background:
        radial-gradient(1200px 400px at 0% -10%, rgba(99,102,241,0.06), transparent 50%),
        radial-gradient(1000px 400px at 100% 0%, rgba(16,185,129,0.06), transparent 50%);
    }
    .page-header { display: flex; align-items: end; justify-content: space-between; gap: 16px; margin-bottom: 16px; }
    .title { margin: 0; font-size: 22px; font-weight: 800; letter-spacing: -0.01em; color: var(--text); }
    .subtitle { margin: 4px 0 0; color: var(--muted); font-size: 12px; }
    .link { color: var(--primary); text-decoration: none; font-size: 13px; }

    /* Glassmorphism container */
    .form-card {
      border-radius: 18px;
      border: 1px solid rgba(255,255,255,0.10);
      background: rgba(255,255,255,0.06);
      box-shadow: 0 8px 30px rgba(0,0,0,0.20), inset 0 1px 0 rgba(255,255,255,0.05);
      -webkit-backdrop-filter: blur(14px) saturate(160%);
      backdrop-filter: blur(14px) saturate(160%);
    }
    .content { display: grid; grid-template-columns: minmax(0,1fr) 320px; gap: 20px; padding: 16px; }
    @media (max-width: 900px){ .content { grid-template-columns: 1fr; } }

    .section { display: grid; gap: 12px; }
    .section + .section { margin-top: 12px; }
    .section-title { font-size: 14px; color: var(--text); margin: 0 0 2px; font-weight: 700; }

    .grid-2 { display: grid; grid-template-columns: repeat(2, minmax(0, 1fr)); gap: 12px; }
    @media (max-width: 640px){ .grid-2 { grid-template-columns: 1fr; } }

    .field { display: grid; gap: 6px; }
    .label { font-size: 12px; color: var(--text); opacity: .9; }
    .req { color: #ef4444; }

    .control {
      width: 100%;
      padding: 10px 12px;
      border: 1px solid rgba(255,255,255,0.18);
      border-radius: 10px;
      background: rgba(255,255,255,0.10);
      font-size: 14px;
      transition: border-color .2s ease, box-shadow .2s ease, background .2s ease;
      color: var(--text);
    }
    .control::placeholder { color: var(--muted); }
    .control:focus {
      outline: none;
      border-color: rgba(91,124,250,0.6);
      box-shadow: 0 0 0 3px rgba(91,124,250,0.28);
      background: rgba(255,255,255,0.12);
    }
    .control.invalid { border-color: rgba(239,68,68,0.6); background: rgba(239,68,68,0.06); }
    textarea.control { resize: vertical; }

    .error { color: #fecaca; font-size: 12px; }
    .hint { color: var(--muted); font-size: 12px; }
    .badge {
      display: inline-block; margin-top: 6px; padding: 2px 8px;
      background: rgba(255,255,255,0.08);
      border: 1px solid rgba(255,255,255,0.12);
      color: var(--text); border-radius: 999px; font-size: 12px;
      -webkit-backdrop-filter: blur(10px) saturate(160%);
      backdrop-filter: blur(10px) saturate(160%);
    }

    .input-group { position: relative; display: grid; grid-template-columns: auto 1fr; align-items: center; gap: 8px; }
    .addon {
      background: rgba(255,255,255,0.10);
      color: var(--text);
      border: 1px solid rgba(255,255,255,0.18);
      padding: 10px 12px; border-radius: 10px;
      -webkit-backdrop-filter: blur(10px) saturate(160%);
      backdrop-filter: blur(10px) saturate(160%);
    }
    .with-addon { border-radius: 10px; }

    .side { display: grid; gap: 12px; }
    .image-uploader { display: grid; gap: 10px; }
    .preview {
      width: 100%; aspect-ratio: 1 / 1;
      border: 1px dashed rgba(255,255,255,0.18);
      border-radius: 12px; display: grid; place-items: center;
      overflow: hidden;
      background: rgba(255,255,255,0.10);
      -webkit-backdrop-filter: blur(8px) saturate(140%);
      backdrop-filter: blur(8px) saturate(140%);
    }
    .preview img { width: 100%; height: 100%; object-fit: cover; }
    .placeholder { color: var(--muted); font-size: 13px; }
    .upload-actions { display: flex; gap: 8px; }

    .actions { display: flex; justify-content: flex-end; gap: 10px; padding: 12px 16px; border-top: 1px solid var(--border); }
    .actions.sticky {
      position: sticky; bottom: 0;
      background: rgba(255,255,255,0.06);
      -webkit-backdrop-filter: blur(10px) saturate(160%);
      backdrop-filter: blur(10px) saturate(160%);
      border-bottom-left-radius: 18px; border-bottom-right-radius: 18px;
    }

    .btn { display: inline-flex; align-items: center; gap: 8px; border-radius: 10px; border: 1px solid rgba(255,255,255,0.16); background: rgba(255,255,255,0.10); padding: 8px 12px; font-size: 14px; cursor: pointer; -webkit-backdrop-filter: blur(8px) saturate(160%); backdrop-filter: blur(8px) saturate(160%); transition: transform .05s ease, box-shadow .2s ease, background .2s ease; color: var(--text); }
    .btn:hover { background: rgba(255,255,255,0.18); box-shadow: 0 6px 16px rgba(0,0,0,0.18); }
    .btn:active { transform: translateY(1px); }
    .btn.primary { color: #fff; border-color: transparent; background: var(--primary-grad); box-shadow: 0 8px 22px rgba(91,124,250,0.28); }
    .btn.ghost { background: rgba(255,255,255,0.06); color: var(--text); }
    .btn:disabled { opacity: 0.6; cursor: not-allowed; }

    /* Fallback for browsers without backdrop-filter */
    @supports not ((-webkit-backdrop-filter: blur(10px)) or (backdrop-filter: blur(10px))) {
      .form-card, .badge, .addon, .preview, .actions.sticky, .btn { background: #151522; }
    }
  `]
})
export class ProductFormComponent {
  private fb = inject(FormBuilder);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private api = inject(ProductService);

  isEdit = false;
  productId: number | null = null;
  file: File | null = null;
  imagePreviewUrl: string | null = null;
  isSaving = false;

  brands: { brandId: number; name: string }[] = [];
  categories: { categoryId: number; name: string }[] = [];
  brandsLoading = false;
  categoriesLoading = false;

  form = this.fb.group({
    name: ['', Validators.required],
    description: [''],
    price: [0, [Validators.required, Validators.min(0.01)]],
    stock: [0, [Validators.required, Validators.min(0)]],
    brandId: [null as number | null],
    categoryId: [null as number | null]
  });

  ngOnInit(){
    this.brandsLoading = true;
    this.categoriesLoading = true;
    this.api.getBrands().subscribe({ next: b => this.brands = b, complete: () => this.brandsLoading = false, error: () => this.brandsLoading = false });
    this.api.getCategories().subscribe({ next: c => this.categories = c, complete: () => this.categoriesLoading = false, error: () => this.categoriesLoading = false });

    const id = this.route.snapshot.paramMap.get('id');
    if (id && id !== 'new') {
      this.isEdit = true;
      this.productId = +id;
      this.api.getById(this.productId).subscribe(p => {
        this.form.patchValue({
          name: p.name,
          description: p.description || '',
          price: p.price,
          stock: p.stock,
          brandId: p.brandId ?? null,
          categoryId: p.categoryId ?? null
        });
      });
    }
  }

  onFile(e: Event){
    const input = e.target as HTMLInputElement;
    this.file = input.files?.[0] ?? null;
    if (this.file) {
      const reader = new FileReader();
      reader.onload = () => this.imagePreviewUrl = reader.result as string;
      reader.readAsDataURL(this.file);
    } else {
      this.imagePreviewUrl = null;
    }
  }

  clearImage(){
    this.file = null;
    this.imagePreviewUrl = null;
  }

  hasError(controlName: string): boolean {
    const ctrl = this.form.get(controlName);
    return !!ctrl && ctrl.invalid && (ctrl.dirty || ctrl.touched);
  }

  navigateBack(){
    this.router.navigate(['/dashboard/products']);
  }

  submit(){
    if (this.form.invalid || this.isSaving) return;
    this.isSaving = true;
    const v = this.form.getRawValue();
    const fd = new FormData();
    fd.append('name', v.name ?? '');
    fd.append('description', v.description ?? '');
    fd.append('price', String(v.price ?? 0));
    fd.append('stock', String(v.stock ?? 0));
    if (v.brandId != null) fd.append('brandId', String(v.brandId));
    if (v.categoryId != null) fd.append('categoryId', String(v.categoryId));
    if (this.file) fd.append('image', this.file);

    const done = () => {
      this.isSaving = false;
      this.router.navigate(['/dashboard/products']);
    };
    if (this.isEdit && this.productId){
      this.api.update(this.productId, fd).subscribe({
        next: done,
        error: err => { this.isSaving = false; alert(err?.error?.message || 'Update failed'); }
      });
    } else {
      this.api.create(fd).subscribe({
        next: done,
        error: err => { this.isSaving = false; alert(err?.error?.message || 'Create failed'); }
      });
    }
  }

  submitAndAddAnother(){
    if (this.form.invalid || this.isSaving) return;
    this.isSaving = true;
    const v = this.form.getRawValue();
    const fd = new FormData();
    fd.append('name', v.name ?? '');
    fd.append('description', v.description ?? '');
    fd.append('price', String(v.price ?? 0));
    fd.append('stock', String(v.stock ?? 0));
    if (v.brandId != null) fd.append('brandId', String(v.brandId));
    if (v.categoryId != null) fd.append('categoryId', String(v.categoryId));
    if (this.file) fd.append('image', this.file);

    this.api.create(fd).subscribe({
      next: () => {
        this.isSaving = false;
        this.form.reset({ name: '', description: '', price: 0, stock: 0, brandId: null, categoryId: null });
        this.clearImage();
        // Keep user on the same page to add another
      },
      error: err => { this.isSaving = false; alert(err?.error?.message || 'Create failed'); }
    });
  }
}



