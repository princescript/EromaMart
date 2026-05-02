# ?? EromaMart Backend API

A scalable backend system for an e-commerce platform built using .NET Web API, Clean Architecture principles, and JWT authentication.

---

## ?? Tech Stack

- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- JWT Authentication
- BCrypt Password Hashing
- Clean Architecture (Service + Repository pattern)
- C#

---

## ?? Current Completed Features

### Authentication System
- User Registration
- User Login
- Password Hashing (BCrypt)
- JWT Token Generation
- Basic validation (input checks, duplicate phone check)

---

## ?? Current Project Structure

Controller ? Service ? Repository ? Database


---

## ?? API Status (Completed)

### Auth APIs
- POST `/api/auth/register`
- POST `/api/auth/login`

---

# ?? Next Development Step-by-Step

---

## ?? Phase 1: Product Module  System (CORE FOUNDATION)

## relationships 

Product  (1) ──── (1) Inventory
Product  (1) ──── (N) Images
Product  (N) ──── (N) Tags

Category (1) ──── (N) Product
Brand    (1) ──── (N) Product
HSN      (1) ──── (N) Product




### Goal: Build product catalog

### Database Tables:
- mst_product
- mst_category

### Features:
- Add Product (Admin)
- Update Product
- Delete Product
- Get Product by skj
- Get All Products (Pagination)
- Filter Products:
  - by category
  - by price range
  - by search keyword

### API Endpoints:
- GET `/api/products`
- GET `/api/products/{id}`
- POST `/api/products`
- PUT `/api/products/{id}`
- DELETE `/api/products/{id}`

---

## ?? Phase 2: Category System

### Goal: Organize products

### Features:
- Create Category
- Update Category
- Get Categories
- Delete Category

### API Endpoints:
- GET `/api/categories`
- POST `/api/categories`
- PUT `/api/categories/{id}`
- DELETE `/api/categories/{id}`

---

## ?? Phase 3: Cart System

### Goal: User shopping cart

### Tables:
- mst_cart
- mst_cart_item

### Features:
- Add item to cart
- Remove item from cart
- Update quantity
- Get user cart

### API Endpoints:
- GET `/api/cart`
- POST `/api/cart/add`
- PUT `/api/cart/update`
- DELETE `/api/cart/remove`

---

## ?? Phase 4: Order System (CORE BUSINESS FLOW)

### Goal: Convert cart into orders

### Tables:
- mst_order
- mst_order_item

### Features:
- Place order from cart
- Order history
- Order status tracking

### Order Flow:
Cart ? Order ? Confirmation

### API Endpoints:
- POST `/api/orders/place`
- GET `/api/orders`
- GET `/api/orders/{id}`

---

## ?? Phase 5: Payment System (Basic)

### Goal: Payment tracking (no gateway initially)

### Features:
- Payment status tracking
- Order payment mapping

### Status:
- Pending
- Paid
- Failed

---

## ?? Phase 6: Admin Panel APIs

### Features:
- Manage users
- Manage products
- Manage orders
- Dashboard stats

---

## ?? Architecture Improvements (Later Stage)

- DTO validation layer (FluentValidation)
- AutoMapper implementation
- Redis caching
- Serilog logging
- Global exception middleware
- Standard API response model
- Pagination wrapper

---

## ?? Advanced Features (Future Scope)

- Refresh Token system
- Role-Based Access Control (Admin/User/Vendor)
- Payment gateway integration (Razorpay/Stripe)
- Inventory system
- Wishlist system
- Product reviews & ratings
- Microservice migration (optional)

---

## ?? Design Principles Followed

- Clean Architecture
- Separation of Concerns
- Repository Pattern
- Service Layer Pattern
- Secure Authentication (JWT)

---

## ?? Project Goal

To build a production-level e-commerce backend system that demonstrates:

- Real-world backend engineering
- Scalable system design
- Clean API architecture
- Strong authentication & business logic separation

---

## ?? Current Focus

?? Build **Product Module first**

Because everything depends on it:

Products ? Cart ? Orders ? Payment

---

## ????? Developer Notes

- Business logic stays in Service layer
- DB logic stays in Repository layer
- Avoid mixing responsibilities
- Design APIs for scaling from day one

