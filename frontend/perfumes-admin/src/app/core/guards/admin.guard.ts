import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

function getRoleFromToken(token: string | null): string | null {
  if (!token) return null;
  try {
    const payload = JSON.parse(atob(token.split('.')[1] || ''));
    return (
      payload['role'] ||
      payload['Role'] ||
      payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] ||
      null
    );
  } catch {
    return null;
  }
}

export const adminGuard: CanActivateFn = (_route, state) => {
  const router = inject(Router);
  const token = localStorage.getItem('token');
  const role = getRoleFromToken(token);
  if (token && role === 'Admin') return true;
  if (token && role && role !== 'Admin') {
    return router.createUrlTree(['/store']);
  }
  // بدون توكن → نرجّعه للّوجين
  return router.createUrlTree(['/login'], { queryParams: { returnUrl: state.url } });
};



