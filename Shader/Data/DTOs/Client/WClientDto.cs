using System.ComponentModel.DataAnnotations;

namespace Shader.Data.Dtos.Client
{
    public class WClientDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string City { get; set; }
        [RegularExpression(@"^(?:\+20|0)?(1[0-2]|15)\d{8}$", ErrorMessage = "Invalid Egyptian phone number.")]
        public string PhoneNumber { get; set; }
    }
}
