using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingApp.Business.Dtos
{
    // Sipariş bilgilerini güncellemek için kullanılan DTO (Data Transfer Object)
    public class UpdateOrderDto
    {
        public DateTime OrderDate { get; set; } // Siparişin verildiği tarih.

        public decimal TotalAmount { get; set; } // Siparişin toplam tutarı.

        public int CustomerId { get; set; } // Siparişi veren müşterinin kimlik numarası.
    }
}
