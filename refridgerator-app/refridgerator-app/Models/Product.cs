using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace refridgerator_app.Models
{
    internal class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Quantity { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
