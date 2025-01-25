using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingApp.Business.Dtos
{
    // Yeni bir sipariş oluşturmak için kullanılan DTO (Data Transfer Object)
    public class CreateOrderDto
    {
        public DateTime OrderDate { get; set; } // Siparişin oluşturulduğu tarih.

        public decimal TotalAmount { get; set; } // Siparişin toplam tutarı.

        public int CustomerId { get; set; } // Siparişi veren müşterinin kimlik numarası.

        public List<OrderProductCreateDto> Products { get; set; } // Siparişe dahil olan ürünlerin listesi.
    }
}
