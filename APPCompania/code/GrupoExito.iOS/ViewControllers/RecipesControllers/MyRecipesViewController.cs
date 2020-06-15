using Foundation;
using GrupoExito.DataAgent.Services.Recipes;
using GrupoExito.Entities.Entiites;
using GrupoExito.Entities.Enumerations;
using GrupoExito.Entities.Responses.Recipes;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.RecipesControllers.Sources;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using GrupoExito.Logic.Models.Recipes;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using SDWebImage;
using System;
using System.Collections.Generic;
using System.Linq;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.RecipesControllers
{
    public partial class MyRecipesViewController : UIViewControllerBase
    {
        #region Attributes 
        private RecipesModel _recipesModel;
        private IList<Recipe> Recipes;
        private RecipeCategory category;
        #endregion

        #region Properties
        public RecipeCategory Category { get => category; set => category = value; }
        #endregion

        #region Constructors 
        public MyRecipesViewController(IntPtr handle) : base(handle)
        {
            _recipesModel = new RecipesModel(new RecipesService(DeviceManager.Instance));
        }

        #endregion

        #region Life Cycle 
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            try
            {
                this.LoadExternalViews();
                this.LoadCorners();
                this.LoadHandlers();
                this.GetRecipesListAsync();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.MyRecipesViewController, ConstantMethodName.ViewDidLoad);
                ShowMessageException(exception);
            }
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            try
            {
                navigationView = (NavigationHeaderView)this.NavigationController.NavigationBar.Subviews[this.NavigationController.NavigationBar.Subviews.Length - 1];
                navigationView.LoadControllers(false, false, true, this);
                navigationView.HiddenCarData();
                navigationView.ShowAccountProfile();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.MyRecipesViewController, ConstantMethodName.ViewWillAppear);
                ShowMessageException(exception);
            }
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            GC.Collect();
        }
        #endregion

        #region Methods Async 
        public async void GetRecipesListAsync()
        {
            try
            {
                StartActivityIndicatorCustom();
                RecipesResponse response = await _recipesModel.GetRecipesByCategory(Category.Id.ToString());

                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    if (response.Result.Messages.Any())
                    {
                        string message = MessagesHelper.GetMessage(response.Result);
                        StartActivityErrorMessage(response.Result.Messages[0].Code, message);
                    }
                }
                else
                {
                    if (response.Recipes != null && response.Recipes.Count > 0)
                    {
                        this.Recipes = response.Recipes;
                        this.DrawRecipesList();
                    }
                    else
                    {
                        StartActivityErrorMessage(EnumErrorCode.NoDataFound.ToString(), AppMessages.NotRecipes);
                        _spinnerActivityIndicatorView.Message.Hidden = false;
                        _spinnerActivityIndicatorView.Message.TextColor = ConstantColor.UiMessageError;
                    }
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.MyRecipesViewController, ConstantMethodName.GetHomeProducts);
                ShowMessageException(exception);
            }
        }
        #endregion

        #region Methods 
        private void DrawRecipesList()
        {
            try
            {
                titleLabel.Text = Category.Name;
                imageView.SetImage(new NSUrl(Category.Image), UIImage.FromFile(ConstantImages.SinImagen));
                ListRecipesHeightConstraint.Constant = listRecipesView.Superview.Frame.Height - 80;
                MyRecipesCollectionView.Source = new MyRecipesViewSource(Recipes, this);
                MyRecipesCollectionView.ReloadData();
                StopActivityIndicatorCustom();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.MyRecipesViewController, ConstantMethodName.DrawCustomerList);
                ShowMessageException(exception);
            }
        }


        private void LoadHandlers()
        {
            _spinnerActivityIndicatorView.Retry.TouchUpInside += RetryTouchUpInside;
        }

       
        private void LoadCorners()
        {
            try
            {
                listRecipesView.Layer.CornerRadius = ConstantStyle.CornerRadius;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.MyRecipesViewController, ConstantMethodName.LoadCorners);
                ShowMessageException(exception);
            }
        }

        private void LoadExternalViews()
        {
            try
            {
                MyRecipesCollectionView.RegisterNibForCell(UINib.FromName(ConstantReusableViewName.MyRecipesCollectionViewCell, NSBundle.MainBundle), ConstantIdentifier.MyRecipesIdentifier);
                LoadNavigationView(this.NavigationController.NavigationBar);
                LoadCustomSpinnerView(customSpinnerView, ref _spinnerActivityIndicatorView);
                CustomSpinnerViewFromBase = customSpinnerView;
                SpinnerActivityIndicatorViewFromBase = spinnerActivityIndicatorView;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.MyRecipesViewController, ConstantMethodName.LoadExternalViews);
                ShowMessageException(exception);
            }
        }
        #endregion


        #region  events
        void RetryTouchUpInside(object sender, EventArgs e)
        {
            this.GetRecipesListAsync();
        }
        #endregion
    }
}

