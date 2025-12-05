using System.ComponentModel.DataAnnotations;

namespace Authentication_2025.Models
{
    public class Fruit_Table
    {
        [Key]
        public int Id { get; set; }
        public string UserOfFruit { get; set; }
        public string FruitName { get; set; }
        public int Quantity { get; set; }
    }
}
