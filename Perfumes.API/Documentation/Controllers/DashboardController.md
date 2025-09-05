# Dashboard Controller API Documentation

## نظرة عامة
يحتوي هذا ال Controller على جميع العمليات المتعلقة بلوحات التحكم والإحصائيات للمديرين والمستخدمين.

## Base URL
```
http://localhost:5036/api/dashboard
```

## متطلبات المصادقة
جميع ال endpoints تتطلب:
- **Authorization:** `Bearer {token}`
- **Role:** Admin (للوحات الإدارية) أو Customer (للوحات المستخدمين)

## Admin Dashboard Endpoints

### 1. لوحة تحكم المدير
**GET** `/api/dashboard/admin?startDate={startDate}&endDate={endDate}`

**الوصف:** الحصول على لوحة تحكم المدير مع الإحصائيات الشاملة.

**Headers:** `Authorization: Bearer {admin_token}`

**الاستجابة:**
```json
{
  "totalRevenue": 50000.00,
  "totalOrders": 150,
  "totalCustomers": 75,
  "totalProducts": 200,
  "pendingOrders": 10,
  "lowStockProducts": 5,
  "averageOrderValue": 333.33,
  "recentOrders": [...],
  "recentCustomers": [...],
  "topProducts": [...]
}
```

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/dashboard/admin?startDate=2024-01-01&endDate=2024-01-31" \
  -H "Authorization: Bearer ADMIN_TOKEN_HERE"
```

### 2. تحليلات المبيعات
**GET** `/api/dashboard/admin/sales-analytics?startDate={startDate}&endDate={endDate}`

**الوصف:** الحصول على تحليلات مفصلة للمبيعات.

**Headers:** `Authorization: Bearer {admin_token}`

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/dashboard/admin/sales-analytics?startDate=2024-01-01&endDate=2024-01-31" \
  -H "Authorization: Bearer ADMIN_TOKEN_HERE"
```

### 3. تحليلات العملاء
**GET** `/api/dashboard/admin/customer-analytics?startDate={startDate}&endDate={endDate}`

**الوصف:** الحصول على تحليلات مفصلة للعملاء.

**Headers:** `Authorization: Bearer {admin_token}`

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/dashboard/admin/customer-analytics?startDate=2024-01-01&endDate=2024-01-31" \
  -H "Authorization: Bearer ADMIN_TOKEN_HERE"
```

### 4. تحليلات المنتجات
**GET** `/api/dashboard/admin/product-analytics?startDate={startDate}&endDate={endDate}`

**الوصف:** الحصول على تحليلات مفصلة للمنتجات.

**Headers:** `Authorization: Bearer {admin_token}`

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/dashboard/admin/product-analytics?startDate=2024-01-01&endDate=2024-01-31" \
  -H "Authorization: Bearer ADMIN_TOKEN_HERE"
```

### 5. تحليلات الطلبات
**GET** `/api/dashboard/admin/order-analytics?startDate={startDate}&endDate={endDate}`

**الوصف:** الحصول على تحليلات مفصلة للطلبات.

**Headers:** `Authorization: Bearer {admin_token}`

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/dashboard/admin/order-analytics?startDate=2024-01-01&endDate=2024-01-31" \
  -H "Authorization: Bearer ADMIN_TOKEN_HERE"
```

### 6. الإحصائيات السريعة للمدير
**GET** `/api/dashboard/admin/quick-stats`

**الوصف:** الحصول على الإحصائيات السريعة للمدير.

**Headers:** `Authorization: Bearer {admin_token}`

**الاستجابة:**
```json
{
  "totalRevenue": 50000.00,
  "totalOrders": 150,
  "totalCustomers": 75,
  "totalProducts": 200,
  "pendingOrders": 10,
  "lowStockProducts": 5,
  "averageOrderValue": 333.33
}
```

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/dashboard/admin/quick-stats" \
  -H "Authorization: Bearer ADMIN_TOKEN_HERE"
```

## User Dashboard Endpoints

### 7. لوحة تحكم المستخدم
**GET** `/api/dashboard/user`

**الوصف:** الحصول على لوحة تحكم المستخدم مع إحصائياته.

**Headers:** `Authorization: Bearer {user_token}`

**الاستجابة:**
```json
{
  "totalOrders": 5,
  "totalSpent": 750.00,
  "wishlistItems": 3,
  "totalReviews": 2,
  "averageRating": 4.5,
  "recentOrders": [...],
  "recentReviews": [...],
  "wishlistItems": [...]
}
```

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/dashboard/user" \
  -H "Authorization: Bearer USER_TOKEN_HERE"
```

### 8. طلبات المستخدم
**GET** `/api/dashboard/user/orders`

**الوصف:** الحصول على جميع طلبات المستخدم.

**Headers:** `Authorization: Bearer {user_token}`

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/dashboard/user/orders" \
  -H "Authorization: Bearer USER_TOKEN_HERE"
```

### 9. قائمة أمنيات المستخدم
**GET** `/api/dashboard/user/wishlist`

**الوصف:** الحصول على قائمة أمنيات المستخدم.

**Headers:** `Authorization: Bearer {user_token}`

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/dashboard/user/wishlist" \
  -H "Authorization: Bearer USER_TOKEN_HERE"
```

### 10. تقييمات المستخدم
**GET** `/api/dashboard/user/reviews`

**الوصف:** الحصول على تقييمات المستخدم.

**Headers:** `Authorization: Bearer {user_token}`

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/dashboard/user/reviews" \
  -H "Authorization: Bearer USER_TOKEN_HERE"
```

### 11. الإحصائيات السريعة للمستخدم
**GET** `/api/dashboard/user/quick-stats`

**الوصف:** الحصول على الإحصائيات السريعة للمستخدم.

**Headers:** `Authorization: Bearer {user_token}`

**الاستجابة:**
```json
{
  "totalOrders": 5,
  "totalSpent": 750.00,
  "wishlistItems": 3,
  "totalReviews": 2,
  "averageRating": 4.5
}
```

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/dashboard/user/quick-stats" \
  -H "Authorization: Bearer USER_TOKEN_HERE"
```

## Reports Endpoints (Admin Only)

### 12. تقرير المبيعات
**GET** `/api/dashboard/admin/reports/sales?startDate={startDate}&endDate={endDate}`

**الوصف:** الحصول على تقرير مفصل للمبيعات.

**Headers:** `Authorization: Bearer {admin_token}`

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/dashboard/admin/reports/sales?startDate=2024-01-01&endDate=2024-01-31" \
  -H "Authorization: Bearer ADMIN_TOKEN_HERE"
```

### 13. تقرير المخزون
**GET** `/api/dashboard/admin/reports/inventory`

**الوصف:** الحصول على تقرير مفصل للمخزون.

**Headers:** `Authorization: Bearer {admin_token}`

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/dashboard/admin/reports/inventory" \
  -H "Authorization: Bearer ADMIN_TOKEN_HERE"
```

### 14. تقرير العملاء
**GET** `/api/dashboard/admin/reports/customers?startDate={startDate}&endDate={endDate}`

**الوصف:** الحصول على تقرير مفصل للعملاء.

**Headers:** `Authorization: Bearer {admin_token}`

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/dashboard/admin/reports/customers?startDate=2024-01-01&endDate=2024-01-31" \
  -H "Authorization: Bearer ADMIN_TOKEN_HERE"
```

## Charts Endpoints (Admin Only)

### 15. رسم بياني للإيرادات
**GET** `/api/dashboard/admin/charts/revenue?startDate={startDate}&endDate={endDate}`

**الوصف:** الحصول على بيانات الرسم البياني للإيرادات.

**Headers:** `Authorization: Bearer {admin_token}`

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/dashboard/admin/charts/revenue?startDate=2024-01-01&endDate=2024-01-31" \
  -H "Authorization: Bearer ADMIN_TOKEN_HERE"
```

### 16. رسم بياني للطلبات
**GET** `/api/dashboard/admin/charts/orders?startDate={startDate}&endDate={endDate}`

**الوصف:** الحصول على بيانات الرسم البياني للطلبات.

**Headers:** `Authorization: Bearer {admin_token}`

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/dashboard/admin/charts/orders?startDate=2024-01-01&endDate=2024-01-31" \
  -H "Authorization: Bearer ADMIN_TOKEN_HERE"
```

### 17. رسم بياني للعملاء
**GET** `/api/dashboard/admin/charts/customers?startDate={startDate}&endDate={endDate}`

**الوصف:** الحصول على بيانات الرسم البياني للعملاء.

**Headers:** `Authorization: Bearer {admin_token}`

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/dashboard/admin/charts/customers?startDate=2024-01-01&endDate=2024-01-31" \
  -H "Authorization: Bearer ADMIN_TOKEN_HERE"
```

### 18. رسم بياني للمنتجات
**GET** `/api/dashboard/admin/charts/products?startDate={startDate}&endDate={endDate}`

**الوصف:** الحصول على بيانات الرسم البياني للمنتجات.

**Headers:** `Authorization: Bearer {admin_token}`

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/dashboard/admin/charts/products?startDate=2024-01-01&endDate=2024-01-31" \
  -H "Authorization: Bearer ADMIN_TOKEN_HERE"
```

## Recent Activity Endpoints

### 19. الطلبات الحديثة (Admin)
**GET** `/api/dashboard/admin/recent-orders`

**الوصف:** الحصول على الطلبات الحديثة للمدير.

**Headers:** `Authorization: Bearer {admin_token}`

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/dashboard/admin/recent-orders" \
  -H "Authorization: Bearer ADMIN_TOKEN_HERE"
```

### 20. العملاء الجدد (Admin)
**GET** `/api/dashboard/admin/recent-customers`

**الوصف:** الحصول على العملاء الجدد للمدير.

**Headers:** `Authorization: Bearer {admin_token}`

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/dashboard/admin/recent-customers" \
  -H "Authorization: Bearer ADMIN_TOKEN_HERE"
```

### 21. أفضل المنتجات (Admin)
**GET** `/api/dashboard/admin/top-products`

**الوصف:** الحصول على أفضل المنتجات للمدير.

**Headers:** `Authorization: Bearer {admin_token}`

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/dashboard/admin/top-products" \
  -H "Authorization: Bearer ADMIN_TOKEN_HERE"
```

### 22. الطلبات الحديثة (User)
**GET** `/api/dashboard/user/recent-orders`

**الوصف:** الحصول على الطلبات الحديثة للمستخدم.

**Headers:** `Authorization: Bearer {user_token}`

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/dashboard/user/recent-orders" \
  -H "Authorization: Bearer USER_TOKEN_HERE"
```

### 23. التقييمات الحديثة (User)
**GET** `/api/dashboard/user/recent-reviews`

**الوصف:** الحصول على التقييمات الحديثة للمستخدم.

**Headers:** `Authorization: Bearer {user_token}`

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/dashboard/user/recent-reviews" \
  -H "Authorization: Bearer USER_TOKEN_HERE"
```

### 24. عناصر قائمة الأمنيات (User)
**GET** `/api/dashboard/user/wishlist-items`

**الوصف:** الحصول على عناصر قائمة الأمنيات للمستخدم.

**Headers:** `Authorization: Bearer {user_token}`

**مثال الاختبار:**
```bash
curl -X GET "http://localhost:5036/api/dashboard/user/wishlist-items" \
  -H "Authorization: Bearer USER_TOKEN_HERE"
```

## أمثلة اختبار شاملة

### سيناريو كامل للوحة تحكم المدير
```bash
# 1. الحصول على لوحة تحكم المدير
curl -X GET "http://localhost:5036/api/dashboard/admin" \
  -H "Authorization: Bearer ADMIN_TOKEN_HERE"

# 2. الحصول على الإحصائيات السريعة
curl -X GET "http://localhost:5036/api/dashboard/admin/quick-stats" \
  -H "Authorization: Bearer ADMIN_TOKEN_HERE"

# 3. تحليلات المبيعات
curl -X GET "http://localhost:5036/api/dashboard/admin/sales-analytics?startDate=2024-01-01&endDate=2024-01-31" \
  -H "Authorization: Bearer ADMIN_TOKEN_HERE"

# 4. تقرير المبيعات
curl -X GET "http://localhost:5036/api/dashboard/admin/reports/sales?startDate=2024-01-01&endDate=2024-01-31" \
  -H "Authorization: Bearer ADMIN_TOKEN_HERE"

# 5. رسم بياني للإيرادات
curl -X GET "http://localhost:5036/api/dashboard/admin/charts/revenue?startDate=2024-01-01&endDate=2024-01-31" \
  -H "Authorization: Bearer ADMIN_TOKEN_HERE"

# 6. الطلبات الحديثة
curl -X GET "http://localhost:5036/api/dashboard/admin/recent-orders" \
  -H "Authorization: Bearer ADMIN_TOKEN_HERE"
```

### سيناريو كامل للوحة تحكم المستخدم
```bash
# 1. الحصول على لوحة تحكم المستخدم
curl -X GET "http://localhost:5036/api/dashboard/user" \
  -H "Authorization: Bearer USER_TOKEN_HERE"

# 2. الحصول على الإحصائيات السريعة
curl -X GET "http://localhost:5036/api/dashboard/user/quick-stats" \
  -H "Authorization: Bearer USER_TOKEN_HERE"

# 3. طلبات المستخدم
curl -X GET "http://localhost:5036/api/dashboard/user/orders" \
  -H "Authorization: Bearer USER_TOKEN_HERE"

# 4. قائمة الأمنيات
curl -X GET "http://localhost:5036/api/dashboard/user/wishlist" \
  -H "Authorization: Bearer USER_TOKEN_HERE"

# 5. تقييمات المستخدم
curl -X GET "http://localhost:5036/api/dashboard/user/reviews" \
  -H "Authorization: Bearer USER_TOKEN_HERE"

# 6. الطلبات الحديثة
curl -X GET "http://localhost:5036/api/dashboard/user/recent-orders" \
  -H "Authorization: Bearer USER_TOKEN_HERE"
```

## رموز الاستجابة

| الكود | الوصف |
|-------|--------|
| 200 | نجح الطلب |
| 400 | خطأ في البيانات المرسلة |
| 401 | غير مصرح |
| 403 | ممنوع (غير مدير للوحات الإدارية) |
| 500 | خطأ في الخادم |

## ملاحظات مهمة
- جميع الطلبات تتطلب مصادقة
- لوحات المدير تتطلب صلاحيات Admin
- لوحات المستخدم متاحة لجميع المستخدمين المسجلين
- يمكن تحديد نطاق زمني للتحليلات والتقارير
- احرص على معالجة الأخطاء بشكل مناسب 