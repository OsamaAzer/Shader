using Shader.Data.Dtos.ClientTransaction;
using Shader.Data.DTOs.ClientTransaction;
using Shader.Data.DTOs.ShaderTransaction;
using Shader.Data.Entities;

namespace Shader.Mapping
{
    public static class MerchantTProfile
    {
        public static IEnumerable<RMerchantTDto> MapToRMerchantTDto(this IEnumerable<MerchantTransaction> merchantTransactions)
        {
            return merchantTransactions.Select(merchantTransaction => new RMerchantTDto
            {
                MerchantName = merchantTransaction.Merchant.Name,
                Date = merchantTransaction.Date,
                TotalAmount = merchantTransaction.TotalAmount,
                //RemainingAmount = merchantTransaction.RemainingAmount,
                TotalCageMortgageAmount = merchantTransaction.TotalCageMortgageAmount,
                //RemainingMortgageAmount = merchantTransaction.RemainingMortgageAmount,
                MerchantTransactionFruits = merchantTransaction.MerchantTransactionFruits.Select(merchantTransactionFruit => new RMerchantTFruitDto
                {
                    FruitName = merchantTransactionFruit.Fruit.FruitName,
                    NumberOfCages = merchantTransactionFruit.NumberOfCages,
                    WeightInKilograms = merchantTransactionFruit.WeightInKilograms,
                    PriceOfKiloGram = merchantTransactionFruit.PriceOfKiloGram
                }).ToList()
            });
        }
        public static RMerchantTDetailsDto MapToRMerchantTDetailsDto(this MerchantTransaction merchantTransaction)
        {
            return new RMerchantTDetailsDto
            {

                MerchantName = merchantTransaction.Merchant.Name,
                Date = merchantTransaction.Date,
                Description = merchantTransaction.Description,
                Price = merchantTransaction.Price,
                DiscountAmount = merchantTransaction.DiscountAmount,
                TotalAmount = merchantTransaction.TotalAmount,
                //AmountPaid = merchantTransaction.AmountPaid,
                //RemainingAmount = merchantTransaction.RemainingAmount,
                TotalCageMortgageAmount = merchantTransaction.TotalCageMortgageAmount,
                //TotalCageMortgageAmountPaid = merchantTransaction.TotalCageMortgageAmountPaid,
                //RemainingMortgageAmount = merchantTransaction.RemainingMortgageAmount,
                MerchantTransactionFruits = merchantTransaction.MerchantTransactionFruits.Select(merchantTransactionFruit => new RMerchantTFruitDto
                {
                    FruitName = merchantTransactionFruit.Fruit.FruitName,
                    NumberOfCages = merchantTransactionFruit.NumberOfCages,
                    WeightInKilograms = merchantTransactionFruit.WeightInKilograms,
                    PriceOfKiloGram = merchantTransactionFruit.PriceOfKiloGram
                }).ToList()
            };
        }

        public static MerchantTransaction MapToMerchantTransaction(this WMerchantTDto stDto, MerchantTransaction? transaction = null)
        {
            transaction ??= new MerchantTransaction();

            transaction.Description = stDto.Description;
            transaction.MerchantId = stDto.ShaderId;
            if (transaction.Date == default)
            {
                transaction.Date = DateTime.Now;
            }
            transaction.Price = stDto.MerchantTransactionFruits
                .Select(c => c.PriceOfKiloGram * c.WeightInKilograms).Sum();
            transaction.TotalAmount = transaction.Price - transaction.DiscountAmount;
            transaction.MerchantTransactionFruits = stDto.MerchantTransactionFruits.Select(stfDto => new MerchantTransactionFruit
            {
                FruitId = stfDto.FruitId,
                NumberOfCages = stfDto.NumberOfCages,
                WeightInKilograms = stfDto.WeightInKilograms,
                PriceOfKiloGram = stfDto.PriceOfKiloGram
            }).ToList();
            //transaction.RemainingAmount = transaction.TotalAmount - transaction.AmountPaid;

            return transaction;
        }
    }
}
