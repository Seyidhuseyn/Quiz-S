using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Quiz.ViewModels
{
    public class CreateProductVM
    {
        public string Name { get; set; }
        [Range(0.0, Double.MaxValue)]
        public double Price { get; set; }
        [NotMapped]
        public IFormFile Image { get; set; }
    }
}
