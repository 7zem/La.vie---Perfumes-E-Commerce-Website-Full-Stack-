import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { BrandService } from '../../../core/services/brand.service';

@Component({
  selector: 'app-brand-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  template: `
    <div class="page-header">
      <div>
        <h2 class="title">{{ isEdit ? 'Edit Brand' : 'Add Brand' }}</h2>
        <p class="subtitle">Create or update a brand and upload its logo.</p>
        <span *ngIf="isEdit && brandId" class="badge">ID: {{ brandId }}</span>
      </div>
      <a class="link" routerLink="/dashboard/brands">← Back to brands</a>
    </div>

    <form class="form-card" [formGroup]="form" (ngSubmit)="submit()" autocomplete="off" novalidate>
      <div class="content">
        <section class="main">
          <div class="section">
            <h3 class="section-title">Basic information</h3>
            <div class="grid-2">
              <label class="field">
                <span class="label">Name <span class="req">*</span></span>
                <input class="control" formControlName="name" placeholder="e.g., Tom Ford" [class.invalid]="hasError('name')" [disabled]="isSaving" />
                <small class="error" *ngIf="hasError('name')">Name is required</small>
              </label>
            </div>
          </div>

          <div class="section">
            <h3 class="section-title">Description</h3>
            <label class="field">
              <textarea class="control" rows="5" formControlName="description" placeholder="Optional" [disabled]="isSaving"></textarea>
            </label>
          </div>
        </section>

        <aside class="side">
          <div class="section">
            <h3 class="section-title">Logo</h3>
            <div class="image-uploader">
              <div class="preview" [class.empty]="!imagePreviewUrl">
                <img *ngIf="imagePreviewUrl" [src]="imagePreviewUrl" alt="Preview" />
                <div *ngIf="!imagePreviewUrl" class="placeholder">No image selected</div>
              </div>
              <small class="hint">Recommended: square image, at least 400×400px, PNG with transparency.</small>
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
        <button type="submit" class="btn primary" [disabled]="form.invalid || isSaving">{{ isSaving ? (isEdit ? 'Saving…' : 'Creating…') : (isEdit ? 'Update brand' : 'Create brand') }}</button>
      </div>
    </form>
  `,
  styles: [`
    :host { display: block; padding: 16px 0; }
    :host {
      background:
        radial-gradient(1200px 400px at 0% -10%, rgba(99,102,241,0.06), transparent 50%),
        radial-gradient(1000px 400px at 100% 0%, rgba(16,185,129,0.06), transparent 50%);
    }
    .page-header { display: flex; align-items: end; justify-content: space-between; gap: 16px; margin-bottom: 16px; }
    .title { margin: 0; font-size: 22px; font-weight: 800; letter-spacing: -0.01em; background: var(--primary-grad); -webkit-background-clip: text; background-clip: text; color: transparent; }
    .subtitle { margin: 4px 0 0; color: var(--muted); font-size: 12px; }
    .link { color: var(--primary); text-decoration: none; font-size: 13px; }

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
      width: 100%; padding: 10px 12px;
      border: 1px solid rgba(255,255,255,0.18);
      border-radius: 10px; background: rgba(255,255,255,0.10);
      font-size: 14px; transition: border-color .2s, box-shadow .2s, background .2s; color: var(--text);
    }
    .control::placeholder { color: var(--muted); }
    .control:focus { outline: none; border-color: rgba(91,124,250,0.6); box-shadow: 0 0 0 3px rgba(91,124,250,0.28); background: rgba(255,255,255,0.12); }
    .control.invalid { border-color: rgba(239,68,68,0.6); background: rgba(239,68,68,0.06); }
    textarea.control { resize: vertical; }

    .error { color: #fecaca; font-size: 12px; }
    .hint { color: var(--muted); font-size: 12px; }
    .badge { display: inline-block; margin-top: 6px; padding: 2px 8px; background: rgba(255,255,255,0.08); border: 1px solid rgba(255,255,255,0.12); color: var(--text); border-radius: 999px; font-size: 12px; -webkit-backdrop-filter: blur(10px) saturate(160%); backdrop-filter: blur(10px) saturate(160%); }

    .side { display: grid; gap: 12px; }
    .image-uploader { display: grid; gap: 10px; }
    .preview { width: 100%; aspect-ratio: 1 / 1; border: 1px dashed rgba(255,255,255,0.18); border-radius: 12px; display: grid; place-items: center; overflow: hidden; background: rgba(255,255,255,0.10); -webkit-backdrop-filter: blur(8px) saturate(140%); backdrop-filter: blur(8px) saturate(140%); }
    .preview img { width: 100%; height: 100%; object-fit: cover; }
    .placeholder { color: #9ca3af; font-size: 13px; }
    .upload-actions { display: flex; gap: 8px; }

    .actions { display: flex; justify-content: flex-end; gap: 10px; padding: 12px 16px; border-top: 1px solid var(--border); }
    .actions.sticky { position: sticky; bottom: 0; background: rgba(255,255,255,0.06); -webkit-backdrop-filter: blur(10px) saturate(160%); backdrop-filter: blur(10px) saturate(160%); border-bottom-left-radius: 18px; border-bottom-right-radius: 18px; }

    .btn { display: inline-flex; align-items: center; gap: 8px; border-radius: 10px; border: 1px solid rgba(255,255,255,0.16); background: rgba(255,255,255,0.10); padding: 8px 12px; font-size: 14px; cursor: pointer; -webkit-backdrop-filter: blur(8px) saturate(160%); backdrop-filter: blur(8px) saturate(160%); transition: transform .05s ease, box-shadow .2s ease, background .2s ease; color: var(--text); }
    .btn:hover { background: rgba(255,255,255,0.18); box-shadow: 0 6px 16px rgba(0,0,0,0.18); }
    .btn:active { transform: translateY(1px); }
    .btn.primary { color: #fff; border-color: transparent; background: var(--primary-grad); box-shadow: 0 8px 22px rgba(91,124,250,0.28); }
    .btn.primary:hover { box-shadow: 0 10px 26px rgba(91,124,250,0.35); }
    .btn.ghost { background: rgba(255,255,255,0.06); color: var(--text); }
    .btn:disabled { opacity: 0.6; cursor: not-allowed; }

    @supports not ((-webkit-backdrop-filter: blur(10px)) or (backdrop-filter: blur(10px))) {
      .form-card, .badge, .preview, .actions.sticky, .btn { background: #151522; }
    }
  `]
})
export class BrandFormComponent {
  private fb = inject(FormBuilder);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private api = inject(BrandService);

  isEdit = false;
  brandId: number | null = null;
  file: File | null = null;
  imagePreviewUrl: string | null = null;
  isSaving = false;

  form = this.fb.group({
    name: ['', Validators.required],
    description: ['']
  });

  ngOnInit(){
    const id = this.route.snapshot.paramMap.get('id');
    if (id && id !== 'new') {
      this.isEdit = true;
      this.brandId = +id;
      this.api.getById(this.brandId).subscribe(b => {
        this.form.patchValue({ name: b.name, description: b.description || '' });
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
    this.router.navigate(['/dashboard/brands']);
  }

  submit(){
    if (this.form.invalid || this.isSaving) return;
    this.isSaving = true;
    const v = this.form.getRawValue();
    const fd = new FormData();
    fd.append('name', v.name ?? '');
    fd.append('description', v.description ?? '');
    if (this.file) fd.append('logo', this.file);

    const done = () => { this.isSaving = false; this.router.navigate(['/dashboard/brands']); };
    if (this.isEdit && this.brandId){
      this.api.update(this.brandId, fd).subscribe({ next: done, error: err => { this.isSaving = false; alert(err?.error?.message || 'Update failed'); }});
    } else {
      this.api.create(fd).subscribe({ next: done, error: err => { this.isSaving = false; alert(err?.error?.message || 'Create failed'); }});
    }
  }

  submitAndAddAnother(){
    if (this.form.invalid || this.isSaving || this.isEdit) return;
    this.isSaving = true;
    const v = this.form.getRawValue();
    const fd = new FormData();
    fd.append('name', v.name ?? '');
    fd.append('description', v.description ?? '');
    if (this.file) fd.append('logo', this.file);

    this.api.create(fd).subscribe({
      next: () => {
        this.isSaving = false;
        this.form.reset({ name: '', description: '' });
        this.clearImage();
      },
      error: err => { this.isSaving = false; alert(err?.error?.message || 'Create failed'); }
    });
  }
}


