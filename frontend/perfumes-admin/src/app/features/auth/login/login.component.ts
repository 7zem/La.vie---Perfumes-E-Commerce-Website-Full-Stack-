import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService, UserLoginDto } from '../../../core/services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  private fb = inject(FormBuilder);
  private auth = inject(AuthService);
  private router = inject(Router);

  loading = signal(false);
  error = signal<string | null>(null);

  form = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]],
    rememberMe: [false]
  });

  submit() {
    if (this.form.invalid) return;
    this.loading.set(true);
    this.error.set(null);
    const v = this.form.getRawValue();
    const dto: UserLoginDto = {
      email: v.email ?? '',
      password: v.password ?? '',
      rememberMe: v.rememberMe ?? false
    };
    this.auth.login(dto).subscribe({
      next: (res) => {
        this.auth.setToken(res.token);
        let role: string | null = null;
        try {
          const payload = JSON.parse(atob((res.token || '').split('.')[1] || ''));
          role = payload['role'] || payload['Role'] || payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || null;
        } catch {}
        if (role === 'Admin') {
          this.router.navigate(['/dashboard']);
        } else {
          // مستخدم Customer → ينتقل لمتجر الواجهة
          this.router.navigate(['/store']);
        }
      },
      error: (err) => {
        this.error.set(err?.error?.message || 'Login failed');
        this.loading.set(false);
      }
    });
  }
}


