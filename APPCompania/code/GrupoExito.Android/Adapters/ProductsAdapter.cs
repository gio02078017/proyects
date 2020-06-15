using Android.Content;
using Android.Graphics;
using Android.Support.V7.Util;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using Com.Bumptech.Glide;
using Com.Bumptech.Glide.Load.Engine;
using Com.Bumptech.Glide.Request;
using GrupoExito.Android.Activities.Generic;
using GrupoExito.Android.Interfaces;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.ViewHolders;
using GrupoExito.DataAgent.DataBase;
using GrupoExito.Entities;
using GrupoExito.Logic.Models.Products;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using System.Collections.Generic;
using System.Linq;

namespace GrupoExito.Android.Adapters
{
    public class ProductAdapter : RecyclerView.Adapter
    {
        private List<Product> Products;
        private Context Context { get; set; }
        private IProducts ProductsInterface { get; set; }
        private IWindowManager WindowManager;
        private int LayoutHeight { get; set; }
        private decimal DefaultQuantityWeight = 0;
        private ProductCarModel _productCarModel;

        public ProductAdapter(List<Product> products, Context context, IProducts productsInterface, IWindowManager windowManager)
        {
            this.Products = products;
            this.Context = context;
            this.WindowManager = windowManager;
            this.ProductsInterface = productsInterface;
            this._productCarModel = new ProductCarModel(ProductCarDataBase.Instance);
        }

        public void Update(List<Product> productList)
        {
            List<Product> newProductList = new List<Product>();
            newProductList.AddRange(ParametersManager.Products);
            newProductList.AddRange(productList);

            DiffUtil.DiffResult diffResult = DiffUtil.CalculateDiff(new ProductCallback(ParametersManager.Products, newProductList));

            ParametersManager.Products.Clear();
            ParametersManager.Products.AddRange(newProductList);

            diffResult.DispatchUpdatesTo(this);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ListItemProduct, parent, false);

            if (WindowManager != null)
            {
                DisplayMetrics displayMetrics = new DisplayMetrics();
                WindowManager.DefaultDisplay.GetMetrics(displayMetrics);
                ViewGroup.LayoutParams layoutParams = itemView.LayoutParameters;
                layoutParams.Width = displayMetrics.WidthPixels / 2;
                itemView.LayoutParameters = layoutParams;
            }

            ProductViewHolder productViewHolder = new ProductViewHolder(itemView, ProductsInterface, Products);

            productViewHolder.IvAdd.Click += delegate { this.AddProduct(Products[productViewHolder.AdapterPosition], productViewHolder); };
            productViewHolder.LyAddProduct.Click += delegate { this.AddProduct(Products[productViewHolder.AdapterPosition], productViewHolder); };
            productViewHolder.IvSubstract.Click += delegate { this.SubstractProduct(Products[productViewHolder.AdapterPosition], productViewHolder); };
            productViewHolder.LyUnit.Click += delegate { UpdatedWeightTab(Products[productViewHolder.AdapterPosition], productViewHolder, false); };
            productViewHolder.LyWeight.Click += delegate { UpdatedWeightTab(Products[productViewHolder.AdapterPosition], productViewHolder, true); };

            return productViewHolder;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var requestOptions = new RequestOptions()
             .Apply(RequestOptions.DiskCacheStrategyOf(DiskCacheStrategy.All)
             .Placeholder(Resource.Drawable.sin_imagen)
             .Error(Resource.Drawable.sin_imagen)
             .CenterCrop()
             .DontAnimate()
             .DontTransform());

            ProductViewHolder productViewHolder = holder as ProductViewHolder;
            Product product = Products[position];

            Product producExists = _productCarModel.GetProduct(product.Id);

            if (producExists != null)
            {
                product.Quantity = producExists.Quantity;
                _productCarModel.UpSertProduct(product);
            }

            Glide.With(holder.ItemView.Context).Load(product.UrlMediumImage).Apply(requestOptions).Thumbnail(0.5f).Into(productViewHolder.IvProduct);
            productViewHolder.IvProductDiscount.Visibility = product.Price.DiscountPercent > 0 ? ViewStates.Visible : ViewStates.Invisible;
            productViewHolder.TvProductPreviousPrice.Visibility = product.Price.DiscountPercent > 0 ? ViewStates.Visible : ViewStates.Invisible;

            if (product.Price.DiscountPercent > 0)
            {
                productViewHolder.TvPum.Visibility = string.IsNullOrEmpty(product.Price.Pum) ? ViewStates.Invisible : ViewStates.Visible;
                productViewHolder.TvPumPrice.Visibility = ViewStates.Gone;
            }
            else
            {
                productViewHolder.TvPum.Visibility = ViewStates.Invisible;
                productViewHolder.TvPumPrice.Visibility = string.IsNullOrEmpty(product.Price.Pum) ? ViewStates.Gone : ViewStates.Visible;
            }

            productViewHolder.TvProductName.Text = product.Name;
            productViewHolder.TvProductPrice.Text = StringFormat.ToPrice(product.Price.ActualPrice);
            productViewHolder.TvOtherPaymentMethods.Text = AppMessages.OtherPaymentMethods + " " + StringFormat.ToPrice(product.Price.PriceOtherMeans);
            productViewHolder.TvProductPrice.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            productViewHolder.TvProductDiscount.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewaySemiBold), TypefaceStyle.Bold);
            productViewHolder.TvOtherPaymentMethods.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            productViewHolder.TvProductName.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            productViewHolder.TvAddProduct.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayExtraBold), TypefaceStyle.Bold);
            productViewHolder.TvWeight.SetTypeface(FontManager.Instance.GetTypeFace(this.Context, FontManager.RalewayMedium), TypefaceStyle.Normal);
            productViewHolder.TvPum.SetTypeface(FontManager.Instance.GetTypeFace(this.Context, FontManager.RalewayMedium), TypefaceStyle.Normal);
            productViewHolder.TvPumPrice.SetTypeface(FontManager.Instance.GetTypeFace(this.Context, FontManager.RalewayMedium), TypefaceStyle.Normal);
            productViewHolder.TvUnit.SetTypeface(FontManager.Instance.GetTypeFace(this.Context, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            productViewHolder.TvProductQuantity.SetTypeface(FontManager.Instance.GetTypeFace(this.Context, FontManager.RalewayMedium), TypefaceStyle.Normal);
            productViewHolder.TvProductQuantityWeight.SetTypeface(FontManager.Instance.GetTypeFace(this.Context, FontManager.RalewayMedium), TypefaceStyle.Normal);
            productViewHolder.TvProductQuantityWeight.Text = product.WeightUnits;
            DefaultQuantityWeight = product.Weight;
            productViewHolder.TvPum.Text = product.Price.Pum;
            productViewHolder.TvPumPrice.Text = product.Price.Pum;
            product.Quantity = _productCarModel.GetProduct(product.Id) != null ? _productCarModel.GetProduct(product.Id).Quantity : 0;

            productViewHolder.TvOtherPaymentMethods.Visibility = product.Price.PriceOtherMeans > 0 &&
                product.Price.PreviousPrice != product.Price.PriceOtherMeans ?
                ViewStates.Visible : ViewStates.Gone;

            this.ProductBackground(product, productViewHolder);
            this.ProductDiscount(product, productViewHolder);
            this.EstimatedWeight(product, productViewHolder);
            this.ValidateProductAdded(product, productViewHolder);
        }

        public override int ItemCount
        {
            get { return Products != null ? Products.Count() : 0; }
        }

        public override void OnViewRecycled(Java.Lang.Object holder)
        {
            try
            {
                RecyclerView.ViewHolder rvHolder = holder as RecyclerView.ViewHolder;
                ProductViewHolder viewHolder = holder as ProductViewHolder;
                Glide.With(rvHolder.ItemView.Context).Clear(viewHolder.IvProduct);
            }
            catch
            {
            }

            base.OnViewRecycled(holder);
        }

        private void AddProduct(Product product, ProductViewHolder productViewHolder)
        {
            ValidateProductAdded(product, productViewHolder);
        }

        private void SubstractProduct(Product product, ProductViewHolder productViewHolder)
        {
            ValidateProductAdded(product, productViewHolder);
        }

        private void UpdatedWeightTab(Product product, ProductViewHolder productViewHolder, bool isWeight)
        {
            product.WeightSelected = isWeight;
            ValidateProductAdded(product, productViewHolder);
        }

        private void ProductBackground(Product product, ProductViewHolder productViewHolder)
        {
            if (WindowManager != null || Products.IndexOf(product) >= Products.Count - 1)
            {
                productViewHolder.LyRootProduct.SetBackgroundResource((Resource.Drawable.border_carousel));
            }
            else
            {
                productViewHolder.LyRootProduct.SetBackgroundResource((Resource.Drawable.border_right));
            }

            if (WindowManager == null && Products.Count % 2 == 1 && Products.IndexOf(product) == Products.Count - 2)
            {
                productViewHolder.LyRootProduct.SetBackgroundResource((Resource.Drawable.border_right));
            }
            else
            {
                productViewHolder.LyRootProduct.SetBackgroundResource((Resource.Drawable.border_carousel));
            }
        }

        private void EstimatedWeight(Product product, ProductViewHolder productViewHolder)
        {
            if (!product.IsEstimatedWeight)
            {
                productViewHolder.LyProductLabels.Visibility = ViewStates.Visible;
                productViewHolder.ViewDivider.Visibility = ViewStates.Visible;
                productViewHolder.TvProductQuantityWeight.Visibility = ViewStates.Gone;
                productViewHolder.TvWeight.Visibility = ViewStates.Gone;
                productViewHolder.LyQuantity.SetBackgroundResource(Resource.Drawable.button_secondary);
                productViewHolder.TvUnit.SetBackgroundColor(Color.Transparent);
                productViewHolder.TvUnit.Gravity = GravityFlags.CenterHorizontal;

                LinearLayout.LayoutParams parameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent)
                {
                    Gravity = GravityFlags.CenterHorizontal
                };

                productViewHolder.TvUnit.LayoutParameters = parameters;
            }
            else
            {
                productViewHolder.TvWeight.Visibility = ViewStates.Visible;
            }
        }

        private void ValidateProductAdded(Product product, ProductViewHolder productViewHolder)
        {
            if (product.Quantity > 0)
            {
                this.ShowQuantityView(productViewHolder, product.Quantity, product);
                productViewHolder.LyProductLabels.Visibility = ViewStates.Visible;
                productViewHolder.ViewDivider.Visibility = ViewStates.Visible;
                productViewHolder.LyQuantity.SetBackgroundResource(Resource.Drawable.button_secondary);
                this.DefineQuantityView(productViewHolder, product);
                productViewHolder.LyAddProduct.Visibility = ViewStates.Gone;
                productViewHolder.LyQuantity.Visibility = ViewStates.Visible;
                productViewHolder.RlQuantity.Visibility = ViewStates.Visible;
            }
            else
            {
                productViewHolder.LyAddProduct.Visibility = ViewStates.Visible;
                productViewHolder.LyQuantity.Visibility = ViewStates.Gone;
                productViewHolder.ViewDivider.Visibility = ViewStates.Gone;
                productViewHolder.LyProductLabels.Visibility = ViewStates.Gone;
                productViewHolder.RlQuantity.Visibility = ViewStates.Gone;
            }
        }

        private void ProductDiscount(Product product, ProductViewHolder productViewHolder)
        {
            if (product.Price.DiscountPercent > 0)
            {
                productViewHolder.TvProductPrice.Text = Context.GetString(Resource.String.str_price_now) + " " + StringFormat.ToPrice(product.Price.ActualPrice);
                productViewHolder.TvProductPrice.SetTextColor(Color.ParseColor("#E2231A"));
                productViewHolder.TvProductPrice.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewaySemiBold), TypefaceStyle.Normal);

                productViewHolder.TvProductPreviousPrice.Visibility = product.Price.PreviousPrice > 0 ? ViewStates.Visible : ViewStates.Gone;
                productViewHolder.TvProductPreviousPrice.PaintFlags = productViewHolder.TvProductPrice.PaintFlags | PaintFlags.StrikeThruText;
                productViewHolder.TvProductPreviousPrice.Text = AppMessages.PriceBefore + " " + StringFormat.ToPrice(product.Price.PreviousPrice);
                productViewHolder.TvProductDiscount.Text = StringFormat.ToPercerntaje(product.Price.DiscountPercent);

                if (product.Price.DiscountImage != null)
                {
                    Glide.With(AndroidApplication.Context).Load(product.Price.DiscountImage).Thumbnail(0.1f).Into(productViewHolder.IvDiscountForCards);
                    productViewHolder.IvDiscountForCards.Visibility = ViewStates.Visible;
                }
                else
                {
                    productViewHolder.IvDiscountForCards.Visibility = ViewStates.Gone;
                }
            }
            else
            {
                productViewHolder.TvProductPrice.SetTextColor(Color.Black);
                productViewHolder.TvProductPrice.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
                productViewHolder.IvDiscountForCards.Visibility = ViewStates.Gone;
            }
        }

        private void ShowLoader(Product product, ProductViewHolder productViewHolder)
        {
            productViewHolder.IvLoader.Visibility = ViewStates.Visible;
            productViewHolder.LyProductContainer.Visibility = ViewStates.Gone;
            productViewHolder.LyAddProduct.Visibility = ViewStates.Gone;
        }

        private void ShowQuantityView(ProductViewHolder productViewHolder, int quantity, Product product)
        {
            productViewHolder.LyAddProduct.Visibility = ViewStates.Gone;
            productViewHolder.LyQuantity.Visibility = ViewStates.Visible;
            productViewHolder.LyProductContainer.Visibility = ViewStates.Visible;
            productViewHolder.TvProductQuantity.Text = !product.WeightSelected ? quantity.ToString() :
           (quantity * product.Weight).ToString();
        }

        private void DefineQuantityView(ProductViewHolder productViewHolder, Product product)
        {
            if (!product.WeightSelected)
            {
                productViewHolder.TvUnit.SetTypeface(FontManager.Instance.GetTypeFace(this.Context, FontManager.RalewayMedium), TypefaceStyle.Normal);
                productViewHolder.TvUnit.SetBackgroundResource(Resource.Drawable.text_selected);
                productViewHolder.TvUnit.SetTextColor(Color.ParseColor("#EF7C32"));
                productViewHolder.TvProductQuantityWeight.Visibility = ViewStates.Gone;
                productViewHolder.TvWeight.SetTypeface(FontManager.Instance.GetTypeFace(this.Context, FontManager.RalewayMedium), TypefaceStyle.Normal);
                productViewHolder.TvWeight.SetBackgroundColor(Color.Transparent);
                productViewHolder.TvWeight.SetTextColor(Color.ParseColor("#7D7D7D"));
            }
            else
            {
                productViewHolder.TvWeight.SetTypeface(FontManager.Instance.GetTypeFace(this.Context, FontManager.RalewayMedium), TypefaceStyle.Normal);
                productViewHolder.TvWeight.SetBackgroundResource(Resource.Drawable.text_selected);
                productViewHolder.TvWeight.SetTextColor(Color.ParseColor("#EF7C32"));
                productViewHolder.TvUnit.SetTypeface(FontManager.Instance.GetTypeFace(this.Context, FontManager.RalewayMedium), TypefaceStyle.Normal);
                productViewHolder.TvUnit.SetBackgroundColor(Color.Transparent);
                productViewHolder.TvUnit.SetTextColor(Color.ParseColor("#7D7D7D"));
                productViewHolder.TvProductQuantityWeight.Visibility = ViewStates.Visible;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }
    }
}