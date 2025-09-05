# Order Controller API Documentation

## نظرة عامة
يحتوي هذا ال Controller على جميع العمليات المتعلقة بإدارة الطلبات والمبيعات في النظام.

## Base URL
```
http://localhost:5036/api/order
```

## Endpoints

### 1. إنشاء طلب جديد
**POST** `/api/order`

**الوصف:** إنشاء طلب جديد في النظام.

**Body (JSON):**
```json
{
  "userId": 1,
  "items": [
    {
      "productId": 1,
      "quantity": 2
    },
    {
      "productId": 2,
      "quantity": 1
    }
  ],
  "shippingAddress": {
    "street": "شارع النيل",
    "city": "القاهرة",
    "state": "القاهرة",
    "zipCode": "12345",
    "country": "مصر"
  },
  "paymentMethod": "CreditCard",
  "couponCode": "DISCOUNT10"
}
```

**الاستجابة:**
```json
{
  "message": "Order created",
  "redirect": "https://payment-gateway.com/pay?token=payment_token"
}
```

**مثال الاختبار:**
```bash
curl -X POST "http://localhost:5036/api/order" \
  -H "Content-Type: application/json" \
  -d '{
    "userId": 1,
    "items": [
      {
        "productId": 1,
        "quantity": 2
      },
      {
        "productId": 2,
        "quantity": 1
      }
    ],
    "shippingAddress": {
      "street": "شارع النيل",
      "city": "القاهرة",
      "state": "القاهرة",
      "zipCode": "12345",
      "country": "مصر"
    },
    "paymentMethod": "CreditCard",
    "couponCode": "DISCOUNT10"
  }'
```

### 2. الحصول على طلب بواسطة ID
**GET** `/api/order/id/{orderId}`

**الوصف:** الحصول على طلب محدد بواسطة الـ ID.

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/order/id/1"
```

### 3. الحصول على طلب بواسطة رقم الطلب
**GET** `/api/order/number/{orderNumber}`

**الوصف:** الحصول على طلب بواسطة رقم الطلب.

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/order/number/ORD-2024-001"
```

### 4. الحصول على طلبات مستخدم
**GET** `/api/order/user/{userId}`

**الوصف:** الحصول على جميع طلبات مستخدم معين.

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/order/user/1"
```

### 5. الحصول على طلبات حسب الحالة
**GET** `/api/order/status/{status}`

**الوصف:** الحصول على طلبات بحالة معينة (Pending, Processing, Shipped, Delivered, Cancelled).

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/order/status/Pending"
```

### 6. الحصول على طلبات حسب النطاق الزمني
**GET** `/api/order/date-range?start={startDate}&end={endDate}`

**الوصف:** الحصول على طلبات في نطاق زمني محدد.

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/order/date-range?start=2024-01-01&end=2024-01-31"
```

### 7. الحصول على الطلبات المعلقة
**GET** `/api/order/pending`

**الوصف:** الحصول على الطلبات المعلقة فقط.

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/order/pending"
```

### 8. الحصول على الطلبات المكتملة
**GET** `/api/order/completed`

**الوصف:** الحصول على الطلبات المكتملة فقط.

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/order/completed"
```

### 9. الحصول على إجمالي المبيعات
**GET** `/api/order/total-sales?start={startDate}&end={endDate}`

**الوصف:** الحصول على إجمالي المبيعات في فترة زمنية.

**الاستجابة:**
```json
{
  "total": 15000.00
}
```

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/order/total-sales?start=2024-01-01&end=2024-01-31"
```

### 10. الحصول على عدد الطلبات
**GET** `/api/order/count?start={startDate}&end={endDate}`

**الوصف:** الحصول على عدد الطلبات في فترة زمنية.

**الاستجابة:**
```json
{
  "count": 150
}
```

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/order/count?start=2024-01-01&end=2024-01-31"
```

### 11. تحديث حالة الطلب
**PUT** `/api/order/{orderId}/status`

**الوصف:** تحديث حالة الطلب.

**Body (JSON):**
```json
"Shipped"
```

**مثال الاختبار:**
```bash
curl -X PUT "http://localhost:5036/api/order/1/status" \
  -H "Content-Type: application/json" \
  -d '"Shipped"'
```

### 12. تحديث معلومات الشحن
**PUT** `/api/order/{orderId}/shipping`

**الوصف:** تحديث معلومات الشحن للطلب.

**Body (JSON):**
```json
{
  "trackingNumber": "TRK123456789",
  "carrier": "DHL",
  "estimatedDelivery": "2024-01-15T00:00:00",
  "shippingNotes": "سيتم التوصيل في الصباح"
}
```

**مثال الاختبار:**
```bash
curl -X PUT "http://localhost:5036/api/order/1/shipping" \
  -H "Content-Type: application/json" \
  -d '{
    "trackingNumber": "TRK123456789",
    "carrier": "DHL",
    "estimatedDelivery": "2024-01-15T00:00:00",
    "shippingNotes": "سيتم التوصيل في الصباح"
  }'
```

### 13. معالجة استجابة الدفع
**POST** `/api/order/payment-callback`

**الوصف:** معالجة استجابة من بوابة الدفع.

**Body (JSON):**
```json
{
  "orderId": 1,
  "paymentId": "PAY123456",
  "status": "Success",
  "amount": 150.00,
  "transactionId": "TXN789012"
}
```

**مثال الاختبار:**
```bash
curl -X POST "http://localhost:5036/api/order/payment-callback" \
  -H "Content-Type: application/json" \
  -d '{
    "orderId": 1,
    "paymentId": "PAY123456",
    "status": "Success",
    "amount": 150.00,
    "transactionId": "TXN789012"
  }'
```

## أمثلة اختبار شاملة

### سيناريو كامل لإدارة الطلبات
```bash
# 1. إنشاء طلب جديد
curl -X POST "http://localhost:5036/api/order" \
  -H "Content-Type: application/json" \
  -d '{
    "userId": 1,
    "items": [
      {
        "productId": 1,
        "quantity": 2
      },
      {
        "productId": 2,
        "quantity": 1
      }
    ],
    "shippingAddress": {
      "street": "شارع النيل",
      "city": "القاهرة",
      "state": "القاهرة",
      "zipCode": "12345",
      "country": "مصر"
    },
    "paymentMethod": "CreditCard",
    "couponCode": "DISCOUNT10"
  }'

# 2. الحصول على الطلب المنشأ
curl -X GET "http://localhost:5036/api/order/id/1"

# 3. تحديث حالة الطلب
curl -X PUT "http://localhost:5036/api/order/1/status" \
  -H "Content-Type: application/json" \
  -d '"Processing"'

# 4. تحديث معلومات الشحن
curl -X PUT "http://localhost:5036/api/order/1/shipping" \
  -H "Content-Type: application/json" \
  -d '{
    "trackingNumber": "TRK123456789",
    "carrier": "DHL",
    "estimatedDelivery": "2024-01-15T00:00:00",
    "shippingNotes": "سيتم التوصيل في الصباح"
  }'

# 5. الحصول على طلبات مستخدم
curl -X GET "http://localhost:5036/api/order/user/1"

# 6. الحصول على إجمالي المبيعات
curl -X GET "http://localhost:5036/api/order/total-sales?start=2024-01-01&end=2024-01-31"

# 7. معالجة استجابة الدفع
curl -X POST "http://localhost:5036/api/order/payment-callback" \
  -H "Content-Type: application/json" \
  -d '{
    "orderId": 1,
    "paymentId": "PAY123456",
    "status": "Success",
    "amount": 150.00,
    "transactionId": "TXN789012"
  }'
```

## رموز الاستجابة

| الكود | الوصف |
|-------|--------|
| 200 | نجح الطلب |
| 201 | تم إنشاء الطلب بنجاح |
| 400 | خطأ في البيانات المرسلة |
| 404 | الطلب غير موجود |
| 500 | خطأ في الخادم |

## حالات الطلب

| الحالة | الوصف |
|--------|--------|
| Pending | معلق |
| Processing | قيد المعالجة |
| Shipped | تم الشحن |
| Delivered | تم التوصيل |
| Cancelled | ملغي |

## ملاحظات مهمة
- تأكد من صحة بيانات المستخدم والمنتجات
- استخدم كوبونات صحيحة للحصول على الخصم
- تأكد من صحة عنوان الشحن
- احرص على تحديث حالة الطلب بشكل صحيح
- تأكد من صحة معلومات الدفع 