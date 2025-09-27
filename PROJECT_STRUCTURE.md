# Project Structure - Electric Vehicle Warranty System
## Cấu trúc Dự án - Hệ thống Quản lý Bảo hành Xe điện

---

## 📁 Overall Structure / Cấu trúc Tổng quan

```
seminar/
├── 📂 be/                          # Backend (.NET)
│   ├── 📂 BE.API/                  # Web API Layer
│   ├── 📂 BE.Services/             # Business Logic Layer  
│   ├── 📂 BE.DAL/                  # Data Access Layer
│   ├── 📂 BE.Common/               # Common/Shared Layer
│   └── 📄 Dockerfile               # Backend Docker config
├── 📂 fe/                          # Frontend (React)
│   ├── 📂 src/                     # Source code
│   ├── 📂 public/                  # Static assets
│   ├── 📄 Dockerfile               # Frontend Docker config
│   └── 📄 nginx.conf               # Nginx configuration
├── 📂 .github/workflows/           # GitHub Actions CI/CD
├── 📂 deployment/                  # Deployment scripts & configs
├── 📄 README.md                    # Main documentation
├── 📄 LICENSE                      # Project license
└── 📄 *.md                        # Various documentation files
```

---

## 🏗️ Backend Structure (be/)

### BE.API - Web API Layer
```
BE.API/
├── 📂 Controllers/                 # API Controllers
│   ├── 📄 AuthController.cs       # Authentication endpoints
│   ├── 📄 VehiclesController.cs   # Vehicle management
│   ├── 📄 CustomersController.cs  # Customer management
│   ├── 📄 PartsController.cs      # Parts management
│   ├── 📄 ServiceHistoryController.cs
│   └── 📄 WarrantyClaimsController.cs
├── 📂 Properties/                  # Launch settings
├── 📂 bin/                        # Compiled binaries
├── 📂 obj/                        # Build objects
├── 📄 Program.cs                  # Application entry point
├── 📄 appsettings.json            # Configuration
├── 📄 appsettings.Development.json # Dev configuration
├── 📄 BE.API.csproj               # Project file
├── 📄 BE.API.sln                  # Solution file
└── 📄 *.md                       # Documentation files
```

### BE.Services - Business Logic Layer
```
BE.Services/
├── 📂 Services/                   # Service interfaces & implementations
│   ├── 📂 Implementation/         # Service implementations
│   │   ├── 📄 AuthService.cs     # Authentication service
│   │   ├── 📄 VehicleService.cs  # Vehicle business logic
│   │   ├── 📄 CustomerService.cs # Customer business logic
│   │   ├── 📄 PartService.cs     # Parts business logic
│   │   ├── 📄 ServiceHistoryService.cs
│   │   └── 📄 WarrantyClaimService.cs
│   ├── 📄 IAuthService.cs         # Auth service interface
│   ├── 📄 IVehicleService.cs      # Vehicle service interface
│   ├── 📄 ICustomerService.cs     # Customer service interface
│   ├── 📄 IPartService.cs         # Part service interface
│   ├── 📄 IServiceHistoryService.cs
│   └── 📄 IWarrantyClaimService.cs
├── 📄 BaseService.cs              # Base service class
└── 📄 BE.Services.csproj          # Project file
```

### BE.DAL - Data Access Layer
```
BE.DAL/
├── 📂 Models/                     # Entity models
│   ├── 📄 WarrantyDbContext.cs   # EF DbContext
│   ├── 📄 User.cs                # User entity
│   ├── 📄 RefreshToken.cs        # Refresh token entity
│   ├── 📄 Vehicle.cs             # Vehicle entity
│   ├── 📄 Customer.cs            # Customer entity
│   ├── 📄 Part.cs                # Part entity
│   ├── 📄 ServiceHistory.cs      # Service history entity
│   └── 📄 WarrantyClaim.cs       # Warranty claim entity
├── 📂 Migrations/                 # EF Core migrations
├── 📂 GenericRepository/          # Generic repository pattern
│   ├── 📄 IGenericRepository.cs  # Repository interface
│   └── 📄 GenericRepository.cs   # Repository implementation
├── 📂 UOW/                       # Unit of Work pattern
├── 📂 DTO/                       # Data Transfer Objects
│   ├── 📄 QueryOptions.cs        # Query configuration
│   └── 📄 PagedResult.cs         # Pagination result
└── 📄 BE.DAL.csproj              # Project file
```

### BE.Common - Shared Layer
```
BE.Common/
├── 📄 AuthDTOs.cs                # Authentication DTOs
├── 📄 VehicleDTO.cs              # Vehicle DTOs
├── 📄 CustomerDTO.cs             # Customer DTOs  
├── 📄 PartDTO.cs                 # Part DTOs
├── 📄 ServiceHistoryDTO.cs       # Service history DTOs
├── 📄 WarrantyClaimDTO.cs        # Warranty claim DTOs
└── 📄 BE.Common.csproj           # Project file
```

---

## 🎨 Frontend Structure (fe/)

### Source Code (src/)
```
src/
├── 📂 components/                 # React components
│   ├── 📂 ui/                    # UI components (shadcn/ui)
│   │   ├── 📄 button.jsx         # Button component
│   │   ├── 📄 card.jsx           # Card component
│   │   ├── 📄 form.jsx           # Form component
│   │   ├── 📄 input.jsx          # Input component
│   │   ├── 📄 label.jsx          # Label component
│   │   ├── 📄 table.jsx          # Table component
│   │   ├── 📄 alert.jsx          # Alert component
│   │   ├── 📄 badge.jsx          # Badge component
│   │   ├── 📄 scroll-area.jsx    # Scroll area component
│   │   └── 📄 textarea.jsx       # Textarea component
│   ├── 📄 AdminLayout.jsx        # Admin layout wrapper
│   └── 📄 StatCard.jsx           # Statistics card component
├── 📂 pages/                     # Page components
│   ├── 📄 Dashboard.jsx          # Dashboard page
│   ├── 📄 VehicleRegistration.jsx # Vehicle management
│   ├── 📄 PartAttachment.jsx     # Parts management
│   ├── 📄 ServiceHistory.jsx     # Service history
│   └── 📄 WarrantyClaim.jsx      # Warranty claims
├── 📂 lib/                       # Utility libraries
│   └── 📄 utils.js               # Utility functions
├── 📂 assets/                    # Static assets
│   └── 📄 react.svg              # React logo
├── 📄 App.jsx                    # Main App component
├── 📄 App.css                    # Global styles
└── 📄 main.jsx                   # Application entry point
```

### Configuration Files
```
fe/
├── 📄 package.json               # NPM dependencies
├── 📄 package-lock.json          # NPM lock file
├── 📄 vite.config.js             # Vite configuration
├── 📄 tailwind.config.js         # TailwindCSS config
├── 📄 postcss.config.js          # PostCSS configuration
├── 📄 eslint.config.js           # ESLint configuration
├── 📄 jsconfig.json              # JavaScript config
├── 📄 components.json            # Component config
├── 📄 index.html                 # HTML template
└── 📄 nginx.conf                 # Nginx configuration
```

---

## 🚀 DevOps Structure

### GitHub Actions (.github/workflows/)
```
.github/workflows/
└── 📄 ci-cd.yml                  # CI/CD pipeline configuration
```

### Deployment (deployment/)
```
deployment/
├── 📄 deploy.sh                  # Main deployment script
├── 📄 backup.sh                  # Database backup script
├── 📄 restore.sh                 # Database restore script
├── 📄 setup-server.sh            # Server setup script
├── 📄 test-ssh-connection.sh     # SSH connection test
├── 📄 docker-compose.yml         # Docker Compose config
├── 📄 .env.development           # Development environment variables
├── 📄 .env.production            # Production environment variables
├── 📄 DEPLOYMENT_GUIDE.md        # Deployment documentation
└── 📄 SSH_PASSWORD_SETUP.md      # SSH setup guide
```

---

## 📚 Documentation Files

```
Root Documentation/
├── 📄 README.md                  # Main project documentation
├── 📄 LICENSE                    # Project license
├── 📄 PROJECT_STRUCTURE.md       # This file
├── 📄 CI_CD_README.md           # CI/CD documentation
├── 📄 ARCHITECTURE.md           # Architecture overview
├── 📄 .gitignore               # Git ignore rules

Backend Documentation/
├── 📄 AUTH_DEMO.md              # Authentication demo
├── 📄 CONFIGURATION_GUIDE.md    # Configuration guide
├── 📄 SETUP_CONFIG.md           # Setup guide
└── 📄 DATABASE_SETUP.md         # Database setup
```

---

## 🔧 Key Files Description / Mô tả Files Quan trọng

### Tiếng Việt

#### Backend Key Files:
- **Program.cs**: Entry point của ứng dụng, cấu hình DI container, middleware
- **WarrantyDbContext.cs**: EF Core DbContext, định nghĩa database schema
- **Controllers/**: Xử lý HTTP requests, routing, validation
- **Services/**: Business logic, xử lý nghiệp vụ chính
- **DTOs**: Data transfer objects để truyền dữ liệu giữa các layers

#### Frontend Key Files:
- **App.jsx**: Component chính của React application
- **main.jsx**: Entry point của React app
- **pages/**: Các trang chính của ứng dụng
- **components/ui/**: Reusable UI components

### English

#### Backend Key Files:
- **Program.cs**: Application entry point, DI container configuration, middleware
- **WarrantyDbContext.cs**: EF Core DbContext, database schema definition
- **Controllers/**: Handle HTTP requests, routing, validation
- **Services/**: Business logic, main business processing
- **DTOs**: Data transfer objects for data exchange between layers

#### Frontend Key Files:
- **App.jsx**: Main React application component
- **main.jsx**: React application entry point
- **pages/**: Main application pages
- **components/ui/**: Reusable UI components

---

## 🏗️ Architecture Patterns Used / Mẫu Kiến trúc Sử dụng

### Backend Patterns:
- **N-Layer Architecture**: Phân tách thành các lớp rõ ràng
- **Repository Pattern**: Trừu tượng hóa data access
- **Unit of Work Pattern**: Quản lý transactions
- **Dependency Injection**: Loose coupling, testability
- **DTO Pattern**: Data transfer và validation

### Frontend Patterns:
- **Component-Based Architecture**: Tái sử dụng components
- **Container/Presentational Pattern**: Tách logic và UI
- **Custom Hooks**: Tái sử dụng stateful logic
- **Atomic Design**: Thiết kế components từ nhỏ đến lớn

---

## 🔗 Dependencies / Thư viện Phụ thuộc

### Backend Dependencies:
```xml
<PackageReference Include="Microsoft.EntityFrameworkCore" />
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" />
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" />
<PackageReference Include="Swashbuckle.AspNetCore" />
<PackageReference Include="System.IdentityModel.Tokens.Jwt" />
```

### Frontend Dependencies:
```json
{
  "dependencies": {
    "react": "^19.1.1",
    "react-dom": "^19.1.1",
    "react-router-dom": "^7.9.1",
    "axios": "^1.12.2",
    "tailwindcss": "^4.1.13",
    "@radix-ui/react-*": "Various Radix UI components"
  }
}
```

---

*Cấu trúc dự án được thiết kế để dễ dàng mở rộng, bảo trì và triển khai trong môi trường production.*

*Project structure is designed for easy scalability, maintainability, and production deployment.*
