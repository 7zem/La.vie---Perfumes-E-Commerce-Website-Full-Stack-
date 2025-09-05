import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  template: `
  <div class="register-container">
    <form [formGroup]="form" (ngSubmit)="submit()">
      <h2>Create account</h2>
      <div class="field"><label>Name</label><input type="text" formControlName="name" /></div>
      <div class="field"><label>Email</label><input type="email" formControlName="email" /></div>
      <div class="field"><label>Password</label><input type="password" formControlName="password" /></div>
      <div class="field"><label>Confirm Password</label><input type="password" formControlName="confirmPassword" /></div>
      <div class="actions"><button type="submit" [disabled]="loading()">Register</button></div>
      <p class="muted">Already have an account? <a routerLink="/login">Sign in</a></p>
      <p class="error" *ngIf="error()">{{ error() }}</p>
    </form>
  </div>
  `,
  styles: [`
    .register-container{min-height:100vh;display:grid;place-items:center;background:#0f0f14;color:#fff;padding:1rem}
    form{width:100%;max-width:480px;background:#151522;border:1px solid rgba(255,255,255,.08);border-radius:12px;padding:24px}
    h2{margin:0 0 16px;font-weight:600}
    .field{display:grid;gap:6px;margin-bottom:12px}
    label{font-size:.9rem;color:#cfd3dc}
    input{width:100%;padding:10px 12px;border-radius:8px;border:1px solid #2a2a3b;background:#0e0e18;color:#fff}
    .actions{display:grid}
    button{background:linear-gradient(90deg,#F0060B,#7702FF);border:none;color:white;border-radius:10px;padding:10px 14px;cursor:pointer}
    .muted{color:#cfd3dc;margin-top:10px}
    .error{color:#ff6b6b;margin-top:10px}
  `]
})
export class RegisterComponent {
  private fb = inject(FormBuilder);
  private auth = inject(AuthService);
  private router = inject(Router);

  loading = signal(false);
  error = signal<string | null>(null);

  form = this.fb.group({
    name: ['', [Validators.required, Validators.maxLength(100)]],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(8)]],
    confirmPassword: ['', [Validators.required]]
  });

  submit(){
    if (this.form.invalid) return;
    if (this.form.value.password !== this.form.value.confirmPassword){
      this.error.set('Passwords do not match');
      return;
    }
    this.loading.set(true);
    this.error.set(null);
    this.auth.register({
      name: this.form.value.name || '',
      email: this.form.value.email || '',
      password: this.form.value.password || '',
      confirmPassword: this.form.value.confirmPassword || ''
    } as any).subscribe({
      next: () => this.router.navigate(['/login']),
      error: (err) => {
        const msg = err?.error?.message || 'Registration failed';
        const errs = err?.error?.errors ? Object.values(err.error.errors).flat().join(' , ') : '';
        this.error.set([msg, errs].filter(Boolean).join(' - '));
        this.loading.set(false);
      }
    });
  }
}


