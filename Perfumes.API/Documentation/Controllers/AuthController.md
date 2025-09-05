# Auth Controller API Documentation

## نظرة عامة
يحتوي هذا ال Controller على جميع عمليات المصادقة وإدارة المستخدمين في النظام.

## Base URL
```
http://localhost:5036/api/auth
```

## Endpoints

### 1. تسجيل حساب جديد
**POST** `/api/auth/register`

**الوصف:** إنشاء حساب جديد للمستخدم.

**Body (JSON):**
```json
{
  "name": "أحمد محمد",
  "email": "ahmed@example.com",
  "password": "password123",
  "confirmPassword": "password123"
}
```

**الاستجابة:**
```json
{
  "user": {
    "userId": 1,
    "name": "أحمد محمد",
    "email": "ahmed@example.com",
    "role": "Customer",
    "isEmailVerified": false,
    "createdAt": "2024-01-01T00:00:00"
  },
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "refresh_token_here"
}
```

**مثال الاختبار:**
```bash
curl -X POST "http://localhost:5036/api/auth/register" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "أحمد محمد",
    "email": "ahmed@example.com",
    "password": "password123",
    "confirmPassword": "password123"
  }'
```

### 2. تسجيل الدخول
**POST** `/api/auth/login`

**الوصف:** تسجيل الدخول للحصول على token.

**Body (JSON):**
```json
{
  "email": "ahmed@example.com",
  "password": "password123"
}
```

**الاستجابة:**
```json
{
  "user": {
    "userId": 1,
    "name": "أحمد محمد",
    "email": "ahmed@example.com",
    "role": "Customer"
  },
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "refresh_token_here"
}
```

**مثال الاختبار:**
```bash
curl -X POST "http://localhost:5036/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "ahmed@example.com",
    "password": "password123"
  }'
```

### 3. تحديث Token
**POST** `/api/auth/refresh`

**الوصف:** تحديث الـ access token باستخدام refresh token.

**Body (JSON):**
```json
{
  "refreshToken": "refresh_token_here"
}
```

**مثال الاختبار:**
```bash
curl -X POST "http://localhost:5036/api/auth/refresh" \
  -H "Content-Type: application/json" \
  -d '{
    "refreshToken": "refresh_token_here"
  }'
```

### 4. تسجيل الخروج
**POST** `/api/auth/logout`

**الوصف:** تسجيل الخروج وإلغاء الـ refresh token.

**Headers:** `Authorization: Bearer {token}`

**Body (JSON):**
```json
{
  "refreshToken": "refresh_token_here"
}
```

**مثال الاختبار:**
```bash
curl -X POST "http://localhost:5036/api/auth/logout" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  -H "Content-Type: application/json" \
  -d '{
    "refreshToken": "refresh_token_here"
  }'
```

### 5. نسيان كلمة المرور
**POST** `/api/auth/forgot-password`

**الوصف:** إرسال رابط إعادة تعيين كلمة المرور.

**Body (JSON):**
```json
{
  "email": "ahmed@example.com"
}
```

**مثال الاختبار:**
```bash
curl -X POST "http://localhost:5036/api/auth/forgot-password" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "ahmed@example.com"
  }'
```

### 6. إعادة تعيين كلمة المرور
**POST** `/api/auth/reset-password`

**الوصف:** إعادة تعيين كلمة المرور باستخدام token.

**Body (JSON):**
```json
{
  "token": "reset_token_here",
  "newPassword": "newpassword123",
  "confirmPassword": "newpassword123"
}
```

**مثال الاختبار:**
```bash
curl -X POST "http://localhost:5036/api/auth/reset-password" \
  -H "Content-Type: application/json" \
  -d '{
    "token": "reset_token_here",
    "newPassword": "newpassword123",
    "confirmPassword": "newpassword123"
  }'
```

### 7. التحقق من البريد الإلكتروني
**POST** `/api/auth/verify-email?token={token}`

**الوصف:** التحقق من البريد الإلكتروني باستخدام token.

**مثال الاختبار:**
```bash
curl -X POST "http://localhost:5036/api/auth/verify-email?token=verification_token_here"
```

### 8. إعادة إرسال رسالة التحقق
**POST** `/api/auth/resend-verification`

**الوصف:** إعادة إرسال رسالة التحقق من البريد الإلكتروني.

**Headers:** `Authorization: Bearer {token}`

**مثال الاختبار:**
```bash
curl -X POST "http://localhost:5036/api/auth/resend-verification" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

### 9. الحصول على الملف الشخصي
**GET** `/api/auth/profile`

**الوصف:** الحصول على معلومات الملف الشخصي للمستخدم.

**Headers:** `Authorization: Bearer {token}`

**الاستجابة:**
```json
{
  "userId": 1,
  "name": "أحمد محمد",
  "email": "ahmed@example.com",
  "phone": "+201234567890",
  "address": "القاهرة، مصر",
  "role": "Customer",
  "isEmailVerified": true,
  "createdAt": "2024-01-01T00:00:00"
}
```

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/auth/profile" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

### 10. تحديث الملف الشخصي
**PUT** `/api/auth/profile`

**الوصف:** تحديث معلومات الملف الشخصي.

**Headers:** `Authorization: Bearer {token}`

**Body (JSON):**
```json
{
  "name": "أحمد محمد علي",
  "phone": "+201234567890",
  "address": "الإسكندرية، مصر"
}
```

**مثال الاختبار:**
```bash
curl -X PUT "http://localhost:5036/api/auth/profile" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "أحمد محمد علي",
    "phone": "+201234567890",
    "address": "الإسكندرية، مصر"
  }'
```

### 11. تغيير كلمة المرور
**POST** `/api/auth/change-password`

**الوصف:** تغيير كلمة المرور للمستخدم الحالي.

**Headers:** `Authorization: Bearer {token}`

**Body (JSON):**
```json
{
  "currentPassword": "oldpassword123",
  "newPassword": "newpassword123",
  "confirmPassword": "newpassword123"
}
```

**مثال الاختبار:**
```bash
curl -X POST "http://localhost:5036/api/auth/change-password" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  -H "Content-Type: application/json" \
  -d '{
    "currentPassword": "oldpassword123",
    "newPassword": "newpassword123",
    "confirmPassword": "newpassword123"
  }'
```

### 12. إنشاء مستخدم مدير (للتنمية فقط)
**POST** `/api/auth/create-admin`

**الوصف:** إنشاء حساب مدير (للتنمية فقط).

**Body (JSON):**
```json
{
  "name": "مدير النظام",
  "email": "admin@example.com",
  "password": "admin123"
}
```

**مثال الاختبار:**
```bash
curl -X POST "http://localhost:5036/api/auth/create-admin" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "مدير النظام",
    "email": "admin@example.com",
    "password": "admin123"
  }'
```

## أمثلة اختبار شاملة

### سيناريو كامل لإدارة المستخدم
```bash
# 1. تسجيل حساب جديد
curl -X POST "http://localhost:5036/api/auth/register" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "أحمد محمد",
    "email": "ahmed@example.com",
    "password": "password123",
    "confirmPassword": "password123"
  }'

# 2. تسجيل الدخول
curl -X POST "http://localhost:5036/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "ahmed@example.com",
    "password": "password123"
  }'

# 3. الحصول على الملف الشخصي
curl -X GET "http://localhost:5036/api/auth/profile" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"

# 4. تحديث الملف الشخصي
curl -X PUT "http://localhost:5036/api/auth/profile" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "أحمد محمد علي",
    "phone": "+201234567890",
    "address": "القاهرة، مصر"
  }'

# 5. تغيير كلمة المرور
curl -X POST "http://localhost:5036/api/auth/change-password" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  -H "Content-Type: application/json" \
  -d '{
    "currentPassword": "password123",
    "newPassword": "newpassword123",
    "confirmPassword": "newpassword123"
  }'

# 6. تسجيل الخروج
curl -X POST "http://localhost:5036/api/auth/logout" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  -H "Content-Type: application/json" \
  -d '{
    "refreshToken": "refresh_token_here"
  }'
```

## رموز الاستجابة

| الكود | الوصف |
|-------|--------|
| 200 | نجح الطلب |
| 201 | تم إنشاء الحساب بنجاح |
| 400 | خطأ في البيانات المرسلة |
| 401 | غير مصرح أو بيانات غير صحيحة |
| 404 | المستخدم غير موجود |
| 409 | البريد الإلكتروني مستخدم بالفعل |
| 500 | خطأ في الخادم |

## ملاحظات مهمة
- جميع الطلبات المحمية تتطلب `Authorization: Bearer {token}`
- تأكد من صحة كلمة المرور (8 أحرف على الأقل)
- تأكد من تطابق كلمة المرور وتأكيدها
- استخدم بريد إلكتروني صحيح للتحقق
- احتفظ بـ refresh token آمناً
- لا تشارك الـ tokens مع أي شخص 