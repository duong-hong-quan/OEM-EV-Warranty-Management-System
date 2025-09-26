using BE.DAL.Models;
using BE.DAL.GenericRepository;
using BE.DAL.UOW;
using BE.Services.Services;
using BE.Services.Services.Implemetation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using BE.DAL.DTO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configure Swagger with JWT support
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Electric Vehicle Warranty API", Version = "v1" });
    
    // Add JWT Authentication to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Configure JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"] ?? "YourSuperSecretKeyThatShouldBeAtLeast32CharactersLong!";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "ElectricVehicleWarrantyAPI";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "ElectricVehicleWarrantyAPI";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtKey)),
            ValidateIssuer = true,
            ValidIssuer = jwtIssuer,
            ValidateAudience = true,
            ValidAudience = jwtAudience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

// Add PostgreSQL EF Core DbContext
builder.Services.AddDbContext<WarrantyDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register IDbContext for dependency injection
builder.Services.AddScoped<IDbContext, WarrantyDbContext>();

// Register generic repository and unit of work
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Register services
builder.Services.AddScoped<IPartService, PartService>();
builder.Services.AddScoped<IServiceHistoryService, ServiceHistoryService>();
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<IWarrantyClaimService, WarrantyClaimService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
builder.WebHost.UseUrls("http://0.0.0.0:5000");

var app = builder.Build();
app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
app.MapOpenApi();

app.UseHttpsRedirection();

// Add Authentication and Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.UseSwagger();
app.UseSwaggerUI();

// Seed demo users for testing
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<WarrantyDbContext>();
    var userRepo = scope.ServiceProvider.GetRequiredService<IGenericRepository<User>>();
    
    // Ensure database is created
    await context.Database.EnsureCreatedAsync();
    
    // Check if users already exist
    var existingUsers = await userRepo.GetAllDataByExpression(new QueryOptions<User>
    {
        PageNumber = 0,
        PageSize = 0,
        Filter = null
    } );
    if (!existingUsers.Items.Any())
    {
        var demoUsers = new[]
        {
            new User
            {
                Id = Guid.NewGuid(),
                Email = "admin@warranty.com",
                Name = "System Admin",
                Role = "Admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            },
            new User
            {
                Id = Guid.NewGuid(),
                Email = "manager@warranty.com",
                Name = "John Manager",
                Role = "Manager",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("manager123"),
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            },
            new User
            {
                Id = Guid.NewGuid(),
                Email = "tech@warranty.com",
                Name = "Jane Technician",
                Role = "Technician",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("tech123"),
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            },
            new User
            {
                Id = Guid.NewGuid(),
                Email = "customer@warranty.com",
                Name = "Bob Customer",
                Role = "Customer",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("customer123"),
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            }
        };

        foreach (var user in demoUsers)
        {
            await userRepo.Insert(user);
        }
        
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        await unitOfWork.SaveChangesAsync();
        
        Console.WriteLine("Demo users created successfully!");
        Console.WriteLine("Admin: admin@warranty.com / admin123");
        Console.WriteLine("Manager: manager@warranty.com / manager123");
        Console.WriteLine("Technician: tech@warranty.com / tech123");
        Console.WriteLine("Customer: customer@warranty.com / customer123");
    }
}

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
