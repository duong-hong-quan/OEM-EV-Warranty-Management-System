# Configuration Guide - Electric Vehicle Warranty API

## 📁 Configuration Files Overview

### File Structure:
```
BE.API/
├── appsettings.json                    # ⚠️  NEVER commit this file
├── appsettings.example.json            # ✅ Template file - safe to commit
├── appsettings.Development.json        # ⚠️  Local dev settings - don't commit
├── appsettings.Development.example.json # ✅ Dev template - safe to commit
├── appsettings.Production.json         # ⚠️  Prod settings - don't commit  
└── appsettings.Production.example.json # ✅ Prod template - safe to commit
```

## 🔧 Setup Instructions

### 1. Initial Setup:
```bash
# Copy example files to create your local configuration
cp appsettings.example.json appsettings.json
cp appsettings.Development.example.json appsettings.Development.json
```

### 2. Update Your Configuration:

#### For Development (Local):
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=warranty-dev.db"  // SQLite for development
  },
  "Jwt": {
    "Key": "YourDevSecretKeyAtLeast32Characters!"
  }
}
```

#### For Production (Supabase/PostgreSQL):
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=your-project.supabase.co;Database=postgres;Username=postgres;Password=YOUR_PASSWORD;SSL Mode=Require;Trust Server Certificate=true"
  },
  "Jwt": {
    "Key": "YourProductionSecretKeyVerySecure64Characters!"
  }
}
```

## 🗄️ Database Connection Examples

### SQLite (Development):
```json
"DefaultConnection": "Data Source=warranty.db"
```

### PostgreSQL Local:
```json
"DefaultConnection": "Host=localhost;Port=5432;Database=evw;Username=postgres;Password=yourpassword"
```

### Supabase PostgreSQL:
```json
"DefaultConnection": "Host=db.xxxxx.supabase.co;Database=postgres;Username=postgres;Password=your-supabase-password;SSL Mode=Require;Trust Server Certificate=true"
```

### Docker PostgreSQL:
```json
"DefaultConnection": "Host=localhost;Port=5432;Database=evw;Username=evw_user;Password=evw_password"
```

## 🔐 JWT Configuration

### Development:
```json
"Jwt": {
  "Key": "DevelopmentSecretKeyAtLeast32Characters!",
  "Issuer": "ElectricVehicleWarrantyAPI-Dev",
  "Audience": "ElectricVehicleWarrantyAPI-Dev"
}
```

### Production:
```json
"Jwt": {
  "Key": "ProductionSecretKey64CharactersLongForMaximumSecurity!",
  "Issuer": "ElectricVehicleWarrantyAPI",
  "Audience": "ElectricVehicleWarrantyAPI"
}
```

## 🚫 .gitignore Configuration

Make sure these files are in your `.gitignore`:
```gitignore
# Configuration files with secrets
appsettings.json
appsettings.Development.json
appsettings.Production.json
appsettings.Local.json

# Database files
*.db
*.sqlite

# Logs
logs/
*.log
```

## ✅ Safe Files to Commit:
- `appsettings.example.json`
- `appsettings.Development.example.json`
- `appsettings.Production.example.json`
- Any `.template.json` files

## ⚠️ Never Commit:
- `appsettings.json` (contains real connection strings)
- `appsettings.Development.json` (local settings)
- `appsettings.Production.json` (production secrets)
- Database files (`.db`, `.sqlite`)

## 🔄 Environment Variables (Alternative)

You can also use environment variables:
```bash
# Set environment variables
export ConnectionStrings__DefaultConnection="your-connection-string"
export Jwt__Key="your-jwt-secret-key"

# Or in Windows:
set ConnectionStrings__DefaultConnection=your-connection-string
set Jwt__Key=your-jwt-secret-key
```

## 🧪 Quick Test Setup

### For Quick Development:
1. Copy `appsettings.Development.example.json` to `appsettings.Development.json`
2. Uses SQLite (no setup required)
3. Run: `dotnet run`

### For Full Production Test:
1. Set up Supabase/PostgreSQL
2. Update `appsettings.json` with real connection string
3. Run migrations: `dotnet ef database update`
4. Run: `dotnet run`

## 🆘 Troubleshooting

### "Configuration not found":
- Ensure `appsettings.json` exists and is valid JSON
- Check file permissions

### "Connection string missing":
- Verify `ConnectionStrings.DefaultConnection` is set
- Check connection string format

### "JWT key too short":
- Ensure JWT key is at least 32 characters
- Use a secure random key for production
