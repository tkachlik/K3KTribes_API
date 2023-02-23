using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;

namespace dusicyon_midnight_tribes_backend.Services
{
    public interface IProductionOptionService
    {
        IResponse ShowAvailableProductionOptions(int playerId, int kingdomId);
    }
}