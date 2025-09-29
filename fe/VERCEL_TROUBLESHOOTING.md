# Vercel Deployment Troubleshooting Guide

## Common Issues and Solutions

### 1. 404 Error on Page Refresh or Direct URL Access

**Problem**: When you visit a route like `/vehicle-registration` directly or refresh the page, you get a 404 error.

**Solution**: 
✅ **Fixed**: The `vercel.json` file in the root of `fe/` directory handles this with SPA routing configuration.

```json
{
  "rewrites": [
    {
      "source": "/(.*)",
      "destination": "/index.html"
    }
  ]
}
```

### 2. API Calls Not Working

**Problem**: Frontend can't connect to backend API.

**Solutions**:

1. **Set Environment Variable**:
   - Go to Vercel Dashboard → Settings → Environment Variables
   - Add: `VITE_API_BASE_URL` = `https://your-backend-url.com`

2. **Backend CORS Configuration**:
   Update your backend to allow your Vercel domain:
   ```csharp
   builder.Services.AddCors(options =>
   {
       options.AddPolicy("Production", policy =>
       {
           policy.WithOrigins("https://your-vercel-domain.vercel.app")
                 .AllowAnyHeader()
                 .AllowAnyMethod();
       });
   });
   ```

3. **HTTPS Required**:
   If frontend is HTTPS (Vercel default), backend should also be HTTPS.

### 3. Build Failures

**Problem**: Build fails during deployment.

**Common Solutions**:

1. **Node.js Version**:
   Add to `package.json`:
   ```json
   {
     "engines": {
       "node": "18.x"
     }
   }
   ```

2. **Missing Dependencies**:
   Ensure all imports are properly installed:
   ```bash
   npm install --save missing-package
   ```

3. **TypeScript Errors**:
   If using TypeScript, fix all type errors or temporarily disable strict mode.

### 4. Environment Variables Not Loading

**Problem**: `import.meta.env.VITE_API_BASE_URL` returns undefined.

**Solutions**:

1. **Prefix Required**: Environment variables must start with `VITE_`
2. **Redeploy**: After adding env vars, trigger a new deployment
3. **Check Spelling**: Ensure variable names match exactly

### 5. Blank Page After Deployment

**Problem**: Site deploys successfully but shows blank page.

**Debug Steps**:

1. **Check Browser Console**: Look for JavaScript errors
2. **Check Network Tab**: See if assets are loading
3. **Verify Build Output**: Check if `dist/` folder contains files locally

**Common Fixes**:
- Fix JavaScript errors
- Ensure all assets are included in build
- Check for missing dependencies

### 6. Routing Issues

**Problem**: Some routes work, others don't.

**Solution**:
✅ **Fixed**: Added catch-all route in `App.jsx`:
```jsx
<Route path="*" element={<Navigate to="/" replace />} />
```

### 7. Slow Initial Load

**Problem**: First page load is slow.

**Optimizations**:
✅ **Implemented**: Code splitting in `vite.config.js`:
```javascript
build: {
  rollupOptions: {
    output: {
      manualChunks: {
        vendor: ['react', 'react-dom'],
        router: ['react-router-dom'],
      },
    },
  },
}
```

## Pre-Deployment Checklist

### ✅ Files Created/Updated:
- [ ] `vercel.json` - SPA routing configuration
- [ ] `vite.config.js` - Build optimization
- [ ] `.env.example` - Environment variables template
- [ ] Updated `App.jsx` - Added catch-all route

### ✅ Environment Variables:
- [ ] `VITE_API_BASE_URL` set in Vercel dashboard
- [ ] Backend CORS allows your Vercel domain
- [ ] Backend is deployed and accessible

### ✅ Testing:
- [ ] `npm run build` works locally
- [ ] `npm run preview` works locally
- [ ] All routes work when testing locally
- [ ] API calls work with production backend URL

## Manual Testing Steps

1. **Local Build Test**:
   ```bash
   cd fe
   npm run build
   npm run preview
   ```

2. **Test All Routes**:
   - Visit each route directly
   - Refresh pages
   - Use browser back/forward buttons

3. **Test API Integration**:
   - Try login functionality
   - Test API calls
   - Check network tab for errors

## Deployment Command for Vercel CLI

If using Vercel CLI:
```bash
cd fe
npx vercel --prod
```

## Support Resources

- [Vercel SPA Documentation](https://vercel.com/guides/deploying-react-with-vercel)
- [Vite Deployment Guide](https://vitejs.dev/guide/static-deploy.html#vercel)
- [React Router Deployment](https://reactrouter.com/en/main/guides/deploying)

---

## Quick Fix Commands

If deployment fails, try these commands:

```bash
# Clear node_modules and reinstall
rm -rf node_modules package-lock.json
npm install

# Clear Vite cache
npx vite --force

# Test build locally
npm run build
npm run preview
```
