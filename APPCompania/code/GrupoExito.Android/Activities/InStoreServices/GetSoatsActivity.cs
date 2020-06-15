using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Views;
using Android.Widget;
using Com.Bumptech.Glide;
using GrupoExito.Android.Activities.Base;
using GrupoExito.Android.Adapters;
using GrupoExito.Android.Interfaces;
using GrupoExito.Android.Services;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.Utilities.Constants;
using GrupoExito.Android.Utilities.Listeners;
using GrupoExito.DataAgent.DataBase;
using GrupoExito.DataAgent.Services.InStoreServices;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Entiites;
using GrupoExito.Entities.Responses.InStoreServices;
using GrupoExito.Logic.Models.Generic;
using GrupoExito.Logic.Models.InStoreServices;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrupoExito.Android.Activities.InStoreServices
{
    [Activity(Label = "Consultar Soats", ScreenOrientation = ScreenOrientation.Portrait)]
    public class GetSoatsActivity : BaseActivity, IGetSoats
    {
        #region Controls

        private RecyclerView RvGetSoats;
        private GetSoatAdapter _GetSoatAdapter;
        private LinearLayoutManager linerLayoutManager;
        private TextView TvConsultSoat;

        private AlertDialog SearchSoatDialog;
        private View ViewSearchSoat;
        private EditText EtDocumentNumber, EtLicensePlate;
        private Button BtnCancel, BtnSearchSoat, BtnReturn;
        private TextView TvCannotFinishChange;
        private ImageView ImgCloseOne, ImgCloseTwo;
        private Spinner SpDocumentType;
        private LinearLayout LySearch, LyError;
        private LinearLayout LyBody;

        #endregion

        #region Properties

        private IList<DocumentType> DocumentTypes;
        private DocumentsDataBaseModel _DocumentsDataBaseModel { get; set; }
        private IList<Soat> ListSoat { get; set; }
        private bool IsDelete { get; set; }
        private Soat SelectedSoat;

        #endregion


        public void OnItemSelected(Soat soat, int position)
        {
            ShowSoat(soat);
        }

        public void OnItemDeleted(Soat soat, int position)
        {
            SelectedSoat = soat;
            IsDelete = true;
            DataDialog dataDialog = new DataDialog(AppMessages.TitleGenericDialog, AppMessages.QuestionDeleteSoatMessage);
            ShowGenericDialogDialog(dataDialog);
        }

        protected override void OnResume()
        {
            base.OnResume();
            RunOnUiThread(async () =>
            {
                await GetSoats();
            });           
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            _DocumentsDataBaseModel = new DocumentsDataBaseModel(DocumentsDataBase.Instance);
            SetContentView(Resource.Layout.ActivityGetSoats);
            HideItemsCarToolbar(this);
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
            SetNoInfoLayout(FindViewById<RelativeLayout>(Resource.Id.layoutNoInfo), FindViewById<NestedScrollView>(Resource.Id.nsvGetSoats), AppMessages.NotSoatMessage, AppMessages.AddSoat);
            SetErrorLayout(FindViewById<RelativeLayout>(Resource.Id.layoutError),
                FindViewById<NestedScrollView>(Resource.Id.nsvGetSoats));

            IvToolbarBack = FindViewById<ImageView>(Resource.Id.ivToolbarBack);
            RvGetSoats = FindViewById<RecyclerView>(Resource.Id.rvGetSoats);
            TvConsultSoat = FindViewById<TextView>(Resource.Id.tvConsultSoat);

            RlError = FindViewById<RelativeLayout>(Resource.Id.layoutError);
            LyBody = FindViewById<LinearLayout>(Resource.Id.lyBody);
            TvError = RlError.FindViewById<TextView>(Resource.Id.tvError);
            BtnError = RlError.FindViewById<Button>(Resource.Id.btnError);
            IvError = RlError.FindViewById<ImageView>(Resource.Id.ivError);

            TvConsultSoat.Click += async delegate
            {
                await ShowSoatDialog();
            };

            IvToolbarBack.Click += delegate
            {
                OnBackPressed();
                Finish();
            };
        }

        private void EditFonts()
        {
            FindViewById<TextView>(Resource.Id.tvTitleGetSoats).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvConsultSoat).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Bold);

        }

        private async Task GetSoats()
        {
            try
            {
                ListSoat = _DocumentsDataBaseModel.GetSoats();

                if (ListSoat != null && ListSoat.Any())
                {
                    this.DrawSoats();
                }
                else
                {
                    ShowNoInfoLayout(true);
                    await ShowSoatDialog();
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

        private void DrawSoats()
        {

            if (ListSoat != null && ListSoat.Any())
            {
                ShowBodyLayout();
                linerLayoutManager = new LinearLayoutManager(this)
                {
                    AutoMeasureEnabled = true
                };
                RvGetSoats.NestedScrollingEnabled = false;
                RvGetSoats.HasFixedSize = true;
                RvGetSoats.SetLayoutManager(linerLayoutManager);
                _GetSoatAdapter = new GetSoatAdapter(ListSoat, this, this);
                RvGetSoats.SetAdapter(_GetSoatAdapter);
            }
            else
            {
                ShowNoInfoLayout(true);
            }
        }

        protected override void EventYesGenericDialog()
        {
            GenericDialog.Hide();

            if (IsDelete)
            {
                IsDelete = false;

                _DocumentsDataBaseModel.DeleteSoat(SelectedSoat.Plate);

                ListSoat.Remove(SelectedSoat);
                _GetSoatAdapter.NotifyDataSetChanged();
                if (!ListSoat.Any())
                {
                    ShowNoInfoLayout(true);
                }
            }
        }

        private async Task ShowSoatDialog()
        {
            ModalSoatScreenView();
            await GetDocumentsType();
            SearchSoatDialog = new AlertDialog.Builder(this).Create();
            ViewSearchSoat = LayoutInflater.Inflate(Resource.Layout.DialogSearchSoat, null);
            SearchSoatDialog.SetView(ViewSearchSoat);
            SearchSoatDialog.SetCanceledOnTouchOutside(false);
            ImgCloseOne = ViewSearchSoat.FindViewById<ImageView>(Resource.Id.imgCloseOne);
            ImgCloseTwo = ViewSearchSoat.FindViewById<ImageView>(Resource.Id.imgCloseTwo);
            EtDocumentNumber = ViewSearchSoat.FindViewById<EditText>(Resource.Id.etDocumentNumber);
            EtLicensePlate = ViewSearchSoat.FindViewById<EditText>(Resource.Id.etLicensePlate);
            BtnCancel = ViewSearchSoat.FindViewById<Button>(Resource.Id.btnCancel);
            BtnSearchSoat = ViewSearchSoat.FindViewById<Button>(Resource.Id.btnSearchSoat);
            SpDocumentType = ViewSearchSoat.FindViewById<Spinner>(Resource.Id.spDocumentTypes);
            LySearch = ViewSearchSoat.FindViewById<LinearLayout>(Resource.Id.lySearch);
            LyError = ViewSearchSoat.FindViewById<LinearLayout>(Resource.Id.lyError);
            TvCannotFinishChange = ViewSearchSoat.FindViewById<TextView>(Resource.Id.tvCannotFinishChange);
            TextView tvDocumentType = ViewSearchSoat.FindViewById<TextView>(Resource.Id.tvDocumentType);
            TextView tvDocumentNumber = ViewSearchSoat.FindViewById<TextView>(Resource.Id.tvDocumentNumber);
            TextView tvLicensePlate = ViewSearchSoat.FindViewById<TextView>(Resource.Id.tvLicensePlate);
            BtnReturn = ViewSearchSoat.FindViewById<Button>(Resource.Id.btnReturn);
            EtDocumentNumber.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Bold);
            EtLicensePlate.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Bold);
            BtnCancel.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Bold);
            BtnSearchSoat.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Bold);
            tvDocumentType.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Bold);
            tvDocumentNumber.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Bold);
            tvLicensePlate.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Bold);
            TvCannotFinishChange.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            LyBody.Visibility = ViewStates.Visible;

            if (DocumentTypes != null)
            {
                RunOnUiThread(() =>
                {
                    Typeface typeface = FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium);
                    CustomSpinnerAdapter documentTypeAdapter = new CustomSpinnerAdapter(this, Resource.Layout.spinner_layout, DocumentTypes.ToList().Select(x => x.Description).ToArray(), typeface);
                    SpDocumentType.Adapter = documentTypeAdapter;
                });
            }

            BtnReturn.Click += delegate
            {
                LySearch.Visibility = ViewStates.Visible;
                LyError.Visibility = ViewStates.Gone;
            };

            SpDocumentType.ItemSelected += SpDocumentType_ItemSelected;
            EtLicensePlate.SetFilters(new IInputFilter[] { new CustomInputFilter(ConstantEventName.RegexFilter, ConstRegularExpression.NumberMoreLetter, 6) });
            EtDocumentNumber.SetFilters(new IInputFilter[] { new CustomInputFilter(ConstantEventName.OnlyLength, "", 13) });

            BtnSearchSoat.Click += async delegate { await GetSoat(); };

            BtnCancel.Click += delegate { SearchSoatDialog.Dismiss(); };
            SearchSoatDialog.Window.SetBackgroundDrawable(new ColorDrawable(Color.Transparent));
            SearchSoatDialog.Show();
        }

        private async Task GetDocumentsType()
        {
            try
            {
                if (DocumentTypes == null)
                {
                    ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);

                    var response = await LoadDocumentsType();

                    if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                    {
                        RunOnUiThread(() =>
                        {
                            DefineShowErrorWay(AppMessages.ApplicationName, MessagesHelper.GetMessage(response.Result), AppMessages.AcceptButtonText);
                        });
                    }
                    else
                    {
                        DocumentTypes = response.DocumentTypes;
                    }
                }
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.MoreServicesFragment, ConstantMethodName.GetDocumentTypes } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
            finally
            {
                HideProgressDialog();
            }
        }

        private void ModifyFieldsStyle()
        {
            if (string.IsNullOrEmpty(EtDocumentNumber.Text))
            {
                EtDocumentNumber.SetBackgroundResource(Resource.Drawable.field_error);
            }
            else
            {
                EtDocumentNumber.SetBackgroundResource(Resource.Drawable.button_products);
            }

            if (string.IsNullOrEmpty(EtLicensePlate.Text))
            {
                EtLicensePlate.SetBackgroundResource(Resource.Drawable.field_error);
            }

            if (SpDocumentType.SelectedItemPosition == 0)
            {
                SpDocumentType.SetBackgroundResource(Resource.Drawable.spinner_error);
            }
            else
            {
                SpDocumentType.SetBackgroundResource(Resource.Drawable.spinner);
            }
        }

        private async Task GetSoat()
        {
            if (SpDocumentType != null && SpDocumentType.SelectedItemPosition != -1)
            {
                Soat soat = GetSoatEntity();
                SoatResponse response = null;
                InsuranceModel _insuranceModel = new InsuranceModel(new InsuranceService(DeviceManager.Instance));

                try
                {
                    ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);
                    string messageValidation = _insuranceModel.ValidateFields(soat);

                    if (string.IsNullOrEmpty(messageValidation))
                    {
                        SearchSoatDialog.Cancel();
                        response = await _insuranceModel.GetSoat(soat);
                        ValidateResponse(response);
                    }
                    else
                    {
                        LyError.Visibility = ViewStates.Visible;
                        LySearch.Visibility = ViewStates.Gone;
                        TvCannotFinishChange.Text = messageValidation;
                        this.ModifyFieldsStyle();
                    }
                }
                catch (Exception exception)
                {
                    Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.SoatActivity, ConstantMethodName.GetSoat } };
                    ShowAndRegisterMessageExceptions(exception, properties);
                    ShowErrorLayout(AppMessages.UnexpectedErrorMessage);
                }
                finally
                {
                    HideProgressDialog();
                }
            }
        }

        private void ValidateResponse(SoatResponse response)
        {
            if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
            {
                RunOnUiThread(() =>
                {
                    HideProgressDialog();
                    SearchSoatDialog.Show();
                    TvCannotFinishChange.Text = MessagesHelper.GetMessage(response.Result);
                    LyError.Visibility = ViewStates.Visible;
                    LySearch.Visibility = ViewStates.Gone;
                });
            }
            else
            {
                if (!string.IsNullOrEmpty(response.MessageSoat))
                {
                    HideProgressDialog();
                    SearchSoatDialog.Show();
                    TvCannotFinishChange.Text = response.MessageSoat;
                    LyError.Visibility = ViewStates.Visible;
                    LySearch.Visibility = ViewStates.Gone;
                }
                else
                {
                    if (!string.IsNullOrEmpty(response.QRCode))
                    {
                        SearchSoatDialog.Dismiss();
                        HideProgressDialog();
                        Soat soatNew = new Soat()
                        {
                            DocumentNumber = EtDocumentNumber.Text,
                            Plate = EtLicensePlate.Text,
                            LicensePlate = EtLicensePlate.Text,
                            QRCode = response.QRCode
                        };

                        SaveSoat(soatNew);
                        ShowSoat(soatNew);
                    }
                    else
                    {
                        HideProgressDialog();
                        SearchSoatDialog.Show();
                        LyError.Visibility = ViewStates.Visible;
                        LySearch.Visibility = ViewStates.Gone;
                        TvCannotFinishChange.Text = response.MessageSoat;
                    }
                }
            }
        }

        private void SaveSoat(Soat soat)
        {
            _DocumentsDataBaseModel.UpSertSoat(soat);
        }

        private void ShowSoat(Soat soat)
        {
            ParametersManager.QRCode = soat.QRCode;
            ParametersManager.SoatPlate = soat.Plate;
            Intent intent = new Intent(this, typeof(SoatActivity));
            StartActivity(intent);
        }

        private Soat GetSoatEntity()
        {
            return new Soat()
            {
                DocumentType = DocumentTypes[SpDocumentType.SelectedItemPosition].Code,
                DocumentNumber = EtDocumentNumber.Text.Trim(),
                ImageFormat = ConstImageFormat.Png,
                LicensePlate = EtLicensePlate.Text.Trim().ToUpper()
            };
        }

        private void ShowErrorLayout(string message, int resource = 0)
        {
            LyBody.Visibility = ViewStates.Gone;
            RlError.Visibility = ViewStates.Visible;
            TvError.Text = message;

            if (resource != 0)
            {
                IvError.SetImageResource(resource);
            }

            if (!BtnError.HasOnClickListeners)
            {
                BtnError.Click += async delegate
                {
                    ShowBodyLayout();
                    await ShowSoatDialog();
                };
            }
        }

        private void SpDocumentType_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            if (SpDocumentType != null && SpDocumentType.SelectedItemPosition != -1)
            {
                string typeDocument = DocumentTypes[SpDocumentType.SelectedItemPosition].Code;
                EtDocumentNumber.Text = "";

                if (typeDocument.Equals("C"))
                {
                    EtDocumentNumber.SetFilters(new IInputFilter[] { new CustomInputFilter(ConstantEventName.OnlyLength, "", 13) });
                }
                else if (typeDocument.Equals("E"))
                {
                    EtDocumentNumber.SetFilters(new IInputFilter[] { new CustomInputFilter(ConstantEventName.OnlyLength, "", 7) });
                }
            }
        }

        private void ModalSoatScreenView()
        {
            FirebaseRegistrationEventsService.Instance.RegisterScreen(this, AnalyticsScreenView.ModalSoat, typeof(GetSoatsActivity).Name);
        }
    }
}