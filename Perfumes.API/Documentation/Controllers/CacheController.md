# Cache Controller API Documentation

## نظرة عامة
يحتوي هذا ال Controller على جميع العمليات المتعلقة بإدارة التخزين المؤقت (Redis Cache) في النظام. يتطلب صلاحيات مدير (Admin).

## Base URL
```
http://localhost:5036/api/cache
```

## متطلبات المصادقة
جميع ال endpoints تتطلب:
- **Authorization:** `Bearer {admin_token}`
- **Role:** Admin

## Endpoints

### 1. الحصول على إحصائيات التخزين المؤقت
**GET** `/api/cache/stats`

**الوصف:** الحصول على إحصائيات قاعدة بيانات Redis.

**Headers:** `Authorization: Bearer {admin_token}`

**الاستجابة:**
```json
{
  "databaseSize": 1024,
  "timestamp": "2024-01-01T12:00:00Z"
}
```

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/cache/stats" \
  -H "Authorization: Bearer ADMIN_TOKEN_HERE"
```

### 2. الحصول على المفاتيح حسب النمط
**GET** `/api/cache/keys?pattern={pattern}`

**الوصف:** الحصول على قائمة المفاتيح حسب نمط معين.

**Headers:** `Authorization: Bearer {admin_token}`

**الاستجابة:**
```json
{
  "keys": [
    "products:1",
    "products:2",
    "categories:1",
    "brands:1"
  ]
}
```

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/cache/keys?pattern=products:*" \
  -H "Authorization: Bearer ADMIN_TOKEN_HERE"
```

### 3. الحصول على قيمة مفتاح
**GET** `/api/cache/keys/{key}`

**الوصف:** الحصول على قيمة مفتاح معين مع معلومات إضافية.

**Headers:** `Authorization: Bearer {admin_token}`

**الاستجابة:**
```json
{
  "key": "products:1",
  "value": {
    "productId": 1,
    "name": "عطر فاخر",
    "price": 150.00
  },
  "exists": true,
  "expiration": "2024-01-01T13:00:00Z"
}
```

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/cache/keys/products:1" \
  -H "Authorization: Bearer ADMIN_TOKEN_HERE"
```

### 4. حذف مفتاح
**DELETE** `/api/cache/keys/{key}`

**الوصف:** حذف مفتاح معين من التخزين المؤقت.

**Headers:** `Authorization: Bearer {admin_token}`

**مثال الاختبار:**
```bash
curl -X DELETE "http://localhost:5036/api/cache/keys/products:1" \
  -H "Authorization: Bearer ADMIN_TOKEN_HERE"
```

### 5. حذف مفاتيح حسب النمط
**DELETE** `/api/cache/pattern/{pattern}`

**الوصف:** حذف جميع المفاتيح التي تطابق نمط معين.

**Headers:** `Authorization: Bearer {admin_token}`

**مثال الاختبار:**
```bash
curl -X DELETE "http://localhost:5036/api/cache/pattern/products:*" \
  -H "Authorization: Bearer ADMIN_TOKEN_HERE"
```

### 6. تفريغ قاعدة البيانات
**POST** `/api/cache/flush`

**الوصف:** تفريغ جميع البيانات من قاعدة بيانات Redis.

**Headers:** `Authorization: Bearer {admin_token}`

**مثال الاختبار:**
```bash
curl -X POST "http://localhost:5036/api/cache/flush" \
  -H "Authorization: Bearer ADMIN_TOKEN_HERE"
```

### 7. إلغاء صلاحية تخزين المنتجات
**POST** `/api/cache/invalidate/products`

**الوصف:** حذف جميع البيانات المخزنة مؤقتاً للمنتجات.

**Headers:** `Authorization: Bearer {admin_token}`

**مثال الاختبار:**
```bash
curl -X POST "http://localhost:5036/api/cache/invalidate/products" \
  -H "Authorization: Bearer ADMIN_TOKEN_HERE"
```

### 8. إلغاء صلاحية تخزين الفئات
**POST** `/api/cache/invalidate/categories`

**الوصف:** حذف جميع البيانات المخزنة مؤقتاً للفئات.

**Headers:** `Authorization: Bearer {admin_token}`

**مثال الاختبار:**
```bash
curl -X POST "http://localhost:5036/api/cache/invalidate/categories" \
  -H "Authorization: Bearer ADMIN_TOKEN_HERE"
```

### 9. إلغاء صلاحية تخزين الماركات
**POST** `/api/cache/invalidate/brands`

**الوصف:** حذف جميع البيانات المخزنة مؤقتاً للماركات.

**Headers:** `Authorization: Bearer {admin_token}`

**مثال الاختبار:**
```bash
curl -X POST "http://localhost:5036/api/cache/invalidate/brands" \
  -H "Authorization: Bearer ADMIN_TOKEN_HERE"
```

### 10. إلغاء صلاحية تخزين لوحة التحكم
**POST** `/api/cache/invalidate/dashboard`

**الوصف:** حذف جميع البيانات المخزنة مؤقتاً للوحة التحكم.

**Headers:** `Authorization: Bearer {admin_token}`

**مثال الاختبار:**
```bash
curl -X POST "http://localhost:5036/api/cache/invalidate/dashboard" \
  -H "Authorization: Bearer ADMIN_TOKEN_HERE"
```

### 11. إلغاء صلاحية جميع التخزين المؤقت
**POST** `/api/cache/invalidate/all`

**الوصف:** حذف جميع البيانات المخزنة مؤقتاً.

**Headers:** `Authorization: Bearer {admin_token}`

**مثال الاختبار:**
```bash
curl -X POST "http://localhost:5036/api/cache/invalidate/all" \
  -H "Authorization: Bearer ADMIN_TOKEN_HERE"
```

### 12. اختبار التخزين المؤقت
**POST** `/api/cache/test`

**الوصف:** اختبار وظائف التخزين المؤقت (إضافة، قراءة، حذف).

**Headers:** `Authorization: Bearer {admin_token}`

**الاستجابة:**
```json
{
  "testKey": "test:cache",
  "setValue": {
    "message": "Cache is working!",
    "timestamp": "2024-01-01T12:00:00Z"
  },
  "retrievedValue": {
    "message": "Cache is working!",
    "timestamp": "2024-01-01T12:00:00Z"
  },
  "exists": true,
  "expiration": "2024-01-01T12:05:00Z",
  "status": "Cache test completed successfully"
}
```

**مثال الاختبار:**
```bash
curl -X POST "http://localhost:5036/api/cache/test" \
  -H "Authorization: Bearer ADMIN_TOKEN_HERE"
```

## أمثلة اختبار شاملة

### سيناريو كامل لإدارة التخزين المؤقت
```bash
# 1. الحصول على إحصائيات التخزين المؤقت
curl -X GET "http://localhost:5036/api/cache/stats" \
  -H "Authorization: Bearer ADMIN_TOKEN_HERE"

# 2. اختبار التخزين المؤقت
curl -X POST "http://localhost:5036/api/cache/test" \
  -H "Authorization: Bearer ADMIN_TOKEN_HERE"

# 3. الحصول على جميع المفاتيح
curl -X GET "http://localhost:5036/api/cache/keys" \
  -H "Authorization: Bearer ADMIN_TOKEN_HERE"

# 4. الحصول على مفاتيح المنتجات
curl -X GET "http://localhost:5036/api/cache/keys?pattern=products:*" \
  -H "Authorization: Bearer ADMIN_TOKEN_HERE"

# 5. الحصول على قيمة مفتاح معين
curl -X GET "http://localhost:5036/api/cache/keys/products:1" \
  -H "Authorization: Bearer ADMIN_TOKEN_HERE"

# 6. إلغاء صلاحية تخزين المنتجات
curl -X POST "http://localhost:5036/api/cache/invalidate/products" \
  -H "Authorization: Bearer ADMIN_TOKEN_HERE"

# 7. إلغاء صلاحية تخزين الفئات
curl -X POST "http://localhost:5036/api/cache/invalidate/categories" \
  -H "Authorization: Bearer ADMIN_TOKEN_HERE"

# 8. حذف مفتاح معين
curl -X DELETE "http://localhost:5036/api/cache/keys/test:cache" \
  -H "Authorization: Bearer ADMIN_TOKEN_HERE"

# 9. حذف مفاتيح حسب النمط
curl -X DELETE "http://localhost:5036/api/cache/pattern/temp:*" \
  -H "Authorization: Bearer ADMIN_TOKEN_HERE"

# 10. تفريغ قاعدة البيانات
curl -X POST "http://localhost:5036/api/cache/flush" \
  -H "Authorization: Bearer ADMIN_TOKEN_HERE"
```

## أنماط المفاتيح الشائعة

| النمط | الوصف |
|-------|--------|
| `products:*` | جميع مفاتيح المنتجات |
| `categories:*` | جميع مفاتيح الفئات |
| `brands:*` | جميع مفاتيح الماركات |
| `dashboard:*` | جميع مفاتيح لوحة التحكم |
| `users:*` | جميع مفاتيح المستخدمين |
| `orders:*` | جميع مفاتيح الطلبات |

## رموز الاستجابة

| الكود | الوصف |
|-------|--------|
| 200 | نجح الطلب |
| 400 | خطأ في البيانات المرسلة |
| 401 | غير مصرح (غير مدير) |
| 403 | ممنوع (غير مدير) |
| 404 | المفتاح غير موجود |
| 500 | خطأ في الخادم |

## ملاحظات مهمة
- جميع العمليات تتطلب صلاحيات مدير
- احرص على التأكد من صحة الـ token قبل الاستخدام
- احرص على عدم حذف بيانات مهمة عن طريق الخطأ
- استخدم أنماط دقيقة عند حذف المفاتيح
- احرص على اختبار التخزين المؤقت قبل الاستخدام
- تأكد من صحة اتصال Redis 