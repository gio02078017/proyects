using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using GrupoExito.Android.Activities.Base;
using GrupoExito.Android.Activities.Generic;
using GrupoExito.Android.Activities.Products;
using GrupoExito.Android.Adapters;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.Utilities.Constants;
using GrupoExito.DataAgent.Services.Products;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Entiites;
using GrupoExito.Entities.Responses.Base;
using GrupoExito.Entities.Responses.Products;
using GrupoExito.Logic.Models.Products;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Asv7 = Android.Support.V7.App;

namespace GrupoExito.Android.Fragments
{
    public class ShoppingListsFragment : Fragment, IMyList
    {
        #region Controls

        private RecyclerView RvMylists;
        private LinearLayoutManager linerLayoutManager;
        private ShoppingListAdapter myListAdapter;
        private TextView TvTitleMyLists, TvBodyMyLists, TvWithoutList;
        private LinearLayout LyTitleAddMyList, LyWithoutList;
        private TextView TvTitleAddMyList, TvNewMyList;
        private View ViewLineStart;
        private LinearLayout LyListRecommend, LyNewList;
        private TextView TvNameSuggest, TvNameListRecommend, TvError, TvNameProductsListRecommend, TvSuggestedListsMessage;
        private Button BtnError;
        private RelativeLayout RlBody, RlError;
        private ImageView IvError;

        private TextView TvTitleDialog;
        private TextView TvMessageDialog;

        private Asv7.AlertDialog GenericDialog, EditDialog;

        private View viewGenericDialog, viewEditDialog;

        private Button BtnYesDialog;

        private Button BtnNotDialog;

        private LinearLayout LyAccept, LyCancel;

        private EditText EtNameList;

        #endregion

        #region Properties

        private ShoppingListModel shoppingListModel;
        private ShoppingListsResponse shoppingListsResponse;
        private ResponseBase responseBase;

        private string shoppingListId;
        private string tempName;

        #endregion

        public static ShoppingListsFragment NewInstance(String question, String answer)
        {
            ShoppingListsFragment myListsFragment = new ShoppingListsFragment();
            return myListsFragment;
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = null;

            if (inflater != null)
            {
                view = inflater.Inflate(Resource.Layout.FragmentShoppingLists, container, false);
            }

            return view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            shoppingListModel = new ShoppingListModel(new ShoppingListService(DeviceManager.Instance));
            this.SetControlsProperties(view);
            this.EditFonts();
        }

        public override async void OnResume()
        {
            base.OnResume();
            await this.GetShoppingLists();
            if (Activity != null)
            {
                ((MainActivity)Activity).RegisterScreen(AnalyticsScreenView.ShoppingLists);
            }
        }

        public void OnEditItemClicked(ShoppingList shoppingList)
        {
            if (ConvertUtilities.StringToDouble(shoppingList.QuantityProducts) > 0)
            {
                GoToRecommendProducst(shoppingList);
            }
            else
            {
                DialogEditList(shoppingList);
            }
        }

        public void OnDelateItemClicked(ShoppingList shoppingList)
        {
            
            shoppingListId = shoppingList.Id;
            DataDialog dataDialog = new DataDialog(AppMessages.TitleGenericDialog, AppMessages.DeleteListChangeMessage);
            ShowGenericDialogDialog(dataDialog);
        }

        public void OnSelectItemClicked(ShoppingList shoppingList)
        {
            if (ConvertUtilities.StringToDouble(shoppingList.QuantityProducts) > 0)
            {
                GoToRecommendProducst(shoppingList);
            }
            else
            {
                Intent intent = new Intent(Context, typeof(TutorialListActivity));
                StartActivity(intent);
            }
        }

        private async Task GetShoppingLists()
        {
            try
            {
                shoppingListsResponse = await shoppingListModel.GetShoppingLists();

                if (shoppingListsResponse.Result != null && shoppingListsResponse.Result.HasErrors && shoppingListsResponse.Result.Messages != null)
                {
                    Activity.RunOnUiThread(() =>
                    {
                        ShowErrorLayout(MessagesHelper.GetMessage(shoppingListsResponse.Result));
                    });
                }
                else
                {
                    if (shoppingListsResponse.ShpoppingLists.Count > 0)
                    {
                        SetMyLists();
                        LyTitleAddMyList.Visibility = ViewStates.Visible;
                        ViewLineStart.Visibility = ViewStates.Visible;                        
                        RvMylists.Visibility = ViewStates.Visible;
                        LyWithoutList.Visibility = ViewStates.Gone;
                    }
                    else
                    {
                        LyTitleAddMyList.Visibility = ViewStates.Gone;
                        ViewLineStart.Visibility = ViewStates.Gone;
                        RvMylists.Visibility = ViewStates.Gone;
                        LyWithoutList.Visibility = ViewStates.Visible;                        
                    }
                    ShowBodyLayout();
                }
            }
            catch (Exception ex)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.ShoppingListsFragment, ConstantMethodName.GetProducts } };

                if (Activity != null)
                {
                    ((BaseProductActivity)Activity).RegisterMessageExceptions(ex, properties);
                }

                ShowErrorLayout(AppMessages.UnexpectedErrorMessage);
            }
        }

        private void SetMyLists()
        {
            linerLayoutManager = new LinearLayoutManager(Activity)
            {
                AutoMeasureEnabled = true
            };
            RvMylists.NestedScrollingEnabled = false;
            RvMylists.HasFixedSize = true;
            RvMylists.SetLayoutManager(linerLayoutManager);
            myListAdapter = new ShoppingListAdapter(shoppingListsResponse.ShpoppingLists, Activity, Activity, this);
            RvMylists.SetAdapter(myListAdapter);
        }

        private void EditFonts()
        {
            TvTitleMyLists.SetTypeface(FontManager.Instance.GetTypeFace(Activity, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            TvBodyMyLists.SetTypeface(FontManager.Instance.GetTypeFace(Activity, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvTitleAddMyList.SetTypeface(FontManager.Instance.GetTypeFace(Activity, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            TvWithoutList.SetTypeface(FontManager.Instance.GetTypeFace(Activity, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            TvNewMyList.SetTypeface(FontManager.Instance.GetTypeFace(Activity, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvNameSuggest.SetTypeface(FontManager.Instance.GetTypeFace(Activity, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            TvNameListRecommend.SetTypeface(FontManager.Instance.GetTypeFace(Activity, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            TvNameProductsListRecommend.SetTypeface(FontManager.Instance.GetTypeFace(Activity, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvSuggestedListsMessage.SetTypeface(FontManager.Instance.GetTypeFace(Activity, FontManager.RalewayMedium), TypefaceStyle.Normal);
            BtnError.SetTypeface(FontManager.Instance.GetTypeFace(Activity, FontManager.RalewayExtraBold), TypefaceStyle.Bold);
        }

        private void SetControlsProperties(View view)
        {
            RlError = view.FindViewById<RelativeLayout>(Resource.Id.layoutError);
            RlBody = view.FindViewById<RelativeLayout>(Resource.Id.rlBody);
            TvError = RlError.FindViewById<TextView>(Resource.Id.tvError);
            BtnError = RlError.FindViewById<Button>(Resource.Id.btnError);
            IvError = RlError.FindViewById<ImageView>(Resource.Id.ivError);

            TvTitleMyLists = view.FindViewById<TextView>(Resource.Id.tvTitleMyLists);
            TvBodyMyLists = view.FindViewById<TextView>(Resource.Id.tvBodyMyLists);
            LyTitleAddMyList = view.FindViewById<LinearLayout>(Resource.Id.lyTitleAddMyList);
            TvTitleAddMyList = view.FindViewById<TextView>(Resource.Id.tvTitleAddMyList);
            TvWithoutList = view.FindViewById<TextView>(Resource.Id.tvWithoutList);
            LyWithoutList = view.FindViewById<LinearLayout>(Resource.Id.lyWithoutList);
            TvNewMyList = view.FindViewById<TextView>(Resource.Id.tvNewMyList);
            ViewLineStart = view.FindViewById<View>(Resource.Id.viewLineStart);
            RvMylists = view.FindViewById<RecyclerView>(Resource.Id.rvMylists);
            LyListRecommend = view.FindViewById<LinearLayout>(Resource.Id.lyListRecommend);
            TvNameSuggest = view.FindViewById<TextView>(Resource.Id.tvNameSuggest);
            TvSuggestedListsMessage = view.FindViewById<TextView>(Resource.Id.tvSuggestedListsMessage);
            TvNameListRecommend = view.FindViewById<TextView>(Resource.Id.tvNameListRecommend);
            TvNameProductsListRecommend = view.FindViewById<TextView>(Resource.Id.tvNameProductsListRecommend);
            LyListRecommend.Click += delegate { EventListRecommend(); };
            RvMylists = view.FindViewById<RecyclerView>(Resource.Id.rvMylists);
            LyNewList = view.FindViewById<LinearLayout>(Resource.Id.lyNewList);
            LyNewList.Click += delegate { GoToCreateNewList(); };
            LyWithoutList.Click += delegate { GoToCreateNewList(); };
        }

        private void EventListRecommend()
        {
            var myRecommendLists = new Intent(Context, typeof(RecommendProducstActivity));
            StartActivity(myRecommendLists);
        }

        private void GoToCreateNewList()
        {
            var myRecommendLists = new Intent(Context, typeof(CreateNewListActivity));
            StartActivity(myRecommendLists);
        }

        private void ShowBodyLayout()
        {
            RlBody.Visibility = ViewStates.Visible;
            RlError.Visibility = ViewStates.Gone;
        }

        private void ShowErrorLayout(string message, int resource = 0)
        {
            RlBody.Visibility = ViewStates.Gone;
            RlError.Visibility = ViewStates.Visible;
            TvError.Text = message;

            if (resource != 0)
            {
                IvError.SetImageResource(resource);
            }

            if (!BtnError.HasOnClickListeners)
            {
                BtnError.Click += async delegate { await GetShoppingLists(); };
            }
        }

        private async Task GoDeleteList()
        {
            try
            {
                responseBase = await shoppingListModel.DeleteShoppingList(shoppingListId);

                if (responseBase.Result != null && responseBase.Result.HasErrors && responseBase.Result.Messages != null)
                {
                    Activity.RunOnUiThread(() =>
                    {
                        ShowErrorLayout(MessagesHelper.GetMessage(responseBase.Result));
                    });
                }
                else
                {
                    await this.GetShoppingLists();
                    shoppingListId = null;
                }
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.ShoppingListsFragment, ConstantMethodName.GoDeleteList } };
                if (Activity != null)
                {
                    ((BaseProductActivity)Activity).RegisterMessageExceptions(exception, properties);
                }

                ShowErrorLayout(AppMessages.UnexpectedErrorMessage);
            }
        }

        private void ShowGenericDialogDialog(DataDialog dataDialog)
        {
            GenericDialog = new Asv7.AlertDialog.Builder(Context).Create();
            viewGenericDialog = LayoutInflater.Inflate(Resource.Layout.DialogMessageGeneric, null);
            GenericDialog.SetView(viewGenericDialog);
            GenericDialog.SetCanceledOnTouchOutside(false);
            TvTitleDialog = viewGenericDialog.FindViewById<TextView>(Resource.Id.tvTitleDialog);
            TvMessageDialog = viewGenericDialog.FindViewById<TextView>(Resource.Id.tvMessageDialog);
            TvTitleDialog.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayExtraBold), TypefaceStyle.Bold);
            TvMessageDialog.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayExtraBold), TypefaceStyle.Bold);
            TvTitleDialog.Text = dataDialog.TitleDialog;
            TvMessageDialog.Text = dataDialog.MensajeDialog;
            BtnYesDialog = viewGenericDialog.FindViewById<Button>(Resource.Id.btnYesDialog);
            BtnNotDialog = viewGenericDialog.FindViewById<Button>(Resource.Id.btnNotDialog);

            if (dataDialog.ButtonYesName != null)
            {
                BtnYesDialog.Text = dataDialog.ButtonYesName;
            }

            if (dataDialog.ButtonNotName != null)
            {
                BtnNotDialog.Text = dataDialog.ButtonNotName;
            }

            BtnYesDialog.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayExtraBold), TypefaceStyle.Bold);
            BtnNotDialog.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayExtraBold), TypefaceStyle.Bold);
            BtnYesDialog.Click += delegate { EventYesGenericDialog(); };
            BtnNotDialog.Click += delegate { EventNotGenericDialog(); };
            GenericDialog.Window.SetBackgroundDrawable(new ColorDrawable(Color.Transparent));
            GenericDialog.Show();
        }

        private void EventNotGenericDialog()
        {
            GenericDialog.Hide();
        }

        private async Task EventYesGenericDialog()
        {
            GenericDialog.Hide();
            await this.GoDeleteList();
        }

        private void DialogEditList(ShoppingList _ShoppingList)
        {
            EditDialog = new Asv7.AlertDialog.Builder(Context).Create();
            viewEditDialog = LayoutInflater.Inflate(Resource.Layout.DialogEditNameList, null);
            EditDialog.SetView(viewEditDialog);
            EditDialog.SetCanceledOnTouchOutside(false);

            TextView TvTitleDialog = viewEditDialog.FindViewById<TextView>(Resource.Id.tvTitleDialog);
            TextView TvMessageChangeList = viewEditDialog.FindViewById<TextView>(Resource.Id.tvMessageChangeList);
            
            EtNameList = viewEditDialog.FindViewById<EditText>(Resource.Id.etNameList);
            EtNameList.Text = _ShoppingList.Name;
           
            LyAccept = viewEditDialog.FindViewById<LinearLayout>(Resource.Id.lyAccept);
            LyCancel = viewEditDialog.FindViewById<LinearLayout>(Resource.Id.lyCancel);
            TextView TvCancel = viewEditDialog.FindViewById<TextView>(Resource.Id.tvCancel);
            TextView TvAccept = viewEditDialog.FindViewById<TextView>(Resource.Id.tvAccept);

            TvTitleDialog.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvMessageChangeList.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayMedium), TypefaceStyle.Normal);
            EtNameList.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            TvCancel.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            TvAccept.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewaySemiBold), TypefaceStyle.Normal);


            LyAccept.Click += async delegate { await UpdateName(_ShoppingList); };
            LyCancel.Click += delegate { EditDialog.Hide(); };
            EditDialog.Window.SetBackgroundDrawable(new ColorDrawable(Color.Transparent));
            EditDialog.Show();
        }

        private async Task UpdateName(ShoppingList _ShoppingList)
        {
            EditDialog.Hide();

            if (!EtNameList.Text.Equals(_ShoppingList.Name))
            {

                tempName = _ShoppingList.Name;

                _ShoppingList.Name = EtNameList.Text;                

                String message = shoppingListModel.ValidateFields(_ShoppingList);

                if (string.IsNullOrEmpty(message))
                {
                    await this.UpdateNameList(_ShoppingList);
                }
                else
                {
                    if (Activity != null)
                    {
                        ((BaseProductActivity)Activity).DefineShowErrorWay(AppMessages.ApplicationName, message, AppMessages.AcceptButtonText, true);
                    }

                    _ShoppingList.Name = tempName;

                }
            }

        }

        private async Task UpdateNameList(ShoppingList shoppingList)
        {
            try
            {

                ResponseBase _ResponseBase = await shoppingListModel.UpdateShoppingList(shoppingList);

                if (_ResponseBase.Result != null && _ResponseBase.Result.HasErrors &&
                    _ResponseBase.Result.Messages != null)
                {
                    ConvertUtilities.MessageToast(AppMessages.ApologyUpdateListNameErrorMessage, Context, true);
                    shoppingList.Name = tempName;
                }
                else
                {
                    RvMylists.GetAdapter().NotifyDataSetChanged();
                }
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                     { ConstantActivityName.RecommendProducstActivity, ConstantMethodName.UpdateList } };
                if (Activity != null)
                {
                    ((BaseProductActivity)Activity).RegisterMessageExceptions(exception, properties);
                }

                ShowErrorLayout(AppMessages.UnexpectedErrorMessage);
            }
        }

        private void GoToRecommendProducst(ShoppingList shoppingList)
        {
            shoppingListId = shoppingList.Id;
            var myRecommendLists = new Intent(Context, typeof(RecommendProducstActivity));
            myRecommendLists.PutExtra(ConstantPreference.ListId, shoppingListId);
            StartActivity(myRecommendLists);
        }
    }
}