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

Product (1) ──── (N) Images
Category (1) ──── (N) Product
Brand    (1) ──── (N) Product

HSN      (1) ──── (N) Product
Product  (N) ──── (N) Tags
Product (1) ──── (N) Reviews

Product (N) ──── (N) Warehouse
           ↓
        Inventory


Master Data (Independent)
	Category
	Brand

Domain Core (Aggregate)
	Product

Child Entities
	ProductImages
	Inventory


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






















---

# 🧠 Product + Inventory Design Notes (Backend)

---

# 1. Core Idea

```plaintext
Product = what you sell
Inventory = how much you have
```

👉 They are **related but separate domains**

---

# 2. Phase 1 Goal (Current Stage)

Build:

```plaintext
Product Module (core)
+ Basic Inventory (simple)
```

---

# 3. Database Design

## Product Relationships

```plaintext
Product (1) ──── (N) Images
Category (1) ──── (N) Product
Brand    (1) ──── (N) Product
HSN      (1) ──── (N) Product
Product  (N) ──── (N) Tags
Product  (1) ──── (N) Reviews
```

---

## Inventory (Current Phase)

```plaintext
Product (1) ──── (1) Inventory
```

Table:

```plaintext
mst_inventory
 ├── product_id (UNIQUE)
 ├── quantity
 ├── is_active
 ├── audit fields
```

---

## Inventory (Future)

```plaintext
Product (N) ──── (N) Warehouse
           ↓
        Inventory
```

---

# 4. API Design

## Product API

```plaintext
POST /products
```

✔ Creates product
✔ System auto-creates inventory

---

## Inventory APIs

```plaintext
POST /inventory/stock-in
POST /inventory/stock-out
GET  /inventory/{productId}
```

---

## ❌ Do NOT create

```plaintext
POST /inventory/create   ❌
```

---

# 5. Internal Service Design

## Product Service

```plaintext
CreateProduct()
   ↓
Save Product
   ↓
Call InventoryService.CreateInventory()
```

---

## Inventory Service

```plaintext
CreateInventory(productId)   ← internal only

StockIn(productId, qty)
StockOut(productId, qty)
```

---

# 6. Flow

## Product Creation

```plaintext
Admin → Create Product
        ↓
System → Save Product
        ↓
System → Create Inventory (qty = 0)
```

---

## Stock Flow

```plaintext
Add Stock:
Admin → StockIn → quantity +

Reduce Stock:
Admin → StockOut → quantity -
```

---

# 7. Rules (Important)

### ✔ Must follow

* Inventory is **auto-created**
* Inventory is **separate table**
* Stock is updated via **operations**, not direct edit

---

### ❌ Avoid

* Creating inventory manually via API
* Storing quantity inside product
* Direct DB updates without logic

---

# 8. Design Principles

### 1. Separation of concerns

```plaintext
Product ≠ Inventory
```

---

### 2. System vs Admin responsibility

```plaintext
System:
- Create inventory

Admin:
- Add stock
- Reduce stock
```

---

### 3. Behavior over CRUD

Instead of:

```plaintext
UpdateInventory ❌
```

Use:

```plaintext
StockIn ✔
StockOut ✔
```

---

# 9. Future Upgrade Path

Later you will add:

### Inventory upgrade

```plaintext
reserved_quantity
available_quantity
```

---

### Warehouse support

```plaintext
product_id + warehouse_id
```

---

### Advanced operations

```plaintext
Reserve
Confirm
Release
```

---

### Audit system

```plaintext
tran_inventory (history table)
```

---

# 10. Final Architecture (Simple)

```plaintext
Controller
   ↓
Service
   ↓
Repository
   ↓
Database
```

---

# 11. Key Takeaways

* ✔ Inventory is **system-managed**
* ✔ No separate create-inventory API
* ✔ Stock handled via **StockIn / StockOut**
* ✔ Start simple, design for future
* ✔ You are building **Phase 1 system correctly**

---

# 🔚 Final Summary (one line)

👉 **Product is created by admin, Inventory is created by system, Stock is controlled by operations.**

---










#region
//namespace Server.Services;

//public interface ICloudinaryService
//{
//    Task<List<string>> UploadMultipleAsync(List<IFormFile> files);
//}

//public class CloudinaryService : ICloudinaryService
//{
//    private readonly HttpClient _http;
//    private readonly IConfiguration _config;

//    public CloudinaryService(HttpClient http, IConfiguration config)
//    {
//        _http = http;
//        _config = config;
//    }

//    public async Task<List<string>> UploadMultipleAsync(List<IFormFile> files)
//    {
//        if (files == null || files.Count == 0)
//            throw new ArgumentException("No files provided");

//        var cloudName = _config["Cloudinary:CloudName"];
//        var uploadPreset = _config["Cloudinary:UploadPreset"];

//        if (string.IsNullOrWhiteSpace(cloudName))
//            throw new Exception("Cloudinary CloudName is missing");

//        if (string.IsNullOrWhiteSpace(uploadPreset))
//            throw new Exception("Cloudinary UploadPreset is missing");

//        var url = $"https://api.cloudinary.com/v1_1/{cloudName}/image/upload";

//        var resultList = new List<string>();

//        foreach (var file in files)
//        {
//            if (file == null || file.Length == 0)
//                continue;

//            using var form = new MultipartFormDataContent();

//            // ✅ preset (must be first for Cloudinary unsigned upload)
//            var presetContent = new StringContent(uploadPreset);
//            presetContent.Headers.ContentDisposition =
//                new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
//                {
//                    Name = "\"upload_preset\""
//                };
//            form.Add(presetContent);

//            // ✅ file
//            await using var stream = file.OpenReadStream();
//            var fileContent = new StreamContent(stream);

//            fileContent.Headers.ContentType =
//                new System.Net.Http.Headers.MediaTypeHeaderValue(
//                    file.ContentType ?? "application/octet-stream"
//                );

//            fileContent.Headers.ContentDisposition =
//                new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
//                {
//                    Name = "\"file\"",
//                    FileName = $"\"{file.FileName}\""
//                };

//            form.Add(fileContent);

//            // request
//            var response = await _http.PostAsync(url, form);
//            var result = await response.Content.ReadAsStringAsync();

//            if (!response.IsSuccessStatusCode)
//                throw new Exception($"Cloudinary upload failed: {result}");

//            using var json = System.Text.Json.JsonDocument.Parse(result);

//            if (json.RootElement.TryGetProperty("secure_url", out var urlElement))
//            {
//                var secureUrl = urlElement.GetString();
//                if (!string.IsNullOrWhiteSpace(secureUrl))
//                    resultList.Add(secureUrl);
//            }
//        }

//        return resultList;
//    }
//}

#endregion