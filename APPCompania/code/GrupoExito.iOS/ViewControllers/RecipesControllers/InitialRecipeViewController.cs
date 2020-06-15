using Foundation;
using GrupoExito.DataAgent.Services.Recipes;
using GrupoExito.Entities.Entiites;
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
    public partial class InitialRecipeViewController : UIViewControllerBase
    {
        #region Attributes
        private RecipesModel _recipesModel;
        private IList<RecipeCategory> GetCategories;
        private RecipeOfDay RecipeOfDay;
        #endregion

        #region Constructors
        public InitialRecipeViewController(IntPtr handle) : base(handle)
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
                this.GetRecipesCategoryListAsync();
                this.LoadHandlers();
                this.LoadCorners();
                this.LoadFonts();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.InitialRecipeViewController, ConstantMethodName.ViewDidLoad);
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
                this.NavigationController.NavigationBarHidden = false;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.InitialRecipeViewController, ConstantMethodName.ViewWillAppear);
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
        public async void GetRecipesCategoryListAsync()
        {
            try
            {
                StartActivityIndicatorCustom();
                RecipeCategoryResponse response = await _recipesModel.GetCategories();

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
                    if (response.Categories != null && response.Categories.Any() && response.Recipe != null)
                    {
                        this.GetCategories = response.Categories;
                        this.RecipeOfDay = response.Recipe;
                    }
                    else
                    {
                        recipeCategoryHeightLayoutConstraint.Constant = 0;

                    }

                    this.DrawRecipesCategoryList();
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.InitialRecipeViewController, ConstantMethodName.GetRecipesCategoryListAsync);
                ShowMessageException(exception);
            }
        }


        private void DrawRecipesCategoryList()
        {
            try
            {
                if (GetCategories != null && GetCategories.Any())
                {
                    var newHeight = GetCategories.Count * ConstantViewSize.RecipesHeightCell;
                    recipeCategoryHeightLayoutConstraint.Constant = newHeight;
                    recipesCategoryTableView.Source = new MyRecipeTableViewSource(GetCategories, this);
                    recipesCategoryTableView.ReloadData();
                    IdeaTitleLabel.Text = RecipeOfDay.Title ?? AppMessages.RecipeOfDayNotFound;
                    if (RecipeOfDay.Image != null)
                    {
                        ideaImageView.SetImage(new NSUrl(RecipeOfDay.Image));
                    }
                    else
                    {
                        ideaContainerView.BackgroundColor = UIColor.Gray;
                        ideaButton.Enabled = false;
                        ideaImageView.Image = UIImage.FromFile(ConstantImages.SinImagen);
                    }
                }
                else
                {
                    recipeCategoryHeightLayoutConstraint.Constant = 0;
                }
                StopActivityIndicatorCustom();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.InitialRecipeViewController, ConstantMethodName.DrawCustomerList);
                ShowMessageException(exception);
            }
        }
        #endregion

        #region Methods
        private void LoadExternalViews()
        {
            try
            {
                recipesCategoryTableView.RegisterNibForCellReuse(UINib.FromName(ConstantReusableViewName.MyRecipesCategoryTableViewCell, NSBundle.MainBundle), ConstantIdentifier.MyRecipesCategoryIdentifier);
                LoadNavigationView(this.NavigationController.NavigationBar);
                LoadCustomSpinnerView(customSpinnerView, ref _spinnerActivityIndicatorView);
                CustomSpinnerViewFromBase = customSpinnerView;
                SpinnerActivityIndicatorViewFromBase = spinnerActivityIndicatorView;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.InitialRecipeViewController, ConstantMethodName.LoadExternalViews);
                ShowMessageException(exception);
            }
        }

        private void LoadFonts()
        {
            try
            {
                ideaLabel.Font = UIFont.BoldSystemFontOfSize(18);
                minutoLabel.Font = UIFont.BoldSystemFontOfSize(18);
                IdeaTitleLabel.Layer.ShadowRadius = 3;
                IdeaTitleLabel.Layer.ShadowOpacity = 5;
            }
            catch (Exception exception)
            {

                Util.LogException(exception, ConstantControllersName.InitialRecipeViewController, ConstantMethodName.LoadFonts);
                ShowMessageException(exception);
            }
        }

        private void LoadHandlers()
        {
            try
            {
                optionsButton.TouchUpInside += OptionsButtonTouchUpInside;
                optionsHiddenTableButton.TouchUpInside += OptionsHiddenTableButtonTouchUpInside;
                ideaButton.TouchUpInside += OptionsideaButtonTouchUpInside;
                _spinnerActivityIndicatorView.Retry.TouchUpInside += RetryTouchUpInside;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.InitialRecipeViewController, ConstantMethodName.LoadHandler);
                ShowMessageException(exception);
            }
        }




        private void LoadCorners()
        {
            try
            {
                ideaImageView.Layer.CornerRadius = ConstantStyle.CornerRadius;
                ideaImageView.ClipsToBounds = true;
                optionsView.Layer.CornerRadius = ConstantStyle.CornerRadius;
                ideaContainerView.Layer.CornerRadius = ConstantStyle.CornerRadius;
                containerOptionView.Layer.CornerRadius = ConstantStyle.CornerRadius;
                optionLabel.Layer.CornerRadius = ConstantStyle.CornerRadius;
                optionLabel.Layer.MaskedCorners = CoreAnimation.CACornerMask.MaxXMinYCorner | CoreAnimation.CACornerMask.MinXMinYCorner;
                optionLabel.ClipsToBounds = true;
                recipesCategoryTableView.Layer.CornerRadius = ConstantStyle.CornerRadius;
                recipesCategoryTableView.ClipsToBounds = true;
                recipesCategoryTableView.Layer.MaskedCorners = CoreAnimation.CACornerMask.MaxXMaxYCorner | CoreAnimation.CACornerMask.MinXMaxYCorner;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.InitialRecipeViewController, ConstantMethodName.LoadCorners);
                ShowMessageException(exception);
            }
        }
        #endregion

        #region Events
        private void OptionsideaButtonTouchUpInside(object sender, EventArgs e)
        {
            if (RecipeOfDay != null)
            {
                DetailRecipeOfTheDayViewController detailRecipeOfTheDay = this.Storyboard.InstantiateViewController(ConstantControllersName.DetailRecipeOfTheDayViewController) as DetailRecipeOfTheDayViewController;
                detailRecipeOfTheDay.RecipeOfDay = RecipeOfDay;
                this.NavigationController.PushViewController(detailRecipeOfTheDay, true);
            }
            else
            {
                Exception exception = new Exception("No se encuentran recetas del dia");
                ShowMessageException(exception);
            }
        }

        private void OptionsHiddenTableButtonTouchUpInside(object sender, EventArgs e)
        {
            optionsButton.Hidden = false;
            containerOptionView.Hidden = true;
            recipesCategoryTableView.Hidden = true;
            optionsView.Hidden = false;
        }

        private void OptionsButtonTouchUpInside(object sender, EventArgs e)
        {
            optionsButton.Hidden = true;
            containerOptionView.Hidden = false;
            recipesCategoryTableView.Hidden = false;
            optionsView.Hidden = true;
        }

        void RetryTouchUpInside(object sender, EventArgs e)
        {
            this.GetRecipesCategoryListAsync();
        }
        #endregion
    }
}
