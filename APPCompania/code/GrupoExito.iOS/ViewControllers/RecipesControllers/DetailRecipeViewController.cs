using System;
using System.Collections.Generic;
using System.Linq;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using GrupoExito.DataAgent.Services.Recipes;
using GrupoExito.Entities.Entiites;
using GrupoExito.Entities.Responses.Recipes;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.RecipesControllers.Sources;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using GrupoExito.iOS.ViewControllers.UserControllers.Cells;
using GrupoExito.iOS.Views.RecipesViews.Cells;
using GrupoExito.Logic.Models.Recipes;
using GrupoExito.Utilities.Helpers;
using SDWebImage;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.RecipesControllers
{
    public partial class DetailRecipeViewController : UIViewControllerBase
    {
        #region Attributes
        private RecipesModel _recipesModel;
        public IList<RecipeDetail> Recipes;
        public Recipe Recipe;
        #endregion


        public DetailRecipeViewController(IntPtr handle) : base(handle)
        {
            _recipesModel = new RecipesModel(new RecipesService(DeviceManager.Instance));
        }
        #region Asycn
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.LoadExternalViews();
            this.GetRecipesListAsync();
            this.LoadCorners();
            this.LoadGradient();
            this.LoadHandlers();
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
                Util.LogException(exception, ConstantControllersName.DetailRecipeViewController, ConstantMethodName.ViewWillAppear);
                ShowMessageException(exception);
            }
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            GC.Collect();
        }
        #endregion

        #region Methods

        private void LoadExternalViews()
        {
            try
            {
                detailRecipeTableView.RegisterNibForCellReuse(HeaderRecipeViewCell.Nib, HeaderRecipeViewCell.Key);
                detailRecipeTableView.RegisterNibForCellReuse(NotificationsViewCell.Nib, NotificationsViewCell.Key);
                LoadNavigationView(this.NavigationController.NavigationBar);
                LoadCustomSpinnerView(customSpinnerView, ref _spinnerActivityIndicatorView);
                CustomSpinnerViewFromBase = customSpinnerView;
                SpinnerActivityIndicatorViewFromBase = spinnerActivityIndicatorView;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.DetailRecipeViewController, ConstantMethodName.LoadExternalViews);
                ShowMessageException(exception);
            }
        }

        private void LoadCorners()
        {
            try
            {
                recipeView.Layer.BorderWidth = ConstantStyle.BorderWidth;
                recipeView.Layer.BorderColor = UIColor.LightGray.ColorWithAlpha(0.2f).CGColor;
                recipeView.Layer.CornerRadius = ConstantStyle.CornerRadius;
                detailRecipeTableView.Layer.BorderWidth = ConstantStyle.BorderWidth;
                detailRecipeTableView.Layer.BorderColor = UIColor.LightGray.ColorWithAlpha(0.2f).CGColor;
                detailRecipeTableView.Layer.CornerRadius = ConstantStyle.CornerRadius;
              

            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.DetailRecipeViewController, ConstantMethodName.LoadCorners);
                ShowMessageException(exception);
            }
        }

        private void LoadGradient()
        {
            var gradient = new CAGradientLayer
            {
                Frame = View.Bounds,
                Colors = new CGColor[] { UIColor.Clear.CGColor, UIColor.Gray.CGColor, UIColor.Gray.CGColor, UIColor.Gray.CGColor }
            };
            viewGradient.Layer.Mask = gradient;
        }

        private void LoadHandlers() 
        {
            _spinnerActivityIndicatorView.Retry.TouchUpInside += RetryTouchUpInside;


        }

        public async void GetRecipesListAsync()
        {
            try
            {
                StartActivityIndicatorCustom();
                RecipeResponse response = await _recipesModel.GetRecipe(Recipe.Id);

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
                    if (response.Recipe != null && response.Recipe.Any())
                    {
                        this.Recipes = response.Recipe;
                    }
                    this.DrawRecipesDetailList();
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.DetailRecipeViewController, ConstantMethodName.GetRecipesListAsync);
                ShowMessageException(exception);
            }
        }


        private void DrawRecipesDetailList()
        {
            if (Recipes != null && Recipes.Any())
            {
                detailRecipeImageView.SetImage(new NSUrl(Recipes[0].Image));
                detailRecipeTableView.Source = new DetailRecipeViewSource(Recipes, Recipes[0].Ingredients, Recipes[0].PreparationSteps);
                detailRecipeTableView.RowHeight = UITableView.AutomaticDimension;
                detailRecipeTableView.EstimatedRowHeight = 50;
                detailRecipeTableView.ReloadData();
            }
           StopActivityIndicatorCustom();
        }



        #endregion


        #region Events

        void RetryTouchUpInside(object sender, EventArgs e)
        {
            this.GetRecipesListAsync();
        }

        #endregion
    }
}

