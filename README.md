
# 🏨 Wa7at ElDr3yah Booking & Financial Management System  
## نظام واحة الدرعية لإدارة الحجوزات والحسابات المالية

A full-stack web application for managing reservations, tracking revenues and expenses, calculating profit/loss, and generating monthly financial reports.

تطبيق ويب متكامل لإدارة الحجوزات، متابعة الإيرادات والمصروفات، حساب الأرباح والخسائر، وإنشاء تقارير مالية شهرية.

---

## 🚀 Overview | نظرة عامة

Wa7at ElDr3yah is a full-stack booking and financial management system designed for venues, rest houses, event spaces, and small businesses.

النظام مصمم لإدارة حجوزات الاستراحات أو القاعات أو الأماكن التجارية، مع متابعة مالية كاملة لكل المدفوعات والمصاريف.

---

## 🎯 Main Features | المميزات الأساسية

### 📅 Booking Management | إدارة الحجوزات
- Create, update, delete bookings  
- Prevent double booking for reserved dates  
- Show already booked dates in red on the calendar  
- Track customer name, phone number, booking type, paid amount, and remaining amount  

### 💰 Financial Management | الإدارة المالية
- Track revenues from paid booking amounts  
- Add and manage expenses manually  
- Add capital amount  
- Calculate total revenue, total expenses, net profit/loss, and profit/loss percentage  

### 📊 Reports | التقارير
- Generate monthly financial reports  
- Show all paid and deducted amounts  
- Display profit/loss result with positive and negative indicators  
- PDF report export support  

### 👥 Admin & Owner Access | صلاحيات الأدمن والأونر
- Admins manage bookings and financial data  
- Owner can manage admins and system access  
- No public customer accounts  

### 🔍 Audit Logs | تتبع العمليات
- Track create, update, and delete operations  
- Know which admin performed each action  
- Useful for internal monitoring and financial transparency  

---

## 🧱 Full Stack Architecture | معمارية المشروع

```txt
Frontend UI
   ↓
RESTful API
   ↓
Service Layer
   ↓
Entity Framework Core
   ↓
SQL Server Database
````

---

## 🖥️ Frontend | الواجهة الأمامية

The frontend provides a clean dashboard for admins and owners to manage bookings, financial records, reports, and calendar availability.

الفرونت إند بيعرض Dashboard بسيطة ومنظمة تساعد الأدمن أو الأونر في إدارة الحجوزات والحسابات والتقارير.

### Frontend Responsibilities

* Booking form
* Calendar with booked dates highlighted
* Expenses management pages
* Financial summary dashboard
* Monthly reports page
* Admin/Owner login interface
* Responsive user interface

---

## ⚙️ Backend | الباك إند

The backend is built as a RESTful API using ASP.NET Core Web API.
It handles business logic, validation, database operations, authentication, authorization, and report generation.

الباك إند معمول Web API وبيتعامل مع اللوجيك الأساسي للنظام زي منع تكرار الحجز، حساب المتبقي، حساب الأرباح والخسائر، وإدارة البيانات.

### Backend Responsibilities

* RESTful API endpoints
* Booking CRUD operations
* Prevent double booking
* Expense CRUD operations
* Capital management
* Financial calculations
* Monthly report generation
* JWT Authentication
* Role-based authorization
* Audit logging

---

## 🛠️ Technologies Used | التقنيات المستخدمة

### Frontend

* HTML
* CSS
* JavaScript
* React / Angular
* Bootstrap / Tailwind CSS
* Axios / Fetch API

### Backend

* C#
* ASP.NET Core Web API
* Entity Framework Core
* SQL Server
* JWT Authentication
* Swagger / OpenAPI
* Fluent API
* LINQ

---

## 🗄️ Database Tables | جداول قاعدة البيانات

```txt
Users
Bookings
Expenses
Capital
MonthlyReports
AuditLogs
```

---

## 📦 API Endpoints Examples | أمثلة على الـ APIs

```txt
GET     /api/bookings
GET     /api/bookings/{id}
POST    /api/bookings
PUT     /api/bookings/{id}
DELETE  /api/bookings/{id}

GET     /api/bookings/booked-dates

GET     /api/expenses
POST    /api/expenses
PUT     /api/expenses/{id}
DELETE  /api/expenses/{id}

GET     /api/finance/summary
GET     /api/reports/monthly
POST    /api/auth/login
```

---

## 🔐 Authentication & Authorization | تسجيل الدخول والصلاحيات

The system supports internal access only.
Only admins and owners can log in and manage the system.

النظام داخلي فقط، مفيش تسجيل حسابات للعملاء.
الأدمن والأونر فقط هم اللي يقدروا يدخلوا ويستخدموا السيستم.

### Roles

```txt
Owner
Admin
```

---

## 📊 Financial Logic | طريقة الحسابات

```txt
Remaining Amount = Total Price - Paid Amount

Total Revenue = Sum of paid booking amounts

Net Profit/Loss = Total Revenue - Total Expenses

Profit/Loss Percentage = Financial performance percentage
```

---

## ⚙️ Setup & Run | تشغيل المشروع

### Backend

```bash
dotnet restore
dotnet ef database update
dotnet run
```

Open Swagger:

```txt
https://localhost:7189/swagger
```

### Frontend

```bash
npm install
npm run dev
```

---

## 🔮 Future Enhancements | تطويرات مستقبلية

* Advanced dashboard analytics
* Real-time notifications using SignalR
* PDF report customization
* Multi-branch support
* Export to Excel
* Advanced filtering and search
* Dark mode

---

## 💡 Why This Project? | ليه المشروع ده مميز؟

This project solves a real business problem by replacing manual Excel-based booking and financial tracking with a clean, scalable, and professional full-stack system.

المشروع بيحل مشكلة حقيقية لأنه بيحوّل شغل الحجوزات والحسابات من Excel يدوي إلى سيستم كامل منظم وسهل التطوير.

---

## ⭐ Project Goal | هدف المشروع

To build a practical full-stack business management system that helps owners and admins manage bookings, money flow, profit, loss, and monthly reports in one place.

بناء نظام عملي يساعد أصحاب المشاريع والأدمنز يديروا الحجوزات والحسابات والتقارير من مكان واحد.

```
```
