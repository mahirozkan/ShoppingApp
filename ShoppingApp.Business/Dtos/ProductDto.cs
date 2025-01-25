using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingApp.Business.Dtos
{
    // Ürün bilgilerini temsil eden DTO (Data Transfer Object)
    public class ProductDto
    {
        public int Id { get; set; } // Ürünün benzersiz kimlik numarası.

        public string ProductName { get; set; } // Ürünün adı.

        public decimal Price { get; set; } // Ürünün fiyatı.

        public int StockQuantity { get; set; } // Ürünün mevcut stok miktarı.
    }
}
