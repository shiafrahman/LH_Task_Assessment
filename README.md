# Full-Stack E-Commerce Assessment – Lemon Hive

This is a full-stack e-commerce application built as part of the Full-Stack assessment task for **Lemon Hive**. The application is implemented using **.NET 8.0**, with a **Razor front-end** and a **Web API backend**, following RESTful principles and using **SQL Server** for data storage.

## Features Implemented

### Frontend (Razor)
- Built using ASP.NET Core Razor pages (MVC pattern)
- Product listing with:
  - **Pagination**
  - **Search functionality**
- **Add Product** form with image upload
- **Add to Cart** functionality
- **Cart icon with item count update**
- **Cart view page** that displays:
  - Product image, name, quantity, price, discount, total
- **Date-based discount**:
  - If current date is within the product's discount period, a percentage discount is applied
  - If not, original price is shown
 
###  Backend (Web API)
- Built with ASP.NET Core Web API (.NET 8.0)
- RESTful endpoints for:
  - `Product` and `Cart` schemas
  - Add, Update, Delete products
  - Add to cart and fetch cart items
  - Search, filter, and paginate products
- Swagger UI for testing and documentation

## Project Structure
AssessmentApp.sln
Assessment_Backend --> ASP.NET Core Web API (Startup project)
Assessment_Frontend --> ASP.NET Core Razor Front-End (Startup project)

## Tech Stack

- .NET 8.0
- Razor Pages (MVC)
- Entity Framework Core
- SQL Server
- Swagger
- Bootstrap
- jQuery, JavaScript
  
## ✅ Setup Instructions
### 1. Clone the repository
### 2. Open the solution
Open AssessmentApp.sln using Visual Studio 2022.

### 3. Set dual startup projects
Right-click the solution > Set Startup Projects

Select Multiple startup projects

Set:

Assessment_Backend → Start

Assessment_Frontend → Start

### 4. Configure database and run migrations
Update your connection string in appsettings.json (Backend project), then run in package manager console: update-database

### Author
Md. Shiafur Rahman

Software Engineer – Mediasoft Data System Limited

Email: shiafrahman@gmail.com






