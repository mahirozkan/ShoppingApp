using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingApp.Business.Types
{
    // Genel hizmet mesajını temsil eden sınıf
    public class ServiceMessage
    {
        public bool IsSucceed { get; set; } // İşlemin başarılı olup olmadığını belirtir.
        public string Message { get; set; } // İşlemin sonucu hakkında bilgi veren mesaj.
    }

    // T tipiyle birlikte veri döndüren hizmet mesajını temsil eden sınıf
    public class ServiceMessage<T>
    {
        public bool IsSucceed { get; set; } // İşlemin başarılı olup olmadığını belirtir.
        public string Message { get; set; } // İşlemin sonucu hakkında bilgi veren mesaj.
        public T? Data { get; set; } // İşlemin döndürdüğü veriyi temsil eder (generic tip).
    }
}