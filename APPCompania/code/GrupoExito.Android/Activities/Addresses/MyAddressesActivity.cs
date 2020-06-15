using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Text.Style;
using Android.Views;
using Android.Widget;
using Com.Bumptech.Glide;
using GrupoExito.Android.Activities.Base;
using GrupoExito.Android.Adapters;
using GrupoExito.Android.Services;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.Utilities.Constants;
using GrupoExito.DataAgent.Services.Addresses;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Entiites;
using GrupoExito.Entities.Responses.Addresses;
using GrupoExito.Logic.Models.Addresses;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrupoExito.Android.Activities.Addresses
{
    [Activity(Label = "Mis direcciones", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MyAddressesActivity : BaseAddressesActivity, IMyAddress
    {
        #region Controls

        private RecyclerView RvMyAddress;
        private MyAddressAdapter myAddressAdapter;
        private LinearLayoutManager AddressLayoutManager;
        private TextView TvTypeDefaultAddress;
        private TextView TvDefaultAddress;
        private TextView TvMessageAlertAddress;
        private ImageView IvDefaultAddress;
        private ImageView IvItemEditAddress;
        private ImageView IvItemDeleteAddress;
        private ImageView IvAddAddress;
        private LinearLayout LyItemDefaultMyAddress;
        private LinearLayout LyAddAddress;

        #endregion

        #region Properties

        private List<UserAddress> Addresses { get; set; }
        private UserAddress DefaultAddress;
        private UserAddress DataActionAddress;
        private string TypeAction;
        IList<UserAddress> ListUserAddressClean;


        #endregion

        public override void OnBackPressed()
        {
            Finish();
        }

        public void OnEditItemClicked(UserAddress userAddress)
        {
            this.EventEditAddress(userAddress);
        }

        public void OnDelateItemClicked(UserAddress userAddress)
        {
            this.EventDeleteAddress(userAddress);
        }

        public void OnSelectDefaultItemClicked(UserAddress userAddress)
        {
            DataActionAddress = userAddress;
            TypeAction = ConstantEventName.DefaultItemClicked;
            DataDialog dataDialog = new DataDialog(AppMessages.TitleGenericDialog, AppMessages.DefaultAddressChangeMessage);
            ShowGenericDialogDialog(dataDialog);
        }

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Addresses = new List<UserAddress>();
            SetContentView(Resource.Layout.ActivityMyAddresses);
            AddressModel = new AddressModel(new AddressService(DeviceManager.Instance));
            HideItemsCarToolbar(this);
            SetActionBar(GetMainToolbar(Resource.Id.mainToolbar, this));
            Window.SetBackgroundDrawable(new ColorDrawable(Color.Transparent));

            if (SupportActionBar != null)
            {
                SupportActionBar.Hide();
            }

            this.SetControlsProperties();
            HideItemsToolbar(this);
            this.EditFonts();
            await this.DrawAddresses();            
        }

        protected override void EventNoInfo()
        {
            base.EventNoInfo();
            EventAddAddress();
        }

        protected override async void EventError()
        {
            base.EventError();
            await DrawAddresses();
        }

        protected override async void OnResume()
        {
            base.OnResume();
            await this.DrawAddresses();
            FirebaseRegistrationEventsService.Instance.RegisterScreen(this, AnalyticsScreenView.MyAddresses, typeof(MyAddressesActivity).Name);
        }

        protected override async void HandlerOkButton(object sender, DialogClickEventArgs e)
        {
            await DrawAddresses();
        }

        protected async override void EventYesGenericDialog()
        {
            GenericDialog.Hide();

            if (TypeAction != null)
            {
                if (TypeAction.Equals(ConstantEventName.DefaultItemClicked))
                {
                    var adress = Addresses.Where(x => x.IsDefaultAddress = true)?.FirstOrDefault();
                    DataActionAddress.SelectedAddress = adress != null ? adress.AddressName : string.Empty;

                    var result = await SetDefaultAddress(DataActionAddress);

                    if (result)
                    {
                        ParametersManager.ChangeAddress = true;
                        ParametersManager.ChangeProductQuantity = true;
                        SaveAddress(DataActionAddress);
                        RegisterNotificationTags();
                        DeletePreferencesLastDateUpdatedPromotion();
                        OnResume();
                    }
                }
                else if (TypeAction.Equals(ConstantEventName.DeleteItemClicked))
                {
                    var result = await DeleteAddress(DataActionAddress);

                    if (result)
                    {
                        SaveAddress(DataActionAddress);
                        OnResume();
                    }
                }
            }
        }

        protected override void EventNotGenericDialog()
        {
            base.EventNotGenericDialog();
            TypeAction = null;
            GenericDialog.Hide();
        }

        public override void OnTrimMemory([GeneratedEnum] TrimMemory level)
        {
            base.OnTrimMemory(level);
            Glide.Get(this).TrimMemory((int)level);
        }

        private void SetControlsProperties()
        {
            SetToolbarControls(FindViewById<TextView>(Resource.Id.tvToolbarPrice), FindViewById<TextView>(Resource.Id.tvToolbarQuantity));
            SetLoader(FindViewById<RelativeLayout>(Resource.Id.layoutLoader));
            SetNoInfoLayout(FindViewById<RelativeLayout>(Resource.Id.layoutNoInfo), FindViewById<RelativeLayout>(Resource.Id.rlMyAccount), AppMessages.NotAddressMessage, AppMessages.AddAddressText);
            SetErrorLayout(FindViewById<RelativeLayout>(Resource.Id.layoutError),
             FindViewById<RelativeLayout>(Resource.Id.rlMyAccount));

            IvToolbarBack = FindViewById<ImageView>(Resource.Id.ivToolbarBack);
            RvMyAddress = FindViewById<RecyclerView>(Resource.Id.rvMyAddress);
            TvTypeDefaultAddress = FindViewById<TextView>(Resource.Id.tvTypeDefaultAddress);
            TvDefaultAddress = FindViewById<TextView>(Resource.Id.tvDefaultAddress);
            IvDefaultAddress = FindViewById<ImageView>(Resource.Id.ivDefaultAddress);
            IvItemEditAddress = FindViewById<ImageView>(Resource.Id.ivItemEditAddress);
            IvItemDeleteAddress = FindViewById<ImageView>(Resource.Id.ivItemDeleteAddress);
            TvMessageAlertAddress = FindViewById<TextView>(Resource.Id.tvMessageAlertAddress);
            LyItemDefaultMyAddress = FindViewById<LinearLayout>(Resource.Id.lyItemDefaultMyAddress);
            LyAddAddress = FindViewById<LinearLayout>(Resource.Id.lyAddAddress);

            IvAddAddress = FindViewById<ImageView>(Resource.Id.ivAddAddress);
            IvItemEditAddress.Click += delegate { this.EventEditAddress(DefaultAddress); };
            IvItemDeleteAddress.Click += delegate { this.EventDeleteAddress(DefaultAddress); };
            IvAddAddress.Click += delegate { this.EventAddAddress(); };
            LyAddAddress.Click += delegate { this.EventAddAddress(); };
            SpannableStringBuilder strMessageAlertAddress = new SpannableStringBuilder(GetString(Resource.String.str_message__alert_address));
            strMessageAlertAddress.SetSpan(new StyleSpan(TypefaceStyle.Bold), 0, 33, SpanTypes.ExclusiveExclusive);
            TvMessageAlertAddress.TextFormatted = strMessageAlertAddress;

            IvToolbarBack.Click += delegate
            {
                OnBackPressed();
                Finish();
            };
        }

        private void EditFonts()
        {
            FindViewById<TextView>(Resource.Id.tvTitleMyAddress).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Bold);
            FindViewById<TextView>(Resource.Id.tvMessageAddress).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvOtherAddress).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvMessageAddAddress).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            TvTypeDefaultAddress.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            TvDefaultAddress.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            TvMessageAlertAddress.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
        }

        private void InitDrawLayout()
        {
            AddressLayoutManager = new LinearLayoutManager(this)
            {
                AutoMeasureEnabled = true
            };

            RvMyAddress.NestedScrollingEnabled = false;
            RvMyAddress.HasFixedSize = true;
            RvMyAddress.SetLayoutManager(AddressLayoutManager);
            LyItemDefaultMyAddress.Visibility = ViewStates.Gone;
            Addresses = new List<UserAddress>();
        }

        private async Task DrawAddresses()
        {
            try
            {
                InitDrawLayout();
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);
                AddressResponse response = await GetAddresses();

                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    RunOnUiThread(() =>
                    {
                        HideProgressDialog();
                        DefineShowErrorWay(AppMessages.ApplicationName, MessagesHelper.GetMessage(response.Result), AppMessages.AcceptButtonText);
                    });
                }
                else
                {
                    ValidateResponseGetAddresses(response);
                }
            }
            catch (Exception exception)
            {
                HideProgressDialog();
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.MyAddressesActivity, ConstantMethodName.DrawAddresses } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
            finally
            {
                HideProgressDialog();
            }
        }

        private void ValidateResponseGetAddresses(AddressResponse response)
        {
            if (response.Addresses != null && response.Addresses.Any())
            {
                ShowBodyLayout();
                Addresses = response.Addresses.ToList();
                ListUserAddressClean = new List<UserAddress>();
                ListUserAddressClean.Clear();

                for (int i = 0; i < Addresses.Count; i++)
                {
                    if (Addresses[i].IsDefaultAddress)
                    {
                        this.AddressDefault(Addresses[i]);
                    }
                    else
                    {
                        ListUserAddressClean.Add(Addresses[i]);
                    }
                }

                myAddressAdapter = new MyAddressAdapter(ListUserAddressClean, this, this);
                RvMyAddress.SetAdapter(myAddressAdapter);
                ShowBodyLayout();
            }
            else
            {
                ShowNoInfoLayout();
            }
        }

        private void AddressDefault(UserAddress userAddress)
        {
            DefaultAddress = userAddress;
            TvTypeDefaultAddress.Text = userAddress.Description;
            TvDefaultAddress.Text = userAddress.AddressComplete;
            IvItemDeleteAddress.Visibility = ViewStates.Gone;

            if (ConvertUtilities.ResourceId(userAddress.Description, "primario") != 0)
            {
                IvDefaultAddress.SetImageResource(ConvertUtilities.ResourceId(userAddress.Description, "primario"));
            }
            else
            {
                IvDefaultAddress.SetImageResource(ConvertUtilities.ResourceId("otro", "primario"));
            }

            LyItemDefaultMyAddress.Visibility = ViewStates.Visible;
        }

        private void EventDeleteAddress(UserAddress userAddress)
        {
            DataActionAddress = userAddress;
            TypeAction = ConstantEventName.DeleteItemClicked;
            DataDialog dataDialog = new DataDialog(AppMessages.TitleGenericDialog, AppMessages.DeleteAddressChangeMessage);
            ShowGenericDialogDialog(dataDialog);
        }

        private void EventEditAddress(UserAddress userAddress)
        {
            Intent intent = new Intent(this, typeof(AddressActivity));

            if (userAddress != null)
            {
                intent.PutExtra(ConstantPreference.Address, JsonService.Serialize<UserAddress>(userAddress));
                intent.PutExtra(ConstantPreference.PreviousActivity, ConstantActivityName.MyAddressActivity);
            }

            StartActivity(intent);
        }

        private void EventAddAddress()
        {
            Intent intent = new Intent(this, typeof(AddressActivity));
            intent.PutExtra(ConstantPreference.PreviousActivity, ConstantActivityName.MyAddressActivity);
            StartActivity(intent);
        }
    }
}