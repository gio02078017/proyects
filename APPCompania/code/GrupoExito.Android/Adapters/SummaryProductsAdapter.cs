using Android.Content;
using Android.Graphics;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Com.Bumptech.Glide;
using Com.Bumptech.Glide.Load.Engine;
using Com.Bumptech.Glide.Request;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.ViewHolders;
using GrupoExito.DataAgent.DataBase;
using GrupoExito.Entities;
using GrupoExito.Logic.Models.Products;
using GrupoExito.Utilities.Helpers;
using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GrupoExito.Android.Adapters
{
    public class SummaryProductAdapter : RecyclerView.Adapter
    {
        private IList<Product> Products { get; set; }
        private Context Context { get; set; }
        private ISummaryProducts ProductsInterface { get; set; }
        private ProductCarModel _productCarModel { get; set; }

        public SummaryProductAdapter(IList<Product> products, Context context, ISummaryProducts productsInterface)
        {
            this.Products = products;
            this.Context = context;
            this.ProductsInterface = productsInterface;
            this._productCarModel = new ProductCarModel(new ProductCarDataBase());
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ListItemSummaryProduct, parent, false);
            SummaryProductViewHolder productViewHolder = new SummaryProductViewHolder(itemView, ProductsInterface, Products);

            productViewHolder.IvSubstract.Click += delegate
            {
                try
                {
                    this.SubstractProduct(Products[productViewHolder.AdapterPosition], productViewHolder);
                }
                catch(Exception exception)
                {
                    Crashes.TrackError(exception, null);
                }
            };

            productViewHolder.IvAdd.Click += delegate { this.AddProduct(Products[productViewHolder.AdapterPosition], productViewHolder); };
            productViewHolder.LyUnit.Click += delegate { UpdatedWeightTab(Products[productViewHolder.AdapterPosition], productViewHolder, false); };
            productViewHolder.LyWeight.Click += delegate { UpdatedWeightTab(Products[productViewHolder.AdapterPosition], productViewHolder, true); };

            return productViewHolder;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var requestOptions = new RequestOptions()
               .Apply(RequestOptions.DiskCacheStrategyOf(DiskCacheStrategy.All)
               .DontAnimate()
               .DontTransform());

            SummaryProductViewHolder productViewHolder = holder as SummaryProductViewHolder;
            Product product = Products[position];

            product.PriceProduct = _productCarModel.GetTotalValueProduct(product.Id);
            Glide.With(holder.ItemView.Context).Load(product.UrlMediumImage).Apply(requestOptions).Thumbnail(0.5f).Into(productViewHolder.IvProduct);
            productViewHolder.TvProductName.Text = product.Name;
            productViewHolder.TvProductPrice.Text = StringFormat.ToPrice(product.PriceProduct);
            productViewHolder.TvProductPrice.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewaySemiBold), TypefaceStyle.Bold);
            productViewHolder.TvProductName.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            productViewHolder.TvWeight.SetTypeface(FontManager.Instance.GetTypeFace(this.Context, FontManager.RalewayMedium), TypefaceStyle.Normal);
            productViewHolder.TvUnit.SetTypeface(FontManager.Instance.GetTypeFace(this.Context, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            productViewHolder.TvProductQuantity.SetTypeface(FontManager.Instance.GetTypeFace(this.Context, FontManager.RalewayMedium), TypefaceStyle.Normal);
            productViewHolder.TvProductQuantityWeight.SetTypeface(FontManager.Instance.GetTypeFace(this.Context, FontManager.RalewayMedium), TypefaceStyle.Normal);
            productViewHolder.TvHowDoYouLike.SetTypeface(FontManager.Instance.GetTypeFace(this.Context, FontManager.RalewayMedium), TypefaceStyle.Normal);
            productViewHolder.TvHowDoYouLike.Visibility = product.IsEstimatedWeight ? ViewStates.Visible : ViewStates.Gone;
            productViewHolder.TvPum.SetTypeface(FontManager.Instance.GetTypeFace(this.Context, FontManager.RalewayMedium), TypefaceStyle.Normal);
            productViewHolder.TvProductQuantityWeight.Text = product.WeightUnits;
            productViewHolder.TvPum.Visibility = string.IsNullOrEmpty(product.Price.Pum) ? ViewStates.Gone: ViewStates.Visible;
            productViewHolder.TvPum.Text = product.Price.Pum;
            this.EstimatedWeight(product, productViewHolder);
            this.ProductLoading(product, productViewHolder);
        }

        public override int ItemCount
        {
            get { return Products != null ? Products.Count() : 0; }
        }

        public override void OnViewRecycled(Java.Lang.Object holder)
        {
            base.OnViewRecycled(holder);
            RecyclerView.ViewHolder rvHolder = holder as RecyclerView.ViewHolder;
            SummaryProductViewHolder viewHolder = holder as SummaryProductViewHolder;
            Glide.With(rvHolder.ItemView.Context).Clear(viewHolder.IvProduct);
        }

        private void AddProduct(Product product, SummaryProductViewHolder productViewHolder)
        {
            ValidateProductAdded(product, productViewHolder);
        }

        private void SubstractProduct(Product product, SummaryProductViewHolder productViewHolder)
        {
            ValidateProductAdded(product, productViewHolder);
        }

        private void UpdatedWeightTab(Product product, SummaryProductViewHolder productViewHolder, bool isWeight)
        {
            product.WeightSelected = isWeight;
            this.ShowQuantityView(productViewHolder, product.Quantity, product);

            if (product.IsEstimatedWeight)
            {
                productViewHolder.LyProductLabels.Visibility = ViewStates.Visible;
                productViewHolder.ViewDivider.Visibility = ViewStates.Visible;
                productViewHolder.TvProductQuantityWeight.Visibility = !product.WeightSelected ?
                    ViewStates.Gone : ViewStates.Visible;
                productViewHolder.LyQuantity.SetBackgroundResource(Resource.Drawable.button_secondary);
                this.DefineQuantityView(productViewHolder, product);
            }
        }

        private void ProductLoading(Product product, SummaryProductViewHolder productViewHolder)
        {
            if (product.IsLoading)
            {
                ShowLoader(product, productViewHolder);
            }
            else
            {
                if (product.Quantity > 0)
                {
                    this.ShowQuantityView(productViewHolder, product.Quantity, product);

                    if (product.IsEstimatedWeight)
                    {
                        productViewHolder.LyProductLabels.Visibility = ViewStates.Visible;
                        productViewHolder.ViewDivider.Visibility = ViewStates.Visible;
                        productViewHolder.TvProductQuantityWeight.Visibility = !product.WeightSelected ?
                            ViewStates.Gone : ViewStates.Visible;
                        productViewHolder.LyQuantity.SetBackgroundResource(Resource.Drawable.button_secondary);
                        DefineQuantityView(productViewHolder, product);
                    }
                    else
                    {
                        productViewHolder.LyQuantity.Visibility = ViewStates.Visible;
                    }
                }
            }
        }

        private void EstimatedWeight(Product product, SummaryProductViewHolder productViewHolder)
        {
            if (!product.IsEstimatedWeight)
            {
                productViewHolder.LyProductLabels.Visibility = ViewStates.Gone;
                productViewHolder.ViewDivider.Visibility = ViewStates.Gone;
                productViewHolder.TvProductQuantityWeight.Visibility = ViewStates.Gone;
                productViewHolder.TvWeight.Visibility = ViewStates.Gone;
                productViewHolder.LyQuantity.SetBackgroundResource(Resource.Drawable.button_secondary);
                productViewHolder.TvUnit.SetBackgroundColor(Color.Transparent);
                productViewHolder.TvUnit.Gravity = GravityFlags.CenterHorizontal;
                LinearLayout.LayoutParams parameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent,
                    LinearLayout.LayoutParams.WrapContent)
                {
                    Gravity = GravityFlags.CenterHorizontal
                };
                productViewHolder.TvUnit.LayoutParameters = parameters;
            }
            else
            {
                productViewHolder.LyProductLabels.Visibility = ViewStates.Visible;
                productViewHolder.TvWeight.Visibility = ViewStates.Visible;
            }
        }

        private void ShowLoader(Product product, SummaryProductViewHolder productViewHolder)
        {
            productViewHolder.LyProductContainer.Visibility = ViewStates.Gone;
        }

        private void ValidateProductAdded(Product product, SummaryProductViewHolder productViewHolder)
        {
            if (product.Quantity > 0)
            {
                this.ShowQuantityView(productViewHolder, product.Quantity, product);

                if (product.IsEstimatedWeight)
                {
                    productViewHolder.LyProductLabels.Visibility = ViewStates.Visible;
                    productViewHolder.ViewDivider.Visibility = ViewStates.Visible;
                    productViewHolder.TvProductQuantityWeight.Visibility = !product.WeightSelected ?
                    ViewStates.Gone : ViewStates.Visible;
                    productViewHolder.LyQuantity.SetBackgroundResource(Resource.Drawable.button_secondary);
                    this.DefineQuantityView(productViewHolder, product);
                }
            }
        }

        private void ShowQuantityView(SummaryProductViewHolder productViewHolder, int quantity, Product product)
        {
            productViewHolder.LyQuantity.Visibility = ViewStates.Visible;
            productViewHolder.LyProductContainer.Visibility = ViewStates.Visible;
            productViewHolder.TvProductQuantity.Text = !product.WeightSelected ? quantity.ToString() :
           (quantity * product.Weight).ToString();

            if (quantity < 2)
            {
                productViewHolder.IvRemoveProduct.Visibility = ViewStates.Visible;
                productViewHolder.IvSubstract.Visibility = ViewStates.Gone;
            }
            else
            {
                productViewHolder.IvRemoveProduct.Visibility = ViewStates.Gone;
                productViewHolder.IvSubstract.Visibility = ViewStates.Visible;
            }
        }

        private void DefineQuantityView(SummaryProductViewHolder productViewHolder, Product product)
        {
            if (!product.WeightSelected)
            {
                productViewHolder.TvUnit.SetTypeface(FontManager.Instance.GetTypeFace(this.Context, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
                productViewHolder.TvUnit.SetBackgroundResource(Resource.Drawable.text_selected);
                productViewHolder.TvUnit.SetTextColor(Color.Black);
                productViewHolder.TvProductQuantityWeight.Visibility = ViewStates.Gone;
                productViewHolder.TvWeight.SetTypeface(FontManager.Instance.GetTypeFace(this.Context, FontManager.RalewayMedium), TypefaceStyle.Normal);
                productViewHolder.TvWeight.SetBackgroundColor(Color.Transparent);
                productViewHolder.TvWeight.SetTextColor(Color.ParseColor("#7D7D7D"));
            }
            else
            {
                productViewHolder.TvWeight.SetTypeface(FontManager.Instance.GetTypeFace(this.Context, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
                productViewHolder.TvWeight.SetBackgroundResource(Resource.Drawable.text_selected);
                productViewHolder.TvWeight.SetTextColor(Color.Black);
                productViewHolder.TvUnit.SetTypeface(FontManager.Instance.GetTypeFace(this.Context, FontManager.RalewayMedium), TypefaceStyle.Normal);
                productViewHolder.TvUnit.SetBackgroundColor(Color.Transparent);
                productViewHolder.TvUnit.SetTextColor(Color.ParseColor("#7D7D7D"));
                productViewHolder.TvProductQuantityWeight.Visibility = !product.WeightSelected ?
                    ViewStates.Gone : ViewStates.Visible;
            }
        }
    }
}