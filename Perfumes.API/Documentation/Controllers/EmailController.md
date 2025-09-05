# Email Controller API Documentation

## نظرة عامة
يحتوي هذا ال Controller على جميع العمليات المتعلقة بإرسال رسائل البريد الإلكتروني باستخدام SendGrid.

## Base URL
```
http://localhost:5036/api/email
```

## Endpoints

### 1. إرسال بريد إلكتروني تجريبي
**POST** `/api/email/send?to={email}`

**الوصف:** إرسال بريد إلكتروني تجريبي لاختبار خدمة SendGrid.

**الاستجابة:**
```json
{
  "message": "Email sent successfully!"
}
```

**مثال الاختبار:**
```bash
curl -X POST "http://localhost:5036/api/email/send?to=test@example.com"
```

### 2. إرسال بريد إلكتروني باستخدام قالب
**POST** `/api/email/send-template?to={email}`

**الوصف:** إرسال بريد إلكتروني باستخدام قالب SendGrid مع متغيرات مخصصة.

**الاستجابة:**
```json
{
  "message": "Template email sent successfully!"
}
```

**مثال الاختبار:**
```bash
curl -X POST "http://localhost:5036/api/email/send-template?to=test@example.com"
```

## أمثلة اختبار شاملة

### سيناريو كامل لاختبار البريد الإلكتروني
```bash
# 1. إرسال بريد إلكتروني تجريبي
curl -X POST "http://localhost:5036/api/email/send?to=ahmed@example.com"

# 2. إرسال بريد إلكتروني باستخدام قالب
curl -X POST "http://localhost:5036/api/email/send-template?to=ahmed@example.com"

# 3. اختبار مع بريد إلكتروني مختلف
curl -X POST "http://localhost:5036/api/email/send?to=test@perfumes.com"

# 4. اختبار قالب مع بريد إلكتروني مختلف
curl -X POST "http://localhost:5036/api/email/send-template?to=test@perfumes.com"
```

## أنواع البريد الإلكتروني

### 1. البريد الإلكتروني التجريبي
- **الموضوع:** Test Email from Perfumes Store
- **المحتوى:** رسالة HTML بسيطة للاختبار
- **الاستخدام:** اختبار خدمة SendGrid

### 2. البريد الإلكتروني بالقالب
- **القالب:** d-c3292025cc1a4ebfab1aae1af921e81d
- **المتغيرات:**
  - `name`: اسم العميل
  - `orderId`: رقم الطلب
- **الاستخدام:** إرسال رسائل مخصصة

## رموز الاستجابة

| الكود | الوصف |
|-------|--------|
| 200 | تم إرسال البريد الإلكتروني بنجاح |
| 400 | خطأ في إرسال البريد الإلكتروني |
| 500 | خطأ في الخادم |

## ملاحظات مهمة
- تأكد من صحة عنوان البريد الإلكتروني
- تأكد من تكوين SendGrid بشكل صحيح
- احرص على اختبار القوالب قبل الاستخدام
- تأكد من صحة متغيرات القالب
- احرص على معالجة الأخطاء بشكل مناسب 