using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingApp.Business.Dtos
{
    // Siparişe yeni bir ürün eklemek için kullanılan DTO (Data Transfer Object)
    public class OrderProductCreateDto
    {
        [Required] // Bu alanın doldurulması zorunludur.
        public int ProductId { get; set; } // Eklenecek ürünün kimlik numarası.

        [Required] // Bu alanın doldurulması zorunludur.
        public int Quantity { get; set; } // Siparişe eklenen ürünün miktarı.
    }
}
