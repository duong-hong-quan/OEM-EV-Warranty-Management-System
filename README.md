# Electric Vehicle Warranty Management System
## Hệ thống Quản lý Bảo hành Xe điện

---

## 🎯 Giới thiệu / Introduction

### Tiếng Việt

Đây là dự án demo được phát triển bởi **Dương Hồng Quân** - Team Leader tại **TGL Solutions**, Middle Software Engineer. Buổi demo tập trung vào việc xây dựng và triển khai .NET API với các best practices trong phát triển phần mềm .

### English

This is a demo project developed by **Dương Hồng Quân** - Team Leader at **TGL Solutions**, Middle Software Engineer. The demo focuses on building and deploying .NET API with modern software development best practices.

---

## 🏗️ Kiến trúc Hệ thống / System Architecture

### Tiếng Việt

Hệ thống được thiết kế theo kiến trúc N-Layer với các nguyên tắc Clean Architecture:

- **Presentation Layer (BE.API)**: Controllers, Authentication, Authorization
- **Business Logic Layer (BE.Services)**: Business rules, Service interfaces
- **Data Access Layer (BE.DAL)**: Repository pattern, Entity Framework Core
- **Common Layer (BE.Common)**: DTOs, Shared models
- **Frontend (FE)**: React.js với Vite, TailwindCSS

### English

The system is designed following N-Layer architecture with Clean Architecture principles:

- **Presentation Layer (BE.API)**: Controllers, Authentication, Authorization
- **Business Logic Layer (BE.Services)**: Business rules, Service interfaces
- **Data Access Layer (BE.DAL)**: Repository pattern, Entity Framework Core
- **Common Layer (BE.Common)**: DTOs, Shared models
- **Frontend (FE)**: React.js with Vite, TailwindCSS

---

## 🎨 Thiết kế Database / Database Design

### Tiếng Việt

#### Lý do thiết kế:

1. **Tách biệt concerns**: Mỗi entity có trách nhiệm riêng biệt
2. **Scalability**: Dễ dàng mở rộng và thêm tính năng mới
3. **Maintainability**: Dễ bảo trì và debug
4. **Performance**: Tối ưu hóa query và indexing

#### Các Entity chính:

- **Users**: Quản lý người dùng và phân quyền
- **Customers**: Thông tin khách hàng
- **Vehicles**: Thông tin xe điện
- **Parts**: Linh kiện và phụ tùng
- **ServiceHistory**: Lịch sử bảo trì
- **WarrantyClaims**: Yêu cầu bảo hành

### English

#### Design Rationale:

1. **Separation of Concerns**: Each entity has distinct responsibilities
2. **Scalability**: Easy to extend and add new features
3. **Maintainability**: Easy to maintain and debug
4. **Performance**: Optimized queries and indexing

#### Main Entities:

- **Users**: User management and authorization
- **Customers**: Customer information
- **Vehicles**: Electric vehicle information
- **Parts**: Components and spare parts
- **ServiceHistory**: Maintenance history
- **WarrantyClaims**: Warranty claim requests

---

## 🔧 Tư duy thiết kế API / API Design Philosophy

### Tiếng Việt

#### Nguyên tắc thiết kế:

1. **RESTful API**: Tuân thủ chuẩn REST với HTTP methods phù hợp
2. **Clean Endpoints**: Tên endpoint rõ ràng, dễ hiểu
3. **Consistent Response**: Cấu trúc response thống nhất
4. **Error Handling**: Xử lý lỗi toàn diện với status codes phù hợp
5. **Authentication & Authorization**: JWT-based với role-based access control
6. **Documentation**: Swagger/OpenAPI documentation
7. **Validation**: Input validation với Data Annotations
8. **Separation of Concerns**: Tách biệt Controller - Service - Repository

#### Cấu trúc API:

```
/api/vehicles          - Quản lý xe điện
/api/customers         - Quản lý khách hàng  
/api/parts            - Quản lý linh kiện
/api/servicehistory   - Lịch sử bảo trì
/api/warrantyclaims   - Yêu cầu bảo hành
/api/auth             - Authentication
```

### English

#### Design Principles:

1. **RESTful API**: Following REST standards with appropriate HTTP methods
2. **Clean Endpoints**: Clear, understandable endpoint names
3. **Consistent Response**: Unified response structure
4. **Error Handling**: Comprehensive error handling with appropriate status codes
5. **Authentication & Authorization**: JWT-based with role-based access control
6. **Documentation**: Swagger/OpenAPI documentation
7. **Validation**: Input validation with Data Annotations
8. **Separation of Concerns**: Separated Controller - Service - Repository

#### API Structure:

```
/api/vehicles          - Vehicle management
/api/customers         - Customer management  
/api/parts            - Parts management
/api/servicehistory   - Service history
/api/warrantyclaims   - Warranty claims
/api/auth             - Authentication
```

---

## 📋 API Endpoints

### Vehicles API
```http
GET    /api/vehicles                    # Get all vehicles
GET    /api/vehicles/{id}               # Get vehicle by ID
POST   /api/vehicles                    # Create new vehicle
PUT    /api/vehicles/{id}               # Update vehicle
DELETE /api/vehicles/{id}               # Delete vehicle
GET    /api/vehicles/{id}/parts         # Get vehicle parts
POST   /api/vehicles/{id}/parts         # Add part to vehicle
PUT    /api/vehicles/parts/{partId}     # Update vehicle part
DELETE /api/vehicles/parts/{partId}     # Remove part from vehicle
```

### Customers API
```http
GET    /api/customers                   # Get all customers (paginated)
GET    /api/customers/{id}              # Get customer by ID
POST   /api/customers                   # Create new customer
PUT    /api/customers/{id}              # Update customer
DELETE /api/customers/{id}              # Delete customer
```

### Parts API
```http
GET    /api/parts                       # Get all parts
GET    /api/parts/{id}                  # Get part by ID
POST   /api/parts                       # Create new part
PUT    /api/parts/{id}                  # Update part
DELETE /api/parts/{id}                  # Delete part
```

### Authentication API
```http
POST   /api/auth/login                  # User login
POST   /api/auth/register               # User registration
POST   /api/auth/refresh                # Refresh token
POST   /api/auth/logout                 # User logout
GET    /api/auth/demo/401               # Demo 401 Unauthorized
GET    /api/auth/demo/403               # Demo 403 Forbidden
```

---

## 🛠️ Công nghệ sử dụng / Technology Stack

### Backend
- **.NET 8.0** - Main framework
- **ASP.NET Core** - Web API framework
- **Entity Framework Core** - ORM
- **SQL Server** - Database
- **JWT Authentication** - Security
- **Swagger/OpenAPI** - API documentation

### Frontend
- **React 19** - UI library
- **Vite** - Build tool
- **TailwindCSS** - CSS framework
- **Axios** - HTTP client
- **React Router** - Navigation
- **React Hook Form** - Form handling

### DevOps
- **Docker** - Containerization
- **GitHub Actions** - CI/CD
- **Nginx** - Reverse proxy
- **LXC** - Container runtime

---

## 🚀 Hướng dẫn cài đặt / Installation Guide

### Tiếng Việt

#### Yêu cầu hệ thống:
- .NET 8.0 SDK
- Node.js 18+
- SQL Server
- Docker (optional)

#### Cài đặt Backend:
```bash
cd be/BE.API
dotnet restore
dotnet ef database update
dotnet run
```

#### Cài đặt Frontend:
```bash
cd fe
npm install
npm run dev
```

### English

#### System Requirements:
- .NET 8.0 SDK
- Node.js 18+
- SQL Server
- Docker (optional)

#### Backend Setup:
```bash
cd be/BE.API
dotnet restore
dotnet ef database update
dotnet run
```

#### Frontend Setup:
```bash
cd fe
npm install
npm run dev
```

---

## 📚 Tài liệu tham khảo / Documentation References

### Configuration & Setup
- [🔧 Configuration Guide](./be/BE.API/CONFIGURATION_GUIDE.md) - Hướng dẫn cấu hình hệ thống
- [⚙️ Setup Configuration](./be/BE.API/SETUP_CONFIG.md) - Cài đặt và cấu hình ban đầu
- [🗄️ Database Setup](./be/BE.API/DATABASE_SETUP.md) - Thiết lập cơ sở dữ liệu

### Authentication & Security
- [🔐 Authentication Demo](./be/BE.API/AUTH_DEMO.md) - Demo hệ thống xác thực và phân quyền

### DevOps & Deployment
- [🚀 CI/CD Pipeline](./CI_CD_README.md) - Hướng dẫn CI/CD với GitHub Actions
- [📦 Deployment Guide](./deployment/DEPLOYMENT_GUIDE.md) - Hướng dẫn triển khai production
- [🔑 SSH Password Setup](./deployment/SSH_PASSWORD_SETUP.md) - Cấu hình SSH cho deployment

### Project Documentation
- [📋 Project Structure](./PROJECT_STRUCTURE.md) - Cấu trúc dự án chi tiết
- [🏗️ Architecture Overview](./ARCHITECTURE.md) - Tổng quan kiến trúc hệ thống

---

## 🎯 Demo Features / Tính năng Demo

### Tiếng Việt
- ✅ JWT Authentication với refresh token
- ✅ Role-based Authorization (Admin, Manager, Technician, Customer)
- ✅ RESTful API với đầy đủ CRUD operations
- ✅ Clean Architecture với Dependency Injection
- ✅ Entity Framework Core với Code-First approach
- ✅ Comprehensive error handling
- ✅ API documentation với Swagger
- ✅ Docker containerization
- ✅ CI/CD pipeline với GitHub Actions
- ✅ Responsive React frontend

### English
- ✅ JWT Authentication with refresh token
- ✅ Role-based Authorization (Admin, Manager, Technician, Customer)
- ✅ RESTful API with full CRUD operations
- ✅ Clean Architecture with Dependency Injection
- ✅ Entity Framework Core with Code-First approach
- ✅ Comprehensive error handling
- ✅ API documentation with Swagger
- ✅ Docker containerization
- ✅ CI/CD pipeline with GitHub Actions
- ✅ Responsive React frontend

---

## 👥 Demo Users / Tài khoản Demo

```javascript
// Admin Account
{
  "email": "admin@warranty.com",
  "password": "Admin123!",
  "role": "Admin"
}

// Manager Account
{
  "email": "manager@warranty.com", 
  "password": "Manager123!",
  "role": "Manager"
}

// Technician Account
{
  "email": "tech@warranty.com",
  "password": "Tech123!",
  "role": "Technician"
}

// Customer Account
{
  "email": "customer@warranty.com",
  "password": "Customer123!",
  "role": "Customer"
}
```

---

## 🔗 Links & Resources

- **API Documentation**: `http://localhost:5000/swagger`
- **Frontend Application**: `http://localhost:3000`
- **GitHub Repository**: `https://github.com/duong-hong-quan/OEM-EV-Warranty-Management-System`
- **Company Website**: `https://tgl-sol.com`


## 🤝 Đóng góp / Contributing

### Tiếng Việt
Mọi đóng góp đều được chào đón! Vui lòng tạo issue hoặc pull request.

### English
All contributions are welcome! Please create an issue or pull request.

---

## 📞 Liên hệ / Contact

- **Tác giả**: Dương Hồng Quân
- **Vị trí**: Team Leader, Middle Software Engineer
- **Công ty**: TGL Solutions
- **Email**: [quan.h.duong@tgl-sol.com]
- **LinkedIn**: [[Quan Duong](https://www.linkedin.com/in/hongquan0312/)]

---

## 📄 Giấy phép / License

Copyright © 2025 Dương Hồng Quân & TGL Solutions. All rights reserved.

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## 🙏 Acknowledgments

- **TGL Solutions** - For supporting this demo project
- **Microsoft** - For .NET and related technologies
- **React Team** - For the amazing React library
- **Community Contributors** - For open source libraries and tools

---

*Dự án này được phát triển nhằm mục đích demo và giáo dục. Không sử dụng trong môi trường production mà không có sự xem xét kỹ lưỡng.*

*This project is developed for demo and educational purposes. Do not use in production without thorough review.*
