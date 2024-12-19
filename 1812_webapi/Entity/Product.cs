using System.ComponentModel.DataAnnotations;

namespace _1812_webapi.Entity
{
    public class Product
    {
        public int Id { get; set; }
        [Required] public string Name { get; set; } = null!;
        public int Price { get; set; }
    }
}
