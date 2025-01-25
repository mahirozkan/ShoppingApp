using System.ComponentModel.DataAnnotations;

namespace ShoppingApp.Business.Dtos
{
    public class ProductCreateModelDto
    {
        [Required]
        public string ProductName { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int StockQuantity { get; set; }
    }
}