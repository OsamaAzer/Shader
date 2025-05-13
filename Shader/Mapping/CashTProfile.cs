using Shader.Data.Dtos.CashTransaction;
using Shader.Data.DTOs.CashTransaction;
using Shader.Data.Entities;

namespace Shader.Mapping
{
    public static class CashTProfile
    {
        public static CashTransaction MapToCashTransaction(this WCashTDto cashTDto, CashTransaction? transaction = null)
        {
            transaction ??= new CashTransaction();
            if (transaction.Date == default)
            {
                transaction.Date = DateTime.Now;
            }
            transaction.Description = cashTDto.Description;
            transaction.Price = cashTDto.CashTransactionFruits.Sum(fruit => fruit.PriceOfKiloGram * fruit.WeightInKilograms);
            transaction.CashTransactionFruits = cashTDto.CashTransactionFruits.Select(fruit => new CashTransactionFruit
            {
                FruitId = fruit.FruitId,
                NumberOfCages = fruit.NumberOfCages,
                PriceOfKiloGram = fruit.PriceOfKiloGram,
                WeightInKilograms = fruit.WeightInKilograms,
                TransactionPrice = fruit.PriceOfKiloGram * fruit.WeightInKilograms
            }).ToList();
            return transaction;
        }

        public static RCashTDto MapToRCashTDto(this CashTransaction cashTransaction)
        {
            return new RCashTDto
            {
                Id = cashTransaction.Id,
                Date = cashTransaction.Date,
                Description = cashTransaction.Description,
                Price = cashTransaction.CashTransactionFruits.Sum(cashTransactionFruit => cashTransactionFruit.PriceOfKiloGram * cashTransactionFruit.WeightInKilograms),
                CashTransactionFruits = cashTransaction.CashTransactionFruits.Select(cashTransactionFruit => new RCashTFruitDto
                {
                    FruitName = cashTransactionFruit.Fruit.FruitName,
                    NumberOfCages = cashTransactionFruit.NumberOfCages,
                    WeightInKilograms = cashTransactionFruit.WeightInKilograms,
                    PriceOfKiloGram = cashTransactionFruit.PriceOfKiloGram,
                    TransactionPrice = cashTransactionFruit.TransactionPrice
                }).ToList()
            };
        }

        public static IEnumerable<RCashTDto> MapToRCashTDto(this IEnumerable<CashTransaction> cashTransactions)
        {
            return cashTransactions.Select(cashTransaction => new RCashTDto
            {
                Id = cashTransaction.Id,
                Date = cashTransaction.Date,
                Description = cashTransaction.Description,
                Price = cashTransaction.Price,
                CashTransactionFruits = cashTransaction.CashTransactionFruits.Select(cashTransactionFruit => new RCashTFruitDto
                {
                    FruitName = cashTransactionFruit.Fruit.FruitName,
                    NumberOfCages = cashTransactionFruit.NumberOfCages,
                    WeightInKilograms = cashTransactionFruit.WeightInKilograms,
                    PriceOfKiloGram = cashTransactionFruit.PriceOfKiloGram,
                    TransactionPrice = cashTransactionFruit.TransactionPrice
                }).ToList()
            });
        }

    }
}
