using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using GrupoExito.Android.Activities.Base;
using GrupoExito.Android.Activities.Recipes;
using GrupoExito.Android.Adapters;
using GrupoExito.Android.Services;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.Utilities.Constants;
using GrupoExito.DataAgent.Services.Products;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Parameters;
using GrupoExito.Entities.Responses.Products;
using GrupoExito.Logic.Models.Products;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GrupoExito.Android.Activities.Products
{
    [Activity(Label = "Lista de productos", ScreenOrientation = ScreenOrientation.Portrait)]
    public class SearcherActivity : BaseProductActivity, TextView.IOnEditorActionListener, ISearcherAdapter
    {
        #region Controls

        private EditText EtSearcher;
        private LinearLayoutManager SearcherLayoutManager;
        private SearcherAdapter SearcherAdapter;
        private RecyclerView RvSearcher;
        private List<Item> Suggestions;
        private ProductsModel _productsModel;

        #endregion

        #region Properties

        private string searched;

        #endregion

        public bool OnEditorAction(TextView v, [GeneratedEnum] ImeAction actionId, KeyEvent e)
        {
            if (actionId == ImeAction.Search)
            {
                Search().ConfigureAwait(true);
                DeviceManager.HideKeyboard(this);
                return true;
            }

            return false;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            OverridePendingTransition(Resource.Animation.slide_in_up, Resource.Animation.slide_out_up);
            base.OnCreate(savedInstanceState);
            _productsModel = new ProductsModel(new ProductsService(DeviceManager.Instance));
            SetContentView(Resource.Layout.ActivitySearcher);
            SetActionBar(GetMainToolbar(Resource.Id.mainToolbar, this));

            if (SupportActionBar != null)
            {
                SupportActionBar.Hide();
            }

            SetControlProperties();            
        }

        protected override void OnResume()
        {
            base.OnResume();
            FirebaseRegistrationEventsService.Instance.RegisterScreen(this, AnalyticsScreenView.Searcher, typeof(SearcherActivity).Name);
        }

        protected override void EventError()
        {
            base.EventError();
            ClearSearcher();
        }

        protected override void EventNoInfo()
        {
            base.EventNoInfo();
            ClearSearcher();
        }

        private void ClearSearcher()
        {
            ShowBodyLayout();
            EtSearcher.Text = string.Empty;
        }

        private void SetControlProperties()
        {
            SetLoader(FindViewById<RelativeLayout>(Resource.Id.layoutLoader));
            SetToolbarControls(FindViewById<TextView>(Resource.Id.tvToolbarPrice), FindViewById<TextView>(Resource.Id.tvToolbarQuantity));
            SetNoInfoLayout(FindViewById<RelativeLayout>(Resource.Id.layoutNoInfo), FindViewById<LinearLayout>(Resource.Id.lySearcher), AppMessages.NotSearcherResultsTitle, AppMessages.NotSearcherResultsMessage);
            SetErrorLayout(FindViewById<RelativeLayout>(Resource.Id.layoutError),
             FindViewById<LinearLayout>(Resource.Id.lySearcher));

            FindViewById<LinearLayout>(Resource.Id.lyUser).Visibility = ViewStates.Invisible;
            IvToolbarBack = FindViewById<ImageView>(Resource.Id.ivToolbarBack);
            FindViewById<LinearLayout>(Resource.Id.lyPrice).Visibility = ViewStates.Invisible;
            RvSearcher = FindViewById<RecyclerView>(Resource.Id.rvSearcher);
            EtSearcher = FindViewById<EditText>(Resource.Id.tvSearcher);
            EtSearcher.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            EtSearcher.TextChanged += ActvSearcher_TextChanged;
            EtSearcher.EditorAction += (sender, args) =>
            {
                if (args.ActionId == ImeAction.Search)
                {
                    OnSearch();
                    args.Handled = true;
                }
            };
            SearcherLayoutManager = new LinearLayoutManager(this)
            {
                AutoMeasureEnabled = true
            };
            RvSearcher.NestedScrollingEnabled = false;
            RvSearcher.HasFixedSize = true;
            RvSearcher.SetLayoutManager(SearcherLayoutManager);
            IvToolbarBack.Click += delegate { OnBackPressed(); };
            LinearLayout lySearch = FindViewById<LinearLayout>(Resource.Id.searcher);
            ImageView ivSearch = lySearch.FindViewById<ImageView>(Resource.Id.ivSearcher);
            ivSearch.Click += delegate { OnSearch(); };
            ImageView ivScan = lySearch.FindViewById<ImageView>(Resource.Id.ivScan);
            ivScan.Click += delegate { this.EventScan(); };
        }

        private async Task Search()
        {
            try
            {
                ProductSearcherParameters productSearcherParameters = new ProductSearcherParameters
                {
                    Size = "10",
                    From = "0",
                    DependencyId = ParametersManager.UserContext.DependencyId,
                    Prefix = EtSearcher.Text
                };

                var response = await ProductSearcher(productSearcherParameters);

                if (response.Result == null || !response.Result.HasErrors || response.Result.Messages == null)
                {
                    Suggestions = new List<Item>();
                    Suggestions.AddRange(response.NameSuggest);
                    Suggestions.AddRange(response.CategorySuggest);
                    Suggestions.AddRange(response.BrandSuggest);

                    if (Suggestions.Count > 0)
                    {
                        SearcherAdapter = new SearcherAdapter(Suggestions, this, this);
                        RvSearcher.SetAdapter(SearcherAdapter);
                        ShowBodyLayout();
                    }
                }
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.SearcherActivity, ConstantMethodName.Search } };
                RegisterMessageExceptions(exception, properties);
                DefineShowErrorWay(AppMessages.ApplicationName, AppMessages.UnexpectedErrorMessage, AppMessages.AcceptButtonText);
            }
        }

        private async void ActvSearcher_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (EtSearcher.Text.Trim().Length >= 3)
            {
                await Search();
            }
            else
            {
                if (SearcherAdapter != null)
                {
                    Suggestions.Clear();
                    SearcherAdapter.NotifyDataSetChanged();
                }
            }
        }

        public void OnSuggestSelected(Item item)
        {
            searched = item.Text;
            RunOnUiThread(async () =>
            {
                DeviceManager.HideKeyboard(this);
                await this.GetProducts(searched);
            });           
        }

        public void OnSearch()
        {
            searched = EtSearcher.Text;
            searched = searched.Trim();
            DeviceManager.HideKeyboard(this);

            if (searched.Length > 0)
            {
                RunOnUiThread(async () =>
                {
                    await this.GetProducts(searched);
                });
            }
            else
            {
                ConvertUtilities.MessageToast(AppMessages.NumberCharactersSearchText, this);
            }
        }

        private void ClearFilters()
        {
            ParametersManager.CategoryNames = new List<string>();
            ParametersManager.BrandNames = new List<string>();
            ParametersManager.OrderBy = ConstOrder.Relevance;
            ParametersManager.OrderType = ConstOrderType.Desc;
            ParametersManager.Products = new List<Product>();
            ParametersManager.From = "0";
        }

        private void EventScan()
        {
            Intent intent = new Intent(this, typeof(MyRecipesActivity));
            StartActivity(intent);
        }

        private SearchProductsParameters SetParameters(bool nextData)
        {
            if (nextData) {
                ParametersManager.From = (int.Parse(ParametersManager.From) + int.Parse(ParametersManager.Size)).ToString();
            }

            return new SearchProductsParameters()
            {
                DependencyId = ParametersManager.UserContext.DependencyId,
                CategoryId = string.Empty,
                CategoriesNames = ParametersManager.CategoryNames.Count == 0 ? null : ParametersManager.CategoryNames,
                Brands = ParametersManager.BrandNames.Count == 0 ? null : ParametersManager.BrandNames,
                Size = ParametersManager.Size,
                From = ParametersManager.From,
                OrderBy = ParametersManager.OrderBy,
                OrderType = ParametersManager.OrderType,
                UserQuery = ParametersManager.UserQuery
            };
        }

        private async Task GetProducts(string UserQuery)
        {
            try
            {
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);
                this.ClearFilters();
                ParametersManager.UserQuery = UserQuery;
                SearchProductsParameters parameters = SetParameters(false);

                var response = await _productsModel.GetProducts(parameters);

                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    DefineShowErrorWay(AppMessages.ApplicationName, MessagesHelper.GetMessage(response.Result), AppMessages.AcceptButtonText);
                }
                else
                {

                    if (response.Products.Count >= ConvertUtilities.StringToInteger(ParametersManager.Size))
                    {
                        GoToProductsActivity(response, UserQuery);
                    }
                    else
                    {
                        if(response.TotalProductsSearch > response.Products.Count)
                        {
                            await NextGetProducts(UserQuery, response.Products);
                        }
                        else
                        {
                            GoToProductsActivity(response, UserQuery);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.ProductsFilterActivity, ConstantMethodName.GetProducts } };
                ShowAndRegisterMessageExceptions(exception, properties);
                DefineShowErrorWay(AppMessages.ApplicationName, AppMessages.UnexpectedErrorMessage, AppMessages.AcceptButtonText);
            }
            finally
            {
                HideProgressDialog();
            }
        }

        public void GoToProductsActivity(ProductsResponse response, string UserQuery)
        {
            ParametersManager.Products = response.Products;
            ParametersManager.Categories = response.Categories;
            ParametersManager.Brands = response.Brands;
            ParametersManager.ChangeProductQuantity = true;
            ParametersManager.FromProductsActivity = false;
            ParametersManager.ChangeProductQuantityFromDetail = false;

            this.RunOnUiThread(() =>
            {
                RegisterEventSearch(UserQuery, true, ParametersManager.Products);
                var intent = new Intent(this, typeof(ProductsActivity));
                intent.PutExtra(ConstantPreference.FromSearch, true);
                StartActivity(intent);
                Finish();
            });
        }

        private async Task NextGetProducts(string UserQuery, List<Product> beforeProducts)
        {
            try
            {
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);
                this.ClearFilters();

                ParametersManager.UserQuery = UserQuery;

                SearchProductsParameters parameters = SetParameters(true);

                var responseNextProducts = await _productsModel.GetProducts(parameters);

                if (responseNextProducts.Result != null && responseNextProducts.Result.HasErrors && responseNextProducts.Result.Messages != null)
                {
                    DefineShowErrorWay(AppMessages.ApplicationName, MessagesHelper.GetMessage(responseNextProducts.Result), AppMessages.AcceptButtonText);
                }
                else
                {
                   beforeProducts.AddRange(responseNextProducts.Products);
                   ParametersManager.Products = beforeProducts;
                   ParametersManager.Categories = responseNextProducts.Categories;
                   ParametersManager.Brands = responseNextProducts.Brands;
                   ParametersManager.ChangeProductQuantity = true;
                   ParametersManager.FromProductsActivity = false;
                   ParametersManager.ChangeProductQuantityFromDetail = false;

                    this.RunOnUiThread(() =>
                    {
                        RegisterEventSearch(UserQuery, true, ParametersManager.Products);
                        var intent = new Intent(this, typeof(ProductsActivity));
                        intent.PutExtra(ConstantPreference.FromSearch, true);
                        StartActivity(intent);
                        Finish();
                    });
                }
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.ProductsFilterActivity, ConstantMethodName.GetProducts } };
                ShowAndRegisterMessageExceptions(exception, properties);
                DefineShowErrorWay(AppMessages.ApplicationName, AppMessages.UnexpectedErrorMessage, AppMessages.AcceptButtonText);
            }
            finally
            {
                HideProgressDialog();
            }
        }

        private void RegisterEventSearch(string query, bool success, IList<Product> products)
        {
            FacebookRegistrationEventsService.Instance.Searched(query, success, products);
        }
    }
}