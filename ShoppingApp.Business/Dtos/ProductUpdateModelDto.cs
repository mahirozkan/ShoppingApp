using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingApp.Business.Dtos
{
    // Ürün bilgilerini güncellemek için kullanılan DTO (Data Transfer Object)
    public class ProductUpdateModelDto
    {
        public string ProductName { get; set; } // Ürünün adı. Güncellenmek için kullanılabilir.

        public decimal Price { get; set; } // Ürünün fiyatı. Güncelleme yapılabilecek bir alan.

        public int StockQuantity { get; set; } // Stok miktarı. Ürünün mevcut stoğunu güncellemek için kullanılır.
    }
}
