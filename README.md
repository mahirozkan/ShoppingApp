# ShoppingApp

ShoppingApp, bir online alışveriş platformu API’sidir. ASP.NET Core ile geliştirilmiştir ve Entity Framework Code First yaklaşımı kullanılarak veritabanı işlemleri gerçekleştirilmiştir. JWT tabanlı kimlik doğrulama ve yetkilendirme, katmanlı mimari, özel middleware'ler ve global exception handling gibi modern yazılım mimarisi prensiplerini barındırır.

## Özellikler

- **Katmanlı Mimari**: Presentation, Business ve Data Access katmanları.
- **Entity Framework Core**: Code First yaklaşımı ile geliştirilmiş.
- **Kimlik Doğrulama ve Yetkilendirme**:
  - JWT tabanlı kimlik doğrulama.
  - Kullanıcı rolleri (Admin ve Customer).
- **CRUD İşlemleri**:
  - Kullanıcı, ürün ve sipariş için CRUD endpoint'leri.
  - Patch (kısmi güncelleme) desteği.
- **Middleware’ler**:
  - LoggingMiddleware: Gelen ve giden isteklerin loglanması.
  - MaintenanceMiddleware: API’yi bakım moduna alma.
  - GlobalExceptionHandlerMiddleware: Hata yönetimi.
- **Validation**: Model doğrulama (e-posta formatı, fiyat, stok gibi alanlar için).
- **Action Filters**: Belirli API’lerin belirli zamanlarda çalışması için filtre.

## Gereksinimler

- .NET 8 SDK
- SQL Server (veya uygun bir veritabanı sunucusu)
- Postman veya benzeri bir API istemcisi (testler için)

## Kurulum

1. **Kaynak Kodunu Klonlayın**:
   ```bash
   git clone https://github.com/mahirozkan/ShoppingApp.git
   cd ShoppingApp
   ```

2. **Veritabanı Ayarı**:
   `appsettings.json` dosyasındaki `ConnectionStrings` bölümünü düzenleyin:
   ```json
   "ConnectionStrings": {
     "default": "Server=YOUR_SERVER;Database=ShoppingAppDb;Trusted_Connection=True;TrustServerCertificate=True;"
   }
   ```

3. **Migration ve Veritabanı Oluşturma**:
   ```bash
   dotnet ef database update
   ```

4. **Uygulamayı Çalıştırın**:
   ```bash
   dotnet run --project ShoppingApp.WebApi
   ```

5. **Swagger Kullanarak API Testi**:
   Uygulama çalıştıktan sonra [http://localhost:5000/swagger](http://localhost:5000/swagger) adresinden API endpoint'lerini test edebilirsiniz.

## Kullanım

### Kimlik Doğrulama

1. **Kullanıcı Kaydı (Register)**:
   ```http
   POST /api/Auth/register
   ```
   **Body:**
   ```json
   {
     "firstName": "John",
     "lastName": "Doe",
     "email": "john.doe@example.com",
     "password": "1234",
     "role": 0
   }
   ```

2. **Giriş Yapma (Login)**:
   ```http
   POST /api/Auth/login
   ```
   **Body:**
   ```json
   {
     "email": "john.doe@example.com",
     "password": "1234"
   }
   ```
   **Response:**
   ```json
   {
     "message": "Giriş başarılı.",
     "token": "{JWT_TOKEN}",
     "data": {
       "id": 1,
       "firstName": "John",
       "lastName": "Doe",
       "email": "john.doe@example.com",
       "role": "Customer"
     }
   }
   ```

### Ürün CRUD

1. **Tüm Ürünleri Listeleme**:
   ```http
   GET /api/Product
   ```

2. **Yeni Ürün Ekleme (Admin)**:
   ```http
   POST /api/Product
   ```
   **Body:**
   ```json
   {
     "productName": "Laptop",
     "price": 1500.99,
     "stockQuantity": 10
   }
   ```

3. **Ürün Güncelleme (Admin)**:
   ```http
   PUT /api/Product/{id}
   ```

4. **Ürün Silme (Admin)**:
   ```http
   DELETE /api/Product/{id}
   ```

### Sipariş CRUD

1. **Sipariş Oluşturma (Customer)**:
   ```http
   POST /api/Order
   ```
   **Body:**
   ```json
   {
     "orderDate": "2025-01-25T12:00:00",
     "totalAmount": 500,
     "customerId": 1,
     "products": [
       { "productId": 1, "quantity": 2 },
       { "productId": 2, "quantity": 1 }
     ]
   }
   ```

2. **Sipariş Silme (Admin)**:
   ```http
   DELETE /api/Order/{id}
   ```

## Test

- **Swagger**: Uygulamayı çalıştırdıktan sonra Swagger arayüzünden tüm endpoint'leri test edebilirsiniz.
- **Postman**: JWT token ile authenticated API istekleri göndererek test yapabilirsiniz.

## Katkılar

Katkı sağlamak için lütfen bir pull request oluşturun veya issue açın. Bu proje geliştirilmeye açıktır.

## Lisans

Bu proje MIT lisansı altında lisanslanmıştır. Ayrıntılar için [LICENSE](LICENSE) dosyasına bakın.

