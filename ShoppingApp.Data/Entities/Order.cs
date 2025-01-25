using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingApp.Data.Entities
{
    // Sipariş tablosunu temsil eden varlık sınıfı
    public class Order
    {
        [Key] // Birincil anahtar tanımı
        public int Id { get; set; } // Sipariş ID'si

        public DateTime OrderDate { get; set; } // Siparişin verildiği tarih

        public decimal TotalAmount { get; set; } // Siparişin toplam tutarı

        public int CustomerId { get; set; } // Siparişi veren müşterinin ID'si

        [ForeignKey("CustomerId")] // CustomerId alanının User tablosuyla ilişkilendirildiğini belirtir
        public User Customer { get; set; } // Siparişi veren müşteri bilgisi

        public ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>(); // Siparişle ilişkilendirilen ürünlerin listesi
    }
}