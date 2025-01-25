using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShoppingApp.Data.Entities
{
    // Ürün tablosunu temsil eden varlık sınıfı
    public class Product
    {
        [Key] // Birincil anahtar tanımı
        public int Id { get; set; } // Ürün ID'si

        [MaxLength(100)] // Ürün adı için maksimum uzunluk 100 karakter
        public string ProductName { get; set; } // Ürünün adı

        [Range(0.01, double.MaxValue, ErrorMessage = "Fiyat 0'dan yüksek olmalıdır.")] // Fiyatın pozitif bir değer olması gerekir
        public decimal Price { get; set; } // Ürünün fiyatı

        [Range(0, int.MaxValue, ErrorMessage = "Stok negatif olamaz.")] // Stok miktarının negatif olamayacağını belirtir
        public int StockQuantity { get; set; } // Ürünün stok miktarı

        public ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>(); // Ürünle ilişkilendirilen sipariş ürünleri listesi
    }
}