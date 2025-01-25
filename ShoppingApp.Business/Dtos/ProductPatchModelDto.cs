using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingApp.Business.Dtos
{
    // Ürün bilgilerini kısmi olarak güncellemek için kullanılan DTO (Data Transfer Object)
    public class ProductPatchModelDto
    {
        public string ProductName { get; set; } // Ürünün adı. Güncellenebilir ancak isteğe bağlı bir alandır.

        public decimal? Price { get; set; } // Ürünün fiyatı. Nullable olarak belirtilmiştir, yani gönderilmek zorunda değildir.

        public int? StockQuantity { get; set; } // Stok miktarı. Nullable olarak belirtilmiştir, isteğe bağlı gönderilebilir.
    }
}
