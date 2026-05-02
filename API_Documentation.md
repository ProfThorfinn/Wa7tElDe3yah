# Wa7at ElDr3yah API Documentation

## 📌 Overview

This API is designed for managing bookings, expenses, capital, users, and financial reporting.

---

## 🌐 Base URL

```txt
https://your-domain.com
```

> Replace with ngrok link or deployed server URL.

---

## 🔐 Authentication

All protected endpoints require JWT Bearer Token.

```txt
Authorization: Bearer YOUR_TOKEN
```

---

## 🔑 Auth APIs

### Login

```http
POST /api/Auth/login
```

**Request**

```json
{
  "email": "user@gmail.com",
  "password": "Password@123"
}
```

**Response**

```json
{
  "token": "JWT_TOKEN",
  "userId": 1,
  "role": "Admin"
}
```

---

### Forgot Password

```http
POST /api/Auth/forgot-password
```

**Rules**

* Sends reset code to email
* Code expires after limited time
* Rate limiting applied

---

### Reset Password

```http
POST /api/Auth/reset-password
```

**Rules**

* Code must be valid
* Password stored as BCrypt hash

---

## 📅 Bookings

### Get All Bookings

```http
GET /api/Bookings
```

### Get Booking By ID

```http
GET /api/Bookings/{id}
```

### Create Booking

```http
POST /api/Bookings
```

**Request**

```json
{
  "customerName": "Mahmoud",
  "contactNumber": "01000000000",
  "bookingDate": "2026-05-10T00:00:00",
  "totalPrice": 5000,
  "paidAmount": 2000
}
```

---

### Update Booking

```http
PUT /api/Bookings/{id}
```

---

### Delete Booking

```http
DELETE /api/Bookings/{id}
```

---

### Get Booked Dates

```http
GET /api/Bookings/booked-dates
```

---

### Filter Bookings

```http
GET /api/Bookings/filter?from=2026-05-01&to=2026-05-31&status=Booked
```

---

### Search Bookings

```http
GET /api/Bookings/search?keyword=Mahmoud
```

---

## 💰 Capitals

### Get All

```http
GET /api/Capitals
```

### Create

```http
POST /api/Capitals
```

**Rules**

* Amount > 0
* Linked to logged-in user

---

### Get By ID

```http
GET /api/Capitals/{id}
```

### Update

```http
PUT /api/Capitals/{id}
```

### Delete

```http
DELETE /api/Capitals/{id}
```

### Total Capital

```http
GET /api/Capitals/total
```

---

## 💸 Expenses

### Get All

```http
GET /api/Expenses
```

### Create Expense

```http
POST /api/Expenses
```

**Rules**

* Amount > 0
* Linked to logged-in user

---

### Filter Expenses

```http
GET /api/Expenses/filter
```

Example:

```txt
?keyword=clean&category=Maintenance&minAmount=100
```

---

### Pagination

```http
GET /api/Expenses/paged?pageNumber=1&pageSize=10
```

---

### Total Expenses

```http
GET /api/Expenses/total
```

---

### Monthly Expenses

```http
GET /api/Expenses/total-by-month?month=5&year=2026
```

---

### Recent Expenses

```http
GET /api/Expenses/recent
```

---

### Top Expenses

```http
GET /api/Expenses/top
```

---

## 📊 Finance

### Summary

```http
GET /api/Finance/summary
```

Returns:

* Total Capital
* Revenue
* Expenses
* Profit / Loss

---

### Monthly Summary

```http
GET /api/Finance/summary-by-month?month=5&year=2026
```

---

## 📈 Monthly Reports

### Get All Reports

```http
GET /api/MonthlyReports
```

---

### Get Report By ID

```http
GET /api/MonthlyReports/{id}
```

---

### Generate Report

```http
POST /api/MonthlyReports/generate?month=5&year=2026
```

Features:

* Generates PDF
* Saves report in database

---

### Export PDF

```http
GET /api/MonthlyReports/export-pdf?month=5&year=2026
```

---

## 👤 Users

### Get All Users

```http
GET /api/Users
```

---

### Create User

```http
POST /api/Users
```

**Rules**

* Email must be unique
* Password is hashed
* Role: Admin / Owner

---

### Get User By ID

```http
GET /api/Users/{id}
```

---

### Update User

```http
PUT /api/Users/{id}
```

---

### Delete User

```http
DELETE /api/Users/{id}
```

---

### Toggle User Status

```http
PATCH /api/Users/{id}/toggle-status
```

---

## ⚠️ Status Codes

```txt
200 → Success  
400 → Bad Request  
401 → Unauthorized  
404 → Not Found  
500 → Server Error  
```

---

## 📌 Notes

* All endpoints (except login) require authentication
* Use JWT token in Authorization header
* Dates should be in ISO format
* Pagination supported in expenses

---

## 🚀 How to Use

1. Login to get JWT token
2. Add token to headers
3. Call protected endpoints

---

## 🧪 Testing

You can test the API using:

* Postman
* Swagger UI
* Any HTTP client

---

## 📎 Swagger JSON

Use the provided Swagger file to import APIs into Postman or Swagger UI.

---

## 👨‍💻 Author

Backend Developer
ASP.NET Core API
