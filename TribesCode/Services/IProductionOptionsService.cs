using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;

namespace dusicyon_midnight_tribes_backend.Services
{
    public interface IProductionOptionsService
    {
        IResponse ShowAllAvailableProductionOptions(int playerId, int kingdomId);
    }
}