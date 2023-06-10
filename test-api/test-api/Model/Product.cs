using System.ComponentModel.DataAnnotations.Schema;

namespace test_api.Model
{
    public class Product
    {
        [Column("ProductID")]
        public int ProductId { get; set; }

        [Column("Name")]
        public string? Name { get; set; }

        [Column("ProductNumber")]
        public string? ProductNumber { get; set; }

        [Column("StandardCost")]
        public double StandardCost { get; set; }

        [Column("ListPrice")]
        public double ListPrice { get; set; }
    }
}

