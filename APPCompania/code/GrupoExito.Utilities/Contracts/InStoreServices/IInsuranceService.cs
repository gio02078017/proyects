namespace GrupoExito.Utilities.Contracts.Generic
{
    using GrupoExito.Entities.Entiites;
    using GrupoExito.Entities.Responses.InStoreServices;
    using System.Threading.Tasks;

    public interface IInsuranceService
    {
        Task<SoatResponse> GetSoat(Soat soat);
    }
}
