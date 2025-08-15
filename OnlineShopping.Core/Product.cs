﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShopping.Core
{
    public class Product
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public int Price { get; set; }
        public string? ProductPhoto { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }

        // Navigation property 
        public virtual Category? Category { get; set; }
    }
}
