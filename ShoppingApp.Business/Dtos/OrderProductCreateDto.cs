﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingApp.Business.Dtos
{
    public class OrderProductCreateDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
