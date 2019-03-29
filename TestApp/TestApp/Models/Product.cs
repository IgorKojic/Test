using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TestApp.Models
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime ValidFrom { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public int ProductCategoryID { get; set; }
    }
}