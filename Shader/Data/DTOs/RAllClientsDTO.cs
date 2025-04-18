using Shader.Enums;

namespace Shader.Data.DTOs
{
    public class RAllClientsDTO
    {
        public string Name { get; set; }
        public string City { get; set; }
        public string PhoneNumber { get; set; }
        public DateOnly? DateOfLastTransaction { get; set; }
        public Status Status { get; set; }
    }
}
