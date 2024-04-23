using GestionHotel.Apis.Models;

namespace GestionHotel.Apis.Services;
public interface IPaiementService
{
    Task<bool> ToPay(int cardCode);
    Task<bool> ToRefund(int idClient);
}