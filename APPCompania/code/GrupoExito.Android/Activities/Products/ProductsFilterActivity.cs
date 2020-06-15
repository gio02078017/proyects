using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Com.Bumptech.Glide;
using GrupoExito.Android.Activities.Base;
using GrupoExito.Android.Adapters;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.Utilities.Constants;
using GrupoExito.DataAgent.Services.Products;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants;
using GrupoExito.Logic.Models.Products;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrupoExito.Android.Activities.Products
{
    [Activity(Label = "Filtro y ordenamiento de productos", ScreenOrientation = ScreenOrientation.Portrait, WindowSoftInputMode = SoftInput.StateHidden)]
    public class ProductsFilterActivity : BaseProductActivity, IFilterAdapter
    {
        #region Controls

        private Category Category;
        private TextView TvFilter;
        private TextView TvOrder;
        private TextView TvFilterBrands;
        private TextView TvFilterCategories;
        private TextView TvProductCategory;
        private TextView TvSearcher;
        private TextView TvOrderPrice;
        private TextView TvOrderName;
        private TextView TvOrderRelevance;
        private LinearLayout LyFilter;
        private LinearLayout LyOrder;
        private LinearLayout LyFilterCategories;
        private LinearLayout LyFilterBrands;
        private ImageView IvFilter;
        private ImageView IvOrder;
        private ImageView IvProductCategory;
        private RecyclerView RvCategories;
        private RecyclerView RvBrands;
        private LinearLayoutManager CategoriesLayoutManager;
        private LinearLayoutManager BrandsLayoutManager;
        private FilterAdapter FilterCategoryAdapter;
        private FilterAdapter FlterBrandAdapter;
        private NestedScrollView NdOrder;
        private NestedScrollView NdFilter;
        private Switch SwOrderNameDescendant;
        private Switch SwOrderNameAscending;
        private Switch SwOrderPriceDescendant;
        private Switch SwOrderPriceAscending;
        private Switch SwOrderRelevance;
        private Button BtnClear;
        private Button BtnApply;

        #endregion

        #region Properties

        private bool IsfilterSelected;
        private ProductsModel _productsModel;

        #endregion

        public void OnCategoryChecked(ProductFilter category)
        {
            bool selected;

            if (ParametersManager.CategoryNames.Contains(category.Key))
            {
                selected = false;
                ParametersManager.CategoryNames.Remove(category.Key);
            }
            else
            {
                selected = true;
                ParametersManager.CategoryNames.Add(category.Key);
            }

            ParametersManager.Categories.Where(x => x.Key == category.Key).First().Checked = selected;
        }

        public void OnBrandChecked(ProductFilter brand)
        {
            bool selected;

            if (ParametersManager.BrandNames.Contains(brand.Key))
            {
                selected = false;
                ParametersManager.BrandNames.Remove(brand.Key);
            }
            else
            {
                selected = true;
                ParametersManager.BrandNames.Add(brand.Key);
            }

            ParametersManager.Brands.Where(x => x.Key == brand.Key).First().Checked = selected;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            _productsModel = new ProductsModel(new ProductsService(DeviceManager.Instance));
            SetContentView(Resource.Layout.ActivityProductsFilter);
            SetActionBar(GetMainToolbar(Resource.Id.mainToolbar, this));

            if (SupportActionBar != null)
            {
                SupportActionBar.Hide();
            }

            this.SetControlsProperties();
            this.EditFonts();
            this.GetParameters();
            this.DefineSelectedFragment(IsfilterSelected);
            var root = FindViewById<LinearLayout>(Resource.Id.rootLayout);
            root.RequestFocus();
        }

        private void EditFonts()
        {
            SetToolbarControls(FindViewById<TextView>(Resource.Id.tvToolbarPrice), FindViewById<TextView>(Resource.Id.tvToolbarQuantity));
            TvSearcher.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            SetLoader(FindViewById<RelativeLayout>(Resource.Id.layoutLoader));
            TvFilterCategories.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvFilterBrands.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvFilter.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvOrder.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            BtnApply.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            BtnClear.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvProductCategory.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            SwOrderNameAscending.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            SwOrderNameDescendant.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            SwOrderPriceAscending.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            SwOrderPriceDescendant.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            SwOrderRelevance.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvOrderName.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvOrderPrice.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvOrderRelevance.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
        }

        private void SetControlsProperties()
        {
            SetToolbarControls(FindViewById<TextView>(Resource.Id.tvToolbarPrice),
            FindViewById<TextView>(Resource.Id.tvToolbarQuantity));

            SetSearcher(FindViewById<LinearLayout>(Resource.Id.searcher));

            SetToolbarCarItems();

            TvSearcher = FindViewById<TextView>(Resource.Id.tvSearcher);
            TvToolbarPrice = FindViewById<TextView>(Resource.Id.tvToolbarPrice);
            TvFilterCategories = FindViewById<TextView>(Resource.Id.tvFilterProductCategory);
            TvFilterBrands = FindViewById<TextView>(Resource.Id.tvProductBrand);
            TvOrderPrice = FindViewById<TextView>(Resource.Id.tvOrderPrice);
            TvOrderName = FindViewById<TextView>(Resource.Id.tvOrderName);
            TvOrderRelevance = FindViewById<TextView>(Resource.Id.tvRelevance);
            IvFilter = FindViewById<ImageView>(Resource.Id.ivProductFilter);
            IvOrder = FindViewById<ImageView>(Resource.Id.ivProductOrder);
            IvProductCategory = FindViewById<ImageView>(Resource.Id.ivProductCategory);
            LyFilter = FindViewById<LinearLayout>(Resource.Id.lyProductFilter);
            LyFilter.Click += delegate { DefineSelectedFragment(); };
            LyOrder = FindViewById<LinearLayout>(Resource.Id.lyProductOrder);
            LyOrder.Click += delegate { DefineSelectedFragment(false); };
            LyFilterBrands = FindViewById<LinearLayout>(Resource.Id.lyFilterBrands);
            LyFilterBrands.Click += delegate { ManageBrandsView(); };
            LyFilterCategories = FindViewById<LinearLayout>(Resource.Id.lyFilterCategories);
            LyFilterCategories.Click += delegate { ManageCategoriesView(); };
            TvFilter = FindViewById<TextView>(Resource.Id.tvProductFilters);
            NdFilter = FindViewById<NestedScrollView>(Resource.Id.filter);
            NdOrder = FindViewById<NestedScrollView>(Resource.Id.order);
            TvOrder = FindViewById<TextView>(Resource.Id.tvProductOrder);
            TvProductCategory = FindViewById<TextView>(Resource.Id.tvProductCategory);
            BtnClear = FindViewById<Button>(Resource.Id.btnCleanFilter);
            BtnApply = FindViewById<Button>(Resource.Id.btnApplyFilter);
            SwOrderNameAscending = FindViewById<Switch>(Resource.Id.swNameAscending);
            SwOrderNameDescendant = FindViewById<Switch>(Resource.Id.swNameDescendant);
            SwOrderPriceAscending = FindViewById<Switch>(Resource.Id.swPriceAscending);
            SwOrderPriceDescendant = FindViewById<Switch>(Resource.Id.swPriceDescendant);
            SwOrderRelevance = FindViewById<Switch>(Resource.Id.swRelevance);
            RvCategories = FindViewById<RecyclerView>(Resource.Id.rvFilterCategories);
            CategoriesLayoutManager = new LinearLayoutManager(this, LinearLayoutManager.Vertical, false)
            {
                AutoMeasureEnabled = true
            };
            RvCategories.NestedScrollingEnabled = false;
            RvCategories.HasFixedSize = false;
            RvCategories.SetLayoutManager(CategoriesLayoutManager);
            RvBrands = FindViewById<RecyclerView>(Resource.Id.rvFilterBrands);
            BrandsLayoutManager = new LinearLayoutManager(this, LinearLayoutManager.Vertical, false)
            {
                AutoMeasureEnabled = true
            };
            RvBrands.NestedScrollingEnabled = false;
            RvBrands.HasFixedSize = false;
            RvBrands.SetLayoutManager(BrandsLayoutManager);
            IvToolbarBack = FindViewById<ImageView>(Resource.Id.ivToolbarBack);

            IvToolbarBack.Click += delegate { OnBackPressed(); };
            BtnClear.Click += delegate { BtnClearDelegate(); };
            BtnApply.Click += async delegate { await BtnApplyDelegate(); };
            SwOrderNameAscending.CheckedChange += delegate { SwOrderNameAscendingCheckedChange(); };
            SwOrderNameDescendant.CheckedChange += delegate { SwOrderNameDescendantCheckedChange(); };
            SwOrderPriceAscending.CheckedChange += delegate { SwOrderPriceAscendingCheckedChange(); };
            SwOrderPriceDescendant.CheckedChange += delegate { SwOrderPriceDescendantCheckedChange(); };
            SwOrderRelevance.CheckedChange += delegate { SwOrderRelevanceCheckedChange(); };
        }

        private void SwOrderNameAscendingCheckedChange()
        {
            if (SwOrderNameAscending.Checked)
            {
                ParametersManager.OrderBy = ConstOrder.Name;
                ParametersManager.OrderType = ConstOrderType.Asc;
                SwOrderNameDescendant.Checked = false;
                SwOrderPriceAscending.Checked = false;
                SwOrderPriceDescendant.Checked = false;
                SwOrderRelevance.Checked = false;
            }
        }

        private void SwOrderNameDescendantCheckedChange()
        {
            if (SwOrderNameDescendant.Checked)
            {
                ParametersManager.OrderBy = ConstOrder.Name;
                ParametersManager.OrderType = ConstOrderType.Desc;
                SwOrderPriceAscending.Checked = false;
                SwOrderNameAscending.Checked = false;
                SwOrderPriceDescendant.Checked = false;
                SwOrderRelevance.Checked = false;
            }
        }

        private void SwOrderPriceAscendingCheckedChange()
        {
            if (SwOrderPriceAscending.Checked)
            {
                ParametersManager.OrderBy = ConstOrder.Price;
                ParametersManager.OrderType = ConstOrderType.Asc;
                SwOrderNameDescendant.Checked = false;
                SwOrderNameAscending.Checked = false;
                SwOrderPriceDescendant.Checked = false;
                SwOrderRelevance.Checked = false;
            }
        }

        private void SwOrderPriceDescendantCheckedChange()
        {
            if (SwOrderPriceDescendant.Checked)
            {
                ParametersManager.OrderBy = ConstOrder.Price;
                ParametersManager.OrderType = ConstOrderType.Desc;
                SwOrderNameDescendant.Checked = false;
                SwOrderPriceAscending.Checked = false;
                SwOrderNameAscending.Checked = false;
                SwOrderRelevance.Checked = false;
            }
        }

        private void SwOrderRelevanceCheckedChange()
        {
            if (SwOrderRelevance.Checked)
            {
                ParametersManager.OrderBy = ConstOrder.Relevance;
                ParametersManager.OrderType = ConstOrderType.Desc;
                SwOrderNameDescendant.Checked = false;
                SwOrderPriceAscending.Checked = false;
                SwOrderNameAscending.Checked = false;
                SwOrderPriceDescendant.Checked = false;
            }
        }

        private void BtnClearDelegate()
        {
            ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);
            this.ClearFilters();
        }

        private async Task BtnApplyDelegate()
        {
            ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);
            await this.GetProductsCategories();
        }

        private void GetParameters()
        {
            Category = !string.IsNullOrEmpty(Intent.Extras.GetString(ConstantPreference.Category)) ?
                    JsonService.Deserialize<Category>(Intent.Extras.GetString(ConstantPreference.Category)) : new Category();

            IsfilterSelected = !string.IsNullOrEmpty(Intent.Extras.GetString(ConstantPreference.FilterView)) ? JsonService.Deserialize<bool>(Intent.Extras.GetString(ConstantPreference.FilterView)) : false;

            if (Category != null && Category.Name != null)
            {
                TvProductCategory.Text = Category.Name;
                Glide.With(ApplicationContext).Load(Category.IconCategoryGray).Into(IvProductCategory);
                this.PaintFilterCategories();
            }

            this.PaintFilterBrands();
            this.PaintOrder();
        }

        private void ManageCategoriesView()
        {
            if (RvCategories.Visibility == ViewStates.Gone)
            {
                RvCategories.Visibility = ViewStates.Visible;
                TvFilterCategories.SetBackgroundResource(Resource.Drawable.gradient_filter);
                TvFilterCategories.SetCompoundDrawablesWithIntrinsicBounds(0, 0, Resource.Drawable.flecha_arriba, 0);
            }
            else
            {
                RvCategories.Visibility = ViewStates.Gone;
                TvFilterCategories.SetBackgroundColor(Color.White);
                TvFilterCategories.SetCompoundDrawablesWithIntrinsicBounds(0, 0, Resource.Drawable.flecha_abajo, 0);
            }
        }

        private void ManageBrandsView()
        {
            if (RvBrands.Visibility == ViewStates.Gone)
            {
                RvBrands.Visibility = ViewStates.Visible;
                TvFilterBrands.SetBackgroundResource(Resource.Drawable.gradient_filter);
                TvFilterBrands.SetCompoundDrawablesWithIntrinsicBounds(0, 0, Resource.Drawable.flecha_arriba, 0);
            }
            else
            {
                RvBrands.Visibility = ViewStates.Gone;
                TvFilterBrands.SetBackgroundColor(Color.White);
                TvFilterBrands.SetCompoundDrawablesWithIntrinsicBounds(0, 0, Resource.Drawable.flecha_abajo, 0);
            }
        }

        private void PaintFilterCategories()
        {
            if (ParametersManager.Categories != null)
            {
                FilterCategoryAdapter = new FilterAdapter(ParametersManager.Categories, this, this, true);
                RvCategories.SetAdapter(FilterCategoryAdapter);
            }
        }

        private void PaintFilterBrands()
        {
            if (ParametersManager.Brands != null)
            {
                FlterBrandAdapter = new FilterAdapter(ParametersManager.Brands, this, this, false);
                RvBrands.SetAdapter(FlterBrandAdapter);
            }
        }

        private void PaintOrder()
        {
            switch (ParametersManager.OrderBy)
            {
                case ConstOrder.Name:
                    if (ParametersManager.OrderType.Equals(ConstOrderType.Desc))
                    {
                        SwOrderNameDescendant.Checked = true;
                    }
                    else
                    {
                        SwOrderNameAscending.Checked = true;
                    }
                    break;
                case ConstOrder.Price:
                    if (ParametersManager.OrderType.Equals(ConstOrderType.Desc))
                    {
                        SwOrderPriceDescendant.Checked = true;
                    }
                    else
                    {
                        SwOrderPriceAscending.Checked = true;
                    }
                    break;
                default:
                    SwOrderRelevance.Checked = true;
                    break;
            }
        }

        private void DefineSelectedFragment(bool? isFilter = true)
        {
            if (isFilter.HasValue && isFilter.Value)
            {
                LyOrder.SetBackgroundResource(Resource.Drawable.button_white);
                TvOrder.SetTextColor(Color.Black);
                IvOrder.SetImageResource(Resource.Drawable.order);
                LyFilter.SetBackgroundResource(Resource.Drawable.button_black);
                TvFilter.SetTextColor(Color.White);
                IvFilter.SetImageResource(Resource.Drawable.filtro_secundario);
                NdOrder.Visibility = ViewStates.Gone;
                NdFilter.Visibility = ViewStates.Visible;
            }
            else
            {
                LyFilter.SetBackgroundResource(Resource.Drawable.button_white);
                TvFilter.SetTextColor(Color.Black);
                IvFilter.SetImageResource(Resource.Drawable.filtro);
                LyOrder.SetBackgroundResource(Resource.Drawable.button_black);
                TvOrder.SetTextColor(Color.White);
                IvOrder.SetImageResource(Resource.Drawable.order_primario);
                NdOrder.Visibility = ViewStates.Visible;
                NdFilter.Visibility = ViewStates.Gone;
            }
        }

        private void ClearFilters()
        {
            ParametersManager.CategoryNames = new List<string>();
            ParametersManager.BrandNames = new List<string>();

            if (ParametersManager.Categories != null)
            {
                ParametersManager.Categories.ToList().ForEach(x => x.Checked = false);
            }

            if (ParametersManager.Brands != null)
            {
                ParametersManager.Brands.ToList().ForEach(x => x.Checked = false);
            }

            ParametersManager.OrderBy = ConstOrder.Relevance;
            ParametersManager.OrderType = ConstOrderType.Desc;

            this.SwOrderNameAscending.Checked = false;
            this.SwOrderNameDescendant.Checked = false;
            this.SwOrderPriceAscending.Checked = false;
            this.SwOrderPriceDescendant.Checked = false;
            this.SwOrderRelevance.Checked = true;
            this.PaintFilterCategories();
            this.PaintFilterBrands();
            HideProgressDialog();
        }

        private async Task GetProductsCategories()
        {
            try
            {
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);
                ParametersManager.From = "0";
                SearchProductsParameters parameters = new SearchProductsParameters
                {
                    DependencyId = ParametersManager.UserContext.DependencyId,
                    CategoryId = Category != null ? Category.Id : string.Empty,
                    CategoriesNames = ParametersManager.CategoryNames.Count == 0 ? null : ParametersManager.CategoryNames,
                    Brands = ParametersManager.BrandNames.Count == 0 ? null : ParametersManager.BrandNames,
                    Size = ParametersManager.Size,
                    From = ParametersManager.From,
                    OrderBy = ParametersManager.OrderBy,
                    OrderType = ParametersManager.OrderType
                };

                var response = await _productsModel.GetProducts(parameters);

                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    DefineShowErrorWay(AppMessages.ApplicationName, MessagesHelper.GetMessage(response.Result), AppMessages.AcceptButtonText);
                }
                else
                {
                    ParametersManager.ChangeProductQuantity = true;
                    ParametersManager.Products = new List<Product>();
                    ParametersManager.Products = response.Products;
                    HideProgressDialog();

                    this.RunOnUiThread(() =>
                    {
                        OnBackPressed();
                    });
                }
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.ProductsFilterActivity, ConstantMethodName.GetProducts } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
            finally
            {
                HideProgressDialog();
            }
        }
    }
}