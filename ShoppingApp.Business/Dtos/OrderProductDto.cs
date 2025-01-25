using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingApp.Business.Dtos
{
    // Siparişde yer alan ürünü temsil eden DTO (Data Transfer Object)
    public class OrderProductDto
    {
        public int ProductId { get; set; } // Ürünün benzersiz kimlik numarası.

        public string ProductName { get; set; } // Siparişdeki ürünün adı.

        public int Quantity { get; set; } // Siparişdeki ürünün miktarı.
    }
}
