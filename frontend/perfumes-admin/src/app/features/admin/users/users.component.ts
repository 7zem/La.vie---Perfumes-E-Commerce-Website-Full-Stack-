import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { ReactiveFormsModule, FormBuilder } from '@angular/forms';

type User = { userId: number; name: string; email: string; role: string; isActive: boolean };

@Component({
  selector: 'app-users',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, RouterLink],
  template: `
    <div class="page-header">
      <h2 class="title">Users</h2>
      <div class="header-actions">
        <button class="btn primary" (click)="openCreate()">Add user</button>
      </div>
    </div>

    <div class="card" *ngIf="users?.length; else empty">
      <table class="table">
        <thead>
          <tr>
            <th>ID</th>
            <th>Name</th>
            <th>Email</th>
            <th>Role</th>
            <th>Status</th>
            <th class="actions-col"></th>
          </tr>
        </thead>
        <tbody>
          <tr *ngFor="let u of users">
            <td>{{ u.userId }}</td>
            <td>{{ u.name }}</td>
            <td>{{ u.email }}</td>
            <td>{{ u.role }}</td>
            <td>
              <span class="chip" [class.success]="u.isActive" [class.danger]="!u.isActive">{{ u.isActive ? 'Active' : 'Blocked' }}</span>
            </td>
            <td class="row-actions">
              <button class="btn ghost sm" (click)="toggleBlock(u)">{{ u.isActive ? 'Block' : 'Unblock' }}</button>
              <a class="btn sm" [routerLink]="['/dashboard/users', u.userId]">Edit</a>
              <button class="btn sm danger" (click)="delete(u)">Delete</button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <ng-template #empty>
      <div class="card empty">
        <p>No users found.</p>
      </div>
    </ng-template>

    <!-- Create user form -->
    <form *ngIf="showCreate" [formGroup]="form" (ngSubmit)="create()" class="form-card" autocomplete="off" novalidate>
      <div class="form-content">
        <h3 class="section-title">Create user</h3>
        <div class="grid-3">
          <label class="field">
            <span class="label">Name</span>
            <input class="control" formControlName="name" placeholder="e.g., John Doe" [class.invalid]="hasError('name')" [disabled]="isSaving" />
            <small class="error" *ngIf="hasError('name')">Name is required</small>
          </label>
          <label class="field">
            <span class="label">Email</span>
            <input class="control" type="email" formControlName="email" placeholder="user@example.com" [class.invalid]="hasError('email')" [disabled]="isSaving" />
            <small class="error" *ngIf="hasError('email')">Valid email is required</small>
          </label>
          <label class="field">
            <span class="label">Password</span>
            <input class="control" type="password" formControlName="password" placeholder="Min 6 characters" [class.invalid]="hasError('password')" [disabled]="isSaving" />
            <small class="error" *ngIf="hasError('password')">Password must be at least 6 chars</small>
          </label>
        </div>
      </div>
      <div class="form-actions">
        <button type="button" class="btn ghost" (click)="showCreate=false" [disabled]="isSaving">Cancel</button>
        <button type="submit" class="btn primary" [disabled]="form.invalid || isSaving">{{ isSaving ? 'Creating…' : 'Create user' }}</button>
      </div>
    </form>
  `,
  styles: [`
    :host { display:block; padding: 16px 0; }
    :host {
      background:
        radial-gradient(1200px 400px at 0% -10%, rgba(99,102,241,0.06), transparent 50%),
        radial-gradient(1000px 400px at 100% 0%, rgba(16,185,129,0.06), transparent 50%);
    }
    .page-header { display:flex; align-items:flex-end; justify-content:space-between; gap:12px; margin-bottom:12px; }
    .title { margin:0; font-size:22px; font-weight:800; letter-spacing:-0.01em; background: var(--primary-grad); -webkit-background-clip:text; background-clip:text; color: transparent; }
    .header-actions { display:flex; gap:8px; }

    .card {
      border-radius: 18px;
      border: 1px solid rgba(255,255,255,0.10);
      background: rgba(255,255,255,0.06);
      box-shadow: 0 8px 30px rgba(0,0,0,0.20), inset 0 1px 0 rgba(255,255,255,0.05);
      -webkit-backdrop-filter: blur(14px) saturate(160%);
      backdrop-filter: blur(14px) saturate(160%);
      padding: 8px;
    }
    .card.empty { padding: 20px; text-align: center; color: var(--muted); }

    .table { width:100%; border-collapse: separate; border-spacing: 0; overflow: hidden; border-radius: 12px; }
    thead th { text-align:left; font-weight:600; font-size:13px; color:var(--muted); padding:10px; background: rgba(255,255,255,0.05); border-bottom: 1px solid var(--border); }
    tbody td { padding:10px; border-bottom: 1px solid var(--border); font-size:14px; color:var(--text); }
    tbody tr:hover { background: rgba(255,255,255,0.06); }
    .actions-col { width: 280px; }
    .row-actions { display:flex; gap:6px; justify-content:flex-end; }

    .chip { display:inline-block; padding:2px 8px; font-size:12px; border-radius:999px; background: rgba(107,114,128,0.15); color:var(--text); border:1px solid rgba(107,114,128,0.25); }
    .chip.success { background: rgba(16,185,129,0.15); color:#bbf7d0; border-color: rgba(16,185,129,0.35); }
    .chip.danger { background: rgba(239,68,68,0.12); color:#fecaca; border-color: rgba(239,68,68,0.35); }

    .btn { display:inline-flex; align-items:center; gap:8px; border-radius:10px; border:1px solid rgba(255,255,255,0.16); background: rgba(255,255,255,0.10); padding:8px 12px; font-size:14px; cursor:pointer; -webkit-backdrop-filter: blur(8px) saturate(160%); backdrop-filter: blur(8px) saturate(160%); transition: transform .05s ease, box-shadow .2s ease, background .2s ease; color:var(--text); }
    .btn:hover { background: rgba(255,255,255,0.18); box-shadow: 0 6px 16px rgba(0,0,0,0.18); }
    .btn:active { transform: translateY(1px); }
    .btn.primary { color:#fff; border-color:transparent; background: var(--primary-grad); box-shadow: 0 8px 22px rgba(91,124,250,0.28); }
    .btn.ghost { background: rgba(255,255,255,0.06); }
    .btn.danger { color:#fecaca; border-color: rgba(239,68,68,0.35); background: rgba(239,68,68,0.12); }
    .btn.sm { padding: 6px 10px; font-size: 12px; }

    /* Create form styles */
    .form-card {
      margin-top: 14px;
      border-radius: 18px;
      border: 1px solid rgba(255,255,255,0.10);
      background: rgba(255,255,255,0.06);
      box-shadow: 0 8px 30px rgba(0,0,0,0.20), inset 0 1px 0 rgba(255,255,255,0.05);
      -webkit-backdrop-filter: blur(14px) saturate(160%);
      backdrop-filter: blur(14px) saturate(160%);
    }
    .form-content { padding: 14px; }
    .section-title { margin: 0 0 8px; font-size: 14px; color: var(--text); font-weight: 700; }
    .grid-3 { display:grid; grid-template-columns: repeat(3,minmax(0,1fr)); gap: 12px; }
    @media (max-width: 800px){ .grid-3 { grid-template-columns: 1fr; } }
    .field { display:grid; gap:6px; }
    .label { font-size:12px; color:var(--text); opacity:.9; }
    .control { width:100%; padding:10px 12px; border:1px solid rgba(255,255,255,0.18); border-radius:10px; background: rgba(255,255,255,0.10); font-size:14px; transition: border-color .2s, box-shadow .2s, background .2s; color:var(--text); }
    .control:focus { outline:none; border-color: rgba(91,124,250,0.6); box-shadow: 0 0 0 3px rgba(91,124,250,0.28); background: rgba(255,255,255,0.12); }
    .control.invalid { border-color:#ef4444; background:rgba(239,68,68,0.06); }
    .error { color:#fecaca; font-size:12px; }
    .form-actions { display:flex; justify-content:flex-end; gap:10px; padding: 10px 14px; border-top:1px solid var(--border); }
  `]
})
export class UsersComponent {
  private http = inject(HttpClient);
  private fb = inject(FormBuilder);
  users: User[] = [];
  showCreate = false;
  isSaving = false;
  form = this.fb.group({ name: [''], email: [''], password: [''] });

  ngOnInit() { this.load(); }
  load(){ this.http.get<User[]>(`${environment.apiBaseUrl}/Admin/users`).subscribe(res => this.users = res); }
  openCreate(){ this.showCreate = true; }
  create(){
    if (this.form.invalid || this.isSaving) return;
    this.isSaving = true;
    const v = this.form.getRawValue();
    const body = { name: v.name, email: v.email, password: v.password } as any;
    this.http.post(`${environment.apiBaseUrl}/Auth/create-admin`, body).subscribe({
      next: () => { this.isSaving = false; this.showCreate=false; this.form.reset(); this.load(); },
      error: () => { this.isSaving = false; alert('Create failed'); }
    });
  }
  hasError(controlName: string): boolean {
    const c = this.form.get(controlName);
    return !!c && c.invalid && (c.dirty || c.touched);
  }
  toggleBlock(u: User){ const op = u.isActive ? 'block' : 'unblock'; this.http.post(`${environment.apiBaseUrl}/Admin/users/${u.userId}/${op}`, {}).subscribe(() => this.load()); }
  delete(u: User){ if(confirm('Delete user?')) this.http.delete(`${environment.apiBaseUrl}/Admin/users/${u.userId}`).subscribe(() => this.load()); }
}



