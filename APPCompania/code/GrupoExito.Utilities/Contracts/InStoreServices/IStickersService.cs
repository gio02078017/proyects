namespace GrupoExito.Utilities.Contracts.InStoreServices
{
    using GrupoExito.Entities.Responses.InStoreServices;
    using System.Threading.Tasks;

    public interface IStickersService
    {
        Task<StickersResponse> GetSckers();
    }
}
