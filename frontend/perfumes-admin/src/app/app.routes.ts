import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'store' },
  {
    path: 'login',
    loadComponent: () => import('./features/auth/login/login.component').then(m => m.LoginComponent)
  },
  {
    path: 'register',
    loadComponent: () => import('./features/auth/register/register.component').then(m => m.RegisterComponent)
  },
  {
    path: 'dashboard',
    loadComponent: () => import('./features/admin/layout/admin-layout.component').then(m => m.AdminLayoutComponent),
    canActivate: [() => import('./core/guards/admin.guard').then(m => m.adminGuard)],
    children: [
      {
        path: '',
        loadComponent: () => import('./features/admin/dashboard/dashboard.component').then(m => m.DashboardComponent)
      },
      {
        path: 'orders',
        loadComponent: () => import('./features/admin/orders/orders-list.component').then(m => m.OrdersListComponent)
      },
      {
        path: 'orders/:id',
        loadComponent: () => import('./features/admin/orders/order-detail.component').then(m => m.OrderDetailComponent)
      },
      {
        path: 'brands',
        loadComponent: () => import('./features/admin/brands/brands-list.component').then(m => m.BrandsListComponent)
      },
      {
        path: 'brands/new',
        loadComponent: () => import('./features/admin/brands/brand-form.component').then(m => m.BrandFormComponent)
      },
      {
        path: 'brands/:id',
        loadComponent: () => import('./features/admin/brands/brand-form.component').then(m => m.BrandFormComponent)
      },
      {
        path: 'categories',
        loadComponent: () => import('./features/admin/categories/categories-list.component').then(m => m.CategoriesListComponent)
      },
      {
        path: 'categories/new',
        loadComponent: () => import('./features/admin/categories/category-form.component').then(m => m.CategoryFormComponent)
      },
      {
        path: 'categories/:id',
        loadComponent: () => import('./features/admin/categories/category-form.component').then(m => m.CategoryFormComponent)
      },
      {
        path: 'products',
        loadComponent: () => import('./features/admin/products/products-list.component').then(m => m.ProductsListComponent)
      },
      {
        path: 'products/new',
        loadComponent: () => import('./features/admin/products/product-form.component').then(m => m.ProductFormComponent)
      },
      {
        path: 'products/:id',
        loadComponent: () => import('./features/admin/products/product-form.component').then(m => m.ProductFormComponent)
      },
      {
        path: 'users',
        loadComponent: () => import('./features/admin/users/users.component').then(m => m.UsersComponent)
      },
      {
        path: 'users/:id',
        loadComponent: () => import('./features/admin/users/user-edit.component').then(m => m.UserEditComponent)
      }
    ]
  },
  {
    path: 'store',
    loadComponent: () => import('./features/store/store-layout.component').then(m => m.StoreLayoutComponent),
    children: [
      { path: '', loadComponent: () => import('./features/store/shop.component').then(m => m.ShopComponent) },
      { path: 'wishlist', loadComponent: () => import('./features/store/wishlist.component').then(m => m.WishlistComponent), canActivate: [() => import('./core/guards/auth.guard').then(m => m.authGuard)] },
      { path: 'cart', loadComponent: () => import('./features/store/cart.component').then(m => m.CartComponent), canActivate: [() => import('./core/guards/auth.guard').then(m => m.authGuard)] },
      { path: 'checkout', loadComponent: () => import('./features/store/checkout.component').then(m => m.CheckoutComponent), canActivate: [() => import('./core/guards/auth.guard').then(m => m.authGuard)] },
      { path: 'payment-result', loadComponent: () => import('./features/store/payment-result.component').then(m => m.PaymentResultComponent) }
    ]
  },
  { path: '**', redirectTo: 'store' }
];
