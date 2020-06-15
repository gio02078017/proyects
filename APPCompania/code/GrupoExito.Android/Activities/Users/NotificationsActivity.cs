using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Widget;
using Com.Bumptech.Glide;
using GrupoExito.Android.Activities.Base;
using GrupoExito.Android.Adapters;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.Utilities.Constants;
using GrupoExito.DataAgent.Services.Users;
using GrupoExito.Entities;
using GrupoExito.Logic.Models.Users;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;

namespace GrupoExito.Android.Activities.Users
{
    [Activity(Label = "Notificaciones", ScreenOrientation = ScreenOrientation.Portrait)]
    public class NotificationsActivity : BaseActivity, INotifications
    {
        #region Controls

        private RecyclerView RvNotifications;
        private NotificationsAdapter _notificationsAdapter;

        #endregion

        #region Properties

        private List<AppNotification> Notifications { get; set; }
        private NotificationsModel _notificationsModel;

        #endregion

        public void OnViewItemClicked(AppNotification notification)
        {
        }

        public void OnDelateItemClicked(AppNotification notification)
        {
        }

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ActivityNotifications);
            SetActionBar(GetMainToolbar(Resource.Id.mainToolbar, this));
            HideItemsToolbar(this);

            _notificationsModel = new NotificationsModel(new NotificationsService(DeviceManager.Instance));

            if (SupportActionBar != null)
            {
                SupportActionBar.Hide();
            }

            SetControlsProperties();
            EditFonts();
            await GetNotifications();
        }

        protected override void OnResume()
        {
            base.OnResume();
        }

        protected override async void EventError()
        {
            base.EventError();
            await this.GetNotifications();
        }

        protected override void EventNoInfo()
        {
            base.EventNoInfo();
            OnBackPressed();
        }

        public override void OnTrimMemory([GeneratedEnum] TrimMemory level)
        {
            base.OnTrimMemory(level);
            Glide.Get(this).TrimMemory((int)level);
        }

        private async Task GetNotifications()
        {
            try
            {
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);

                var notificationsResponse = await _notificationsModel.GetNotifications();

                if (notificationsResponse.Result != null && notificationsResponse.Result.HasErrors && notificationsResponse.Result.Messages != null)
                {
                    RunOnUiThread(() =>
                    {
                        ShowErrorLayout(MessagesHelper.GetMessage(notificationsResponse.Result));
                    });
                }
                else
                {
                    if (notificationsResponse.Notifications.Any())
                    {
                        DrawNotifications(notificationsResponse.Notifications);
                        ShowBodyLayout();
                    }
                    else
                    {
                        ShowNoInfoLayout();
                    }
                }
            }
            catch (Exception exception)
            {
                ShowErrorLayout(AppMessages.UnexpectedErrorMessage);
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.NotificationsActivity, ConstantMethodName.GetNotifications } };

                RegisterMessageExceptions(exception, properties);
            }
            finally
            {
                HideProgressDialog();
            }
        }

        private void EditFonts()
        {
            FindViewById<TextView>(Resource.Id.tvNotification).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
        }

        private void SetControlsProperties()
        {
            SetToolbarControls(FindViewById<TextView>(Resource.Id.tvToolbarPrice), FindViewById<TextView>(Resource.Id.tvToolbarQuantity));
            SetLoader(FindViewById<RelativeLayout>(Resource.Id.layoutLoader));
            SetNoInfoLayout(FindViewById<RelativeLayout>(Resource.Id.layoutNoInfo),
                     FindViewById<RelativeLayout>(Resource.Id.rlNotifications),
                     AppMessages.NoNotificationsMessage, AppMessages.NotificationsAction);
            SetErrorLayout(FindViewById<RelativeLayout>(Resource.Id.layoutError),
                FindViewById<RelativeLayout>(Resource.Id.rlNotifications));
            IvToolbarBack = FindViewById<ImageView>(Resource.Id.ivToolbarBack);
            RvNotifications = FindViewById<RecyclerView>(Resource.Id.rvNotifications);

            IvToolbarBack.Click += delegate
            {
                OnBackPressed();
                Finish();
            };
        }

        private void DrawNotifications(IList<AppNotification> listNotifications)
        {
            LinearLayoutManager linerLayoutManager = new LinearLayoutManager(this, LinearLayoutManager.Vertical, false)
            {
                AutoMeasureEnabled = true
            };
            RvNotifications.NestedScrollingEnabled = false;
            RvNotifications.HasFixedSize = true;
            RvNotifications.SetLayoutManager(linerLayoutManager);
            _notificationsAdapter = new NotificationsAdapter(listNotifications, this, this);
            RvNotifications.SetAdapter(_notificationsAdapter);
        }
    }
}