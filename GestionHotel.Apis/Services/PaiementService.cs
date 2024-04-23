
using Microsoft.AspNetCore.Http.HttpResults;

namespace GestionHotel.Apis.Services;
public class PaiementService : IPaiementService
{
    public Task<bool> ToPay(int cardCode)
    {
        // Fake paiement
        Console.WriteLine("Paiement effectué");
        return Task.FromResult(true);
    }

    public Task<bool> ToRefund(int idClient)
    {
        // Fake refund
        Console.WriteLine("Remboursement effectué");
        return Task.FromResult(true);
    }
}
