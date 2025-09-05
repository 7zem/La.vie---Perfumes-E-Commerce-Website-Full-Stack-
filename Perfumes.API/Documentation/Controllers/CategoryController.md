# Category Controller API Documentation

## نظرة عامة
يحتوي هذا ال Controller على جميع العمليات المتعلقة بإدارة فئات المنتجات (العطور) في النظام.

## Base URL
```
http://localhost:5036/api/category
```

## Endpoints

### 1. الحصول على جميع الفئات
**GET** `/api/category`

**الوصف:** الحصول على قائمة جميع الفئات المتاحة.

**الاستجابة:**
```json
[
  {
    "categoryId": 1,
    "name": "عطور رجالية",
    "description": "عطور مخصصة للرجال",
    "imageUrl": "https://example.com/men.jpg",
    "isActive": true,
    "parentId": null,
    "createdAt": "2024-01-01T00:00:00"
  },
  {
    "categoryId": 2,
    "name": "عطور نسائية",
    "description": "عطور مخصصة للنساء",
    "imageUrl": "https://example.com/women.jpg",
    "isActive": true,
    "parentId": null,
    "createdAt": "2024-01-01T00:00:00"
  }
]
```

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/category"
```

### 2. الحصول على فئة بواسطة ID
**GET** `/api/category/{id}`

**الوصف:** الحصول على فئة محددة بواسطة الـ ID.

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/category/1"
```

### 3. إنشاء فئة جديدة
**POST** `/api/category`

**الوصف:** إنشاء فئة جديدة في النظام.

**Body (JSON):**
```json
{
  "name": "عطور فاخرة",
  "description": "عطور فاخرة من أفضل الماركات",
  "imageUrl": "https://example.com/luxury.jpg",
  "isActive": true,
  "parentId": null
}
```

**مثال الاختبار:**
```bash
curl -X POST "http://localhost:5036/api/category" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "عطور فاخرة",
    "description": "عطور فاخرة من أفضل الماركات",
    "imageUrl": "https://example.com/luxury.jpg",
    "isActive": true,
    "parentId": null
  }'
```

### 4. تحديث فئة
**PUT** `/api/category/{id}`

**الوصف:** تحديث بيانات فئة موجودة.

**Body (JSON):**
```json
{
  "name": "عطور فاخرة محدثة",
  "description": "عطور فاخرة محدثة من أفضل الماركات",
  "imageUrl": "https://example.com/luxury-updated.jpg",
  "isActive": true
}
```

**مثال الاختبار:**
```bash
curl -X PUT "http://localhost:5036/api/category/1" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "عطور فاخرة محدثة",
    "description": "عطور فاخرة محدثة من أفضل الماركات",
    "imageUrl": "https://example.com/luxury-updated.jpg",
    "isActive": true
  }'
```

### 5. حذف فئة
**DELETE** `/api/category/{id}`

**الوصف:** حذف فئة من النظام.

**مثال الاختبار:**
```bash
curl -X DELETE "http://localhost:5036/api/category/1"
```

### 6. الحصول على الفئات النشطة
**GET** `/api/category/active`

**الوصف:** الحصول على الفئات النشطة فقط.

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/category/active"
```

### 7. الحصول على الفئات الرئيسية
**GET** `/api/category/parents`

**الوصف:** الحصول على الفئات الرئيسية (بدون فئة أب).

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/category/parents"
```

### 8. الحصول على الفئات الفرعية
**GET** `/api/category/{parentId}/subcategories`

**الوصف:** الحصول على الفئات الفرعية لفئة معينة.

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/category/1/subcategories"
```

### 9. الحصول على فئة مع منتجاتها
**GET** `/api/category/{id}/products`

**الوصف:** الحصول على فئة مع جميع المنتجات التابعة لها.

**الاستجابة:**
```json
{
  "categoryId": 1,
  "name": "عطور رجالية",
  "description": "عطور مخصصة للرجال",
  "imageUrl": "https://example.com/men.jpg",
  "isActive": true,
  "parentId": null,
  "createdAt": "2024-01-01T00:00:00",
  "products": [
    {
      "productId": 1,
      "name": "عطر رجالي فاخر",
      "price": 150.00,
      "imageUrl": "https://example.com/perfume1.jpg"
    }
  ]
}
```

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/category/1/products"
```

### 10. الحصول على شجرة الفئات
**GET** `/api/category/tree`

**الوصف:** الحصول على هيكل شجري للفئات مع الفئات الفرعية.

**الاستجابة:**
```json
[
  {
    "categoryId": 1,
    "name": "عطور رجالية",
    "description": "عطور مخصصة للرجال",
    "imageUrl": "https://example.com/men.jpg",
    "isActive": true,
    "parentId": null,
    "createdAt": "2024-01-01T00:00:00",
    "subCategories": [
      {
        "categoryId": 3,
        "name": "عطور رجالية كلاسيكية",
        "description": "عطور رجالية كلاسيكية",
        "imageUrl": "https://example.com/classic.jpg",
        "isActive": true,
        "parentId": 1,
        "createdAt": "2024-01-01T00:00:00",
        "subCategories": []
      }
    ]
  }
]
```

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/category/tree"
```

## أمثلة اختبار شاملة

### سيناريو كامل لإدارة الفئات
```bash
# 1. إنشاء فئة رئيسية
curl -X POST "http://localhost:5036/api/category" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "عطور فاخرة",
    "description": "عطور فاخرة من أفضل الماركات",
    "imageUrl": "https://example.com/luxury.jpg",
    "isActive": true,
    "parentId": null
  }'

# 2. إنشاء فئة فرعية
curl -X POST "http://localhost:5036/api/category" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "عطور فاخرة كلاسيكية",
    "description": "عطور فاخرة كلاسيكية",
    "imageUrl": "https://example.com/classic.jpg",
    "isActive": true,
    "parentId": 1
  }'

# 3. الحصول على الفئة المنشأة
curl -X GET "http://localhost:5036/api/category/1"

# 4. تحديث الفئة
curl -X PUT "http://localhost:5036/api/category/1" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "عطور فاخرة محدثة",
    "description": "عطور فاخرة محدثة من أفضل الماركات",
    "imageUrl": "https://example.com/luxury-updated.jpg",
    "isActive": true
  }'

# 5. الحصول على الفئات الفرعية
curl -X GET "http://localhost:5036/api/category/1/subcategories"

# 6. الحصول على شجرة الفئات
curl -X GET "http://localhost:5036/api/category/tree"

# 7. حذف الفئة
curl -X DELETE "http://localhost:5036/api/category/1"
```

## رموز الاستجابة

| الكود | الوصف |
|-------|--------|
| 200 | نجح الطلب |
| 201 | تم إنشاء الفئة بنجاح |
| 204 | تم التحديث/الحذف بنجاح |
| 400 | خطأ في البيانات المرسلة |
| 404 | الفئة غير موجودة |
| 500 | خطأ في الخادم |

## ملاحظات مهمة
- يمكن إنشاء فئات رئيسية وفرعية
- تأكد من صحة الـ parentId عند إنشاء فئة فرعية
- لا يمكن حذف فئة تحتوي على منتجات
- استخدم الـ tree view للحصول على هيكل كامل للفئات
- تأكد من صحة الـ imageUrl عند الإرسال 