namespace GrupoExito.Utilities.Contracts.Generic
{
    using System.Threading.Tasks;
    using GrupoExito.Entities.Parameters;
    using GrupoExito.Entities.Responses.InStoreServices;

    public interface IDiscountsService
    {
        Task<DisccountResponse> ActiveDisccount(DiscountParameters parameters);
        Task<DiscountsResponse> GetDiscounts();
        Task<ValidateActivatedCouponsResponse> ValidateActivatedCoupons();
        Task<DisccountResponse> InactiveDisccount(DiscountParameters parameters);
    }
}
