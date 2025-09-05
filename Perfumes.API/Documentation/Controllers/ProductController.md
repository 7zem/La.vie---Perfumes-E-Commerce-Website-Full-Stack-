# Product Controller API Documentation

## نظرة عامة
يحتوي هذا ال Controller على جميع العمليات المتعلقة بإدارة المنتجات (العطور) في النظام.

## Base URL
```
http://localhost:5036/api/product
```

## Endpoints

### 1. الحصول على جميع المنتجات
**GET** `/api/product`

**الوصف:** الحصول على قائمة جميع المنتجات المتاحة.

**الاستجابة:**
```json
[
  {
    "productId": 1,
    "name": "عطر فاخر",
    "description": "عطر رجالي فاخر",
    "sku": "PERF001",
    "price": 150.00,
    "stock": 50,
    "imageUrl": "https://example.com/image.jpg",
    "brandName": "شانيل",
    "categoryName": "عطور رجالية",
    "weight": 100.0,
    "dimensions": "10x5x3",
    "fragranceNotes": "خشب، فانيليا",
    "concentration": "Eau de Parfum",
    "volume": "100ml",
    "gender": "Male",
    "season": "All",
    "longevity": "8-10 hours",
    "sillage": "Moderate",
    "isActive": true,
    "createdAt": "2024-01-01T00:00:00",
    "averageRating": 4.5,
    "reviewsCount": 25
  }
]
```

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/product" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

### 2. الحصول على منتج بواسطة ID
**GET** `/api/product/{id}`

**الوصف:** الحصول على منتج محدد بواسطة الـ ID.

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/product/1" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

### 3. إنشاء منتج جديد
**POST** `/api/product`

**الوصف:** إنشاء منتج جديد في النظام.

**Body (Form Data):**
```
name: اسم المنتج
description: وصف المنتج
sku: رمز المنتج
price: السعر
stock: الكمية المتاحة
brandId: معرف الماركة
categoryId: معرف الفئة
weight: الوزن
dimensions: الأبعاد
fragranceNotes: نوتات العطر
concentration: التركيز
volume: الحجم
gender: الجنس
season: الموسم
longevity: مدة البقاء
sillage: قوة الانتشار
isActive: نشط أم لا
image: ملف الصورة (اختياري)
```

**مثال الاختبار:**
```bash
curl -X POST "http://localhost:5036/api/product" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  -F "name=عطر جديد" \
  -F "description=عطر رجالي فاخر" \
  -F "sku=PERF002" \
  -F "price=200.00" \
  -F "stock=25" \
  -F "brandId=1" \
  -F "categoryId=1" \
  -F "weight=100.0" \
  -F "dimensions=10x5x3" \
  -F "fragranceNotes=خشب، فانيليا، مسك" \
  -F "concentration=Eau de Parfum" \
  -F "volume=100ml" \
  -F "gender=Male" \
  -F "season=All" \
  -F "longevity=8-10 hours" \
  -F "sillage=Moderate" \
  -F "isActive=true"
```

### 4. تحديث منتج
**PUT** `/api/product/{id}`

**الوصف:** تحديث بيانات منتج موجود.

**مثال الاختبار:**
```bash
curl -X PUT "http://localhost:5036/api/product/1" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  -F "name=عطر محدث" \
  -F "price=180.00" \
  -F "stock=30"
```

### 5. حذف منتج
**DELETE** `/api/product/{id}`

**الوصف:** حذف منتج من النظام.

**مثال الاختبار:**
```bash
curl -X DELETE "http://localhost:5036/api/product/1" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

### 6. الحصول على منتج مع التقييمات
**GET** `/api/product/{id}/reviews`

**الوصف:** الحصول على منتج مع جميع التقييمات الخاصة به.

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/product/1/reviews" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

### 7. الحصول على منتجات حسب الفئة
**GET** `/api/product/category/{categoryId}`

**الوصف:** الحصول على جميع المنتجات في فئة معينة.

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/product/category/1" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

### 8. الحصول على منتجات حسب الماركة
**GET** `/api/product/brand/{brandId}`

**الوصف:** الحصول على جميع المنتجات لماركة معينة.

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/product/brand/1" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

### 9. الحصول على منتجات حسب الجنس
**GET** `/api/product/gender/{gender}`

**الوصف:** الحصول على منتجات حسب الجنس (Male/Female/Unisex).

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/product/gender/Male" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

### 10. الحصول على منتجات حسب السعر
**GET** `/api/product/price?min={min}&max={max}`

**الوصف:** الحصول على منتجات في نطاق سعر معين.

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/product/price?min=100&max=300" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

### 11. الحصول على المنتجات النشطة
**GET** `/api/product/active`

**الوصف:** الحصول على المنتجات النشطة فقط.

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/product/active" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

### 12. الحصول على المنتجات المميزة
**GET** `/api/product/featured`

**الوصف:** الحصول على المنتجات المميزة.

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/product/featured" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

### 13. الحصول على المنتجات الجديدة
**GET** `/api/product/new`

**الوصف:** الحصول على المنتجات الجديدة.

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/product/new" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

### 14. الحصول على أفضل المبيعات
**GET** `/api/product/best`

**الوصف:** الحصول على المنتجات الأكثر مبيعاً.

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/product/best" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

### 15. البحث في المنتجات
**GET** `/api/product/search?term={searchTerm}`

**الوصف:** البحث في المنتجات حسب الاسم أو الوصف.

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/product/search?term=عطر" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

## أمثلة اختبار شاملة

### سيناريو كامل لإدارة منتج
```bash
# 1. إنشاء منتج جديد
curl -X POST "http://localhost:5036/api/product" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  -F "name=عطر فاخر جديد" \
  -F "description=عطر رجالي فاخر من أفضل الماركات" \
  -F "sku=PERF003" \
  -F "price=250.00" \
  -F "stock=15" \
  -F "brandId=1" \
  -F "categoryId=1" \
  -F "gender=Male" \
  -F "isActive=true"

# 2. الحصول على المنتج المنشأ
curl -X GET "http://localhost:5036/api/product/1" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"

# 3. تحديث المنتج
curl -X PUT "http://localhost:5036/api/product/1" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  -F "price=275.00" \
  -F "stock=20"

# 4. البحث عن المنتج
curl -X GET "http://localhost:5036/api/product/search?term=فاخر" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"

# 5. حذف المنتج
curl -X DELETE "http://localhost:5036/api/product/1" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

## رموز الاستجابة

| الكود | الوصف |
|-------|--------|
| 200 | نجح الطلب |
| 201 | تم إنشاء المنتج بنجاح |
| 204 | تم الحذف بنجاح |
| 400 | خطأ في البيانات المرسلة |
| 401 | غير مصرح |
| 404 | المنتج غير موجود |
| 500 | خطأ في الخادم |

## ملاحظات مهمة
- جميع الطلبات تتطلب مصادقة باستخدام JWT Token
- عند رفع الصور، استخدم `multipart/form-data`
- تأكد من صحة البيانات المرسلة قبل الإرسال
- استخدم الترميز UTF-8 للدعم العربي 