# Frontend Deployment on Vercel

## Quick Deployment Steps

### 1. Connect to Vercel
1. Go to [vercel.com](https://vercel.com)
2. Connect your GitHub repository
3. Select the `fe` folder as the root directory

### 2. Configure Environment Variables
In Vercel dashboard, go to **Settings** > **Environment Variables** and add:
```
VITE_API_BASE_URL=https://your-backend-api-url.com
```

### 3. Configure Build Settings
Vercel should auto-detect these settings, but verify:
- **Framework Preset**: Vite
- **Root Directory**: `fe`
- **Build Command**: `npm run build`
- **Output Directory**: `dist`
- **Install Command**: `npm install`

### 4. Deploy
Click "Deploy" and Vercel will handle the rest!

## Troubleshooting

### Issue: 404 on page refresh or direct URL access
**Solution**: The `vercel.json` file in this directory handles SPA routing.

### Issue: API calls failing
**Solution**: 
1. Ensure `VITE_API_BASE_URL` environment variable is set correctly
2. Backend must have CORS configured for your Vercel domain
3. If using HTTPS frontend, backend should also use HTTPS

### Issue: Build failures
**Solution**:
1. Check that all dependencies are in `package.json`
2. Ensure Node.js version compatibility
3. Check build logs in Vercel dashboard

## Local Testing Before Deployment

```bash
# Install dependencies
npm install

# Build for production
npm run build

# Preview production build locally
npm run preview
```

## Environment Variables Setup

1. Copy `.env.example` to `.env.local`:
```bash
cp .env.example .env.local
```

2. Update the API URL in `.env.local`:
```
VITE_API_BASE_URL=http://localhost:5165
```

## File Structure for Vercel

```
fe/
├── vercel.json          # Vercel configuration for SPA routing
├── vite.config.js       # Vite configuration optimized for Vercel
├── .env.example         # Environment variables template
├── package.json         # Dependencies and scripts
├── src/                 # Source code
└── dist/               # Build output (auto-generated)
```

## Backend CORS Configuration

Ensure your .NET backend has CORS configured for your Vercel domain:

```csharp
// In Program.cs
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVercel", policy =>
    {
        policy.WithOrigins("https://your-vercel-domain.vercel.app")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Use CORS
app.UseCors("AllowVercel");
```

## Custom Domain (Optional)

1. In Vercel dashboard, go to **Settings** > **Domains**
2. Add your custom domain
3. Update `VITE_API_BASE_URL` if backend domain changes
4. Update backend CORS configuration for new domain
