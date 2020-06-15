using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using GrupoExito.Android.Adapters;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.Utilities.Constants;
using GrupoExito.Android.Interfaces;
using GrupoExito.Entities;
using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GrupoExito.Android.ViewHolders
{
    class SummaryProductViewHolder : RecyclerView.ViewHolder
    {
        public ImageView IvProduct { get; private set; }
        public ImageView IvProductCategory { get; private set; }
        public TextView TvProductName { get; private set; }
        public TextView TvProductCategory { get; private set; }
        public TextView TvProductPrice { get; private set; }
        public RelativeLayout RlQuantity { get; private set; }
        public LinearLayout LyQuantity { get; private set; }
        public LinearLayout LyProductLabels { get; private set; }
        public LinearLayout LyUnit { get; private set; }
        public LinearLayout LyWeight { get; private set; }
        public LinearLayout LyProductContainer { get; private set; }
        public TextView TvUnit { get; private set; }
        public TextView TvWeight { get; private set; }
        public TextView TvProductQuantity { get; private set; }
        public TextView TvProductQuantityWeight { get; private set; }
        public ImageView IvAdd { get; private set; }
        public ImageView IvSubstract { get; private set; }
        public ImageView IvRemoveProduct { get; private set; }
        public View ViewDivider { get; private set; }
        public LinearLayout LyProductCategory { get; private set; }
        public TextView TvHowDoYouLike { get; private set; }
        public TextView TvPum { get; private set; }

        public SummaryProductViewHolder(View itemView, ISummaryProducts iProducts, IList<Product> products) : base(itemView)
        {
            IvProduct = itemView.FindViewById<ImageView>(Resource.Id.ivProduct);
            IvProductCategory = itemView.FindViewById<ImageView>(Resource.Id.ivProductCategory);
            TvProductName = itemView.FindViewById<TextView>(Resource.Id.tvProductName);
            TvProductPrice = itemView.FindViewById<TextView>(Resource.Id.tvProductPrice);
            TvProductCategory = itemView.FindViewById<TextView>(Resource.Id.tvProductCategory);
            RlQuantity = itemView.FindViewById<RelativeLayout>(Resource.Id.rlQuantity);
            LyQuantity = itemView.FindViewById<LinearLayout>(Resource.Id.lyQuantity);
            LyProductLabels = itemView.FindViewById<LinearLayout>(Resource.Id.lyProductLabels);
            LyUnit = itemView.FindViewById<LinearLayout>(Resource.Id.lyUnit);
            LyWeight = itemView.FindViewById<LinearLayout>(Resource.Id.lyWeight);
            LyProductContainer = itemView.FindViewById<LinearLayout>(Resource.Id.lyQuantity);
            TvUnit = itemView.FindViewById<TextView>(Resource.Id.tvUnit);
            TvWeight = itemView.FindViewById<TextView>(Resource.Id.tvWeight);
            TvProductQuantity = itemView.FindViewById<TextView>(Resource.Id.tvProductQuantity);
            TvProductQuantityWeight = itemView.FindViewById<TextView>(Resource.Id.tvProductQuantityWeight);
            IvAdd = itemView.FindViewById<ImageView>(Resource.Id.ivAdd);
            IvSubstract = itemView.FindViewById<ImageView>(Resource.Id.ivSubstract);
            IvRemoveProduct = itemView.FindViewById<ImageView>(Resource.Id.ivRemoveProduct);
            ViewDivider = itemView.FindViewById<View>(Resource.Id.viewProductDivider);
            TvHowDoYouLike = itemView.FindViewById<TextView>(Resource.Id.tvHowDoYouLike);
            TvPum = itemView.FindViewById<TextView>(Resource.Id.tvPum);
            IvAdd.Click += delegate { OnAddPressed(iProducts, products); };
            IvSubstract.Click += delegate { OnSubstractPressed(iProducts, products); };
            IvRemoveProduct.Click += delegate { iProducts.OnRemoveTouched(products[AdapterPosition]); };
            TvHowDoYouLike.Click += delegate { iProducts.OnHoWDoYouLikeTouched(products[AdapterPosition]); };
            IvProduct.Click += delegate { iProducts.OnProductClicked(products[AdapterPosition]); };
        }

        private void OnAddPressed(IProducts iProducts, IList<Product> products)
        {
            try
            {
                iProducts.OnAddPressed(products[AdapterPosition]);
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.ProductViewHolder, ConstantMethodName.OnAddPressed } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
        }

        private void OnSubstractPressed(IProducts iProducts, IList<Product> products)
        {
            try
            {
                iProducts.OnSubstractPressed(products[AdapterPosition]);
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.ProductViewHolder, ConstantMethodName.OnSubstractPressed } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
        }

        public void ShowAndRegisterMessageExceptions(Exception exception, Dictionary<string, string> properties)
        {
            var st = new StackTrace(exception, true);
            var frame = st.GetFrame(0);
            var line = frame.GetFileLineNumber();
            properties.Add(ConstantActivityName.LineError, line.ToString());
            Crashes.TrackError(exception, properties);
        }
    }
}