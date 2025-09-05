# Perfumes API Documentation

## نظرة عامة
هذا المشروع عبارة عن API لإدارة متجر العطور مع نظام مصادقة متكامل وإدارة للمنتجات والمستخدمين والطلبات.

## كيفية تشغيل المشروع
```bash
cd Perfumes.API
dotnet run
```

## Base URL
```
http://localhost:5036
```

## Swagger UI
```
http://localhost:5036/swagger
```

## المصادقة (Authentication)
معظم ال APIs تتطلب مصادقة باستخدام JWT Token. للحصول على token:

1. تسجيل حساب جديد: `POST /api/auth/register`
2. تسجيل الدخول: `POST /api/auth/login`
3. استخدام ال token في header: `Authorization: Bearer {token}`

## Controllers Documentation
- [ProductController](./Controllers/ProductController.md)
- [AuthController](./Controllers/AuthController.md)
- [AdminController](./Controllers/AdminController.md)
- [CategoryController](./Controllers/CategoryController.md)
- [BrandController](./Controllers/BrandController.md)
- [OrderController](./Controllers/OrderController.md)
- [CartController](./Controllers/CartController.md)
- [CouponController](./Controllers/CouponController.md)
- [DashboardController](./Controllers/DashboardController.md)
- [PaymentController](./Controllers/PaymentController.md)
- [EmailController](./Controllers/EmailController.md)
- [CacheController](./Controllers/CacheController.md)

## أمثلة على الاختبار باستخدام curl

### تسجيل الدخول
```bash
curl -X POST "http://localhost:5036/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "user@example.com",
    "password": "password123"
  }'
```

### الحصول على المنتجات
```bash
curl -X GET "http://localhost:5036/api/product" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

### إنشاء منتج جديد
```bash
curl -X POST "http://localhost:5036/api/product" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  -F "name=عطر جديد" \
  -F "description=وصف العطر" \
  -F "price=150.00" \
  -F "stock=10"
``` 