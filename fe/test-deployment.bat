@echo off
REM Pre-deployment Test Script for Vercel (Windows)
REM Run this script before deploying to catch issues early

echo 🚀 Testing frontend build for Vercel deployment...

REM Check if we're in the fe directory
if not exist "package.json" (
    echo ❌ Please run this script from the fe/ directory
    exit /b 1
)

REM Check for required files
echo 📋 Checking required files...

if not exist "vercel.json" (
    echo ❌ Missing vercel.json file
    exit /b 1
)

if not exist "vite.config.js" (
    echo ❌ Missing vite.config.js file
    exit /b 1
)

if not exist ".env.example" (
    echo ❌ Missing .env.example file
    exit /b 1
)

echo ✅ All required files present

REM Install dependencies
echo 📦 Installing dependencies...
npm install

if %errorlevel% neq 0 (
    echo ❌ npm install failed
    exit /b 1
)

REM Run linting
echo 🧹 Running linter...
npm run lint
if %errorlevel% neq 0 (
    echo ⚠️  Linting issues found, but continuing...
)

REM Build the project
echo 🏗️  Building project...
npm run build

if %errorlevel% neq 0 (
    echo ❌ Build failed
    exit /b 1
)

REM Check if dist directory was created
if not exist "dist" (
    echo ❌ dist directory not created
    exit /b 1
)

REM Check if index.html exists in dist
if not exist "dist\index.html" (
    echo ❌ dist\index.html not found
    exit /b 1
)

echo ✅ Build successful

REM Start preview server
echo 🔍 Starting preview server...
echo 📱 Test the following:
echo    - Visit http://localhost:4173
echo    - Test all routes
echo    - Refresh pages
echo    - Check browser console for errors
echo.
echo Press Ctrl+C to stop the preview server

npm run preview
