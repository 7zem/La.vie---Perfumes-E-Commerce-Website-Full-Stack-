import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';

type User = { userId: number; name: string; email: string; role: string; isActive: boolean };

@Component({
  selector: 'app-user-edit',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
    <h3>Edit User</h3>
    <form [formGroup]="form" (ngSubmit)="save()" class="form">
      <label>Name<input formControlName="name" /></label>
      <label>Email<input formControlName="email" /></label>
      <label>Role
        <select formControlName="role">
          <option value="Admin">Admin</option>
          <option value="Customer">Customer</option>
        </select>
      </label>
      <div class="actions">
        <button type="submit" class="btn">Save</button>
        <button type="button" (click)="cancel()">Cancel</button>
      </div>
    </form>
  `,
  styles: [`.form{display:grid;gap:10px;max-width:480px}input,select{padding:8px;border:1px solid #e5e7eb;border-radius:6px}.btn{background:#2563eb;color:#fff;padding:6px 10px;border-radius:6px;border:none;cursor:pointer}`]
})
export class UserEditComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private http = inject(HttpClient);
  private fb = inject(FormBuilder);

  userId!: number;
  form = this.fb.group({ name: [''], email: [''], role: ['Customer'] });

  ngOnInit(): void {
    this.userId = +(this.route.snapshot.paramMap.get('id') || 0);
    this.http.get<User>(`${environment.apiBaseUrl}/Admin/users/${this.userId}`).subscribe(u => {
      this.form.patchValue({ name: u.name, email: u.email, role: u.role });
    });
  }

  save(){
    const v = this.form.getRawValue();
    // نُحدّث الدور فقط كما طلبت، ويمكن لاحقًا تفعيل تعديل الاسم/الإيميل لو API يدعم
    this.http.put(`${environment.apiBaseUrl}/Admin/users/${this.userId}/role`, { role: v.role }).subscribe(() => {
      this.router.navigate(['/dashboard/users']);
    });
  }

  cancel(){ this.router.navigate(['/dashboard/users']); }
}


