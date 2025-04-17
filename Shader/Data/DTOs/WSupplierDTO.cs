using System.ComponentModel.DataAnnotations;

namespace Shader.Data.DTOs
{
    public class WSupplierDTO
    {
        [Required, MaxLength(100)]
        public string Name { get; set; }
        [Required, MaxLength(100)]
        public string City { get; set; }
        [RegularExpression(@"^(?:\+20|0)?(1[0-2]|15)\d{8}$", ErrorMessage = "Invalid Egyptian phone number.")]
        public string PhoneNumber { get; set; }
    }
}
