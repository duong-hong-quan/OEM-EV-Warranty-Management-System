# System Architecture Overview
## Tổng quan Kiến trúc Hệ thống

---

## 🎯 Architecture Goals / Mục tiêu Kiến trúc

### Tiếng Việt
- **Scalability**: Dễ dàng mở rộng theo chiều ngang và chiều dọc
- **Maintainability**: Dễ bảo trì, debug và cập nhật
- **Testability**: Hỗ trợ unit testing và integration testing
- **Security**: Bảo mật cao với authentication và authorization
- **Performance**: Tối ưu hóa hiệu suất và response time
- **Flexibility**: Linh hoạt trong việc thay đổi và mở rộng tính năng

### English
- **Scalability**: Easy horizontal and vertical scaling
- **Maintainability**: Easy to maintain, debug, and update
- **Testability**: Support for unit testing and integration testing
- **Security**: High security with authentication and authorization
- **Performance**: Optimized performance and response time
- **Flexibility**: Flexible in changing and extending features

---

## 🏗️ High-Level Architecture / Kiến trúc Tổng quan

```mermaid
graph TB
    subgraph "Client Layer"
        WEB[Web Browser]
        MOB[Mobile App]
        API_CLIENT[API Client]
    end
    
    subgraph "Presentation Layer"
        REACT[React Frontend]
        NGINX[Nginx Reverse Proxy]
    end
    
    subgraph "API Gateway Layer"
        GATEWAY[API Gateway]
        AUTH[Authentication Service]
    end
    
    subgraph "Application Layer"
        API[ASP.NET Core Web API]
        SWAGGER[Swagger Documentation]
    end
    
    subgraph "Business Layer"
        SERVICES[Business Services]
        VALIDATION[Validation Layer]
        MAPPER[DTO Mapping]
    end
    
    subgraph "Data Layer"
        REPO[Repository Pattern]
        UOW[Unit of Work]
        EF[Entity Framework Core]
    end
    
    subgraph "Database Layer"
        SQL[SQL Server Database]
        REDIS[Redis Cache]
    end
    
    subgraph "Infrastructure Layer"
        DOCKER[Docker Containers]
        CI_CD[GitHub Actions CI/CD]
        MONITOR[Monitoring & Logging]
    end
    
    WEB --> REACT
    MOB --> API
    API_CLIENT --> API
    
    REACT --> NGINX
    NGINX --> API
    
    API --> AUTH
    API --> SERVICES
    API --> SWAGGER
    
    SERVICES --> VALIDATION
    SERVICES --> MAPPER
    SERVICES --> REPO
    
    REPO --> UOW
    UOW --> EF
    EF --> SQL
    
    API --> REDIS
    
    DOCKER --> API
    DOCKER --> REACT
    DOCKER --> SQL
```

---

## 🔧 Clean Architecture Implementation / Triển khai Clean Architecture

### Tiếng Việt

#### Nguyên tắc Dependency Inversion:
- Các layer bên trong không phụ thuộc vào layer bên ngoài
- Interface được định nghĩa ở layer bên trong
- Implementation được thực hiện ở layer bên ngoài

#### Các Layer chính:

1. **Entities** (BE.DAL/Models): Core business objects
2. **Use Cases** (BE.Services): Business logic và rules  
3. **Interface Adapters** (BE.API/Controllers): Convert data formats
4. **Frameworks & Drivers** (Infrastructure): External systems

### English

#### Dependency Inversion Principle:
- Inner layers don't depend on outer layers
- Interfaces defined in inner layers
- Implementations provided by outer layers

#### Main Layers:

1. **Entities** (BE.DAL/Models): Core business objects
2. **Use Cases** (BE.Services): Business logic and rules
3. **Interface Adapters** (BE.API/Controllers): Convert data formats
4. **Frameworks & Drivers** (Infrastructure): External systems

```mermaid
graph TD
    subgraph "Clean Architecture Layers"
        subgraph "Entities"
            ENTITY[Business Entities]
            RULES[Business Rules]
        end
        
        subgraph "Use Cases"
            APP_LOGIC[Application Logic]
            INTERFACES[Application Interfaces]
        end
        
        subgraph "Interface Adapters"
            CONTROLLERS[Controllers]
            PRESENTERS[Presenters]
            GATEWAYS[Gateways]
        end
        
        subgraph "Frameworks & Drivers"
            WEB[Web Framework]
            DATABASE[Database]
            EXTERNAL[External APIs]
        end
    end
    
    INTERFACES --> ENTITY
    APP_LOGIC --> ENTITY
    CONTROLLERS --> INTERFACES
    PRESENTERS --> INTERFACES
    GATEWAYS --> INTERFACES
    WEB --> CONTROLLERS
    DATABASE --> GATEWAYS
    EXTERNAL --> GATEWAYS
```

---

## 🌐 Frontend Architecture / Kiến trúc Frontend

### React Component Architecture

```mermaid
graph TD
    subgraph "React Application"
        APP[App.jsx - Root Component]
        
        subgraph "Layout Components"
            ADMIN_LAYOUT[AdminLayout.jsx]
            HEADER[Header Component]
            SIDEBAR[Sidebar Component]
        end
        
        subgraph "Page Components"
            DASHBOARD[Dashboard.jsx]
            VEHICLES[VehicleRegistration.jsx]
            PARTS[PartAttachment.jsx]
            SERVICE[ServiceHistory.jsx]
            WARRANTY[WarrantyClaim.jsx]
        end
        
        subgraph "UI Components"
            BUTTON[Button]
            CARD[Card]
            FORM[Form]
            INPUT[Input]
            TABLE[Table]
        end
        
        subgraph "Utility Layer"
            UTILS[utils.js]
            API_CLIENT[Axios API Client]
            HOOKS[Custom Hooks]
        end
    end
    
    APP --> ADMIN_LAYOUT
    ADMIN_LAYOUT --> HEADER
    ADMIN_LAYOUT --> SIDEBAR
    ADMIN_LAYOUT --> DASHBOARD
    ADMIN_LAYOUT --> VEHICLES
    ADMIN_LAYOUT --> PARTS
    ADMIN_LAYOUT --> SERVICE
    ADMIN_LAYOUT --> WARRANTY
    
    DASHBOARD --> CARD
    VEHICLES --> FORM
    VEHICLES --> TABLE
    PARTS --> BUTTON
    SERVICE --> INPUT
    
    DASHBOARD --> API_CLIENT
    VEHICLES --> HOOKS
    PARTS --> UTILS
```

---

## 🔐 Security Architecture / Kiến trúc Bảo mật

### Authentication & Authorization Flow

```mermaid
sequenceDiagram
    participant Client
    participant Frontend
    participant API
    participant AuthService
    participant Database
    
    Client->>Frontend: 1. Login Request
    Frontend->>API: 2. POST /api/auth/login
    API->>AuthService: 3. Validate Credentials
    AuthService->>Database: 4. Check User & Password
    Database-->>AuthService: 5. User Data
    AuthService-->>API: 6. Generate JWT + Refresh Token
    API-->>Frontend: 7. Return Tokens
    Frontend-->>Client: 8. Store Tokens
    
    Note over Frontend: Subsequent API Calls
    Frontend->>API: 9. API Request + JWT Bearer Token
    API->>API: 10. Validate JWT
    API->>AuthService: 11. Check Authorization
    AuthService-->>API: 12. Access Granted/Denied
    API-->>Frontend: 13. API Response
```

### Role-Based Access Control (RBAC)

```mermaid
graph LR
    subgraph "User Roles"
        ADMIN[Admin]
        MANAGER[Manager]
        TECH[Technician]
        CUSTOMER[Customer]
    end
    
    subgraph "Permissions"
        CREATE[Create]
        READ[Read]
        UPDATE[Update]
        DELETE[Delete]
    end
    
    subgraph "Resources"
        VEHICLES[Vehicles]
        CUSTOMERS[Customers]
        PARTS[Parts]
        SERVICE[Service History]
        WARRANTY[Warranty Claims]
    end
    
    ADMIN --> CREATE
    ADMIN --> READ
    ADMIN --> UPDATE
    ADMIN --> DELETE
    
    MANAGER --> READ
    MANAGER --> UPDATE
    
    TECH --> READ
    TECH --> UPDATE
    
    CUSTOMER --> READ
    
    CREATE --> VEHICLES
    READ --> VEHICLES
    UPDATE --> VEHICLES
    DELETE --> VEHICLES
    
    CREATE --> CUSTOMERS
    READ --> CUSTOMERS
    UPDATE --> CUSTOMERS
    DELETE --> CUSTOMERS
```

---

## 📊 Data Architecture / Kiến trúc Dữ liệu

### Database Schema Design

```mermaid
erDiagram
    Users ||--o{ RefreshTokens : has
    Users ||--o{ Customers : manages
    Customers ||--o{ Vehicles : owns
    Vehicles ||--o{ Parts : contains
    Vehicles ||--o{ ServiceHistory : has
    Vehicles ||--o{ WarrantyClaims : generates
    Parts ||--o{ ServiceHistory : involved_in
    Parts ||--o{ WarrantyClaims : claimed_for
    
    Users {
        guid Id PK
        string Email
        string PasswordHash
        string Role
        datetime CreatedAt
        datetime UpdatedAt
    }
    
    Customers {
        guid Id PK
        string Name
        string Email
        string Phone
        string Address
        datetime CreatedAt
    }
    
    Vehicles {
        guid Id PK
        guid CustomerId FK
        string Make
        string Model
        int Year
        string VIN
        datetime PurchaseDate
        datetime WarrantyExpiration
    }
    
    Parts {
        guid Id PK
        guid VehicleId FK
        string PartName
        string PartNumber
        decimal Price
        datetime InstallationDate
        datetime WarrantyExpiration
    }
```

### Repository Pattern Implementation

```mermaid
graph TD
    subgraph "Repository Layer"
        IREPO[IGenericRepository<T>]
        REPO[GenericRepository<T>]
        IUOW[IUnitOfWork]
        UOW[UnitOfWork]
    end
    
    subgraph "Service Layer"
        ISERVICE[IService<T>]
        SERVICE[BaseService<T>]
        VEHICLE_SERVICE[VehicleService]
        CUSTOMER_SERVICE[CustomerService]
    end
    
    subgraph "Data Context"
        DBCONTEXT[WarrantyDbContext]
        DBSET[DbSet<T>]
    end
    
    IREPO --> REPO
    IUOW --> UOW
    ISERVICE --> SERVICE
    SERVICE --> IREPO
    SERVICE --> IUOW
    VEHICLE_SERVICE --> SERVICE
    CUSTOMER_SERVICE --> SERVICE
    
    REPO --> DBCONTEXT
    UOW --> DBCONTEXT
    DBCONTEXT --> DBSET
```

---

## 🚀 DevOps Architecture / Kiến trúc DevOps

### CI/CD Pipeline

```mermaid
graph LR
    subgraph "Development"
        DEV[Developer]
        GIT[Git Repository]
    end
    
    subgraph "CI/CD Pipeline"
        TRIGGER[Push Trigger]
        BUILD[Build & Test]
        SECURITY[Security Scan]
        ARTIFACTS[Create Artifacts]
        DEPLOY_DEV[Deploy to Dev]
        DEPLOY_PROD[Deploy to Prod]
    end
    
    subgraph "Infrastructure"
        DEV_SERVER[Development Server]
        PROD_SERVER[Production Server]
        LXC[LXC Containers]
        MONITORING[Monitoring]
    end
    
    DEV --> GIT
    GIT --> TRIGGER
    TRIGGER --> BUILD
    BUILD --> SECURITY
    SECURITY --> ARTIFACTS
    ARTIFACTS --> DEPLOY_DEV
    DEPLOY_DEV --> DEV_SERVER
    ARTIFACTS --> DEPLOY_PROD
    DEPLOY_PROD --> PROD_SERVER
    
    DEV_SERVER --> LXC
    PROD_SERVER --> LXC
    LXC --> MONITORING
```

### Container Architecture

```mermaid
graph TB
    subgraph "Docker Environment"
        subgraph "Frontend Container"
            REACT_APP[React Application]
            NGINX_SERVER[Nginx Server]
        end
        
        subgraph "Backend Container"
            NET_API[.NET API]
            KESTREL[Kestrel Server]
        end
        
        subgraph "Database Container"
            SQL_SERVER[SQL Server]
            DATA_VOLUME[Data Volume]
        end
        
        subgraph "Reverse Proxy"
            NGINX_PROXY[Nginx Reverse Proxy]
            SSL_CERT[SSL Certificates]
        end
    end
    
    REACT_APP --> NGINX_SERVER
    NET_API --> KESTREL
    SQL_SERVER --> DATA_VOLUME
    NGINX_PROXY --> SSL_CERT
    
    NGINX_PROXY --> NGINX_SERVER
    NGINX_PROXY --> KESTREL
    KESTREL --> SQL_SERVER
```

---

## 📈 Performance Architecture / Kiến trúc Hiệu suất

### Caching Strategy

```mermaid
graph TD
    subgraph "Caching Layers"
        BROWSER[Browser Cache]
        CDN[CDN Cache]
        NGINX_CACHE[Nginx Cache]
        APP_CACHE[Application Cache]
        REDIS_CACHE[Redis Cache]
        DB_CACHE[Database Cache]
    end
    
    CLIENT[Client Request]
    API[API Server]
    DATABASE[Database]
    
    CLIENT --> BROWSER
    BROWSER --> CDN
    CDN --> NGINX_CACHE
    NGINX_CACHE --> API
    API --> APP_CACHE
    APP_CACHE --> REDIS_CACHE
    REDIS_CACHE --> DATABASE
    DATABASE --> DB_CACHE
```

### Load Balancing & Scaling

```mermaid
graph TD
    subgraph "Load Balancer"
        LB[Nginx Load Balancer]
    end
    
    subgraph "API Instances"
        API1[API Instance 1]
        API2[API Instance 2]
        API3[API Instance 3]
    end
    
    subgraph "Database Cluster"
        DB_MASTER[Master DB]
        DB_REPLICA1[Replica DB 1]
        DB_REPLICA2[Replica DB 2]
    end
    
    CLIENT[Clients] --> LB
    LB --> API1
    LB --> API2
    LB --> API3
    
    API1 --> DB_MASTER
    API2 --> DB_MASTER
    API3 --> DB_MASTER
    
    DB_MASTER --> DB_REPLICA1
    DB_MASTER --> DB_REPLICA2
```

---

## 🔍 Monitoring & Logging / Giám sát & Ghi log

### Observability Stack

```mermaid
graph TB
    subgraph "Application"
        API[API Server]
        FRONTEND[Frontend App]
        DATABASE[Database]
    end
    
    subgraph "Monitoring Stack"
        LOGS[Application Logs]
        METRICS[Performance Metrics]
        TRACES[Distributed Tracing]
        HEALTH[Health Checks]
    end
    
    subgraph "Observability Tools"
        ELASTIC[Elasticsearch]
        KIBANA[Kibana Dashboard]
        PROMETHEUS[Prometheus]
        GRAFANA[Grafana]
    end
    
    API --> LOGS
    API --> METRICS
    API --> TRACES
    API --> HEALTH
    
    FRONTEND --> LOGS
    DATABASE --> METRICS
    
    LOGS --> ELASTIC
    ELASTIC --> KIBANA
    METRICS --> PROMETHEUS
    PROMETHEUS --> GRAFANA
    TRACES --> ELASTIC
    HEALTH --> PROMETHEUS
```

---

## 🛡️ Security Considerations / Các khía cạnh Bảo mật

### Security Layers

```mermaid
graph TD
    subgraph "Security Layers"
        subgraph "Network Security"
            FIREWALL[Firewall]
            SSL_TLS[SSL/TLS Encryption]
            VPN[VPN Access]
        end
        
        subgraph "Application Security"
            AUTH[Authentication]
            AUTHZ[Authorization]
            VALIDATION[Input Validation]
            CSRF[CSRF Protection]
        end
        
        subgraph "Data Security"
            ENCRYPTION[Data Encryption]
            HASHING[Password Hashing]
            BACKUP[Secure Backup]
        end
        
        subgraph "Infrastructure Security"
            CONTAINER[Container Security]
            SECRETS[Secret Management]
            AUDIT[Audit Logging]
        end
    end
    
    FIREWALL --> SSL_TLS
    SSL_TLS --> AUTH
    AUTH --> AUTHZ
    AUTHZ --> VALIDATION
    VALIDATION --> ENCRYPTION
    ENCRYPTION --> CONTAINER
    CONTAINER --> SECRETS
    SECRETS --> AUDIT
```

---

## 📋 Best Practices Implemented / Best Practices Được áp dụng

### Tiếng Việt

#### Backend Best Practices:
- ✅ **SOLID Principles**: Tuân thủ các nguyên tắc thiết kế SOLID
- ✅ **Clean Code**: Mã nguồn sạch, dễ đọc và bảo trì
- ✅ **Exception Handling**: Xử lý lỗi toàn diện
- ✅ **Async/Await**: Lập trình bất đồng bộ
- ✅ **Dependency Injection**: Loose coupling
- ✅ **Repository Pattern**: Trừu tượng hóa data access
- ✅ **Unit of Work**: Quản lý transaction

#### Frontend Best Practices:
- ✅ **Component Reusability**: Tái sử dụng components
- ✅ **State Management**: Quản lý state hiệu quả
- ✅ **Performance Optimization**: Tối ưu hóa hiệu suất
- ✅ **Responsive Design**: Thiết kế responsive
- ✅ **Accessibility**: Hỗ trợ accessibility
- ✅ **Error Boundaries**: Xử lý lỗi React
- ✅ **Code Splitting**: Phân chia mã nguồn

### English

#### Backend Best Practices:
- ✅ **SOLID Principles**: Following SOLID design principles
- ✅ **Clean Code**: Clean, readable, and maintainable code
- ✅ **Exception Handling**: Comprehensive error handling
- ✅ **Async/Await**: Asynchronous programming
- ✅ **Dependency Injection**: Loose coupling
- ✅ **Repository Pattern**: Data access abstraction
- ✅ **Unit of Work**: Transaction management

#### Frontend Best Practices:
- ✅ **Component Reusability**: Reusable components
- ✅ **State Management**: Efficient state management
- ✅ **Performance Optimization**: Performance optimization
- ✅ **Responsive Design**: Responsive design
- ✅ **Accessibility**: Accessibility support
- ✅ **Error Boundaries**: React error handling
- ✅ **Code Splitting**: Code splitting

---

*Kiến trúc này được thiết kế để đáp ứng các yêu cầu của một ứng dụng enterprise với khả năng mở rộng, bảo mật cao và hiệu suất tốt.*

*This architecture is designed to meet enterprise application requirements with scalability, high security, and good performance.*
