using Microsoft.EntityFrameworkCore;
using Shader.Data;
using Shader.Data.Dtos.ClientTransaction;
using Shader.Data.DTOs.ClientTransaction;
using Shader.Data.Entities;

namespace Shader.Mapping
{
    public static class ClientTProfile
    {
        public static IEnumerable<RClientTDto> MapToRClientTDto(this IEnumerable<ClientTransaction> clientTransactions)
        {
            return clientTransactions.Select(clientTransaction => new RClientTDto
            {
                ClientName = clientTransaction.Client.Name,
                Date = clientTransaction.Date,
                TotalAmount = clientTransaction.TotalAmount,
                TotalCageMortgageAmount = clientTransaction.TotalCageMortgageAmount,
                ClientTransactionFruits = clientTransaction.ClientTransactionFruits.Select(clientTransactionFruit => new RClientTFruitDto
                {
                    FruitName = clientTransactionFruit.Fruit.FruitName,
                    NumberOfCages = clientTransactionFruit.NumberOfCages,
                    WeightInKilograms = clientTransactionFruit.WeightInKilograms,
                    PriceOfKiloGram = clientTransactionFruit.PriceOfKiloGram
                }).ToList()
            });
        }
        public static RClientTDetailsDto MapToRClientTDetailsDto(this ClientTransaction clientTransaction)
        {
            return new RClientTDetailsDto
            {

                ClientName = clientTransaction.Client.Name,
                Date = clientTransaction.Date,
                Description = clientTransaction.Description,
                Price = clientTransaction.Price,
                DiscountAmount = clientTransaction.DiscountAmount,
                TotalAmount = clientTransaction.TotalAmount,
                TotalCageMortgageAmount = clientTransaction.TotalCageMortgageAmount,
                ClientTransactionFruits = clientTransaction.ClientTransactionFruits.Select(clientTransactionFruit => new RClientTFruitDto
                {
                    FruitName = clientTransactionFruit.Fruit.FruitName,
                    NumberOfCages = clientTransactionFruit.NumberOfCages,
                    WeightInKilograms = clientTransactionFruit.WeightInKilograms,
                    PriceOfKiloGram = clientTransactionFruit.PriceOfKiloGram
                }).ToList()
            };
        }

        public static ClientTransaction MapToClientTransaction(this WClientTDto ctDto, ClientTransaction? transaction = null)
        {
            transaction ??= new ClientTransaction();
            transaction.DiscountAmount = ctDto.DiscountAmount;
            transaction.Description = ctDto.Description;
            transaction.ClientId = ctDto.ClientId;
            if (transaction.Date == default)
            {
                transaction.Date = DateTime.Now;
            }
            transaction.Price = ctDto.ClientTransactionFruits
                .Select(c => c.PriceOfKiloGram * c.WeightInKilograms).Sum();
            transaction.TotalAmount = transaction.Price - transaction.DiscountAmount;
            transaction.ClientTransactionFruits = ctDto.ClientTransactionFruits.Select(clientTransactionFruitDto => new ClientTransactionFruit
            {
                FruitId = clientTransactionFruitDto.FruitId,
                NumberOfCages = clientTransactionFruitDto.NumberOfCages,
                WeightInKilograms = clientTransactionFruitDto.WeightInKilograms,
                PriceOfKiloGram = clientTransactionFruitDto.PriceOfKiloGram
            }).ToList();
            return transaction;
        }

    }
}
