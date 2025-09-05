import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from '../../../environments/environment';

export interface AuthResponseDto {
  success: boolean;
  token?: string;
  refreshToken?: string;
  user?: { userId: number; name: string; email: string; role?: string };
  message?: string;
  expiresAt: string;
}

export interface UserLoginDto {
  email: string;
  password: string;
  rememberMe?: boolean;
}

export interface UserRegisterDto {
  name: string;
  email: string;
  password: string;
  confirmPassword: string;
}

@Injectable({ providedIn: 'root' })
export class AuthService {
  private http = inject(HttpClient);
  private router = inject(Router);

  login(dto: UserLoginDto) {
    return this.http.post<AuthResponseDto>(`${environment.apiBaseUrl}/Auth/login`, dto);
  }

  logout() {
    localStorage.removeItem('token');
    this.router.navigate(['/login']);
  }

  register(dto: UserRegisterDto){
    return this.http.post(`${environment.apiBaseUrl}/Auth/register`, dto);
  }

  setToken(token?: string) {
    if (token) localStorage.setItem('token', token);
  }

  isAuthenticated(): boolean {
    return !!localStorage.getItem('token');
  }
}


