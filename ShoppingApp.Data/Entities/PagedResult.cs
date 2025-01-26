using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingApp.Data.Entities
{
    public class PagedResult<T> // Sayfalama sonuçlarını taşımak için genel bir sınıf tanımlıyoruz.
    {
        public int CurrentPage { get; set; } // Şu anki sayfa numarası
        public int PageSize { get; set; } // Her sayfadaki öğe sayısı
        public int TotalCount { get; set; } // Toplam öğe sayısı
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize); // Toplam sayfa sayısını hesaplamak için bir özellik
        public List<T> Items { get; set; } // Sayfaya ait verilerin listesi
    }

}
