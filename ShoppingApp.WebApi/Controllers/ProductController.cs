using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingApp.Business.Dtos;
using ShoppingApp.Business.Interfaces;
using System.Threading.Tasks;

namespace ShoppingApp.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
                return NotFound(new { Message = "Ürün bulunamadı." });

            return Ok(product);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreateModelDto productDto)
        {
            var result = await _productService.CreateProductAsync(productDto);

            if (!result.IsSucceed)
                return BadRequest(new { Message = result.Message });

            return Created("", new { Message = result.Message });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductUpdateModelDto productDto)
        {
            var result = await _productService.UpdateProductAsync(id, productDto);

            if (!result.IsSucceed)
                return NotFound(new { Message = result.Message });

            return Ok(new { Message = result.Message });
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PatchProduct(int id, [FromBody] ProductPatchModelDto productDto)
        {
            var result = await _productService.PatchProductAsync(id, productDto);

            if (!result.IsSucceed)
                return NotFound(new { Message = result.Message });

            return Ok(new { Message = result.Message });
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _productService.DeleteProductAsync(id);

            if (!result.IsSucceed)
                return NotFound(new { Message = result.Message });

            return Ok(new { Message = result.Message });
        }
    }
}
