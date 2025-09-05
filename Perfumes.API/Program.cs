using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Perfumes.BLL.Configuration;
using Perfumes.BLL.Mapping;
using Perfumes.BLL.Services.Implementations;
using Perfumes.BLL.Services.Interfaces;
using Perfumes.DAL;
using Perfumes.DAL.Repositories;
using Perfumes.DAL.UnitOfWork;
using AutoMapper;
using Perfumes.API.Middleware;
using Perfumes.DAL.Data;
using StackExchange.Redis;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Register DbContext with provider from configuration (SqlServer or Sqlite)
var dbProvider = builder.Configuration.GetValue<string>("DatabaseProvider") ?? "SqlServer";
builder.Services.AddDbContext<PerfumesDbContext>(options =>
{
    if (dbProvider.Equals("Sqlite", StringComparison.OrdinalIgnoreCase))
    {
        var sqliteConn = builder.Configuration.GetConnectionString("SqliteConnection") ?? "Data Source=perfumes_dev.db";
        options.UseSqlite(sqliteConn);
    }
    else
    {
        options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection"),
            sqlOptions =>
            {
                // Enable transient fault handling for SQL Server
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorNumbersToAdd: null);
            });
    }
});

// Register Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Register Repositories
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IBrandRepository, BrandRepository>();
builder.Services.AddScoped<ICouponRepository, CouponRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// Register Generic Repository
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Register AutoMapper with restricted method mapping to avoid extension-method scanning issues
builder.Services.AddAutoMapper(cfg =>
{
    cfg.ShouldMapMethod = _ => false;
}, typeof(MappingProfile).Assembly);

// Register Services
builder.Services.AddScoped<IPaymentService, PaymobPaymentService>();
builder.Services.AddScoped<IEmailService, SendGridEmailService>();
// Register base services
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IWishlistService, WishlistService>();
builder.Services.AddScoped<IFileStorageService, FileStorageService>();
builder.Services.AddScoped<ICouponService, CouponService>();
builder.Services.AddScoped<IChatbotService, ChatbotService>();
builder.Services.AddScoped<JwtService>();

// Note: Removed explicit cached decorator registrations to simplify DI graph

// Register Memory Cache
builder.Services.AddMemoryCache();

// Configure Redis
var redisSettings = builder.Configuration.GetSection("RedisSettings").Get<RedisSettings>();
builder.Services.AddSingleton<IConnectionMultiplexer>(provider =>
{
    var configuration = ConfigurationOptions.Parse(redisSettings?.ConnectionString ?? "localhost:6379");
    configuration.ConnectRetry = redisSettings?.ConnectRetry ?? 3;
    configuration.ConnectTimeout = redisSettings?.ConnectTimeout ?? 5000;
    configuration.SyncTimeout = redisSettings?.SyncTimeout ?? 5000;
    return ConnectionMultiplexer.Connect(configuration);
});

// Register new services
builder.Services.AddScoped<ILoggingService, LoggingService>();
builder.Services.AddScoped<ICachingService, CachingService>();
builder.Services.AddScoped<IRedisCacheService, RedisCacheService>();

// Configure JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings?.SecretKey ?? "default-secret-key")),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings?.Issuer ?? "PerfumesAPI",
        ValidateAudience = true,
        ValidAudience = jwtSettings?.Audience ?? "PerfumesClient",
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("CustomerOnly", policy => policy.RequireRole("Customer"));
});

// CORS for Angular frontend (localhost:4200)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        // Allow localhost and any ngrok tunnels for development
        policy
            .SetIsOriginAllowed(origin =>
            {
                if (string.IsNullOrWhiteSpace(origin)) return false;
                if (origin.StartsWith("http://localhost:4200") || origin.StartsWith("https://localhost:4200")) return true;
                // ngrok domains
                return origin.Contains("ngrok-free.app") || origin.Contains("ngrok.app");
            })
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
});

// Link Settings to AppSettings
builder.Services.Configure<PaymobSettings>(builder.Configuration.GetSection("Paymob"));
builder.Services.Configure<SendGridSettings>(builder.Configuration.GetSection("SendGrid"));

// Configure Swagger with JWT support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Perfumes API", Version = "v1" });
    
    // Use default schema IDs (unique type names) for Swagger
    
    // Add JWT authentication to Swagger
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Add Global Exception Handler
app.UseMiddleware<GlobalExceptionHandler>();

// Serve static files from wwwroot (for product images)
app.UseStaticFiles();

// Use CORS before auth
app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Initialize database with seed data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<PerfumesDbContext>();
    await DbInitializer.InitializeAsync(context);
}

app.Run();
