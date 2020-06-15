using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Widget;
using GrupoExito.Android.Activities.Base;
using GrupoExito.Android.Adapters;
using GrupoExito.Android.Services;
using GrupoExito.Android.Utilities;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Entiites;
using GrupoExito.Logic.Models.Contacts;
using GrupoExito.Utilities.Resources;
using System.Collections.Generic;

namespace GrupoExito.Android.Activities.Generic
{
    [Activity(Label = "Contáctanos", ScreenOrientation = ScreenOrientation.Portrait)]
    public class ContactUsActivity : BaseActivity
    {
        #region Controls

        private RecyclerView RvContactUs;
        private ContactUsAdapter contactUsAdapter;
        private LinearLayoutManager linerLayoutManager;
        private TextView TvAttentionDays;
        private TextView TvAttentionRank;

        #endregion

        #region Properties

        private ContactsModel ContactsModel;

        #endregion

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ActivityContactUs);
            SetActionBar(GetMainToolbar(Resource.Id.mainToolbar, this));
            ContactsModel = new ContactsModel();
            HideItemsToolbar(this);

            if (SupportActionBar != null)
            {
                SupportActionBar.Hide();
            }

            this.SetControlsProperties();
            this.EditFonts();            
        }

        protected override void OnResume()
        {
            base.OnResume();
            FirebaseRegistrationEventsService.Instance.RegisterScreen(this, AnalyticsScreenView.ContactUs, typeof(ContactUsActivity).Name);
        }

        private void SetControlsProperties()
        {
            SetToolbarControls(FindViewById<TextView>(Resource.Id.tvToolbarPrice), FindViewById<TextView>(Resource.Id.tvToolbarQuantity));
            SetLoader(FindViewById<RelativeLayout>(Resource.Id.layoutLoader));
            IvToolbarBack = FindViewById<ImageView>(Resource.Id.ivToolbarBack);
            RvContactUs = FindViewById<RecyclerView>(Resource.Id.rvContactUs);
            TvAttentionDays = FindViewById<TextView>(Resource.Id.tvAttentionDays);
            TvAttentionRank = FindViewById<TextView>(Resource.Id.tvAttentionRank);

            IvToolbarBack.Click += delegate
            {
                OnBackPressed();
                Finish();
            };

            this.DrawAttentionSchedule();
            this.DrawContact();
        }

        private void DrawContact()
        {

            List<Contact> listContact = ContactsModel.GetContacts();
            linerLayoutManager = new LinearLayoutManager(this)
            {
                AutoMeasureEnabled = true
            };
            RvContactUs.NestedScrollingEnabled = false;
            RvContactUs.HasFixedSize = true;
            RvContactUs.SetLayoutManager(linerLayoutManager);
            contactUsAdapter = new ContactUsAdapter(listContact, this);
            RvContactUs.SetAdapter(contactUsAdapter);
        }

        private void DrawAttentionSchedule()
        {
            string attentionSchedule = AppConfigurations.AttentionSchedule;
            attentionSchedule = attentionSchedule.Replace("de", "-").TrimStart();
            string[] splitAttentionSchedule = attentionSchedule.Split('-');
            TvAttentionDays.Text = splitAttentionSchedule[0];
            TvAttentionRank.Text = "de " + splitAttentionSchedule[1];
        }

        private void EditFonts()
        {
            FindViewById<TextView>(Resource.Id.tvTitleContactUs).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);            
            FindViewById<TextView>(Resource.Id.tvTitleAttentionTime).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);            
            TvAttentionDays.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);            
            TvAttentionRank.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);            
        }
    }
}