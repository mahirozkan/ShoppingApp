﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingApp.Business.Dtos
{
    public class ProductPatchModelDto
    {
        public string ProductName { get; set; }
        public decimal? Price { get; set; }
        public int? StockQuantity { get; set; }
    }
}
