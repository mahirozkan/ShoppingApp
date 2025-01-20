using System.ComponentModel.DataAnnotations;

namespace ShoppingApp.Business.Dtos
{
    public class ProductUpdateModelDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string ProductName { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Fiyat 0'dan büyük olmalıdır.")]
        public decimal Price { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Stok 0'dan büyük olmalıdır.")]
        public int StockQuantity { get; set; }
    }
}