using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ShoppingApp.Business.Interfaces;
using ShoppingApp.Business.Services;
using ShoppingApp.Data.Context;
using ShoppingApp.WebApi.Filters;
using ShoppingApp.WebApi.Middlewares;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Uygulama için veri koruma hizmetlerini ekler (örneðin: veri þifreleme)
builder.Services.AddDataProtection();

// Controller tabanlý bir API geliþtirme için gerekli hizmetleri ekler
builder.Services.AddControllers();

// Swagger/OpenAPI desteði ekler (API dökümantasyonu için)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // JWT için Swagger üzerinden güvenlik þemasý tanýmlar
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Scheme = "Bearer", // Bearer token kullanýmý
        BearerFormat = "JWT",
        Name = "Jwt Authentication",
        In = ParameterLocation.Header, // JWT, HTTP header'da gönderilir
        Type = SecuritySchemeType.Http,
        Description = "Put **_ONLY_** your JWT Bearer Token on Textbox below!",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme,
        }
    };

    // Swagger'da JWT güvenlik tanýmýný ekler
    options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    // Swagger için güvenlik gereksinimlerini ekler
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() },
    });
});

// JWT tabanlý kimlik doðrulama yapýlandýrmasý
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, // Token saðlayýcýsýný doðrular
            ValidIssuer = builder.Configuration["Jwt:Issuer"], // Geçerli saðlayýcý
            ValidateAudience = true, // Token'ýn hedef kitlesini doðrular
            ValidAudience = builder.Configuration["Jwt:Audience"], // Geçerli hedef kitle
            ValidateLifetime = true, // Token süresini doðrular
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!)) // Þifreleme anahtarý
        };
    });

// Veritabaný baðlantý dizesini yapýlandýrýr ve DbContext'i ekler
var connectionString = builder.Configuration.GetConnectionString("default");
builder.Services.AddDbContext<ShoppingAppDbContext>(options => options.UseSqlServer(connectionString));

// Scoped (request baþýna) yaþam döngüsü ile baðýmlýlýklarý ekler
builder.Services.AddScoped<TimeBasedAuthorizationFilter>(); // Zaman bazlý yetkilendirme filtresi
builder.Services.AddScoped<IOrderService, OrderService>(); // Sipariþ servisi
builder.Services.AddScoped<IUserService, UserService>(); // Kullanýcý servisi
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>(); // Þifreleme servisi
builder.Services.AddScoped<IProductService, ProductService>(); // Ürün servisi
builder.Services.AddMemoryCache();

// Controller desteði ekler
builder.Services.AddControllers();

// API uç noktalarýný keþfetmeyi saðlayan hizmeti ekler
builder.Services.AddEndpointsApiExplorer();

// Yetkilendirme hizmetini ekler
builder.Services.AddAuthorization();

var app = builder.Build();

// Geliþtirme ortamýnda Swagger dökümantasyonu aktif hale getirilir
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Uygulama seviyesinde middleware'leri sýrasýyla ekler
app.UseMiddleware<MaintenanceMiddleware>(); // Bakým modu middleware
app.UseMiddleware<GlobalExceptionHandlerMiddleware>(); // Genel hata yakalama middleware
app.UseMiddleware<LoggingMiddleware>(); // Ýstek/yanýt loglama middleware

// HTTPS yönlendirme middleware
app.UseHttpsRedirection();

// Kimlik doðrulama ve yetkilendirme middleware
app.UseAuthentication(); // Kullanýcý doðrulama
app.UseAuthorization();  // Kullanýcý yetkilendirme

// Controller rotalarýný haritalandýrýr
app.MapControllers();

// Uygulamayý baþlatýr
app.Run();
