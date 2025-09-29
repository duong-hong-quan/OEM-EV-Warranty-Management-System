# Backend Environment Configuration for Production

## Environment Variables Needed

### Database Configuration
```
DefaultConnection=postgresql://username:password@hostname:port/database_name
```

### JWT Configuration
```
Jwt:Key=YourSuperSecretKeyThatShouldBeAtLeast32CharactersLong!
Jwt:Issuer=ElectricVehicleWarrantyAPI
Jwt:Audience=ElectricVehicleWarrantyAPI
Jwt:ExpiryMinutes=60
```

### CORS Configuration for Production
Update Program.cs to allow specific origins:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("Production", policy =>
    {
        policy.WithOrigins(
                "https://your-vercel-domain.vercel.app",
                "https://your-custom-domain.com"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// Use the Production policy
app.UseCors("Production");
```

## Deploy Backend Options

### Option 1: Railway
1. Connect GitHub repository
2. Set environment variables
3. Deploy

### Option 2: Heroku
1. Create new app
2. Connect GitHub repository
3. Set Config Vars
4. Deploy

### Option 3: Azure App Service
1. Create App Service
2. Deploy from GitHub
3. Configure Application Settings

### Option 4: AWS App Runner
1. Create App Runner service
2. Connect to GitHub
3. Configure environment variables

## Database Setup

### PostgreSQL on Cloud
- **Supabase**: Free tier available
- **Railway**: PostgreSQL addon
- **Heroku Postgres**: Free tier available
- **Azure Database for PostgreSQL**
- **AWS RDS PostgreSQL**

## Connection String Examples

### Supabase
```
postgresql://postgres:[YOUR-PASSWORD]@db.[PROJECT-ID].supabase.co:5432/postgres
```

### Railway
```
postgresql://postgres:[PASSWORD]@[HOST]:[PORT]/railway
```

### Heroku
```
postgres://[USERNAME]:[PASSWORD]@[HOST]:[PORT]/[DATABASE]
```

## Steps to Deploy

1. **Choose hosting platform**
2. **Setup PostgreSQL database**
3. **Configure environment variables**
4. **Update CORS policy for your frontend domain**
5. **Deploy**

## Testing Production Backend

```bash
# Test API health
curl https://your-backend-url.com/api/auth/demo/401

# Test with your frontend
# Update VITE_API_BASE_URL in Vercel environment variables
```
