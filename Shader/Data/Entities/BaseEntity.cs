using Shader.Enums;
using System.ComponentModel.DataAnnotations;

namespace Shader.Data.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        [Phone]
        public string? PhoneNumber { get; set; }
    }
}
