namespace GrupoExito.Utilities.Contracts.Generic
{
    using GrupoExito.Entities.Parameters.Generic;
    using GrupoExito.Entities.Responses.Generic;
    using System.Threading.Tasks;

    public interface IContentsService
    {
        Task<ContentHomeResponse> GetContentHome();

        Task<PromotionResponse> GetPromotions(PromotionParameters parameters);
    }
}
