using Shader.Data.Dtos.Fruit;
using Shader.Data.Entities;

namespace Shader.Services.Abstraction
{
    public interface IFruitService
    {
        Task<IEnumerable<RFruitDto>> GetAllSupplierFruitsAsync(int supplierId);
        Task<IEnumerable<RFruitDto>> GetInStockSupplierFruitsAsync(int supplierId);
        Task<IEnumerable<Fruit>> GetSupplierFruitsToBeBilledAsync(int supplierId);
        Task<IEnumerable<RFruitDto>> GetAllFruitsAsync();
        Task<IEnumerable<RFruitDto>> GetUnAvailableFruitsAsync();
        Task<IEnumerable<RFruitDto>> GetInStockFruitsAsync();
        Task<IEnumerable<RFruitDto>> SearchWithFruitNameAsync(string fruitName);
        Task<IEnumerable<RFruitDto>> AddFruitsAsync(int supplierId, List<WRangeFruitDto> fruitDtos);
        Task<RFruitDto> GetFruitByIdAsync(int id);
        Task<RFruitDto> AddFruitCagesAsync(int id, int numberOfCages);
        Task<RFruitDto> UpdateFruitAsync(int id, UFruitDto fruit);
        Task<bool> DeleteFruitAsync(int id);

        // todo : check if the fruit has a bill or not
        // todo : check the transactions mortgage values have been done on the fruit if the mortgage amount changed or unchecked!
        // todo : a method to display today transactions on the fruit by fruit id
    }
}
