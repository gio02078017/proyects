using System;
using System.Timers;
using Foundation;
using GrupoExito.Entities;
using GrupoExito.iOS.Utilities;
using GrupoExito.Utilities.Helpers;
using SDWebImage;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ReusableControllers
{
    public partial class SummaryProductWeightTableViewCell : UITableViewCell
    {
        #region Attributes
        private Action<int> addButtonTouchUpAction;
        private Action<int> substractButtonTouchUpAction;
        private Action deleteButtonTouchUpAction;
        private Action<Product> howDoYouLikeItAction;
        private Timer Timer;
        private int FinalQuantity;
        private Product _product;
        #endregion

        #region Properties 
        public Action<int> AddButtonTouchUpAction { get => addButtonTouchUpAction; set => addButtonTouchUpAction = value; }
        public Action<int> SubstractButtonTouchUpAction { get => substractButtonTouchUpAction; set => substractButtonTouchUpAction = value; }
        public Action DeleteButtonTouchUpAction { get => deleteButtonTouchUpAction; set => deleteButtonTouchUpAction = value; }
        public Action<Product> HowDoYouLikeItAction { get => howDoYouLikeItAction; set => howDoYouLikeItAction = value; }
        #endregion

        #region Constructors
        static SummaryProductWeightTableViewCell() 
        {
            //Static Default constructor this class 
        }
        protected SummaryProductWeightTableViewCell(IntPtr handle) : base(handle) 
        {
            //Default constructor this class  
        }
        #endregion

        #region Override Methods
        public override void AwakeFromNib()
        {
            Timer = new Timer() { Interval = 2000 };
            Timer.Elapsed += TimeElapsed;

            FinalQuantity = 0;

            substractButton.TouchUpInside += (o, s) =>
            {
                if (SubstractButtonTouchUpAction != null)
                {
                    Timer.Stop();
                    Timer.Start();

                    RecalculateQuantity(-1);
                }
            };

            addButton.TouchUpInside += (o, s) =>
            {
                if (AddButtonTouchUpAction != null)
                {
                    Timer.Stop();
                    Timer.Start();

                    RecalculateQuantity(1);
                }
            };

            unitButton.TouchUpInside += (o, s) =>
            {
                SetWeightUnitButtons(false);
                SetWeightUnitLabel(false);
            };

            weightButton.TouchUpInside += (o, s) =>
            {
                SetWeightUnitButtons(true);
                SetWeightUnitLabel(true);
            };

            deleteButton.TouchUpInside += (o, s) =>
            {
                DeleteButtonTouchUpAction?.Invoke();
            };

            howDoYouLikeButton.TouchUpInside += (o, s) =>
            {
                HowDoYouLikeItAction?.Invoke(_product);
            };
        }
        #endregion

        #region methods
        public void LoadData(Product product)
        {
            if (!product.IsLoading)
            {
                contentView.Hidden = false;
                loadingIndicator.StopAnimating();

                _product = product;
                _product.WeightSelected = product.IsEstimatedWeight;

                this.productTitleLabel.Text = product.Name;
                this.priceLabel.Text = StringFormat.ToPrice(product.Price.ActualPrice * product.Quantity);
                this.productImageView.SetImage(new NSUrl(product.UrlMediumImage));

                SetWeightUnitButtons(product.IsEstimatedWeight);
                SetWeightUnitLabel(product.IsEstimatedWeight);

                this.howDoYouLikeButton.Hidden = _product.IsEstimatedWeight ? false : true;
            }
            else
            {
                contentView.Hidden = true;
                loadingIndicator.StartAnimating();
            }
        }

        private void SetWeightUnitButtons(bool isWeightSelected)
        {
            unitButton.Enabled = isWeightSelected;
            weightButton.Enabled = !isWeightSelected;

            weightMarkView.Hidden = !isWeightSelected;
            unitMarkView.Hidden = isWeightSelected;

            UIFont subtitleFont = UIFont.FromName(ConstantFontSize.LetterSubtitle, ConstantFontSize.ProductByCategoryFilter);
            UIFont mediumFont = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.ProductByCategoryFilter);

            unitButton.Font = isWeightSelected ? mediumFont : subtitleFont;
            weightButton.Font = isWeightSelected ? subtitleFont : mediumFont;
        }

        private void SetWeightUnitLabel(bool isWeightSelected)
        {
            try
            {
                if (isWeightSelected)
                {
                    decimal minimumWeight = _product.Weight;
                    //decimal currentWeight = Convert.ToDecimal(unitWeightLabel.Text.Split(' ')[0]);

                    //int result = (int)Decimal.Floor(Decimal.Divide(currentWeight, minimumWeight));

                    decimal totalWeight = minimumWeight * _product.Quantity;
                    string textLabel = totalWeight.ToString() + " " + _product.WeightUnits;
                    unitWeightLabel.Text = textLabel;
                }
                else
                {
                    //int currentUnits = Convert.ToInt16(unitWeightLabel.Text.Split(' ')[0]);
                    unitWeightLabel.Text = _product.Quantity.ToString();
                }
            }
            catch (Exception)
            {

            }
        }

        private void TimeElapsed(Object sender, ElapsedEventArgs e)
        {
            NSThread.MainThread.InvokeOnMainThread(() =>
            {
                Timer.Stop();

                if (FinalQuantity > 0)
                {
                    AddButtonTouchUpAction(FinalQuantity);
                }
                else if (FinalQuantity < 0)
                {
                    SubstractButtonTouchUpAction(FinalQuantity);
                }
            });
        }

        private void RecalculateQuantity(int quantity)
        {
            FinalQuantity = FinalQuantity + quantity;
        }
        #endregion
    }
}
