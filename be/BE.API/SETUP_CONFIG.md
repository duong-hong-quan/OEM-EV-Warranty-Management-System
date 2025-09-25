# 🔧 Configuration Setup Instructions

## ⚠️ IMPORTANT: Bảo mật thông tin cấu hình

### 📁 Files đã tạo:

✅ **An toàn để commit:**
- `appsettings.example.json` - Template chính
- `appsettings.Development.example.json` - Template cho development  
- `appsettings.Production.example.json` - Template cho production
- `CONFIGURATION_GUIDE.md` - Hướng dẫn chi tiết
- `.gitignore` - Bảo vệ file nhạy cảm

⚠️ **KHÔNG được commit:**
- `appsettings.json` - Chứa connection string thật
- `appsettings.Development.json` - Cấu hình local
- `appsettings.Production.json` - Cấu hình production

## 🚀 Hướng dẫn setup nhanh:

### 1. Copy file template:
```bash
# Tạo file cấu hình từ template
cp appsettings.example.json appsettings.json
cp appsettings.Development.example.json appsettings.Development.json
```

### 2. Cập nhật cấu hình:
Mở `appsettings.json` và thay đổi:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "YOUR_REAL_CONNECTION_STRING_HERE"
  },
  "Jwt": {
    "Key": "YOUR_SECURE_JWT_KEY_32_CHARS_MIN"
  }
}
```

### 3. Chạy migration:
```bash
dotnet ef database update --project ../BE.DAL --startup-project .
```

### 4. Chạy ứng dụng:
```bash
dotnet run
```

## 🗄️ Database Options:

### SQLite (Đơn giản nhất):
```json
"DefaultConnection": "Data Source=warranty.db"
```

### Supabase (Như bạn đang dùng):
```json  
"DefaultConnection": "Host=db.xxxxx.supabase.co;Database=postgres;Username=postgres;Password=your-password;SSL Mode=Require;Trust Server Certificate=true"
```

### PostgreSQL Local:
```json
"DefaultConnection": "Host=localhost;Port=5432;Database=evw;Username=postgres;Password=yourpassword"
```

## 🔐 Demo Users:
Sau khi chạy ứng dụng, các user demo sẽ tự động được tạo:

| Role | Email | Password |
|------|-------|----------|
| Admin | admin@warranty.com | admin123 |
| Manager | manager@warranty.com | manager123 |
| Technician | tech@warranty.com | tech123 |
| Customer | customer@warranty.com | customer123 |

## 🧪 Test Authentication:

### 1. Login để lấy token:
```bash
curl -X POST "http://localhost:5000/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@warranty.com","password":"admin123"}'
```

### 2. Sử dụng token:
```bash  
curl -X GET "http://localhost:5000/api/customers" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

### 3. Test 401 (Unauthorized):
```bash
curl -X GET "http://localhost:5000/api/customers"
# Trả về: 401 Unauthorized
```

### 4. Test 403 (Forbidden):  
```bash
# Login với customer, thử truy cập endpoint admin
curl -X GET "http://localhost:5000/api/customers" \
  -H "Authorization: Bearer CUSTOMER_TOKEN"
# Trả về: 403 Forbidden
```

## ✅ Checklist:

- [ ] Đã copy file example thành file thật
- [ ] Đã cập nhật connection string
- [ ] Đã cập nhật JWT key (tối thiểu 32 ký tự)
- [ ] Đã chạy migration thành công
- [ ] File `appsettings.json` KHÔNG được commit
- [ ] Đã test login và lấy token
- [ ] Đã test 401/403 responses

## 🆘 Troubleshooting:

### Connection failed:
- Kiểm tra database server đang chạy
- Kiểm tra connection string đúng format
- Kiểm tra username/password

### JWT errors:  
- Đảm bảo JWT key >= 32 ký tự
- Kiểm tra Issuer/Audience khớp nhau

### Migration errors:
- Chạy `dotnet clean && dotnet build`
- Kiểm tra connection tới database
