using Android.Content;
using Android.Graphics;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Com.Bumptech.Glide;
using Com.Bumptech.Glide.Load.Engine;
using Com.Bumptech.Glide.Request;
using GrupoExito.Android.Interfaces;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.ViewHolders;
using GrupoExito.DataAgent.DataBase;
using GrupoExito.Entities;
using GrupoExito.Entities.Entiites.Products;
using GrupoExito.Logic.Models.Products;
using GrupoExito.Utilities.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace GrupoExito.Android.Adapters
{
    public class RecommendProductsAdapter : RecyclerView.Adapter
    {
        private IList<ProductList> Products { get; set; }
        private IRecommendedAdapter InterfaceMyList { get; set; }
        private Context Context { get; set; }
        private bool ViewDelete { get; set; }
        private ProductCarModel _productCarModel { get; set; }

        public RecommendProductsAdapter(IList<ProductList> products, Context context, IRecommendedAdapter interfaceMyList, bool ViewDelete = false)
        {
            this.InterfaceMyList = interfaceMyList;
            this.Context = context;
            this.Products = products;
            this.ViewDelete = ViewDelete;
            this._productCarModel = new ProductCarModel(ProductCarDataBase.Instance);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ListItemRecommendListProduct, parent, false);
            RecommendProductsViewHolder _MyRecommendListViewHolder = new RecommendProductsViewHolder(itemView);
            SetViewHolderEvents(_MyRecommendListViewHolder);
            return _MyRecommendListViewHolder;
        }

        private void SetViewHolderEvents(RecommendProductsViewHolder recommendProductsViewHolder)
        {
            recommendProductsViewHolder.LySelectList.Click += delegate
            {
                ActionSelected(recommendProductsViewHolder);
            };

            recommendProductsViewHolder.IvProduct.Click += delegate
            {
                ActionSelected(recommendProductsViewHolder);
            };

            recommendProductsViewHolder.IvAdd.Click += delegate
            {
                int position = recommendProductsViewHolder.AdapterPosition;
                ProductList product = Products[position];
                product.Quantity += 1;

                if (!product.Selected)
                {
                    product.Selected = true;
                    EventSelected(recommendProductsViewHolder, product.Selected);
                }

                InterfaceMyList.OnAddPressed(product, position);
            };

            recommendProductsViewHolder.IvSubstract.Click += delegate
            {
                int position = recommendProductsViewHolder.AdapterPosition;
                ProductList product = Products[position];

                if (product.Quantity > 0)
                {
                    product.Quantity -= 1;

                    if (product.Selected && product.Quantity == 0)
                    {
                        product.Selected = false;
                        EventSelected(recommendProductsViewHolder, product.Selected);
                    }
                   
                    InterfaceMyList.OnSubstractPressed(product, position);
                }
            };

            recommendProductsViewHolder.LyUnit.Click += delegate
            {
                Product product = Products[recommendProductsViewHolder.AdapterPosition];
                product.WeightSelected = false;
                EventWeightSeleted(recommendProductsViewHolder, false);
            };

            recommendProductsViewHolder.LyWeight.Click += delegate
            {
                Product product = Products[recommendProductsViewHolder.AdapterPosition];
                product.WeightSelected = true;
                EventWeightSeleted(recommendProductsViewHolder, true);
            };

            recommendProductsViewHolder.LyDelete.Click += delegate
            {
                int position = recommendProductsViewHolder.AdapterPosition;
                ProductList product = Products[position];
                InterfaceMyList.OnItemDeleted(product, position);
            };
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var requestOptions = new RequestOptions()
                 .Apply(RequestOptions.DiskCacheStrategyOf(DiskCacheStrategy.All)
                 .Placeholder(Resource.Drawable.sin_imagen)
                 .Error(Resource.Drawable.sin_imagen)
                 .Fallback(Resource.Drawable.sin_imagen)
                 .DontAnimate()
                 .DontTransform());

            RecommendProductsViewHolder recommendProductsViewHolder = holder as RecommendProductsViewHolder;
            Product product = Products[position];

            recommendProductsViewHolder.TvProductName.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            recommendProductsViewHolder.TvDescription.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayMedium), TypefaceStyle.Normal);
            recommendProductsViewHolder.TvPrice.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            recommendProductsViewHolder.TvUnit.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            recommendProductsViewHolder.TvWeight.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayMedium), TypefaceStyle.Normal);
            recommendProductsViewHolder.TvProductQuantity.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayMedium), TypefaceStyle.Normal);
            recommendProductsViewHolder.TvProductQuantityWeight.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayMedium), TypefaceStyle.Normal);
            recommendProductsViewHolder.TvPum.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayMedium), TypefaceStyle.Normal);
            recommendProductsViewHolder.LyProductLabels.Visibility = ViewStates.Gone;
            recommendProductsViewHolder.ViewProductDivider.Visibility = ViewStates.Gone;
            Glide.With(holder.ItemView.Context).Load(product.UrlMediumImage).Apply(requestOptions).Thumbnail(0.5f).Into(recommendProductsViewHolder.IvProduct);
            recommendProductsViewHolder.TvProductName.Text = product.Name;
            recommendProductsViewHolder.TvPrice.Text = StringFormat.ToPrice(product.Price.ActualPrice);
            recommendProductsViewHolder.TvDescription.Text = product.Description;
            recommendProductsViewHolder.TvDescription.Visibility = !string.IsNullOrEmpty(product.Description) ? ViewStates.Visible : ViewStates.Gone;
            recommendProductsViewHolder.TvPum.Text = product.Price.Pum;
            recommendProductsViewHolder.TvPum.Visibility = string.IsNullOrEmpty(product.Price.Pum) ? ViewStates.Gone : ViewStates.Visible;
            recommendProductsViewHolder.LyDelete.Visibility = ViewDelete ? ViewStates.Visible : ViewStates.Gone;

            this.RefreshQuantity(recommendProductsViewHolder, product);
            this.EventSelected(recommendProductsViewHolder, product.Selected);
            this.VisibleWeight(recommendProductsViewHolder, product.IsEstimatedWeight, product);
        }

        public void EventSelected(RecommendProductsViewHolder recommendProductsViewHolder, bool Selected)
        {
            if (Selected)
            {
                recommendProductsViewHolder.IvSelect.SetImageResource(Resource.Drawable.seleccionar);
                recommendProductsViewHolder.LyRecommendList.SetBackgroundResource(Resource.Drawable.button_little_card_caducity);
                recommendProductsViewHolder.LySelectList.SetBackgroundResource(Resource.Drawable.button_little_primary);
            }
            else
            {
                recommendProductsViewHolder.IvSelect.SetImageResource(Resource.Drawable.checkbox);
                recommendProductsViewHolder.LyRecommendList.SetBackgroundResource(Color.Transparent);
                recommendProductsViewHolder.LySelectList.SetBackgroundResource(Color.Transparent);
            }
        }

        public void VisibleWeight(RecommendProductsViewHolder recommendProductsViewHolder, bool Visible, Product product)
        {
            if (Visible)
            {
                recommendProductsViewHolder.LyProductLabels.Visibility = ViewStates.Visible;
                recommendProductsViewHolder.ViewProductDivider.Visibility = ViewStates.Visible;
                recommendProductsViewHolder.TvDescription.Visibility = ViewStates.Gone;
            }
            else
            {
                recommendProductsViewHolder.LyProductLabels.Visibility = ViewStates.Gone;
                recommendProductsViewHolder.ViewProductDivider.Visibility = ViewStates.Gone;
                recommendProductsViewHolder.TvDescription.Visibility = ViewStates.Visible;
            }

            this.EventWeightSeleted(recommendProductsViewHolder, product.WeightSelected);
        }

        public void EventWeightSeleted(RecommendProductsViewHolder recommendProductsViewHolder, bool WeightSeleted)
        {
            if (WeightSeleted)
            {
                this.EventProductLabels(recommendProductsViewHolder.TvWeight, true);
                this.EventProductLabels(recommendProductsViewHolder.TvUnit, false);
                this.EventTypeQuantity(recommendProductsViewHolder.TvProductQuantityWeight, true);
                this.EventTypeQuantity(recommendProductsViewHolder.TvProductQuantity, false);
            }
            else
            {
                this.EventProductLabels(recommendProductsViewHolder.TvWeight, false);
                this.EventProductLabels(recommendProductsViewHolder.TvUnit, true);
                this.EventTypeQuantity(recommendProductsViewHolder.TvProductQuantityWeight, false);
                this.EventTypeQuantity(recommendProductsViewHolder.TvProductQuantity, true);
            }
        }

        public void EventProductLabels(TextView textView, bool Event)
        {
            if (Event)
            {
                textView.SetBackgroundResource(Resource.Drawable.text_selected);
                textView.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
                textView.SetTextColor(new Color(ContextCompat.GetColor(Context, Resource.Color.colorLink)));
            }
            else
            {
                textView.SetBackgroundResource(Color.Transparent);
                textView.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayMedium), TypefaceStyle.Normal);
                textView.SetTextColor(new Color(ContextCompat.GetColor(Context, Resource.Color.colorDescription)));
            }
        }

        public void EventTypeQuantity(TextView textView, bool QuantityWeight)
        {
            if (QuantityWeight)
            {
                textView.Visibility = ViewStates.Visible;
            }
            else
            {
                textView.Visibility = ViewStates.Gone;
            }
        }

        private void RefreshQuantity(RecommendProductsViewHolder recommendProductsViewHolder, Product product)
        {
            if (product != null)
            {
                recommendProductsViewHolder.TvProductQuantity.Text = product.Quantity.ToString();
                recommendProductsViewHolder.TvProductQuantityWeight.Text = (product.Quantity * product.Weight).ToString();
                recommendProductsViewHolder.TvProductQuantityWeight.Text += product.WeightUnits ?? string.Empty;
            }
        }

        private void ActionSelected(RecommendProductsViewHolder recommendProductsViewHolder)
        {
            int position = recommendProductsViewHolder.AdapterPosition;
            ProductList product = Products[position];
            product.Selected = !product.Selected;

            if (product.Quantity < 1)
            {
                product.Quantity = product.Quantity < 1 ? 1 : product.Quantity;
            }
            else
            {
                if (!ViewDelete)
                {
                    product.Quantity = 0;
                }
            }

            InterfaceMyList.OnItemSelected(product, position);
            EventSelected(recommendProductsViewHolder, product.Selected);
        }

        public override int ItemCount
        {
            get { return Products != null ? Products.Count() : 0; }
        }

        public override void OnViewRecycled(Java.Lang.Object holder)
        {
            base.OnViewRecycled(holder);
            RecyclerView.ViewHolder rvHolder = holder as RecyclerView.ViewHolder;
            RecommendProductsViewHolder viewHolder = holder as RecommendProductsViewHolder;
            Glide.With(rvHolder.ItemView.Context).Clear(viewHolder.IvProduct);
        }
    }
}