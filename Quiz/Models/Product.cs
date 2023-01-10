using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quiz.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        [Range(0.0, Double.MaxValue)]
        public double Price { get; set; }
        [NotMapped]
        public IFormFile Image { get; set; }
    }
}
