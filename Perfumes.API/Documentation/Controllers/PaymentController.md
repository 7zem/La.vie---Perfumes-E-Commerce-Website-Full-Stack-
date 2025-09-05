# Payment Controller API Documentation

## نظرة عامة
يحتوي هذا ال Controller على جميع العمليات المتعلقة بمعالجة المدفوعات في النظام باستخدام بوابة Paymob.

## Base URL
```
http://localhost:5036/api/payment
```

## Endpoints

### 1. بدء عملية الدفع
**POST** `/api/payment/initiate`

**الوصف:** بدء عملية دفع جديدة للحصول على رابط الدفع.

**Body (JSON):**
```json
{
  "orderId": 1,
  "amount": 150.00,
  "currency": "EGP",
  "customerName": "أحمد محمد",
  "customerEmail": "ahmed@example.com",
  "customerPhone": "+201234567890",
  "customerAddress": "القاهرة، مصر",
  "items": [
    {
      "name": "عطر فاخر",
      "amount": 150.00,
      "quantity": 1
    }
  ]
}
```

**الاستجابة:**
```json
{
  "paymentUrl": "https://paymob.com/payment/iframe/123456"
}
```

**مثال الاختبار:**
```bash
curl -X POST "http://localhost:5036/api/payment/initiate" \
  -H "Content-Type: application/json" \
  -d '{
    "orderId": 1,
    "amount": 150.00,
    "currency": "EGP",
    "customerName": "أحمد محمد",
    "customerEmail": "ahmed@example.com",
    "customerPhone": "+201234567890",
    "customerAddress": "القاهرة، مصر",
    "items": [
      {
        "name": "عطر فاخر",
        "amount": 150.00,
        "quantity": 1
      }
    ]
  }'
```

### 2. معالجة استجابة الدفع
**POST** `/api/payment/callback?hmac={hmac}`

**الوصف:** معالجة استجابة من بوابة الدفع بعد اكتمال العملية.

**Headers:** `Content-Type: application/json`

**Body (JSON):**
```json
{
  "type": "TRANSACTION",
  "obj": {
    "id": 123456,
    "amount_cents": 15000,
    "currency": "EGP",
    "order": {
      "id": 1
    },
    "success": true,
    "is_void": false,
    "is_refund": false,
    "is_3d_secure": false,
    "integration_id": 5087003,
    "profile_id": 123456,
    "has_parent_transaction": false,
    "data": {
      "order_id": 1
    }
  }
}
```

**مثال الاختبار:**
```bash
curl -X POST "http://localhost:5036/api/payment/callback?hmac=valid_hmac_signature" \
  -H "Content-Type: application/json" \
  -d '{
    "type": "TRANSACTION",
    "obj": {
      "id": 123456,
      "amount_cents": 15000,
      "currency": "EGP",
      "order": {
        "id": 1
      },
      "success": true,
      "is_void": false,
      "is_refund": false,
      "is_3d_secure": false,
      "integration_id": 5087003,
      "profile_id": 123456,
      "has_parent_transaction": false,
      "data": {
        "order_id": 1
      }
    }
  }'
```

## أمثلة اختبار شاملة

### سيناريو كامل لمعالجة الدفع
```bash
# 1. بدء عملية دفع
curl -X POST "http://localhost:5036/api/payment/initiate" \
  -H "Content-Type: application/json" \
  -d '{
    "orderId": 1,
    "amount": 250.00,
    "currency": "EGP",
    "customerName": "أحمد محمد",
    "customerEmail": "ahmed@example.com",
    "customerPhone": "+201234567890",
    "customerAddress": "القاهرة، مصر",
    "items": [
      {
        "name": "عطر فاخر",
        "amount": 150.00,
        "quantity": 1
      },
      {
        "name": "عطر آخر",
        "amount": 100.00,
        "quantity": 1
      }
    ]
  }'

# 2. معالجة استجابة الدفع الناجحة
curl -X POST "http://localhost:5036/api/payment/callback?hmac=valid_hmac_signature" \
  -H "Content-Type: application/json" \
  -d '{
    "type": "TRANSACTION",
    "obj": {
      "id": 123456,
      "amount_cents": 25000,
      "currency": "EGP",
      "order": {
        "id": 1
      },
      "success": true,
      "is_void": false,
      "is_refund": false,
      "is_3d_secure": false,
      "integration_id": 5087003,
      "profile_id": 123456,
      "has_parent_transaction": false,
      "data": {
        "order_id": 1
      }
    }
  }'

# 3. معالجة استجابة الدفع الفاشلة
curl -X POST "http://localhost:5036/api/payment/callback?hmac=valid_hmac_signature" \
  -H "Content-Type: application/json" \
  -d '{
    "type": "TRANSACTION",
    "obj": {
      "id": 123457,
      "amount_cents": 25000,
      "currency": "EGP",
      "order": {
        "id": 1
      },
      "success": false,
      "is_void": false,
      "is_refund": false,
      "is_3d_secure": false,
      "integration_id": 5087003,
      "profile_id": 123456,
      "has_parent_transaction": false,
      "data": {
        "order_id": 1
      }
    }
  }'
```

## حالات الدفع

| الحالة | الوصف |
|--------|--------|
| success: true | نجح الدفع |
| success: false | فشل الدفع |
| is_void: true | تم إلغاء الدفع |
| is_refund: true | تم استرداد المبلغ |

## رموز الاستجابة

| الكود | الوصف |
|-------|--------|
| 200 | نجح الطلب |
| 400 | خطأ في البيانات المرسلة |
| 401 | توقيع HMAC غير صحيح |
| 500 | خطأ في الخادم |

## ملاحظات مهمة
- تأكد من صحة بيانات العميل
- تأكد من صحة المبلغ والعملة
- احرص على التحقق من توقيع HMAC
- تأكد من صحة معرف الطلب
- احرص على معالجة جميع حالات الدفع
- تأكد من تحديث حالة الطلب بعد الدفع 