using System.ComponentModel.DataAnnotations;

namespace ShoppingApp.Business.Dtos
{
    public class ProductCreateModelDto
    {
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
    }
}