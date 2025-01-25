using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingApp.Business.Dtos
{
    // Sipariş bilgilerini temsil eden DTO (Data Transfer Object)
    public class OrderDto
    {
        public int Id { get; set; } // Siparişin benzersiz kimlik numarası.

        public DateTime OrderDate { get; set; } // Siparişin oluşturulduğu tarih.

        public decimal TotalAmount { get; set; } // Siparişin toplam tutarı.

        public int CustomerId { get; set; } // Siparişi veren müşterinin kimlik numarası.

        public List<OrderProductDto> Products { get; set; } // Siparişde yer alan ürünlerin listesi.
    }
}
