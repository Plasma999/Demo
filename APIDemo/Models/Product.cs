using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIDemo.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Price { get; set; }
        public int Stock { get; set; }
    }
}