import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { CategoryService } from '../../../core/services/category.service';

@Component({
  selector: 'app-category-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  template: `
    <div class="page-header">
      <div>
        <h2 class="title">{{ isEdit ? 'Edit Category' : 'Add Category' }}</h2>
        <p class="subtitle">Create or update a category.</p>
        <span *ngIf="isEdit && categoryId" class="badge">ID: {{ categoryId }}</span>
      </div>
      <a class="link" routerLink="/dashboard/categories">← Back to categories</a>
    </div>

    <form class="form-card" [formGroup]="form" (ngSubmit)="submit()" autocomplete="off" novalidate>
      <div class="content">
        <section class="main">
          <div class="section">
            <h3 class="section-title">Basic information</h3>
            <div class="grid-2">
              <label class="field">
                <span class="label">Name <span class="req">*</span></span>
                <input class="control" formControlName="name" placeholder="e.g., Amber" [class.invalid]="hasError('name')" [disabled]="isSaving" />
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
      </div>

      <div class="actions sticky">
        <button type="button" class="btn ghost" (click)="navigateBack()" [disabled]="isSaving">Cancel</button>
        <button *ngIf="!isEdit" type="button" class="btn" (click)="submitAndAddAnother()" [disabled]="form.invalid || isSaving">Save & add another</button>
        <button type="submit" class="btn primary" [disabled]="form.invalid || isSaving">{{ isSaving ? (isEdit ? 'Saving…' : 'Creating…') : (isEdit ? 'Update category' : 'Create category') }}</button>
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
    .title { margin: 0; font-size: 22px; color: var(--text); font-weight: 800; letter-spacing: -0.01em; }
    .subtitle { margin: 4px 0 0; color: var(--muted); font-size: 12px; }
    .link { color: #2563eb; text-decoration: none; font-size: 13px; }

    .form-card {
      border-radius: 18px;
      border: 1px solid rgba(255,255,255,0.10);
      background: rgba(255,255,255,0.06);
      box-shadow: 0 8px 30px rgba(0, 0, 0, 0.20), inset 0 1px 0 rgba(255,255,255,0.05);
      -webkit-backdrop-filter: blur(14px) saturate(160%);
      backdrop-filter: blur(14px) saturate(160%);
    }
    .content { display: grid; grid-template-columns: minmax(0,1fr); gap: 20px; padding: 16px; }

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
    .control:focus { outline: none; border-color: rgba(37,99,235,0.55); box-shadow: 0 0 0 3px rgba(37,99,235,0.25); background: rgba(255,255,255,0.12); }
    .control.invalid { border-color: #ef4444; background: #fff7f7; }
    textarea.control { resize: vertical; }

    .error { color: #fecaca; font-size: 12px; }
    .badge { display: inline-block; margin-top: 6px; padding: 2px 8px; background: rgba(255,255,255,0.08); border: 1px solid rgba(255,255,255,0.12); color: var(--text); border-radius: 999px; font-size: 12px; -webkit-backdrop-filter: blur(10px) saturate(160%); backdrop-filter: blur(10px) saturate(160%); }

    .actions { display: flex; justify-content: flex-end; gap: 10px; padding: 12px 16px; border-top: 1px solid rgba(255,255,255,0.10); }
    .actions.sticky { position: sticky; bottom: 0; background: rgba(255,255,255,0.06); -webkit-backdrop-filter: blur(10px) saturate(160%); backdrop-filter: blur(10px) saturate(160%); border-bottom-left-radius: 18px; border-bottom-right-radius: 18px; }

    .btn { display: inline-flex; align-items: center; gap: 8px; border-radius: 10px; border: 1px solid rgba(255,255,255,0.18); background: rgba(255,255,255,0.10); padding: 8px 12px; font-size: 14px; cursor: pointer; -webkit-backdrop-filter: blur(8px) saturate(160%); backdrop-filter: blur(8px) saturate(160%); transition: transform .05s ease, box-shadow .2s ease, background .2s ease; color: var(--text); }
    .btn:hover { background: rgba(255,255,255,0.18); box-shadow: 0 6px 16px rgba(0,0,0,0.18); }
    .btn:active { transform: translateY(1px); }
    .btn.primary { color: #fff; border-color: transparent; background: var(--primary-grad); box-shadow: 0 8px 22px rgba(37,99,235,0.25); }
    .btn.primary:hover { box-shadow: 0 10px 26px rgba(37,99,235,0.32); }
    .btn.ghost { background: rgba(255,255,255,0.08); color: var(--text); }
    .btn:disabled { opacity: 0.6; cursor: not-allowed; }

    @supports not ((-webkit-backdrop-filter: blur(10px)) or (backdrop-filter: blur(10px))) {
      .form-card, .badge, .actions.sticky, .btn { background: #ffffff; }
    }
  `]
})
export class CategoryFormComponent {
  private fb = inject(FormBuilder);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private api = inject(CategoryService);

  isEdit = false;
  categoryId: number | null = null;
  isSaving = false;

  form = this.fb.group({
    name: ['', Validators.required],
    description: ['']
  });

  ngOnInit(){
    const id = this.route.snapshot.paramMap.get('id');
    if (id && id !== 'new') {
      this.isEdit = true;
      this.categoryId = +id;
      this.api.getById(this.categoryId).subscribe(c => {
        this.form.patchValue({ name: c.name, description: c.description || '' });
      });
    }
  }

  hasError(controlName: string): boolean {
    const ctrl = this.form.get(controlName);
    return !!ctrl && ctrl.invalid && (ctrl.dirty || ctrl.touched);
  }

  navigateBack(){
    this.router.navigate(['/dashboard/categories']);
  }

  submit(){
    if (this.form.invalid || this.isSaving) return;
    this.isSaving = true;
    const v = this.form.getRawValue();
    const body = { name: v.name ?? '', description: v.description ?? '' };
    const done = () => { this.isSaving = false; this.router.navigate(['/dashboard/categories']); };
    if (this.isEdit && this.categoryId){
      this.api.update(this.categoryId, body).subscribe({ next: done, error: err => { this.isSaving = false; alert(err?.error?.message || 'Update failed'); }});
    } else {
      this.api.create(body).subscribe({ next: done, error: err => { this.isSaving = false; alert(err?.error?.message || 'Create failed'); }});
    }
  }

  submitAndAddAnother(){
    if (this.form.invalid || this.isSaving || this.isEdit) return;
    this.isSaving = true;
    const v = this.form.getRawValue();
    const body = { name: v.name ?? '', description: v.description ?? '' };
    this.api.create(body).subscribe({
      next: () => {
        this.isSaving = false;
        this.form.reset({ name: '', description: '' });
      },
      error: err => { this.isSaving = false; alert(err?.error?.message || 'Create failed'); }
    });
  }
}


