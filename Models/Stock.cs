using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Models
{
    public class Stock
    {
        [Key]
        public int ID { get; set; }
        public string? Name { get; set; }

        public int Quantity { get; set; }

        public int Price { get; set; }

        public string? Category { get; set; }
    }
}
