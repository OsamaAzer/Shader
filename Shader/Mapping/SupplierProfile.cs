using Shader.Data.DTOs;
using Shader.Data.Entities;

namespace Shader.Mapping
{
    public static class SupplierProfile
    {
        public static WSupplierDTO ToDTO(this Supplier supplier)
        {
            return new WSupplierDTO
            {
                Name = supplier.Name,
                City = supplier.City,
                PhoneNumber = supplier.PhoneNumber
            };
        }
       
        public static Supplier ToEntity(this WSupplierDTO supplierDTO)
        {
            return new Supplier
            {
                Name = supplierDTO.Name,
                City = supplierDTO.City,
                PhoneNumber = supplierDTO.PhoneNumber
            };
        }

        public static IEnumerable<WSupplierDTO> ToDTO(this IEnumerable<Supplier> suppliers)
        {
            var supplierDTOs =  suppliers.Select(s => s.ToDTO());
            return supplierDTOs;
        }
    }
}
