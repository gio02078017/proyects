using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Com.Bumptech.Glide;
using GrupoExito.Android.Activities.Base;
using GrupoExito.Android.Adapters;
using GrupoExito.Android.Interfaces;
using GrupoExito.Android.Services;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.Utilities.Constants;
using GrupoExito.DataAgent.Services.Products;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Entiites;
using GrupoExito.Entities.Entiites.Products;
using GrupoExito.Entities.Responses.Base;
using GrupoExito.Entities.Responses.Products;
using GrupoExito.Logic.Models.Products;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GrupoExito.Android.Activities.Products
{
    [Activity(Label = "Crear Nuevas Listas", ScreenOrientation = ScreenOrientation.Portrait)]
    public class CreateNewListActivity : BaseActivity, ISelectedList
    {
        #region Controls
        private LinearLayoutManager linerLayoutManager;
        private RecyclerView RvMyLists;
        private SelectedListAdapter _SelectedListAdapter;
        private TextView TvAddProducts;
        private LinearLayout LyCreateList, LyAddToList, LyMainAddToList;
        private EditText EtNameList;

        #endregion

        #region Properties

        private IList<TypeList> ListTypeLists { get; set; }
        private int CurrentPosition { get; set; }

        private string TypeListSelected { get; set; }

        private ResponseBase _ResponseBase;
        private ShoppingListsResponse _ShoppingListsResponse;
        private ShoppingListModel ShoppingListModel { get; set; }

        private ShoppingList SelectedShoppingList;

        private ProductList ProductSelected { get; set; }

        #endregion

        #region Public Methods

        public void OnItemSelected(ShoppingList _ShoppingList)
        {
            SelectedShoppingList = _ShoppingList;
            LyAddToList.Enabled = true;
            LyAddToList.Clickable = true;
            LyAddToList.SetBackgroundResource(Resource.Drawable.button_yellow);
        }

        public override void OnTrimMemory([GeneratedEnum] TrimMemory level)
        {
            base.OnTrimMemory(level);
            Glide.Get(this).TrimMemory((int)level);
        }

        #endregion

        #region Protected Methods

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ActivityCreateNewList);
            SetActionBar(GetMainToolbar(Resource.Id.mainToolbar, this));
            HideItemsCarToolbar(this);
            ShoppingListModel = new ShoppingListModel(new ShoppingListService(DeviceManager.Instance));
            if (SupportActionBar != null)
            {
                SupportActionBar.Hide();
            }

            this.SetControlsProperties();
            this.EditFonts();


            Bundle bundle = Intent.Extras;

            if (bundle != null)
            {
                if (!string.IsNullOrEmpty(Intent.Extras.GetString(ConstantPreference.Product)))
                {
                    ProductSelected = JsonService.Deserialize<ProductList>(Intent.Extras.GetString(ConstantPreference.Product));
                    LyMainAddToList.Visibility = ViewStates.Visible;
                    await this.ShowLists();
                }
            }

            EtNameList.ClearFocus();
            LyCreateList.RequestFocus();            
        }

        protected override void OnResume()
        {
            base.OnResume();
            FirebaseRegistrationEventsService.Instance.RegisterScreen(this, AnalyticsScreenView.CreateNewList, typeof(CreateNewListActivity).Name);
        }

        protected async override void EventError()
        {
            base.EventError();
        }

        protected override void EventNoInfo()
        {
            base.EventNoInfo();
            OnBackPressed();
        }

        #endregion

        #region Private Methods

        private void EditFonts()
        {
            FindViewById<TextView>(Resource.Id.tvTitleNewList).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvMessageNewList).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvNameList).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvNameList).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            EtNameList.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvCreateList).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvAddProducts).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvYourLists).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvAddToList).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvSuggestNameList).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
        }

        private void SetControlsProperties()
        {
            SetToolbarControls(FindViewById<TextView>(Resource.Id.tvToolbarPrice), FindViewById<TextView>(Resource.Id.tvToolbarQuantity));
            SetLoader(FindViewById<RelativeLayout>(Resource.Id.layoutLoader));
            SetSearcher(FindViewById<LinearLayout>(Resource.Id.searcher));
            SetErrorLayout(FindViewById<RelativeLayout>(Resource.Id.layoutError),
               FindViewById<RelativeLayout>(Resource.Id.rlBody));
            SetNoInfoLayout(FindViewById<RelativeLayout>(Resource.Id.layoutNoInfo),
                 FindViewById<RelativeLayout>(Resource.Id.rlBody),
                 AppMessages.NotRecommendList, AppMessages.GenericBackAction);
            IvToolbarBack = FindViewById<ImageView>(Resource.Id.ivToolbarBack);
            TvAddProducts = FindViewById<TextView>(Resource.Id.tvAddProducts);

            EtNameList = FindViewById<EditText>(Resource.Id.etNameList);
            LyCreateList = FindViewById<LinearLayout>(Resource.Id.lyCreateList);
            LyAddToList = FindViewById<LinearLayout>(Resource.Id.lyAddToList);
            LyMainAddToList = FindViewById<LinearLayout>(Resource.Id.lyMainAddToList);
            RvMyLists = FindViewById<RecyclerView>(Resource.Id.rvMyLists);

            IvToolbarBack.Click += delegate
            {
                OnBackPressed();
                Finish();
            };

            LyCreateList.Click += async delegate
            {
                await CreateList();
            };

            LyAddToList.Click += async delegate
            {
                await AddToList();
            };

            EtNameList.SetOnTouchListener(new MyTouchListener());
        }

        private async Task CreateList()
        {
            try
            {
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);

                ShoppingList shoppingList = new ShoppingList
                {
                    Name = EtNameList.Text.Trim()
                };

                String message = ShoppingListModel.ValidateFields(shoppingList);

                if (string.IsNullOrEmpty(message))
                {
                    _ShoppingListsResponse = await ShoppingListModel.AddShoppingList(shoppingList);

                    if (_ShoppingListsResponse.Result != null && _ShoppingListsResponse.Result.HasErrors && _ShoppingListsResponse.Result.Messages != null)
                    {
                        HideProgressDialog();
                        DefineShowErrorWay(AppMessages.ApplicationName, MessagesHelper.GetMessage(_ShoppingListsResponse.Result), AppMessages.AcceptButtonText);
                    }
                    else
                    {
                        if (ProductSelected != null)
                        {
                            SelectedShoppingList = new ShoppingList
                            {
                                Id = _ShoppingListsResponse.Id
                            };
                            LyCreateList.Visibility = ViewStates.Gone;
                            await AddToList();
                            RegisterNewList();
                        }
                        else
                        {
                            ConvertUtilities.MessageToast(AppMessages.MessageCreateList + " " + EtNameList.Text, this, true);
                            Intent intent = new Intent(this, typeof(TutorialListActivity));
                            StartActivity(intent);
                            Finish();
                        }
                    }
                }
                else
                {
                    if (message.Equals(AppMessages.RequiredFieldName))
                    {
                        this.ModifyFieldsStyle(shoppingList);
                    }

                    HideProgressDialog();
                    DefineShowErrorWay(AppMessages.ApplicationName, message, AppMessages.AcceptButtonText, true);
                }
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.CreateNewListActivity, ConstantMethodName.CreateList } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
            finally
            {
                HideProgressDialog();
            }
        }

        private async Task ShowLists()
        {
            try
            {
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);

                _ShoppingListsResponse = await ShoppingListModel.GetShoppingLists();

                if (_ShoppingListsResponse.Result != null && _ShoppingListsResponse.Result.HasErrors && _ShoppingListsResponse.Result.Messages != null)
                {
                    HideProgressDialog();
                    DefineShowErrorWay(AppMessages.ApplicationName, MessagesHelper.GetMessage(_ShoppingListsResponse.Result), AppMessages.AcceptButtonText);
                }
                else
                {
                    DrawList(_ShoppingListsResponse.ShpoppingLists);
                }
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.CreateNewListActivity, ConstantMethodName.ShowLists } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
            finally
            {
                HideProgressDialog();
            }
        }

        private void DrawList(IList<ShoppingList> listShoppingList)
        {
            linerLayoutManager = new LinearLayoutManager(this, LinearLayoutManager.Vertical, false)
            {
                AutoMeasureEnabled = true
            };

            RvMyLists.NestedScrollingEnabled = false;
            RvMyLists.HasFixedSize = true;
            RvMyLists.SetLayoutManager(linerLayoutManager);

            _SelectedListAdapter = new SelectedListAdapter(listShoppingList, this, this);
            RvMyLists.SetAdapter(_SelectedListAdapter);
        }

        private async Task AddToList()
        {
            if (ProductSelected != null && SelectedShoppingList != null)
            {
                try
                {
                    ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);
                    RegisterEventAddedToWishList(ProductSelected);
                    ProductSelected.ShoppingListId = SelectedShoppingList.Id;
                    _ResponseBase = await ShoppingListModel.AddProductShoppingList(ProductSelected);

                    if (_ResponseBase.Result != null && _ResponseBase.Result.HasErrors && _ResponseBase.Result.Messages != null)
                    {
                        HideProgressDialog();
                        DefineShowErrorWay(AppMessages.ApplicationName, MessagesHelper.GetMessage(_ResponseBase.Result), AppMessages.AcceptButtonText);
                    }
                    else
                    {
                        HideProgressDialog();
                        ConvertUtilities.MessageToast(AppMessages.MessageAddProductToList, this, true);
                        OnBackPressed();
                        Finish();
                    }
                }
                catch (Exception exception)
                {
                    Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.CreateNewListActivity, ConstantMethodName.AddToList } };
                    ShowAndRegisterMessageExceptions(exception, properties);
                }
                finally
                {
                    HideProgressDialog();
                }
            }
        }

        private void ModifyFieldsStyle(ShoppingList shoppingList)
        {
            if (string.IsNullOrEmpty(shoppingList.Name))
            {
                EtNameList.SetBackgroundResource(Resource.Drawable.field_error);
            }
            else
            {
                EtNameList.SetBackgroundResource(Resource.Drawable.button_products);
            }
        }

        private void RegisterEventAddedToWishList(ProductList productList)
        {
            Product product = new Product()
            {
                Id = productList.Id,
                Name = productList.Name,
                Brand = productList.Brand,
                CategoryName = productList.CategoryName,
                Price = productList.Price
            };

            FacebookRegistrationEventsService.Instance.AddToWishlist(product);
        }

        public class MyTouchListener : Java.Lang.Object, View.IOnTouchListener
        {
            public bool OnTouch(View v, MotionEvent e)
            {
                v.FocusableInTouchMode = true;
                v.Focusable = true;
                return false;
            }
        }

        public void RegisterNewList()
        {
            FirebaseRegistrationEventsService.Instance.RegisterScreen(this, AnalyticsScreenView.NewList, typeof(CreateNewListActivity).Name);
        }

        #endregion
    }
}