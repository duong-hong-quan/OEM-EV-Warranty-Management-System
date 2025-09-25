@echo off
REM Database Migration Script for Electric Vehicle Warranty System

echo 🔧 Electric Vehicle Warranty - Database Migration
echo =================================================

REM Check if PostgreSQL is running
echo 📋 Checking PostgreSQL connection...

REM Option 1: Update PostgreSQL database
echo 🗄️ Updating PostgreSQL database...
dotnet ef database update --project ../BE.DAL --startup-project .

if %ERRORLEVEL% EQU 0 (
    echo ✅ Database updated successfully!
    echo.
    echo 🔐 Demo users will be automatically created on first run:
    echo    Admin: admin@warranty.com / admin123
    echo    Manager: manager@warranty.com / manager123  
    echo    Technician: tech@warranty.com / tech123
    echo    Customer: customer@warranty.com / customer123
    echo.
    echo 🚀 You can now run the application with: dotnet run
) else (
    echo ❌ Database update failed!
    echo 💡 Please check:
    echo    1. PostgreSQL server is running
    echo    2. Connection string in appsettings.json is correct
    echo    3. Database and user exist
    echo.
    echo 📝 Current connection string: Host=192.168.1.100;Port=5432;Database=evw;Username=evw_user;Password=evw_user123@
)

pause
