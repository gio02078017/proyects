using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Widget;
using Com.Bumptech.Glide;
using GrupoExito.Android.Activities.Base;
using GrupoExito.Android.Adapters;
using GrupoExito.Android.Services;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.Utilities.Constants;
using GrupoExito.DataAgent.Services.Payments;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Entiites;
using GrupoExito.Entities.Responses.Payments;
using GrupoExito.Logic.Models.Paymentes;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrupoExito.Android.Activities.Payments
{
    [Activity(Label = "Mis tarjetas", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MyCardsActivity : BaseCreditCardActivity, IMyCards
    {
        #region Controls

        private RecyclerView RvMyCards;
        private MyCardsAdapter MyCardsAdapter;
        private LinearLayoutManager linerLayoutManager;

        #endregion

        #region Properties

        private bool IsDeleteCard { get; set; }
        private CreditCard SelectedCard { get; set; }
        private IList<CreditCard> Cards { get; set; }

        private CreditCardResponse _CreditCardResponse;

        #endregion

        public void OnDeleteMyCardsClicked(CreditCard card)
        {
            SelectedCard = card;
            IsDeleteCard = true;
            DataDialog dataDialog = new DataDialog(AppMessages.TitleGenericDialog, AppMessages.QuestionDeleteCreditCardMessage);
            ShowGenericDialogDialog(dataDialog);
        }

        protected async override void OnResume()
        {
            base.OnResume();
            await GetCards();
            FirebaseRegistrationEventsService.Instance.RegisterScreen(this, AnalyticsScreenView.MyCards, typeof(MyCardsActivity).Name);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            CreditCardModel = new CreditCardModel(new CreditCardService(DeviceManager.Instance));
            SetContentView(Resource.Layout.ActivityMyCards);
            HideItemsToolbar(this);
            SetActionBar(GetMainToolbar(Resource.Id.mainToolbar, this));

            if (SupportActionBar != null)
            {
                SupportActionBar.Hide();
            }

            this.SetControlsProperties();
            this.EditFonts();            
        }

        protected override void EventError()
        {
            base.EventError();
            OnResume();
        }

        protected override void EventNoInfo()
        {
            base.EventNoInfo();
            AddCreditCard();
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
            SetNoInfoLayout(FindViewById<RelativeLayout>(Resource.Id.layoutNoInfo), FindViewById<NestedScrollView>(Resource.Id.nsvMyCards), AppMessages.NotCardMessage, AppMessages.AddCreditCard);
            SetErrorLayout(FindViewById<RelativeLayout>(Resource.Id.layoutError),
                FindViewById<NestedScrollView>(Resource.Id.nsvMyCards));

            FindViewById<LinearLayout>(Resource.Id.lyAddCreditCard).Click += delegate { AddCreditCard(); };            
            IvToolbarBack = FindViewById<ImageView>(Resource.Id.ivToolbarBack);
            RvMyCards = FindViewById<RecyclerView>(Resource.Id.rvMyCards);

            IvToolbarBack.Click += delegate
            {
                OnBackPressed();
                Finish();
            };
        }

        private void AddCreditCard()
        {
            Intent intent = new Intent(this, typeof(AddCreditCardActivity));
            StartActivity(intent);
        }

        private void AddCreditCardPaymentez()
        {
            Intent intent = new Intent(this, typeof(AddCreditCardPaymentezActivity));
            StartActivity(intent);
        }

        private void EditFonts()
        {
            FindViewById<TextView>(Resource.Id.tvTitleMyCards).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvMessageMyCards).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvOtherCards).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);            
        }

        private async Task GetCards()
        {
            try
            {
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);
                _CreditCardResponse = await CreditCardModel.GetAllCreditCards();

                if (_CreditCardResponse.Result != null && _CreditCardResponse.Result.HasErrors && _CreditCardResponse.Result.Messages != null)
                {
                    RunOnUiThread(() =>
                    {
                        DefineShowErrorWay(AppMessages.ApplicationName, MessagesHelper.GetMessage(_CreditCardResponse.Result), AppMessages.AcceptButtonText);
                    });
                }
                else
                {
                    Cards = _CreditCardResponse.CreditCards;
                    this.DrawMyCards();
                }
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.MyCardsActivity, ConstantMethodName.GetCards } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
            finally
            {
                HideProgressDialog();
            }
        }

        private void DrawMyCards()
        {

            if (Cards != null)
            {
                if (Cards.Any())
                {
                    ShowBodyLayout();
                    linerLayoutManager = new LinearLayoutManager(this)
                    {
                        AutoMeasureEnabled = true
                    };
                    RvMyCards.NestedScrollingEnabled = false;
                    RvMyCards.HasFixedSize = true;
                    RvMyCards.SetLayoutManager(linerLayoutManager);
                    MyCardsAdapter = new MyCardsAdapter(Cards, this, this);
                    RvMyCards.SetAdapter(MyCardsAdapter);
                }
                else
                {
                    ShowNoInfoLayout(true);
                }
            }
        }

        protected async override void EventYesGenericDialog()
        {
            GenericDialog.Hide();

            if (IsDeleteCard)
            {
                IsDeleteCard = false;

                var result = !SelectedCard.Paymentez ? await DeleteCreditCard(SelectedCard): await DeleteCreditCardPaymentez(SelectedCard);                           

                if (result)
                {
                    Cards.Remove(SelectedCard);
                    MyCardsAdapter.NotifyDataSetChanged();
                    if(!Cards.Any())
                    {
                        ShowNoInfoLayout(true);
                    }
                }
            }
        }

        public void OnCardSelected(CreditCard myCard)
        {
        }
    }
}