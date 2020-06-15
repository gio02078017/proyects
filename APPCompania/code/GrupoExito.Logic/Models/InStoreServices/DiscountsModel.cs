namespace GrupoExito.Logic.Models.InStoreServices
{
    using GrupoExito.Entities.Entiites;
    using GrupoExito.Entities.Parameters;
    using GrupoExito.Entities.Responses.InStoreServices;
    using GrupoExito.Utilities.Contracts.Generic;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class DiscountsModel
    {
        private IDiscountsService _discountsService { get; set; }

        public DiscountsModel(IDiscountsService discountsService)
        {
            _discountsService = discountsService;
        }

        public async Task<DisccountResponse> ActiveDisccount(DiscountParameters parameters)
        {
            return await _discountsService.ActiveDisccount(parameters);
        }

        public async Task<DisccountResponse> InactiveDisccount(DiscountParameters parameters)
        {
            return await _discountsService.InactiveDisccount(parameters);
        }

        public async Task<DiscountsResponse> GetDiscounts()
        {
            DiscountsResponse discountsResponse = await _discountsService.GetDiscounts();

            if (discountsResponse.Result != null && discountsResponse.Result.HasErrors && discountsResponse.Result.Messages != null)
            {
            }
            else
            {
                discountsResponse = GetCountersDiscounts(discountsResponse);
            }

            return discountsResponse;
        }


        public async Task<ValidateActivatedCouponsResponse> ValidateActivatedCoupons()
        {
            return await _discountsService.ValidateActivatedCoupons();
        }

        public DiscountsResponse ValidateRedeemedDiscounts(DiscountsResponse discountsResponse)
        {
            int total = discountsResponse.ActivatedDiscounts != null ? discountsResponse.ActivatedDiscounts.Count : 0;
            if (total >= discountsResponse.ActivateCoupons)
            {
                discountsResponse.IsRedeemable = false;
                discountsResponse.ActiveDiscounts.Killers.Where(killer => !killer.Active).ToList().ForEach(k => k.Redeemable = false);
                discountsResponse.ActiveDiscounts.AlreadyPurchased.Where(killer => !killer.Active).ToList().ForEach(k => k.Redeemable = false);
                discountsResponse.ActiveDiscounts.CouldLike.Where(killer => !killer.Active).ToList().ForEach(k => k.Redeemable = false);
                discountsResponse.Coupons.Where(killer => !killer.Active).ToList().ForEach(k => k.Redeemable = false);
            }
            else if (!discountsResponse.IsRedeemable)
            {
                discountsResponse.ActiveDiscounts.Killers.Where(killer => !killer.Active).ToList().ForEach(k => k.Redeemable = true);
                discountsResponse.ActiveDiscounts.AlreadyPurchased.Where(killer => !killer.Active).ToList().ForEach(k => k.Redeemable = true);
                discountsResponse.ActiveDiscounts.CouldLike.Where(killer => !killer.Active).ToList().ForEach(k => k.Redeemable = true);
                discountsResponse.Coupons.Where(killer => !killer.Active).ToList().ForEach(k => k.Redeemable = true);
                discountsResponse.IsRedeemable = true;
            }

            return discountsResponse;
        }

        private DiscountsResponse GetCountersDiscounts(DiscountsResponse discountsResponse)
        {
            discountsResponse.TotalActivatedDiscounts = discountsResponse.ActivatedDiscounts != null ? discountsResponse.ActivatedDiscounts.Count() : 0;
            discountsResponse.TotalRedeemedDiscounts = discountsResponse.RedeemedDiscounts != null ? discountsResponse.RedeemedDiscounts.Count() : 0;
            discountsResponse.TotalActiveDiscounts = GetTotalActiveDiscounts(discountsResponse);
            return discountsResponse;
        }

        public DiscountsResponse UpdatActivatedDiscounts(DiscountsResponse response, Discount discountActivated)
        {
            Discount discountConsulted = null;

            if (response.ActivatedDiscounts != null)
            {
                discountConsulted = response.ActivatedDiscounts != null && response.ActivatedDiscounts.Any() ?
                    response.ActivatedDiscounts.ToList().Find(discount => discount.PosCode.Equals(discountActivated.PosCode) &&
                    discount.StartDate.Equals(discountActivated.StartDate)) : null;

                if (discountConsulted != null)
                {
                    response.ActivatedDiscounts.Remove(discountConsulted);
                }
                else
                {
                    response.ActivatedDiscounts.Add(discountActivated);
                }

                if (response.ActiveDiscounts != null)
                {
                    discountConsulted = response.ActiveDiscounts.AlreadyPurchased != null && response.ActiveDiscounts.AlreadyPurchased.Any() ?
                        response.ActiveDiscounts.AlreadyPurchased.ToList().Find(discount => discount.PosCode.Equals(discountActivated.PosCode) &&
                    discount.StartDate.Equals(discountActivated.StartDate)) : null;

                    if (discountConsulted != null)
                    {
                        discountConsulted.Active = !discountConsulted.Active;
                    }
                    else
                    {
                        discountConsulted = response.ActiveDiscounts.CouldLike != null && response.ActiveDiscounts.CouldLike.Any() ?
                        response.ActiveDiscounts.CouldLike.ToList().Find(discount => discount.PosCode.Equals(discountActivated.PosCode) &&
                        discount.StartDate.Equals(discountActivated.StartDate)) : null;

                        if (discountConsulted != null)
                        {
                            discountConsulted.Active = !discountConsulted.Active;
                        }
                        else
                        {
                            discountConsulted = response.ActiveDiscounts.Killers != null && response.ActiveDiscounts.Killers.Any() ?
                                response.ActiveDiscounts.Killers.ToList().Find(discount => discount.PosCode.Equals(discountActivated.PosCode) &&
                                discount.StartDate.Equals(discountActivated.StartDate)) : null;

                            if (discountConsulted != null)
                            {
                                discountConsulted.Active = !discountConsulted.Active;
                            }
                        }
                    }
                }
            }

            response = GetCountersDiscounts(response);
            return response;
        }

        private int GetTotalActiveDiscounts(DiscountsResponse response)
        {
            int total = 0;

            if (response != null && response.ActiveDiscounts != null)
            {
                total += response.ActiveDiscounts.AlreadyPurchased != null ? response.ActiveDiscounts.AlreadyPurchased.Count : 0;
                total += response.ActiveDiscounts.CouldLike != null ? response.ActiveDiscounts.CouldLike.Count : 0;
                total += response.ActiveDiscounts.Killers != null ? response.ActiveDiscounts.Killers.Count : 0;
            }

            return total;
        }
    }
}
