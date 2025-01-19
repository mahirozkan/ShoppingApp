using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingApp.Business.Dtos
{
    public class UpdateOrderDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public List<int> ProductIds { get; set; } = new List<int>();
    }
}
