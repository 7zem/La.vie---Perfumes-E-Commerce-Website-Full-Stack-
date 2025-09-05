# Brand Controller API Documentation

## نظرة عامة
يحتوي هذا ال Controller على جميع العمليات المتعلقة بإدارة ماركات العطور في النظام.

## Base URL
```
http://localhost:5036/api/brand
```

## Endpoints

### 1. الحصول على جميع الماركات
**GET** `/api/brand`

**الوصف:** الحصول على قائمة جميع الماركات المتاحة.

**الاستجابة:**
```json
[
  {
    "brandId": 1,
    "name": "شانيل",
    "description": "ماركة فرنسية فاخرة",
    "logoUrl": "https://example.com/chanel.jpg",
    "website": "https://www.chanel.com",
    "isActive": true,
    "createdAt": "2024-01-01T00:00:00"
  },
  {
    "brandId": 2,
    "name": "ديور",
    "description": "ماركة فرنسية فاخرة",
    "logoUrl": "https://example.com/dior.jpg",
    "website": "https://www.dior.com",
    "isActive": true,
    "createdAt": "2024-01-01T00:00:00"
  }
]
```

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/brand"
```

### 2. الحصول على ماركة بواسطة ID
**GET** `/api/brand/{id}`

**الوصف:** الحصول على ماركة محددة بواسطة الـ ID.

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/brand/1"
```

### 3. إنشاء ماركة جديدة
**POST** `/api/brand`

**الوصف:** إنشاء ماركة جديدة في النظام.

**Body (Form Data):**
```
name: اسم الماركة
description: وصف الماركة
website: موقع الويب
isActive: نشط أم لا
logo: ملف الشعار (اختياري)
```

**مثال الاختبار:**
```bash
curl -X POST "http://localhost:5036/api/brand" \
  -F "name=ماركة جديدة" \
  -F "description=ماركة عطور فاخرة" \
  -F "website=https://www.newbrand.com" \
  -F "isActive=true"
```

### 4. تحديث ماركة
**PUT** `/api/brand/{id}`

**الوصف:** تحديث بيانات ماركة موجودة.

**Body (Form Data):**
```
name: اسم الماركة
description: وصف الماركة
website: موقع الويب
isActive: نشط أم لا
logo: ملف الشعار (اختياري)
```

**مثال الاختبار:**
```bash
curl -X PUT "http://localhost:5036/api/brand/1" \
  -F "name=ماركة محدثة" \
  -F "description=ماركة عطور فاخرة محدثة" \
  -F "website=https://www.updatedbrand.com" \
  -F "isActive=true"
```

### 5. حذف ماركة
**DELETE** `/api/brand/{id}`

**الوصف:** حذف ماركة من النظام.

**مثال الاختبار:**
```bash
curl -X DELETE "http://localhost:5036/api/brand/1"
```

### 6. الحصول على الماركات النشطة
**GET** `/api/brand/active`

**الوصف:** الحصول على الماركات النشطة فقط.

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/brand/active"
```

### 7. البحث في الماركات
**GET** `/api/brand/search?term={searchTerm}`

**الوصف:** البحث في الماركات حسب الاسم أو الوصف.

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/brand/search?term=شانيل"
```

### 8. الحصول على الماركات مع عدد المنتجات
**GET** `/api/brand/with-count`

**الوصف:** الحصول على الماركات مع عدد المنتجات لكل ماركة.

**الاستجابة:**
```json
[
  {
    "brandId": 1,
    "name": "شانيل",
    "description": "ماركة فرنسية فاخرة",
    "logoUrl": "https://example.com/chanel.jpg",
    "website": "https://www.chanel.com",
    "isActive": true,
    "createdAt": "2024-01-01T00:00:00",
    "productCount": 15
  }
]
```

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/brand/with-count"
```

### 9. الحصول على ماركة مع منتجاتها
**GET** `/api/brand/{id}/products`

**الوصف:** الحصول على ماركة مع جميع المنتجات التابعة لها.

**الاستجابة:**
```json
{
  "brandId": 1,
  "name": "شانيل",
  "description": "ماركة فرنسية فاخرة",
  "logoUrl": "https://example.com/chanel.jpg",
  "website": "https://www.chanel.com",
  "isActive": true,
  "createdAt": "2024-01-01T00:00:00",
  "products": [
    {
      "productId": 1,
      "name": "عطر شانيل فاخر",
      "price": 200.00,
      "imageUrl": "https://example.com/perfume1.jpg"
    }
  ]
}
```

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/brand/1/products"
```

## أمثلة اختبار شاملة

### سيناريو كامل لإدارة الماركات
```bash
# 1. إنشاء ماركة جديدة
curl -X POST "http://localhost:5036/api/brand" \
  -F "name=ماركة فاخرة" \
  -F "description=ماركة عطور فاخرة من أوروبا" \
  -F "website=https://www.luxurybrand.com" \
  -F "isActive=true"

# 2. الحصول على الماركة المنشأة
curl -X GET "http://localhost:5036/api/brand/1"

# 3. تحديث الماركة
curl -X PUT "http://localhost:5036/api/brand/1" \
  -F "name=ماركة فاخرة محدثة" \
  -F "description=ماركة عطور فاخرة محدثة من أوروبا" \
  -F "website=https://www.updatedluxurybrand.com" \
  -F "isActive=true"

# 4. البحث عن الماركة
curl -X GET "http://localhost:5036/api/brand/search?term=فاخرة"

# 5. الحصول على الماركة مع منتجاتها
curl -X GET "http://localhost:5036/api/brand/1/products"

# 6. الحصول على الماركات مع عدد المنتجات
curl -X GET "http://localhost:5036/api/brand/with-count"

# 7. حذف الماركة
curl -X DELETE "http://localhost:5036/api/brand/1"
```

## رموز الاستجابة

| الكود | الوصف |
|-------|--------|
| 200 | نجح الطلب |
| 201 | تم إنشاء الماركة بنجاح |
| 204 | تم التحديث/الحذف بنجاح |
| 400 | خطأ في البيانات المرسلة |
| 404 | الماركة غير موجودة |
| 500 | خطأ في الخادم |

## ملاحظات مهمة
- عند رفع الشعار، استخدم `multipart/form-data`
- تأكد من صحة الـ website URL
- لا يمكن حذف ماركة تحتوي على منتجات
- استخدم البحث للعثور على ماركات محددة
- تأكد من صحة الـ logoUrl عند الإرسال 