using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Widget;
using Com.Bumptech.Glide;
using GrupoExito.Android.Activities.Base;
using GrupoExito.Android.Adapters;
using GrupoExito.Android.Services;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.Utilities.Constants;
using GrupoExito.DataAgent.DataBase;
using GrupoExito.DataAgent.Services.Products;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Responses.Products;
using GrupoExito.Logic.Models.Products;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrupoExito.Android.Activities.Products
{
    [Activity(Label = "Detalle del producto", ScreenOrientation = ScreenOrientation.Portrait)]
    public class ProductActivity : BaseProductActivity, ViewPager.IOnPageChangeListener
    {
        #region Controls

        private ImageView IvProduct;
        private ImageView IvNutritional;
        private TextView TvProductName;
        private TextView TvProductPrice;
        private TextView TvOtherPaymentMethods;
        private ImageView IvProductDiscount;
        private ImageView IvZoom;
        private ImageView IvAdd;
        private ImageView IvSubstract;
        private TextView TvProductDiscount;
        private TextView TvProductPreviousPrice;
        private TextView TvProductDescription;
        private TextView TvProductQuestionDescription;
        private TextView TvProductQuestion;
        private TextView TvProductAddList;
        private TextView TvProductAddListDescription;
        private EditText EtProductAddListDescription;
        private TextView TvProductNotesQuestion;
        private TextView TvAddProduct;
        private TextView TvProductActions;
        private TextView TvUnits;
        private TextView TvWeight;
        private TextView TvProductQuantityWeight;
        private TextView TvPum;
        private LinearLayout LyProductNotes;
        private GalleryAdapter GalleryAdapter;
        private ViewPager VpGallery;
        private View[] DotsGalleryViews;
        private LinearLayout LyImagecount;
        private LinearLayout LyVariableWeight;
        private LinearLayout LyAddProduct;
        private ImageView IvDiscountForCards;
        private View ViewProductDescription;
        private LinearLayout LyProductDescription;
        private View ViewProductQuestion;
        private LinearLayout LyProductQuestion;
        private LinearLayout LyInformationProduct;
        private NestedScrollView NsvDetailProduct;
        private ImageView IvAddToList;

        #endregion

        #region Properties

        private int Quantity { get; set; }
        private Product Product { get; set; }
        private ProductModel _productModel;
        private ProductCarModel _productCarModel;
        private decimal defaultQuantityWeight;
        private bool UnitSelected { get; set; }
        private Product ProductSelected { get; set; }

        #endregion       

        public void OnPageScrollStateChanged(int state)
        {
        }

        public void OnPageScrolled(int position, float positionOffset, int positionOffsetPixels)
        {
        }

        public void OnPageSelected(int position)
        {
            for (int i = 0; i < Product.UrlImagesXL.Count; i++)
            {
                DotsGalleryViews[i].SetBackgroundResource(Resource.Drawable.circle_stroke);
            }

            DotsGalleryViews[position].SetBackgroundResource(Resource.Drawable.circle_solid);
        }

        public override void OnBackPressed()
        {
            RunOnUiThread(() =>
            {
                Finish();
            });
        }

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            _productModel = new ProductModel(new ProductsService(DeviceManager.Instance));
            _productCarModel = new ProductCarModel(ProductCarDataBase.Instance);
            SetContentView(Resource.Layout.ActivityProduct);
            SetActionBar(GetMainToolbar(Resource.Id.mainToolbar, this));

            if (SupportActionBar != null)
            {
                SupportActionBar.Hide();
            }

            this.SetControlsProperties();

            if (!string.IsNullOrEmpty(Intent.Extras.GetString(ConstantPreference.Product)))
            {
                ProductSelected = JsonService.Deserialize<Product>(Intent.Extras.GetString(ConstantPreference.Product));
                await this.GetProductDetail(ProductSelected);
            }

            if (ParametersManager.ParameterActionBanner != null && !string.IsNullOrEmpty(ParametersManager.ParameterActionBanner.Id))
            {
                ProductSelected = new Product
                {
                    SkuId = ParametersManager.ParameterActionBanner.SkuId,
                    Id = ParametersManager.ParameterActionBanner.Id,
                    ProductType = ParametersManager.ParameterActionBanner.ProductType
                };

                ParametersManager.ParameterActionBanner = null;
                await this.GetProductDetail(ProductSelected);
            }            
        }

        protected override void OnResume()
        {
            base.OnResume();
            FirebaseRegistrationEventsService.Instance.RegisterScreen(this, AnalyticsScreenView.Product, typeof(ProductActivity).Name);
        }

        protected override void RefreshProductList(Product ActualProduct)
        {
            RunOnUiThread(() =>
            {
                OnBackPressed();
                base.RefreshProductList(ActualProduct);
            });
        }

        protected override void EventError()
        {
            base.EventError();
            this.RunOnUiThread(async () =>
            {
                await this.GetProductDetail(ProductSelected);
            });
        }

        private void SetControlsProperties()
        {
            SetToolbarControls(FindViewById<TextView>(Resource.Id.tvToolbarPrice), FindViewById<TextView>(Resource.Id.tvToolbarQuantity));
            SetSearcher(FindViewById<LinearLayout>(Resource.Id.searcher));
            SetLoader(FindViewById<RelativeLayout>(Resource.Id.layoutLoader));
            SetErrorLayout(FindViewById<RelativeLayout>(Resource.Id.layoutError),
             FindViewById<RelativeLayout>(Resource.Id.rlBody));
            FindViewById<TextView>(Resource.Id.tvSearcher).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);

            NsvDetailProduct = FindViewById<NestedScrollView>(Resource.Id.nsvDetailProduct);
            LyInformationProduct = FindViewById<LinearLayout>(Resource.Id.lyInformationProduct);

            VpGallery = FindViewById<ViewPager>(Resource.Id.vpGallery);
            LyImagecount = FindViewById<LinearLayout>(Resource.Id.lyImagecount);
            LyVariableWeight = FindViewById<LinearLayout>(Resource.Id.lyVariableWeight);
            IvProduct = FindViewById<ImageView>(Resource.Id.ivProduct);
            IvZoom = FindViewById<ImageView>(Resource.Id.ivZoom);
            IvAdd = FindViewById<ImageView>(Resource.Id.ivAdd);
            IvSubstract = FindViewById<ImageView>(Resource.Id.ivSubstract);
            TvProductName = FindViewById<TextView>(Resource.Id.tvProductName);
            TvProductDescription = FindViewById<TextView>(Resource.Id.tvProductDescription);
            ViewProductDescription = FindViewById<View>(Resource.Id.viewProductDescription);
            LyProductDescription = FindViewById<LinearLayout>(Resource.Id.lyProductDescription);
            ViewProductQuestion = FindViewById<View>(Resource.Id.viewProductQuestion);
            LyProductQuestion = FindViewById<LinearLayout>(Resource.Id.lyProductQuestion);
            TvProductPrice = FindViewById<TextView>(Resource.Id.tvProductPrice);
            TvProductPreviousPrice = FindViewById<TextView>(Resource.Id.tvProductPreviousPrice);
            TvPum = FindViewById<TextView>(Resource.Id.tvPum);
            TvOtherPaymentMethods = FindViewById<TextView>(Resource.Id.tvOtherPaymentMethods);
            IvNutritional = FindViewById<ImageView>(Resource.Id.ivNutritional);
            LyProductNotes = FindViewById<LinearLayout>(Resource.Id.lyProductNotes);
            LyAddProduct = FindViewById<LinearLayout>(Resource.Id.lyAddProduct);
            TvProductQuestionDescription = FindViewById<TextView>(Resource.Id.tvProductQuestionDescription);
            TvProductQuestion = FindViewById<TextView>(Resource.Id.tvProductQuestion);
            TvProductAddList = FindViewById<TextView>(Resource.Id.tvProductAddList);
            TvProductAddListDescription = FindViewById<TextView>(Resource.Id.tvProductAddListDescription);
            TvUnits = FindViewById<TextView>(Resource.Id.tvUnits);
            TvWeight = FindViewById<TextView>(Resource.Id.tvWeight);
            EtProductAddListDescription = FindViewById<EditText>(Resource.Id.etProductAddListDescription);
            TvProductNotesQuestion = FindViewById<TextView>(Resource.Id.tvProductNotesQuestion);
            TvAddProduct = FindViewById<TextView>(Resource.Id.tvAddProduct);
            TvProductActions = FindViewById<TextView>(Resource.Id.tvProductActions);
            TvProductQuantityWeight = FindViewById<TextView>(Resource.Id.tvProductQuantityWeight);
            FindViewById<ImageView>(Resource.Id.ivToolbarBack).Click += delegate { OnBackPressed(); };
            TvProductDiscount = FindViewById<TextView>(Resource.Id.tvProductDiscount);
            IvProductDiscount = FindViewById<ImageView>(Resource.Id.ivProductDiscount);
            IvDiscountForCards = FindViewById<ImageView>(Resource.Id.ivDiscountForCards);
            IvAddToList = FindViewById<ImageView>(Resource.Id.ivAddToList);
            IvAdd.Click += delegate { AddProducts(); };
            IvSubstract.Click += delegate { SubstractProducts(); };
            LyAddProduct.Click += delegate { AddProduct(); };
            TvUnits.Click += delegate { DefineQuantityView(true); };
            TvWeight.Click += delegate { DefineQuantityView(false); };
            IvAddToList.Click += delegate { OnCreateList(); };
            this.ScrollEnd(LyInformationProduct, 0);
        }

        private void AddProduct()
        {
            try
            {
                var productQuantity = UnitSelected ? int.Parse(TvProductActions.Text)
                    : decimal.Parse(TvProductActions.Text) / defaultQuantityWeight;

                int quantity = Convert.ToInt32(productQuantity);
                Product.Quantity = quantity;
                Product.Note = EtProductAddListDescription.Text;
                _productCarModel.UpSertProduct(Product);
                _productCarModel.RecalculateSummary();
                SetToolbarCarItems();
                OnBackPressed();
                ParametersManager.ChangeProductQuantity = true;
                RegisterAddProductEvent(Product);
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.ProductActivity, ConstantMethodName.AddProduct } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
        }

        private void SubstractProducts()
        {           
            var productQuantity = UnitSelected ? int.Parse(TvProductActions.Text)
                  : decimal.Parse(TvProductActions.Text) / defaultQuantityWeight;

            if (productQuantity > 1)
            {
                TvProductActions.Text = UnitSelected ? (ConvertUtilities.StringToDouble(TvProductActions.Text) - 1).ToString() :
                    (decimal.Parse(TvProductActions.Text) - defaultQuantityWeight).ToString();
            }
        }

        private void AddProducts()
        {
            TvProductActions.Text = UnitSelected ?
                     (ConvertUtilities.StringToDouble(TvProductActions.Text) + 1).ToString() :
                     (decimal.Parse(TvProductActions.Text) + defaultQuantityWeight).ToString();
        }

        private async Task GetProductDetail(Product productSelected)
        {
            try
            {
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);
                ProductParameters parameters = GetParameters(productSelected);
                ProductResponse response = await _productModel.GetProduct(parameters);

                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    RunOnUiThread(() =>
                    {
                        DefineShowErrorWay(AppMessages.ApplicationName, MessagesHelper.GetMessage(response.Result), AppMessages.AcceptButtonText);
                    });
                }
                else
                {
                    Product = response.Product;
                    Product productLocal = _productCarModel.GetProduct(Product.Id);
                    Product.Quantity = productLocal != null ? productLocal.Quantity : 0;
                    Product.Note = productLocal != null ? productLocal.Note : string.Empty;
                    Quantity = Product.Quantity;
                    this.DrawProduct();
                    SetToolbarCarItems();
                    ShowBodyLayout();
                }
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.ProductActivity, ConstantMethodName.GetProducts } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
            finally
            {
                HideProgressDialog();
            }
        }

        private ProductParameters GetParameters(Product productSelected)
        {
            ProductParameters parameters = new ProductParameters
            {
                SkuId = productSelected.SkuId,
                ProductId = productSelected.Id,
                DependencyId = ParametersManager.UserContext.DependencyId,
                ProductType = productSelected.ProductType
            };

            return parameters;
        }

        private void DrawProduct()
        {
            RunOnUiThread(() =>
            {
                if (Product != null && Product.UrlImagesDefault != null)
                {
                    GalleryAdapter = new GalleryAdapter(this, Product.UrlImagesDefault);
                    VpGallery.Adapter = GalleryAdapter;
                    this.DrawProductInformation();
                    VpGallery.AddOnPageChangeListener(this);
                }
            });

            HideProgressDialog();
        }

        private void DrawProductInformation()
        {
            this.DrawImage();
            this.ImageNutrition();
            this.EditFont();
            this.ProductDiscountPercent();

            Product.UrlMediumImage = Product.UrlImagesXL.Any() ? Product.UrlImagesXL[0] : string.Empty;
            EtProductAddListDescription.Text = Product.Note;
            IvProductDiscount.Visibility = Product.Price.DiscountPercent > 0 ? ViewStates.Visible : ViewStates.Gone;
            TvProductDiscount.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            LyVariableWeight.Visibility = Product.IsEstimatedWeight ? ViewStates.Visible : ViewStates.Gone;
            TvProductQuantityWeight.Text = Product.WeightUnits;
            defaultQuantityWeight = Product.Weight;
            TvProductActions.Text = Product.Quantity > 0 ? Product.Quantity.ToString() : "1";
            TvPum.Text = Product.Price.Pum;
            TvPum.Visibility = string.IsNullOrEmpty(Product.Price.Pum) ? ViewStates.Gone : ViewStates.Visible;
            TvOtherPaymentMethods.Visibility = Product.Price.PriceOtherMeans > 0 &&
                Product.Price.PreviousPrice != Product.Price.PriceOtherMeans ? ViewStates.Visible : ViewStates.Gone;
        }

        private void ImageNutrition()
        {
            if (!string.IsNullOrEmpty(Product.UrlImageNutritionFact))
            {
                Glide.With(ApplicationContext).Load(Product.UrlImageNutritionFact).Into(IvNutritional);
                IvNutritional.Visibility = ViewStates.Visible;
            }
            else
            {
                IvNutritional.Visibility = ViewStates.Gone;
            }
        }

        private void ProductDiscountPercent()
        {
            if (Product.Price.DiscountPercent > 0)
            {
                TvProductPrice.Text = this.GetString(Resource.String.str_price_now) + " " + StringFormat.ToPrice(Product.Price.ActualPrice);
                TvProductPrice.SetTextColor(Color.ParseColor(ConstantColors.Red));
                TvProductPrice.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);

                TvProductPreviousPrice.Visibility = Product.Price.PreviousPrice > 0 ? ViewStates.Visible : ViewStates.Gone;
                TvProductPreviousPrice.PaintFlags = TvProductPrice.PaintFlags | PaintFlags.StrikeThruText;
                TvProductPreviousPrice.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
                TvProductPreviousPrice.Text = AppMessages.PriceBefore + " " + StringFormat.ToPrice(Product.Price.PreviousPrice);
                TvProductDiscount.Text = StringFormat.ToPercerntaje(Product.Price.DiscountPercent);

                if (Product.Price.DiscountImage != null)
                {
                    Glide.With(ApplicationContext).Load(Product.Price.DiscountImage).Thumbnail(0.1f).Into(IvDiscountForCards);
                    IvDiscountForCards.Visibility = ViewStates.Visible;
                }
            }
        }

        private void EditFont()
        {
            LyProductNotes.Visibility = Product.IsEstimatedWeight ? ViewStates.Visible : ViewStates.Gone;
            TvProductName.Text = Product.Name;
            TvProductName.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvProductDescription.Text = Product.Description;

            if (Product.Description == null || Product.Description.Equals(""))
            {
                ViewProductDescription.Visibility = ViewStates.Gone;
                LyProductDescription.Visibility = ViewStates.Gone;
            }

            if (!Product.IsEstimatedWeight)
            {
                ViewProductQuestion.Visibility = ViewStates.Gone;
                LyProductQuestion.Visibility = ViewStates.Gone;
            }

            TvProductDescription.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvProductPrice.Text = StringFormat.ToPrice(Product.Price.ActualPrice);
            TvOtherPaymentMethods.Text = AppMessages.OtherPaymentMethods + " " + StringFormat.ToPrice(Product.Price.PriceOtherMeans);
            TvProductPrice.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvOtherPaymentMethods.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            TvProductPreviousPrice.Visibility = Product.Price.DiscountPercent > 0 ? ViewStates.Visible : ViewStates.Gone;
            TvProductQuestion.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvProductQuestionDescription.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvProductAddList.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvProductAddListDescription.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            EtProductAddListDescription.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvProductNotesQuestion.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvAddProduct.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvProductActions.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvUnits.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvWeight.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvPum.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            IvZoom.Visibility = ViewStates.Visible;
            UnitSelected = true;
        }

        private void DrawImage()
        {
            DotsGalleryViews = new View[Product.UrlImagesXL.Count];

            if (Product.UrlImagesXL.Count < 2)
            {
                LyImagecount.Visibility = ViewStates.Gone;
            }

            for (int i = 0; i < Product.UrlImagesXL.Count; i++)
            {
                View view = new TextView(this);
                LinearLayout.LayoutParams parameters = new LinearLayout.LayoutParams(new LinearLayout.LayoutParams(40, 40));
                parameters.SetMargins(0, 0, 20, 0);
                view.LayoutParameters = parameters;

                if (i == 0)
                {
                    view.SetBackgroundResource(Resource.Drawable.circle_solid);
                }
                else
                {
                    view.SetBackgroundResource(Resource.Drawable.circle_stroke);
                }

                DotsGalleryViews[i] = view;
                LyImagecount.AddView(DotsGalleryViews[i]);
            }
        }

        private void DefineQuantityView(bool unit)
        {
            UnitSelected = unit;

            if (unit)
            {
                if (UnitSelected)
                {
                    TvProductActions.Text = (decimal.Parse(TvProductActions.Text) / defaultQuantityWeight).ToString();
                }

                TvUnits.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
                TvUnits.SetBackgroundResource(Resource.Drawable.text_selected);
                TvUnits.SetTextColor(Color.Black);

                TvWeight.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
                TvWeight.SetBackgroundColor(Color.Transparent);
                TvWeight.SetTextColor(Color.ParseColor(ConstantColors.Gray));
                TvProductQuantityWeight.Visibility = ViewStates.Gone;
            }
            else
            {
                if (!UnitSelected)
                {
                    TvProductActions.Text = (decimal.Parse(TvProductActions.Text) * defaultQuantityWeight).ToString();
                }

                TvWeight.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
                TvWeight.SetBackgroundResource(Resource.Drawable.text_selected);
                TvWeight.SetTextColor(Color.Black);

                TvUnits.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
                TvUnits.SetBackgroundColor(Color.Transparent);
                TvUnits.SetTextColor(Color.ParseColor(ConstantColors.Gray));
                TvProductQuantityWeight.Visibility = ViewStates.Visible;
            }
        }

        private void ScrollEnd(View view, int start)
        {
            Handler handler = new Handler();

            void productAction()
            {
                NsvDetailProduct.SmoothScrollingEnabled = true;
                int scrollTo = ((View)view.Parent.Parent).Top + view.Top + start;
                NsvDetailProduct.SmoothScrollTo(0, scrollTo);
            }

            handler.PostDelayed(productAction, 200);
        }

        private void OnCreateList()
        {
            var productQuantity = UnitSelected ? int.Parse(TvProductActions.Text)
                                  : decimal.Parse(TvProductActions.Text) / defaultQuantityWeight;

            int quantity = Convert.ToInt32(productQuantity);
            Product.Quantity = quantity;
            var productView = new Intent(this, typeof(CreateNewListActivity));
            productView.PutExtra(ConstantPreference.Product, JsonService.Serialize(Product));
            StartActivity(productView);
        }

        private void RegisterAddProductEvent(Product product)
        {
            FirebaseRegistrationEventsService.Instance.AddProductToCart(product, product.CategoryName);
            FacebookRegistrationEventsService.Instance.AddProductToCart(product);
        }
    }
}