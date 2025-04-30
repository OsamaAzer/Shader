using System.ComponentModel.DataAnnotations;

namespace Shader.Data.Entities
{
    public class Supplier
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; }
        [Required, MaxLength(100)]
        public string City { get; set; }
        [RegularExpression(@"^(?:\+20|0)?(1[0-2]|15)\d{8}$", ErrorMessage = "Invalid Egyptian phone number.")]
        public string? PhoneNumber { get; set; }
        public DateOnly? DateOfLastTransaction { get; set; } = null;
        public ICollection<Fruit> Fruits { get; set; } = new List<Fruit>();
    }
}
