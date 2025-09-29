#!/bin/bash

# Pre-deployment Test Script for Vercel
# Run this script before deploying to catch issues early

echo "🚀 Testing frontend build for Vercel deployment..."

# Check if we're in the fe directory
if [ ! -f "package.json" ]; then
    echo "❌ Please run this script from the fe/ directory"
    exit 1
fi

# Check for required files
echo "📋 Checking required files..."

if [ ! -f "vercel.json" ]; then
    echo "❌ Missing vercel.json file"
    exit 1
fi

if [ ! -f "vite.config.js" ]; then
    echo "❌ Missing vite.config.js file"
    exit 1
fi

if [ ! -f ".env.example" ]; then
    echo "❌ Missing .env.example file"
    exit 1
fi

echo "✅ All required files present"

# Install dependencies
echo "📦 Installing dependencies..."
npm install

if [ $? -ne 0 ]; then
    echo "❌ npm install failed"
    exit 1
fi

# Run linting
echo "🧹 Running linter..."
npm run lint
if [ $? -ne 0 ]; then
    echo "⚠️  Linting issues found, but continuing..."
fi

# Build the project
echo "🏗️  Building project..."
npm run build

if [ $? -ne 0 ]; then
    echo "❌ Build failed"
    exit 1
fi

# Check if dist directory was created
if [ ! -d "dist" ]; then
    echo "❌ dist directory not created"
    exit 1
fi

# Check if index.html exists in dist
if [ ! -f "dist/index.html" ]; then
    echo "❌ dist/index.html not found"
    exit 1
fi

echo "✅ Build successful"

# Start preview server
echo "🔍 Starting preview server..."
echo "📱 Test the following:"
echo "   - Visit http://localhost:4173"
echo "   - Test all routes"
echo "   - Refresh pages"
echo "   - Check browser console for errors"
echo ""
echo "Press Ctrl+C to stop the preview server"

npm run preview
