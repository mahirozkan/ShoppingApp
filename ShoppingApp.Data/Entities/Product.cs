using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShoppingApp.Data.Entities
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(100)]
        public string ProductName { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "Fiyat 0'dan yüksek olmalıdır.")]
        public decimal Price { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Stok negatif olamaz.")]
        public int StockQuantity { get; set; }
        public ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
    }
}
