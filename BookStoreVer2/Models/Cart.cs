using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStoreVer2.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public decimal Money
        {
            get
            {
                 return Quantity * Price;
            }
        }        
    }
}