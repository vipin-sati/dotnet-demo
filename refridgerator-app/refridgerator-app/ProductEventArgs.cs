using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using refridgerator_app.Models;

namespace refridgerator_app
{
    internal class ProductEventArgs: EventArgs
    {
        public Product ExpiringProduct { get; }

        public ProductEventArgs(Product product)
        {
            ExpiringProduct = product;
        }
    }
}
