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

// Uygulama i�in veri koruma hizmetlerini ekler (�rne�in: veri �ifreleme)
builder.Services.AddDataProtection();

// Controller tabanl� bir API geli�tirme i�in gerekli hizmetleri ekler
builder.Services.AddControllers();

// Swagger/OpenAPI deste�i ekler (API d�k�mantasyonu i�in)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // JWT i�in Swagger �zerinden g�venlik �emas� tan�mlar
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Scheme = "Bearer", // Bearer token kullan�m�
        BearerFormat = "JWT",
        Name = "Jwt Authentication",
        In = ParameterLocation.Header, // JWT, HTTP header'da g�nderilir
        Type = SecuritySchemeType.Http,
        Description = "Put **_ONLY_** your JWT Bearer Token on Textbox below!",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme,
        }
    };

    // Swagger'da JWT g�venlik tan�m�n� ekler
    options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    // Swagger i�in g�venlik gereksinimlerini ekler
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() },
    });
});

// JWT tabanl� kimlik do�rulama yap�land�rmas�
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, // Token sa�lay�c�s�n� do�rular
            ValidIssuer = builder.Configuration["Jwt:Issuer"], // Ge�erli sa�lay�c�
            ValidateAudience = true, // Token'�n hedef kitlesini do�rular
            ValidAudience = builder.Configuration["Jwt:Audience"], // Ge�erli hedef kitle
            ValidateLifetime = true, // Token s�resini do�rular
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!)) // �ifreleme anahtar�
        };
    });

// Veritaban� ba�lant� dizesini yap�land�r�r ve DbContext'i ekler
var connectionString = builder.Configuration.GetConnectionString("default");
builder.Services.AddDbContext<ShoppingAppDbContext>(options => options.UseSqlServer(connectionString));

// Scoped (request ba��na) ya�am d�ng�s� ile ba��ml�l�klar� ekler
builder.Services.AddScoped<TimeBasedAuthorizationFilter>(); // Zaman bazl� yetkilendirme filtresi
builder.Services.AddScoped<IOrderService, OrderService>(); // Sipari� servisi
builder.Services.AddScoped<IUserService, UserService>(); // Kullan�c� servisi
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>(); // �ifreleme servisi
builder.Services.AddScoped<IProductService, ProductService>(); // �r�n servisi
builder.Services.AddMemoryCache();

// Controller deste�i ekler
builder.Services.AddControllers();

// API u� noktalar�n� ke�fetmeyi sa�layan hizmeti ekler
builder.Services.AddEndpointsApiExplorer();

// Yetkilendirme hizmetini ekler
builder.Services.AddAuthorization();

var app = builder.Build();

// Geli�tirme ortam�nda Swagger d�k�mantasyonu aktif hale getirilir
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Uygulama seviyesinde middleware'leri s�ras�yla ekler
app.UseMiddleware<MaintenanceMiddleware>(); // Bak�m modu middleware
app.UseMiddleware<GlobalExceptionHandlerMiddleware>(); // Genel hata yakalama middleware
app.UseMiddleware<LoggingMiddleware>(); // �stek/yan�t loglama middleware

// HTTPS y�nlendirme middleware
app.UseHttpsRedirection();

// Kimlik do�rulama ve yetkilendirme middleware
app.UseAuthentication(); // Kullan�c� do�rulama
app.UseAuthorization();  // Kullan�c� yetkilendirme

// Controller rotalar�n� haritaland�r�r
app.MapControllers();

// Uygulamay� ba�lat�r
app.Run();
