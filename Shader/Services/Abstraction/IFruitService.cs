using Shader.Data.Dtos.CashTransaction;
using Shader.Data.Dtos.ClientTransaction;
using Shader.Data.Dtos.Fruit;
using Shader.Data.DTOs.ShaderTransaction;
using Shader.Data.Entities;
using Shader.Helpers;

namespace Shader.Services.Abstraction
{
    public interface IFruitService
    {
        Task<IEnumerable<RFruitsDto>> GetAllSupplierFruitsAsync(int supplierId);
        Task<IEnumerable<RFruitsDto>> GetInStockSupplierFruitsAsync(int supplierId);
        Task<IEnumerable<RFruitsDto>> GetSupplierFruitsToBeBilledAsync(int supplierId);
        Task<IEnumerable<RFruitsDto>> GetAllFruitsAsync();
        Task<IEnumerable<RFruitsDto>> GetUnAvailableFruitsAsync();
        Task<IEnumerable<RFruitsDto>> GetInStockFruitsAsync();
        Task<IEnumerable<RFruitsDto>> SearchWithFruitNameAsync(string fruitName);
        Task<IEnumerable<RFruitsDto>> AddFruitsAsync(int supplierId, List<WRangeFruitDto> fruitDtos);
        Task<RFruitDetailsDto> GetFruitByIdAsync(int id);
        Task<RFruitDetailsDto> AddFruitCagesAsync(int id, int numberOfCages);
        Task<RFruitDetailsDto> UpdateFruitAsync(int id, UFruitDto fruit);
        Fruit UpdateTookFruitInClientTransaction(Fruit fruit, WClientTFruitDto ctf);
        Fruit UpdateReturnedFruitInClientTransaction(Fruit fruit, WClientTFruitDto ctf);
        Fruit UpdateReturnedFruitInClientTransaction(Fruit fruit, ClientTransactionFruit ctf);
        Fruit UpdateTookFruitInCashTransaction(Fruit fruit, WCashTFruitDto ctf);
        Fruit UpdateReturnedFruitInCashTransaction(Fruit fruitEntity, CashTransactionFruit ctf);
        Fruit UpdateTookFruitInMerchantTransaction(Fruit fruit, WMerchantTFruitDto mtf);
        Fruit UpdateReturnedFruitInMerchantTransaction(Fruit removedFruit, MerchantTransactionFruit mtf); 
        Task<bool> DeleteFruitAsync(int id);
        // check if merchant fruits entered with mortgage and discount or not

        // todo : check if the fruit has a bill or not
        // todo : check the transactions mortgage values have been done on the fruit if the mortgage amount changed or unchecked!
        // todo : a method to display today transactions on the fruit by fruit id
    }
}
