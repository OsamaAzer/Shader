using Shader.Data.Dtos.Client;
using Shader.Data.Entities;

namespace Shader.Mapping
{
    public static class ClientProfile
    {
        public static IEnumerable<RAllClientsDto> MapToRAllClientsDto(this IEnumerable<Client> clients)
        {
            return clients.Select(client =>  new RAllClientsDto
            {
                Id = client.Id,
                Name = client.Name,
                City = client.City,
                PhoneNumber = client.PhoneNumber,
                Status = client.Status,
                TotalRemainingAmount = client.TotalRemainingAmount,
                TotalRemainingMortgageAmount = client.TotalRemainingMortgageAmount
            });
        }

        public static RClientDto MapToRClientDto(this Client client)
        {
            return new RClientDto
            {
                Id = client.Id,
                Name = client.Name,
                City = client.City,
                PhoneNumber = client.PhoneNumber,
                Status = client.Status,
                Price = client.Price,
                TotalAmount = client.TotalAmount,
                AmountPaid = client.AmountPaid,
                TotalDiscountAmount = client.TotalDiscountAmount,
                TotalRemainingAmount = client.TotalRemainingAmount,
                TotalMortgageAmount = client.TotalMortgageAmount,
                TotalMortgageAmountPaid = client.TotalMortgageAmountPaid,
                TotalRemainingMortgageAmount = client.TotalRemainingMortgageAmount
            };
        }

        public static Client MapToClient(this WClientDto clientDto, Client? client = null)
        {
            client ??= new Client();
            client.Name = clientDto.Name;
            client.City = clientDto.City;
            client.PhoneNumber = clientDto.PhoneNumber;
            return client;
        }
    }
}
