using ShoppingApp.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingApp.Data.Entities
{
    // Sipariş ve Ürün arasındaki ilişkiyi temsil eden ara tablo
    public class OrderProduct
    {
        public int OrderId { get; set; } // Sipariş ID'si

        [ForeignKey("OrderId")] // OrderId alanının Order tablosuyla ilişkilendirildiğini belirtir
        public Order Order { get; set; } // Sipariş bilgisi

        public int ProductId { get; set; } // Ürün ID'si

        [ForeignKey("ProductId")] // ProductId alanının Product tablosuyla ilişkilendirildiğini belirtir
        public Product Product { get; set; } // Ürün bilgisi

        public int Quantity { get; set; } // Sipariş edilen ürün miktarı
    }
}