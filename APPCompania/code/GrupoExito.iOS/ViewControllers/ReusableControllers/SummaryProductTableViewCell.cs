using System;
using Foundation;
using GrupoExito.Entities;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.Utilities.Helpers;
using SDWebImage;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ReusableControllers
{
    #region Interface
    public interface ISummarySource
    {
        bool areCellActionsEnabled(Product product);
        bool isHowDoYouLikeEnabled(Product product);
    }
    #endregion

    public partial class SummaryProductTableViewCell : UITableViewCell
    {
        #region Attributes
        //private Timer Timer;
        private int FinalQuantity;
        private Product _product;
        private bool IsWeightSelected;
        #endregion

        #region Handlers
        private Action<int> addButtonTouchUpAction;
        private Action<int> substractButtonTouchUpAction;
        private Action deleteButtonTouchUpAction;
        private Action<Product> howDoYouLikeItAction;
        private ISummarySource summaryDelegate;
        #endregion

        #region Properties
        public Action<int> AddButtonTouchUpAction { get => addButtonTouchUpAction; set => addButtonTouchUpAction = value; }
        public Action<int> SubstractButtonTouchUpAction { get => substractButtonTouchUpAction; set => substractButtonTouchUpAction = value; }
        public Action DeleteButtonTouchUpAction { get => deleteButtonTouchUpAction; set => deleteButtonTouchUpAction = value; }
        public Action<Product> HowDoYouLikeItAction { get => howDoYouLikeItAction; set => howDoYouLikeItAction = value; }
        public ISummarySource SummaryDelegate { get => summaryDelegate; set => summaryDelegate = value; }

        public Product Product
        {
            get
            {
                return _product;
            }
        }
        #endregion

        #region Constructors
        static SummaryProductTableViewCell() 
        {
            //Static default constructor this class 
        }
        protected SummaryProductTableViewCell(IntPtr handle) : base(handle) 
        {
            //Default constructor this class 
        }
        #endregion

        #region Override Methods 
        public override void AwakeFromNib()
        {
            //ElapsedEventHandler handler = new ElapsedEventHandler(TimeElapsed);
            //Timer = new Timer() { Interval = 900 };
            //Timer.Elapsed += TimeElapsed;
            //timer.Elapsed += handler;

            FinalQuantity = 0;

            substractButton.TouchUpInside += (o, s) =>
            {
                if (SubstractButtonTouchUpAction != null)
                {
                    if (SummaryDelegate != null && SummaryDelegate.areCellActionsEnabled(_product))
                    {
                        if(_product.Quantity > 0)
                        {
                            //_product.Quantity -= 1;
                            SubstractButtonTouchUpAction(-1);
                            DecreaseAmount();
                        }
                    }
                }
            };

            addButton.TouchUpInside += (o, s) =>
            {
                if (AddButtonTouchUpAction != null)
                {
                    if (SummaryDelegate != null && SummaryDelegate.areCellActionsEnabled(_product))
                {
                        //_product.Quantity += 1;
                        AddButtonTouchUpAction(1);
                        IncreaseAmount();
                    }
                }
            };

            unitButton.TouchUpInside += (o, s) =>
            {
                IsWeightSelected = false;
                SetWeightUnitButtons();
                SetWeightUnitLabel();
            };

            weightButton.TouchUpInside += (o, s) =>
            {
                IsWeightSelected = true;
                SetWeightUnitButtons();
                SetWeightUnitLabel();
            };

            deleteButton.TouchUpInside += (o, s) =>
            {
                if (DeleteButtonTouchUpAction != null)
                {
                    if (SummaryDelegate != null && SummaryDelegate.areCellActionsEnabled(_product))
                    {
                        DeleteButtonTouchUpAction();
                    }
                }
            };

            howDoYouLikeButton.TouchUpInside += (o, s) =>
            {
                if (HowDoYouLikeItAction != null)
                {
                    if (SummaryDelegate != null && SummaryDelegate.isHowDoYouLikeEnabled(_product))
                    {
                        HowDoYouLikeItAction(_product);
                    }
                }
            };
        }
        #endregion

        #region methods

        public void EnableQuantityModifications()
        {
            addButton.Enabled = true;
            substractButton.Enabled = true;
            deleteButton.Enabled = true;
            howDoYouLikeButton.Enabled = true;
        }

        public void DisableQuantityModifications()
        {
            addButton.Enabled = false;
            substractButton.Enabled = false;
            deleteButton.Enabled = false;
            howDoYouLikeButton.Enabled = false;
        }

        public void LoadData(Product product)
        {
            IsWeightSelected = product.IsEstimatedWeight;
            if (!product.IsLoading)
            {
                contentView.Hidden = false;
                loadingIndicator.StopAnimating();

                _product = product;
                _product.WeightSelected = product.IsEstimatedWeight;

                this.productTitleLabel.Text = product.Name;
                this.priceLabel.Text = StringFormat.ToPrice(product.Price.ActualPrice * product.Quantity);
                this.productImageView.SetImage(new NSUrl(product.UrlMediumImage));

                SetWeightUnitButtons();
                SetWeightUnitLabel();
                ConfigureUnitWeightView(product.IsEstimatedWeight);

                if (product.CategoryId != null)
                {
                    categoryView.Hidden = false;
                    if (product.CategoryImage != null)
                    {
                        categoryImageView.SetImage(new NSUrl(product.CategoryImage));
                    }
                    categoryTitleLabel.Text = product.CategoryName;
                }
                else
                {
                    categoryView.Hidden = true;
                }

                this.howDoYouLikeButton.Hidden = _product.IsEstimatedWeight ? false : true;
            }
            else
            {
                contentView.Hidden = true;
                loadingIndicator.StartAnimating();
            }
        }

        public void CancelDelete()
        {
            SetWeightUnitLabel();
        }


        private void IncreaseAmount()
        {
            RecalculateQuantity(1);
            SetWeightUnitLabel();
        }

        private void DecreaseAmount()
        {
            RecalculateQuantity(-1);
            SetWeightUnitLabel();
        }

        private void SetWeightUnitButtons()
        {
            unitButton.Enabled = IsWeightSelected;
            weightButton.Enabled = !IsWeightSelected;

            weightMarkView.Hidden = !IsWeightSelected;
            unitMarkView.Hidden = IsWeightSelected;

            UIFont subtitleFont = UIFont.FromName(ConstantFontSize.LetterSubtitle, ConstantFontSize.ProductByCategoryFilter);
            UIFont mediumFont = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.ProductByCategoryFilter);

            unitButton.Font = IsWeightSelected ? mediumFont : subtitleFont;
            weightButton.Font = IsWeightSelected ? subtitleFont : mediumFont;
        }

        private void SetWeightUnitLabel()
        {
            try
            {
                if (IsWeightSelected)
                {
                    decimal minimumWeight = _product.Weight;
                    //decimal currentWeight = Convert.ToDecimal(unitWeightLabel.Text.Split(' ')[0]);

                    //int result = (int)Decimal.Floor(Decimal.Divide(currentWeight, minimumWeight));

                    //decimal totalWeight = minimumWeight * _product.Quantity;
                    //int quantity = _product.Quantity + FinalQuantity;
                    decimal totalWeight = minimumWeight * _product.Quantity;
                    unitWeightLabel.Text = totalWeight < 0 ? "0.0" + " " + _product.WeightUnits : totalWeight.ToString() + " " + _product.WeightUnits;
                }
                else
                {
                    //int quantity = _product.Quantity + FinalQuantity;
                    unitWeightLabel.Text = _product.Quantity < 0 ? "0" : _product.Quantity.ToString();
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.SummaryProductTableViewCell, ConstantMethodName.SetWeightUnitLabel);
            }
        }

        private void UpdateWeightUnitLabel()
        {
            
        }


        //private void TimeElapsed(Object sender, ElapsedEventArgs e)
        //{
        //    NSThread.MainThread.InvokeOnMainThread(() =>
        //    {
        //        Timer.Stop();

        //        if (FinalQuantity > 0)
        //        {
        //            AddButtonTouchUpAction(FinalQuantity);
        //        }
        //        else if (FinalQuantity < 0)
        //        {
        //            SubstractButtonTouchUpAction(FinalQuantity);
        //        }

        //        FinalQuantity = 0;
        //    });
        //}

        private void RecalculateQuantity(int quantity)
        {
            FinalQuantity = FinalQuantity + quantity;
        }

        private void ConfigureUnitWeightView(bool IsEstimatedWeight)
        {
            if(IsEstimatedWeight)
            {
                unitWeightButtonsStackView.Hidden = false;
                marksView.Hidden = false;
                lineView.Hidden = false;
                unitWeightViewHeightConstraint.Constant = 87;
            }
            else
            {
                unitWeightButtonsStackView.Hidden = true;
                marksView.Hidden = true;
                lineView.Hidden = true;
                unitWeightViewHeightConstraint.Constant = 46;
            }

            unitWeightView.Layer.CornerRadius = ConstantStyle.CornerRadius;
            unitWeightView.Layer.BorderColor = ConstantColor.UiPrimary.CGColor;
            unitWeightView.Layer.BorderWidth = ConstantStyle.BorderWidth;
        }
        #endregion
    }
}
