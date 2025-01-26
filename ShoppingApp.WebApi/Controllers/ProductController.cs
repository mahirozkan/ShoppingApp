using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingApp.Business.Dtos;
using ShoppingApp.Business.Interfaces;
using System.Threading.Tasks;

namespace ShoppingApp.WebApi.Controllers
{
    // Ürünlerle ilgili işlemleri sağlayan API kontrolcüsü
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService; // Ürün servisinin bağımlılığı

        public ProductController(IProductService productService)
        {
            // Ürün servisinin bağımlılığını başlatır
            _productService = productService;
        }

        // Tüm ürünleri getirir
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllProducts()
        {
            // Ürünleri servis aracılığıyla alır
            var products = await _productService.GetAllProductsAsync();
            return Ok(products); // Ürün listesini döner
        }

        // Belirtilen ID'ye göre ürünü getirir
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductById(int id)
        {
            // Ürünü ID'ye göre servis aracılığıyla alır
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
                return NotFound(new { Message = "Ürün bulunamadı." }); // Ürün bulunamazsa hata mesajı döner

            return Ok(product); // Ürünü döner
        }

        // Yeni bir ürün oluşturur
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreateModelDto productDto)
        {
            // Ürün oluşturma işlemini gerçekleştirir
            var result = await _productService.CreateProductAsync(productDto);

            if (!result.IsSucceed)
                return BadRequest(new { Message = "Aynı isimli ürün mevcut." }); // Hata durumunda mesaj döner

            return Created("", new { Message = result.Message }); // Başarı durumunda mesaj döner
        }

        // Belirtilen ID'ye göre ürünü tamamen günceller
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductUpdateModelDto productDto)
        {
            // Ürün güncelleme işlemini gerçekleştirir
            var result = await _productService.UpdateProductAsync(id, productDto);

            if (!result.IsSucceed)
                return NotFound(new { Message = result.Message }); // Ürün bulunamazsa hata mesajı döner

            return Ok(new { Message = result.Message }); // Başarı durumunda mesaj döner
        }

        // Belirtilen ID'ye göre ürünü kısmen günceller
        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PatchProduct(int id, [FromBody] ProductPatchModelDto productDto)
        {
            // Gelen model doğrulanmazsa hata mesajı döner
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Ürün kısmi güncelleme işlemini gerçekleştirir
            var result = await _productService.PatchProductAsync(id, productDto);

            if (!result.IsSucceed)
            {
                return NotFound(new { Message = result.Message }); // Ürün bulunamazsa hata mesajı döner
            }

            return Ok(new { Message = result.Message }); // Başarı durumunda mesaj döner
        }

        // Belirtilen ID'ye göre ürünü siler
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            // Ürün silme işlemini gerçekleştirir
            var result = await _productService.DeleteProductAsync(id);

            if (!result.IsSucceed)
                return NotFound(new { Message = result.Message }); // Ürün bulunamazsa hata mesajı döner

            return Ok(new { Message = result.Message }); // Başarı durumunda mesaj döner
        }

        [HttpGet("paged")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPagedProducts([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            // Varsayılan olarak sayfa 1 ve sayfa büyüklüğü 10 kullanılır.
            // page: İstenen sayfa numarası
            // pageSize: Bir sayfada kaç öğe olduğu

            // Servisten sayfalama sonuçlarını alıyoruz
            var pagedResult = await _productService.GetPagedProductsAsync(page, pageSize);
            return Ok(pagedResult); // Sayfalama sonuçlarını API tüketicisine döndürüyoruz
        }
    }
}