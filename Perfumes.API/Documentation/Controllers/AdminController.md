# Admin Controller API Documentation

## نظرة عامة
يحتوي هذا ال Controller على جميع العمليات الإدارية لإدارة المستخدمين في النظام. يتطلب صلاحيات مدير (Admin).

## Base URL
```
http://localhost:5036/api/admin
```

## متطلبات المصادقة
جميع ال endpoints تتطلب:
- **Authorization:** `Bearer {admin_token}`
- **Role:** Admin

## Endpoints

### 1. الحصول على جميع المستخدمين
**GET** `/api/admin/users`

**الوصف:** الحصول على قائمة جميع المستخدمين في النظام.

**Headers:** `Authorization: Bearer {admin_token}`

**الاستجابة:**
```json
[
  {
    "userId": 1,
    "name": "أحمد محمد",
    "email": "ahmed@example.com",
    "role": "Customer",
    "isEmailVerified": true,
    "isBlocked": false,
    "createdAt": "2024-01-01T00:00:00"
  },
  {
    "userId": 2,
    "name": "مدير النظام",
    "email": "admin@example.com",
    "role": "Admin",
    "isEmailVerified": true,
    "isBlocked": false,
    "createdAt": "2024-01-01T00:00:00"
  }
]
```

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/admin/users" \
  -H "Authorization: Bearer ADMIN_TOKEN_HERE"
```

### 2. الحصول على مستخدم بواسطة ID
**GET** `/api/admin/users/{id}`

**الوصف:** الحصول على معلومات مستخدم محدد.

**Headers:** `Authorization: Bearer {admin_token}`

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/admin/users/1" \
  -H "Authorization: Bearer ADMIN_TOKEN_HERE"
```

### 3. تحديث دور المستخدم
**PUT** `/api/admin/users/{id}/role`

**الوصف:** تغيير دور المستخدم (Customer/Admin).

**Headers:** `Authorization: Bearer {admin_token}`

**Body (JSON):**
```json
{
  "role": "Admin"
}
```

**مثال الاختبار:**
```bash
curl -X PUT "http://localhost:5036/api/admin/users/1/role" \
  -H "Authorization: Bearer ADMIN_TOKEN_HERE" \
  -H "Content-Type: application/json" \
  -d '{
    "role": "Admin"
  }'
```

### 4. حظر مستخدم
**POST** `/api/admin/users/{id}/block`

**الوصف:** حظر مستخدم من النظام.

**Headers:** `Authorization: Bearer {admin_token}`

**مثال الاختبار:**
```bash
curl -X POST "http://localhost:5036/api/admin/users/1/block" \
  -H "Authorization: Bearer ADMIN_TOKEN_HERE"
```

### 5. إلغاء حظر مستخدم
**POST** `/api/admin/users/{id}/unblock`

**الوصف:** إلغاء حظر مستخدم محظور.

**Headers:** `Authorization: Bearer {admin_token}`

**مثال الاختبار:**
```bash
curl -X POST "http://localhost:5036/api/admin/users/1/unblock" \
  -H "Authorization: Bearer ADMIN_TOKEN_HERE"
```

### 6. حذف مستخدم
**DELETE** `/api/admin/users/{id}`

**الوصف:** حذف حساب مستخدم من النظام نهائياً.

**Headers:** `Authorization: Bearer {admin_token}`

**مثال الاختبار:**
```bash
curl -X DELETE "http://localhost:5036/api/admin/users/1" \
  -H "Authorization: Bearer ADMIN_TOKEN_HERE"
```

## أمثلة اختبار شاملة

### سيناريو كامل لإدارة المستخدمين
```bash
# 1. الحصول على جميع المستخدمين
curl -X GET "http://localhost:5036/api/admin/users" \
  -H "Authorization: Bearer ADMIN_TOKEN_HERE"

# 2. الحصول على مستخدم محدد
curl -X GET "http://localhost:5036/api/admin/users/1" \
  -H "Authorization: Bearer ADMIN_TOKEN_HERE"

# 3. ترقية مستخدم إلى مدير
curl -X PUT "http://localhost:5036/api/admin/users/1/role" \
  -H "Authorization: Bearer ADMIN_TOKEN_HERE" \
  -H "Content-Type: application/json" \
  -d '{
    "role": "Admin"
  }'

# 4. حظر مستخدم
curl -X POST "http://localhost:5036/api/admin/users/2/block" \
  -H "Authorization: Bearer ADMIN_TOKEN_HERE"

# 5. إلغاء حظر مستخدم
curl -X POST "http://localhost:5036/api/admin/users/2/unblock" \
  -H "Authorization: Bearer ADMIN_TOKEN_HERE"

# 6. حذف مستخدم
curl -X DELETE "http://localhost:5036/api/admin/users/3" \
  -H "Authorization: Bearer ADMIN_TOKEN_HERE"
```

## رموز الاستجابة

| الكود | الوصف |
|-------|--------|
| 200 | نجح الطلب |
| 400 | خطأ في البيانات المرسلة |
| 401 | غير مصرح (غير مدير) |
| 403 | ممنوع (غير مدير) |
| 404 | المستخدم غير موجود |
| 500 | خطأ في الخادم |

## ملاحظات مهمة
- جميع العمليات تتطلب صلاحيات مدير
- تأكد من صحة الـ token قبل الاستخدام
- احرص على التأكد من هوية المستخدم قبل الحذف
- يمكن حظر المستخدمين بدلاً من حذفهم للحفاظ على البيانات
- تأكد من صحة الدور المراد تعيينه (Customer/Admin) 