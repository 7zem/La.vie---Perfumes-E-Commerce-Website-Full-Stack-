# Cart Controller API Documentation

## نظرة عامة
يحتوي هذا ال Controller على جميع العمليات المتعلقة بإدارة عربة التسوق في النظام.

## Base URL
```
http://localhost:5036/api/cart
```

## Endpoints

### 1. الحصول على عناصر العربة
**GET** `/api/cart?userId={userId}&visitorId={visitorId}`

**الوصف:** الحصول على جميع عناصر عربة التسوق.

**الاستجابة:**
```json
{
  "cartId": 1,
  "userId": 1,
  "visitorId": null,
  "items": [
    {
      "cartItemId": 1,
      "productId": 1,
      "productName": "عطر فاخر",
      "productImage": "https://example.com/perfume1.jpg",
      "price": 150.00,
      "quantity": 2,
      "subtotal": 300.00
    },
    {
      "cartItemId": 2,
      "productId": 2,
      "productName": "عطر آخر",
      "productImage": "https://example.com/perfume2.jpg",
      "price": 200.00,
      "quantity": 1,
      "subtotal": 200.00
    }
  ],
  "totalItems": 3,
  "totalAmount": 500.00
}
```

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/cart?userId=1"
```

### 2. إضافة منتج إلى العربة
**POST** `/api/cart`

**الوصف:** إضافة منتج جديد إلى عربة التسوق.

**Body (JSON):**
```json
{
  "userId": 1,
  "visitorId": null,
  "productId": 1,
  "quantity": 2
}
```

**مثال الاختبار:**
```bash
curl -X POST "http://localhost:5036/api/cart" \
  -H "Content-Type: application/json" \
  -d '{
    "userId": 1,
    "visitorId": null,
    "productId": 1,
    "quantity": 2
  }'
```

### 3. تحديث الكمية
**PUT** `/api/cart/{cartId}/quantity?quantity={quantity}`

**الوصف:** تحديث كمية منتج محدد في العربة.

**مثال الاختبار:**
```bash
curl -X PUT "http://localhost:5036/api/cart/1/quantity?quantity=3"
```

### 4. زيادة الكمية
**PUT** `/api/cart/{cartId}/increase`

**الوصف:** زيادة كمية منتج بمقدار واحد.

**مثال الاختبار:**
```bash
curl -X PUT "http://localhost:5036/api/cart/1/increase"
```

### 5. تقليل الكمية
**PUT** `/api/cart/{cartId}/decrease`

**الوصف:** تقليل كمية منتج بمقدار واحد.

**مثال الاختبار:**
```bash
curl -X PUT "http://localhost:5036/api/cart/1/decrease"
```

### 6. إزالة منتج من العربة
**DELETE** `/api/cart/{cartId}`

**الوصف:** إزالة منتج محدد من عربة التسوق.

**مثال الاختبار:**
```bash
curl -X DELETE "http://localhost:5036/api/cart/1"
```

### 7. تفريغ العربة
**DELETE** `/api/cart/clear?userId={userId}&visitorId={visitorId}`

**الوصف:** إزالة جميع المنتجات من عربة التسوق.

**مثال الاختبار:**
```bash
curl -X DELETE "http://localhost:5036/api/cart/clear?userId=1"
```

### 8. الحصول على عدد العناصر
**GET** `/api/cart/count?userId={userId}&visitorId={visitorId}`

**الوصف:** الحصول على عدد العناصر في عربة التسوق.

**الاستجابة:**
```json
{
  "count": 3
}
```

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/cart/count?userId=1"
```

### 9. التحقق من وجود منتج
**GET** `/api/cart/exists?productId={productId}&userId={userId}&visitorId={visitorId}`

**الوصف:** التحقق من وجود منتج معين في عربة التسوق.

**الاستجابة:**
```json
{
  "exists": true
}
```

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/cart/exists?productId=1&userId=1"
```

## أمثلة اختبار شاملة

### سيناريو كامل لإدارة عربة التسوق
```bash
# 1. إضافة منتج إلى العربة
curl -X POST "http://localhost:5036/api/cart" \
  -H "Content-Type: application/json" \
  -d '{
    "userId": 1,
    "visitorId": null,
    "productId": 1,
    "quantity": 2
  }'

# 2. إضافة منتج آخر
curl -X POST "http://localhost:5036/api/cart" \
  -H "Content-Type: application/json" \
  -d '{
    "userId": 1,
    "visitorId": null,
    "productId": 2,
    "quantity": 1
  }'

# 3. الحصول على محتويات العربة
curl -X GET "http://localhost:5036/api/cart?userId=1"

# 4. الحصول على عدد العناصر
curl -X GET "http://localhost:5036/api/cart/count?userId=1"

# 5. التحقق من وجود منتج
curl -X GET "http://localhost:5036/api/cart/exists?productId=1&userId=1"

# 6. زيادة كمية منتج
curl -X PUT "http://localhost:5036/api/cart/1/increase"

# 7. تحديث كمية منتج
curl -X PUT "http://localhost:5036/api/cart/1/quantity?quantity=5"

# 8. تقليل كمية منتج
curl -X PUT "http://localhost:5036/api/cart/2/decrease"

# 9. إزالة منتج من العربة
curl -X DELETE "http://localhost:5036/api/cart/2"

# 10. تفريغ العربة
curl -X DELETE "http://localhost:5036/api/cart/clear?userId=1"
```

## أمثلة للزوار (بدون تسجيل دخول)

### إدارة عربة للزوار
```bash
# 1. إضافة منتج لعربة الزائر
curl -X POST "http://localhost:5036/api/cart" \
  -H "Content-Type: application/json" \
  -d '{
    "userId": null,
    "visitorId": "visitor123",
    "productId": 1,
    "quantity": 1
  }'

# 2. الحصول على عربة الزائر
curl -X GET "http://localhost:5036/api/cart?visitorId=visitor123"

# 3. الحصول على عدد العناصر
curl -X GET "http://localhost:5036/api/cart/count?visitorId=visitor123"

# 4. تفريغ عربة الزائر
curl -X DELETE "http://localhost:5036/api/cart/clear?visitorId=visitor123"
```

## رموز الاستجابة

| الكود | الوصف |
|-------|--------|
| 200 | نجح الطلب |
| 400 | خطأ في البيانات المرسلة |
| 404 | المنتج أو العنصر غير موجود |
| 500 | خطأ في الخادم |

## ملاحظات مهمة
- يمكن استخدام `userId` للمستخدمين المسجلين
- يمكن استخدام `visitorId` للزوار غير المسجلين
- تأكد من صحة معرف المنتج عند الإضافة
- الكمية يجب أن تكون أكبر من صفر
- عند تقليل الكمية إلى صفر، سيتم إزالة المنتج تلقائياً
- احرص على التحقق من المخز قبل الإضافة 