# Database Migration Guide

## ✅ Migration Created Successfully!

The Entity Framework migration `AddUserAuthentication` has been created and includes:

### New Tables:
- **Users** - Authentication and user management
- **RefreshTokens** - JWT refresh token management  
- **Updated Vehicles** - Added VehicleName column

### Migration Details:
- **File**: `BE.DAL/Migrations/20250925130344_AddUserAuthentication.cs`
- **Status**: ✅ Created
- **Next Step**: Update Database

## 🗄️ Database Update Options

### Option 1: PostgreSQL (Current Configuration)
```bash
# Make sure PostgreSQL is running on 192.168.1.100:5432
dotnet ef database update --project ../BE.DAL --startup-project .
```

**Connection String**: `Host=192.168.1.100;Port=5432;Database=evw;Username=evw_user;Password=evw_user123@`

### Option 2: Local PostgreSQL
Update `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=evw;Username=postgres;Password=yourpassword"
  }
}
```

### Option 3: Docker PostgreSQL
```bash
# Run PostgreSQL in Docker
docker run --name postgres-evw -e POSTGRES_DB=evw -e POSTGRES_USER=evw_user -e POSTGRES_PASSWORD=evw_user123 -p 5432:5432 -d postgres:13

# Then run migration
dotnet ef database update --project ../BE.DAL --startup-project .
```

## 🚀 Quick Start Scripts

### Windows:
```batch
update-database.bat
```

### Linux/Mac:
```bash
chmod +x update-database.sh
./update-database.sh
```

## 🔐 Demo Users (Auto-Created)

After database update, these users will be automatically seeded:

| Role | Email | Password |
|------|-------|----------|
| Admin | admin@warranty.com | admin123 |
| Manager | manager@warranty.com | manager123 |
| Technician | tech@warranty.com | tech123 |
| Customer | customer@warranty.com | customer123 |

## 🧪 Testing Authentication

### 1. Login:
```bash
curl -X POST "http://localhost:5000/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@warranty.com","password":"admin123"}'
```

### 2. Use Token:
```bash
curl -X GET "http://localhost:5000/api/customers" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

### 3. Test 401 (Unauthorized):
```bash
curl -X GET "http://localhost:5000/api/customers"
# Returns: 401 Unauthorized
```

### 4. Test 403 (Forbidden):
```bash
# Login as customer, try to access admin endpoint
curl -X GET "http://localhost:5000/api/customers" \
  -H "Authorization: Bearer CUSTOMER_JWT_TOKEN"
# Returns: 403 Forbidden
```

## 🛠️ Troubleshooting

### Database Connection Failed:
1. ✅ Check PostgreSQL is running
2. ✅ Verify connection string
3. ✅ Ensure database exists
4. ✅ Check firewall settings

### Migration Issues:
```bash
# Remove last migration
dotnet ef migrations remove --project ../BE.DAL --startup-project .

# Recreate migration  
dotnet ef migrations add AddUserAuthentication --project ../BE.DAL --startup-project .
```

### Build Errors:
```bash
# Clean and rebuild
dotnet clean
dotnet build
```

## 📝 Next Steps

1. 🗄️ **Update Database** (choose option above)
2. 🚀 **Run Application**: `dotnet run`
3. 🌐 **Open Swagger**: `https://localhost:7000/swagger`
4. 🔐 **Test Authentication** endpoints
5. 🧪 **Verify 401/403** responses
