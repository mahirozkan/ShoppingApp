using System.ComponentModel.DataAnnotations;

namespace ShoppingApp.Business.Dtos
{
    public class ProductCreateModelDto
    {
        // Ürün adı. Gerekli bir alan olarak işaretlenmiştir.
        [Required]
        public string ProductName { get; set; }

        // Ürün fiyatı. Gerekli bir alan olarak işaretlenmiştir.
        [Required]
        public decimal Price { get; set; }

        // Stok miktarı. Gerekli bir alan olarak işaretlenmiştir.
        [Required]
        public int StockQuantity { get; set; }
    }
}