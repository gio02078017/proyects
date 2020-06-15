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

namespace GrupoExito.Android.ViewHolders
{
    class ProductViewHolder : RecyclerView.ViewHolder
    {
        public ImageView IvProduct { get; private set; }
        public ImageView IvProductDiscount { get; private set; }
        public ImageView IvProductAddToList { get; private set; }
        public TextView TvProductName { get; private set; }
        public TextView TvProductPrice { get; private set; }
        public TextView TvProductPreviousPrice { get; private set; }
        public TextView TvProductDiscount { get; private set; }
        public TextView TvAddProduct { get; private set; }
        public RelativeLayout LyRootProduct { get; private set; }
        public RelativeLayout RlQuantity { get; private set; }
        public LinearLayout LyAddProduct { get; private set; }
        public LinearLayout LyQuantity { get; private set; }
        public LinearLayout LyProductLabels { get; private set; }
        public LinearLayout LyUnit { get; private set; }
        public LinearLayout LyWeight { get; private set; }
        public LinearLayout LyProductContainer { get; private set; }
        public TextView TvUnit { get; private set; }
        public TextView TvWeight { get; private set; }
        public TextView TvProductQuantity { get; private set; }
        public TextView TvProductQuantityWeight { get; private set; }
        public TextView TvPum { get; private set; }
        public TextView TvPumPrice { get; private set; }
        public TextView TvOtherPaymentMethods { get; private set; }
        public ImageView IvAdd { get; private set; }
        public ImageView IvSubstract { get; private set; }
        public View ViewDivider { get; private set; }
        public ImageView IvLoader { get; private set; }
        public ImageView IvDiscountForCards { get; private set; }

        public ProductViewHolder(View itemView, IProducts iProducts, IList<Product> products) : base(itemView)
        {
            IvProduct = itemView.FindViewById<ImageView>(Resource.Id.ivProduct);
            IvProductDiscount = itemView.FindViewById<ImageView>(Resource.Id.ivProductDiscount);
            TvProductName = itemView.FindViewById<TextView>(Resource.Id.tvProductName);
            TvProductPrice = itemView.FindViewById<TextView>(Resource.Id.tvProductPrice);
            TvProductDiscount = itemView.FindViewById<TextView>(Resource.Id.tvProductDiscount);
            TvProductPreviousPrice = itemView.FindViewById<TextView>(Resource.Id.tvProductPreviousPrice);
            TvAddProduct = itemView.FindViewById<TextView>(Resource.Id.tvAddProduct);
            TvPum = itemView.FindViewById<TextView>(Resource.Id.tvPum);
            TvPumPrice = itemView.FindViewById<TextView>(Resource.Id.tvPumPrice);
            TvOtherPaymentMethods = itemView.FindViewById<TextView>(Resource.Id.tvOtherPaymentMethods);
            LyRootProduct = itemView.FindViewById<RelativeLayout>(Resource.Id.lyRootProduct);
            LyAddProduct = itemView.FindViewById<LinearLayout>(Resource.Id.lyAddProduct);
            RlQuantity = itemView.FindViewById<RelativeLayout>(Resource.Id.rlQuantity);
            LyQuantity = itemView.FindViewById<LinearLayout>(Resource.Id.lyQuantity);
            LyProductLabels = itemView.FindViewById<LinearLayout>(Resource.Id.lyProductLabels);
            LyUnit = itemView.FindViewById<LinearLayout>(Resource.Id.lyUnit);
            LyWeight = itemView.FindViewById<LinearLayout>(Resource.Id.lyWeight);
            LyProductContainer = itemView.FindViewById<LinearLayout>(Resource.Id.lyProductContainer);
            TvUnit = itemView.FindViewById<TextView>(Resource.Id.tvUnit);
            TvWeight = itemView.FindViewById<TextView>(Resource.Id.tvWeight);
            TvProductQuantity = itemView.FindViewById<TextView>(Resource.Id.tvProductQuantity);
            TvProductQuantityWeight = itemView.FindViewById<TextView>(Resource.Id.tvProductQuantityWeight);
            IvAdd = itemView.FindViewById<ImageView>(Resource.Id.ivAdd);
            IvSubstract = itemView.FindViewById<ImageView>(Resource.Id.ivSubstract);
            ViewDivider = itemView.FindViewById(Resource.Id.viewProductDivider);
            IvLoader = itemView.FindViewById<ImageView>(Resource.Id.ivLoader);
            IvDiscountForCards = itemView.FindViewById<ImageView>(Resource.Id.ivDiscountForCards);
            IvProductAddToList = itemView.FindViewById<ImageView>(Resource.Id.ivProductAddToList);

            IvAdd.Click += delegate { OnAddPressed(iProducts, products); };
            IvSubstract.Click += delegate { OnSubstractPressed(iProducts, products); };
            LyAddProduct.Click += delegate { OnAddProduct(iProducts, products); };
            IvProduct.Click += delegate { iProducts.OnProductClicked(products[AdapterPosition]); };
            IvProductAddToList.Click += delegate { iProducts.OnAddToListClicked(products[AdapterPosition]); };
        }

        private void OnAddPressed(IProducts iProducts, IList<Product> products)
        {
            try
            {
                iProducts.OnAddPressed(products[AdapterPosition]);
            }
            catch (Exception ex)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.ProductViewHolder, ConstantMethodName.OnAddPressed } };
                ShowAndRegisterMessageExceptions(ex, properties);
            }
        }

        private void OnSubstractPressed(IProducts iProducts, IList<Product> products)
        {
            try
            {
                iProducts.OnSubstractPressed(products[AdapterPosition]);
            }
            catch (Exception ex)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.ProductViewHolder, ConstantMethodName.OnSubstractPressed } };
                ShowAndRegisterMessageExceptions(ex, properties);
            }
        }

        private void OnAddProduct(IProducts iProducts, IList<Product> products)
        {
            try
            {
                iProducts.OnAddProduct(products[AdapterPosition]);
            }
            catch (Exception ex)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.ProductViewHolder, ConstantMethodName.OnAddProducts } };
                ShowAndRegisterMessageExceptions(ex, properties);
            }
        }

        public void ShowAndRegisterMessageExceptions(Exception exception, Dictionary<string, string> properties)
        {
            Crashes.TrackError(exception, properties);
        }
    }
};