using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using GrupoExito.Android.Activities.Base;
using GrupoExito.Android.Services;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.Utilities.Constants;
using GrupoExito.DataAgent.Services.Users;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Logic.Models.Users;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GrupoExito.Android.Activities.Users
{
    [Activity(Label = "Mis Puntos", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MyPointsActivity : BaseActivity
    {
        #region Controls

        private TextView TvAccumulatedQuantities;
        private TextView  TvAccumulatedDate;
        private TextView TvToBeOvercomeQuantities;
        private TextView  TvToBeOvercomeDate;
        private LinearLayout LyAccumulated;
        private LinearLayout  LyToBeOvercome;

        #endregion

        #region Properties

        private PointsModel _pointsModel { get; set; }

        #endregion

        public override void OnBackPressed()
        {
            Finish();
        }

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ActivityMyPoints);
            _pointsModel = new PointsModel(new PointsService(DeviceManager.Instance));
            HideItemsCarToolbar(this);
            SetActionBar(GetMainToolbar(Resource.Id.mainToolbar, this));

            if (SupportActionBar != null)
            {
                SupportActionBar.Hide();
            }

            this.SetControlsProperties();
            HideItemsToolbar(this);
            this.EditFonts();
            await this.DrawPoints();            
        }

        protected override void EventNoInfo()
        {
            base.EventNoInfo();
        }

        protected override async void EventError()
        {
            base.EventError();
            await DrawPoints();
        }

        protected override async void OnResume()
        {
            base.OnResume();
            await this.DrawPoints();
            FirebaseRegistrationEventsService.Instance.RegisterScreen(this, AnalyticsScreenView.MyPoints, typeof(MyPointsActivity).Name);
        }


        private void SetControlsProperties()
        {
            SetToolbarControls(FindViewById<TextView>(Resource.Id.tvToolbarPrice), FindViewById<TextView>(Resource.Id.tvToolbarQuantity));
            SetLoader(FindViewById<RelativeLayout>(Resource.Id.layoutLoader));
            SetNoInfoLayout(FindViewById<RelativeLayout>(Resource.Id.layoutNoInfo), FindViewById<LinearLayout>(Resource.Id.lyBody), AppMessages.NotAddressMessage, AppMessages.AddAddressText);
            SetErrorLayout(FindViewById<RelativeLayout>(Resource.Id.layoutError),
             FindViewById<LinearLayout>(Resource.Id.lyBody));
            IvToolbarBack = FindViewById<ImageView>(Resource.Id.ivToolbarBack);
            LyAccumulated = FindViewById<LinearLayout>(Resource.Id.lyAccumulated);
            LyToBeOvercome = FindViewById<LinearLayout>(Resource.Id.lyToBeOvercome);
            TvAccumulatedQuantities = FindViewById<TextView>(Resource.Id.tvAccumulatedQuantities);
            TvAccumulatedDate = FindViewById<TextView>(Resource.Id.tvAccumulatedDate);
            TvToBeOvercomeQuantities = FindViewById<TextView>(Resource.Id.tvToBeOvercomeQuantities);
            TvToBeOvercomeDate = FindViewById<TextView>(Resource.Id.tvToBeOvercomeDate);

            IvToolbarBack.Click += delegate
            {
                OnBackPressed();
                Finish();
            };
        }

        private void EditFonts()
        {
            FindViewById<TextView>(Resource.Id.tvTitleMyPoints).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Bold);
            FindViewById<TextView>(Resource.Id.tvMessageMyPoints).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvTitleAccumulated).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvTitleToBeOvercome).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvAccumulatedQuantities.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvAccumulatedDate.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvToBeOvercomeQuantities.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvToBeOvercomeDate.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
        }

        private async Task DrawPoints()
        {
            try
            {
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);
                var response = await _pointsModel.GetUserPoints();

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
                    if (response != null)
                    {
                        ShowBodyLayout();

                        if(response.AvailablePoints > 999999)
                        {
                            TvAccumulatedQuantities.SetTextSize(ComplexUnitType.Sp,27);
                            TvToBeOvercomeQuantities.SetTextSize(ComplexUnitType.Sp,27);
                        }

                        TvAccumulatedQuantities.Text = response.AvailablePoints > 0 ? StringFormat.NumberFormatForThousand(response.AvailablePoints) : response.AvailablePoints.ToString();
                        TvAccumulatedDate.Text = response.AcumulatedDate != null ? StringFormat.DateWithPrefix(response.AcumulatedDate, "Al") : "";
                        LyToBeOvercome.Visibility = response.ExpirationDate != null ? ViewStates.Visible : ViewStates.Gone;
                        TvToBeOvercomeQuantities.Text = response.ExpirationPoints > 0 ? StringFormat.NumberFormatForThousand(response.ExpirationPoints):response.ExpirationPoints.ToString();
                        TvToBeOvercomeDate.Text = response.ExpirationDate != null ? StringFormat.DateWithPrefix(response.ExpirationDate,"El"):"";
                        
                    }
                    else
                    {
                        ShowNoInfoLayout();
                    }
                }
            }
            catch (Exception exception)
            {
                HideProgressDialog();
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.MyPointsActivity, ConstantMethodName.DrawPoints } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
            finally
            {
                HideProgressDialog();
            }
        }
    }
}