# Controllers Documentation Summary

## نظرة عامة
هذا المجلد يحتوي على توثيق مفصل لجميع الـ Controllers في نظام إدارة متجر العطور.

## قائمة الـ Controllers

### 1. [ProductController](./ProductController.md)
**الوصف:** إدارة المنتجات (العطور)
- إنشاء، تحديث، حذف المنتجات
- البحث والتصفية حسب الفئات، الماركات، الجنس، السعر
- إدارة المخزون والصور
- الحصول على المنتجات المميزة والجديدة

### 2. [AuthController](./AuthController.md)
**الوصف:** إدارة المصادقة والمستخدمين
- تسجيل الدخول والخروج
- إنشاء وإدارة الحسابات
- إدارة الملف الشخصي
- إعادة تعيين كلمة المرور
- التحقق من البريد الإلكتروني

### 3. [AdminController](./AdminController.md)
**الوصف:** إدارة المستخدمين (للمديرين فقط)
- عرض جميع المستخدمين
- تحديث أدوار المستخدمين
- حظر وإلغاء حظر المستخدمين
- حذف الحسابات

### 4. [CategoryController](./CategoryController.md)
**الوصف:** إدارة فئات المنتجات
- إنشاء وإدارة الفئات الرئيسية والفرعية
- عرض شجرة الفئات
- إدارة المنتجات في كل فئة

### 5. [BrandController](./BrandController.md)
**الوصف:** إدارة ماركات العطور
- إنشاء وإدارة الماركات
- عرض المنتجات حسب الماركة
- إدارة شعارات الماركات

### 6. [OrderController](./OrderController.md)
**الوصف:** إدارة الطلبات والمبيعات
- إنشاء وتتبع الطلبات
- إدارة حالات الطلبات
- تحليلات المبيعات
- معالجة الدفع

### 7. [CartController](./CartController.md)
**الوصف:** إدارة عربة التسوق
- إضافة وإزالة المنتجات
- تحديث الكميات
- إدارة عربة الزوار والمستخدمين المسجلين

### 8. [CouponController](./CouponController.md)
**الوصف:** إدارة كوبونات الخصم
- إنشاء وإدارة الكوبونات
- تطبيق الكوبونات على الطلبات
- التحقق من صلاحية الكوبونات

### 9. [PaymentController](./PaymentController.md)
**الوصف:** معالجة المدفوعات
- بدء عمليات الدفع
- معالجة استجابات بوابة الدفع
- التحقق من صحة المعاملات

### 10. [EmailController](./EmailController.md)
**الوصف:** إرسال رسائل البريد الإلكتروني
- إرسال رسائل تجريبية
- إرسال رسائل باستخدام قوالب
- اختبار خدمة SendGrid

### 11. [CacheController](./CacheController.md)
**الوصف:** إدارة التخزين المؤقت (للمديرين فقط)
- عرض إحصائيات Redis
- إدارة المفاتيح والقيم
- إلغاء صلاحية التخزين المؤقت
- اختبار وظائف التخزين المؤقت

### 12. [DashboardController](./DashboardController.md)
**الوصف:** لوحات التحكم والإحصائيات
- لوحة تحكم المدير مع التحليلات
- لوحة تحكم المستخدم
- التقارير والرسوم البيانية
- النشاطات الحديثة

## متطلبات المصادقة

### للمستخدمين العاديين
- **Authorization:** `Bearer {user_token}`
- **Role:** Customer

### للمديرين
- **Authorization:** `Bearer {admin_token}`
- **Role:** Admin

## كيفية الحصول على Token

### 1. تسجيل حساب جديد
```bash
curl -X POST "http://localhost:5036/api/auth/register" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "أحمد محمد",
    "email": "ahmed@example.com",
    "password": "password123",
    "confirmPassword": "password123"
  }'
```

### 2. تسجيل الدخول
```bash
curl -X POST "http://localhost:5036/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "ahmed@example.com",
    "password": "password123"
  }'
```

### 3. إنشاء مدير (للتنمية فقط)
```bash
curl -X POST "http://localhost:5036/api/auth/create-admin" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "مدير النظام",
    "email": "admin@example.com",
    "password": "admin123"
  }'
```

## أمثلة اختبار سريعة

### اختبار المصادقة
```bash
# تسجيل الدخول
curl -X POST "http://localhost:5036/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{"email": "test@example.com", "password": "password123"}'

# الحصول على الملف الشخصي
curl -X GET "http://localhost:5036/api/auth/profile" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

### اختبار المنتجات
```bash
# الحصول على جميع المنتجات
curl -X GET "http://localhost:5036/api/product"

# البحث في المنتجات
curl -X GET "http://localhost:5036/api/product/search?term=عطر"

# الحصول على منتج محدد
curl -X GET "http://localhost:5036/api/product/1"
```

### اختبار الفئات
```bash
# الحصول على جميع الفئات
curl -X GET "http://localhost:5036/api/category"

# الحصول على شجرة الفئات
curl -X GET "http://localhost:5036/api/category/tree"
```

### اختبار الماركات
```bash
# الحصول على جميع الماركات
curl -X GET "http://localhost:5036/api/brand"

# الحصول على ماركة مع منتجاتها
curl -X GET "http://localhost:5036/api/brand/1/products"
```

## رموز الاستجابة الشائعة

| الكود | الوصف |
|-------|--------|
| 200 | نجح الطلب |
| 201 | تم الإنشاء بنجاح |
| 204 | تم التحديث/الحذف بنجاح |
| 400 | خطأ في البيانات المرسلة |
| 401 | غير مصرح |
| 403 | ممنوع (غير مدير) |
| 404 | المورد غير موجود |
| 500 | خطأ في الخادم |

## ملاحظات مهمة

### للأمان
- احرص على استخدام HTTPS في الإنتاج
- تأكد من صحة الـ tokens قبل الاستخدام
- لا تشارك الـ tokens مع أي شخص
- احرص على تحديث كلمات المرور بانتظام

### للأداء
- استخدم التخزين المؤقت عند الحاجة
- احرص على إلغاء صلاحية التخزين المؤقت عند تحديث البيانات
- استخدم الصفحات (Pagination) للقوائم الكبيرة

### للاختبار
- استخدم Swagger UI للاختبار التفاعلي
- احرص على اختبار جميع الحالات (النجاح والفشل)
- تأكد من صحة البيانات المرسلة

## روابط مفيدة

- **Swagger UI:** `http://localhost:5036/swagger`
- **Base URL:** `http://localhost:5036`
- **التوثيق العام:** [../README.md](../README.md)

## الدعم

إذا واجهت أي مشاكل أو لديك أسئلة، يرجى:
1. مراجعة التوثيق المفصل لكل Controller
2. اختبار الـ endpoints باستخدام Swagger UI
3. التحقق من رموز الاستجابة وأسباب الخطأ 