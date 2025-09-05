# Coupon Controller API Documentation

## نظرة عامة
يحتوي هذا ال Controller على جميع العمليات المتعلقة بإدارة كوبونات الخصم في النظام.

## Base URL
```
http://localhost:5036/api/coupon
```

## Endpoints

### 1. إنشاء كوبون جديد
**POST** `/api/coupon`

**الوصف:** إنشاء كوبون خصم جديد في النظام.

**Body (JSON):**
```json
{
  "code": "DISCOUNT20",
  "description": "خصم 20% على جميع المنتجات",
  "discountType": "Percentage",
  "discountValue": 20.0,
  "minimumAmount": 100.0,
  "maximumDiscount": 50.0,
  "startDate": "2024-01-01T00:00:00",
  "endDate": "2024-12-31T23:59:59",
  "maxUsage": 100,
  "isActive": true
}
```

**الاستجابة:**
```json
{
  "couponId": 1,
  "code": "DISCOUNT20",
  "description": "خصم 20% على جميع المنتجات",
  "discountType": "Percentage",
  "discountValue": 20.0,
  "minimumAmount": 100.0,
  "maximumDiscount": 50.0,
  "startDate": "2024-01-01T00:00:00",
  "endDate": "2024-12-31T23:59:59",
  "maxUsage": 100,
  "currentUsage": 0,
  "isActive": true,
  "createdAt": "2024-01-01T00:00:00"
}
```

**مثال الاختبار:**
```bash
curl -X POST "http://localhost:5036/api/coupon" \
  -H "Content-Type: application/json" \
  -d '{
    "code": "DISCOUNT20",
    "description": "خصم 20% على جميع المنتجات",
    "discountType": "Percentage",
    "discountValue": 20.0,
    "minimumAmount": 100.0,
    "maximumDiscount": 50.0,
    "startDate": "2024-01-01T00:00:00",
    "endDate": "2024-12-31T23:59:59",
    "maxUsage": 100,
    "isActive": true
  }'
```

### 2. تحديث كوبون
**PUT** `/api/coupon/{id}`

**الوصف:** تحديث بيانات كوبون موجود.

**Body (JSON):**
```json
{
  "code": "DISCOUNT25",
  "description": "خصم 25% محدث على جميع المنتجات",
  "discountType": "Percentage",
  "discountValue": 25.0,
  "minimumAmount": 150.0,
  "maximumDiscount": 75.0,
  "startDate": "2024-01-01T00:00:00",
  "endDate": "2024-12-31T23:59:59",
  "maxUsage": 200,
  "isActive": true
}
```

**مثال الاختبار:**
```bash
curl -X PUT "http://localhost:5036/api/coupon/1" \
  -H "Content-Type: application/json" \
  -d '{
    "code": "DISCOUNT25",
    "description": "خصم 25% محدث على جميع المنتجات",
    "discountType": "Percentage",
    "discountValue": 25.0,
    "minimumAmount": 150.0,
    "maximumDiscount": 75.0,
    "startDate": "2024-01-01T00:00:00",
    "endDate": "2024-12-31T23:59:59",
    "maxUsage": 200,
    "isActive": true
  }'
```

### 3. حذف كوبون
**DELETE** `/api/coupon/{id}`

**الوصف:** حذف كوبون من النظام.

**مثال الاختبار:**
```bash
curl -X DELETE "http://localhost:5036/api/coupon/1"
```

### 4. الحصول على جميع الكوبونات
**GET** `/api/coupon`

**الوصف:** الحصول على قائمة جميع الكوبونات.

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/coupon"
```

### 5. الحصول على كوبون بواسطة ID
**GET** `/api/coupon/{id}`

**الوصف:** الحصول على كوبون محدد بواسطة الـ ID.

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/coupon/1"
```

### 6. الحصول على كوبون بواسطة الكود
**GET** `/api/coupon/code/{code}`

**الوصف:** الحصول على كوبون بواسطة كود الخصم.

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/coupon/code/DISCOUNT20"
```

### 7. الحصول على الكوبونات النشطة
**GET** `/api/coupon/active`

**الوصف:** الحصول على الكوبونات النشطة فقط.

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/coupon/active"
```

### 8. الحصول على الكوبونات الصالحة
**GET** `/api/coupon/valid`

**الوصف:** الحصول على الكوبونات الصالحة للاستخدام حالياً.

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/coupon/valid"
```

### 9. الحصول على الكوبونات المنتهية الصلاحية
**GET** `/api/coupon/expired`

**الوصف:** الحصول على الكوبونات المنتهية الصلاحية.

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/coupon/expired"
```

### 10. الحصول على كوبونات حسب النطاق الزمني
**GET** `/api/coupon/range?start={startDate}&end={endDate}`

**الوصف:** الحصول على كوبونات في نطاق زمني محدد.

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/coupon/range?start=2024-01-01&end=2024-12-31"
```

### 11. التحقق من إمكانية استخدام كوبون
**GET** `/api/coupon/can-use/{code}`

**الوصف:** التحقق من إمكانية استخدام كوبون معين.

**الاستجابة:**
```json
{
  "canUse": true
}
```

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/coupon/can-use/DISCOUNT20"
```

### 12. تطبيق كوبون
**POST** `/api/coupon/apply`

**الوصف:** تطبيق كوبون على طلب معين.

**Body (JSON):**
```json
{
  "code": "DISCOUNT20",
  "orderId": 1,
  "userId": 1,
  "orderAmount": 200.0
}
```

**الاستجابة:**
```json
{
  "applied": true,
  "discountAmount": 40.0,
  "finalAmount": 160.0,
  "message": "تم تطبيق الكوبون بنجاح"
}
```

**مثال الاختبار:**
```bash
curl -X POST "http://localhost:5036/api/coupon/apply" \
  -H "Content-Type: application/json" \
  -d '{
    "code": "DISCOUNT20",
    "orderId": 1,
    "userId": 1,
    "orderAmount": 200.0
  }'
```

## أمثلة اختبار شاملة

### سيناريو كامل لإدارة الكوبونات
```bash
# 1. إنشاء كوبون جديد
curl -X POST "http://localhost:5036/api/coupon" \
  -H "Content-Type: application/json" \
  -d '{
    "code": "SUMMER2024",
    "description": "خصم صيفي 30%",
    "discountType": "Percentage",
    "discountValue": 30.0,
    "minimumAmount": 200.0,
    "maximumDiscount": 100.0,
    "startDate": "2024-06-01T00:00:00",
    "endDate": "2024-08-31T23:59:59",
    "maxUsage": 500,
    "isActive": true
  }'

# 2. الحصول على الكوبون المنشأ
curl -X GET "http://localhost:5036/api/coupon/1"

# 3. التحقق من إمكانية الاستخدام
curl -X GET "http://localhost:5036/api/coupon/can-use/SUMMER2024"

# 4. تطبيق الكوبون
curl -X POST "http://localhost:5036/api/coupon/apply" \
  -H "Content-Type: application/json" \
  -d '{
    "code": "SUMMER2024",
    "orderId": 1,
    "userId": 1,
    "orderAmount": 300.0
  }'

# 5. تحديث الكوبون
curl -X PUT "http://localhost:5036/api/coupon/1" \
  -H "Content-Type: application/json" \
  -d '{
    "code": "SUMMER2024",
    "description": "خصم صيفي محدث 35%",
    "discountType": "Percentage",
    "discountValue": 35.0,
    "minimumAmount": 250.0,
    "maximumDiscount": 150.0,
    "startDate": "2024-06-01T00:00:00",
    "endDate": "2024-08-31T23:59:59",
    "maxUsage": 1000,
    "isActive": true
  }'

# 6. الحصول على الكوبونات الصالحة
curl -X GET "http://localhost:5036/api/coupon/valid"

# 7. حذف الكوبون
curl -X DELETE "http://localhost:5036/api/coupon/1"
```

## أنواع الخصم

| النوع | الوصف |
|-------|--------|
| Percentage | خصم بنسبة مئوية |
| FixedAmount | خصم بمبلغ ثابت |

## رموز الاستجابة

| الكود | الوصف |
|-------|--------|
| 200 | نجح الطلب |
| 201 | تم إنشاء الكوبون بنجاح |
| 400 | خطأ في البيانات المرسلة |
| 404 | الكوبون غير موجود |
| 500 | خطأ في الخادم |

## ملاحظات مهمة
- تأكد من صحة التواريخ (startDate و endDate)
- الكوبون يجب أن يكون نشطاً للاستخدام
- تأكد من عدم تجاوز الحد الأقصى للاستخدام
- احرص على صحة نوع الخصم وقيمته
- تأكد من الحد الأدنى للمبلغ المطلوب 