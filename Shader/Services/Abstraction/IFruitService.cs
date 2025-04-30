using Shader.Data.DTOs;
using Shader.Data.Entities;

namespace Shader.Services.Abstraction
{
    public interface IFruitService
    {
        Task<IEnumerable<RAllFruitsDTO>> GetAllSupplierFruitsAsync(int supplierId);
        Task<IEnumerable<RAllFruitsDTO>> GetAllFruitsAsync();
        Task<RFruitDTO?> GetFruitByIdAsync(int id);
        Task<IEnumerable<RFruitDTO>> AddFruitsAsync(int supplierId, List<WRangeFruitDTO> fruitDTOs);
        Task<RFruitDTO> AddFruitAsync(AFruitDTO fruit);
        Task<RFruitDTO> UpdateFruitAsync(int id, UFruitDTO fruit);
        Task<bool> DeleteFruitAsync(int id);
    }
}
