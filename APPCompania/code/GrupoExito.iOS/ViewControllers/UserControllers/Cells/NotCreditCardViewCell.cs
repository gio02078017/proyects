using System;
using Foundation;
using GrupoExito.iOS.Utilities.Constant;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.UserControllers.Cells
{
    public partial class NotCreditCardViewCell : UITableViewCell
    {
        #region Attributes
        public static readonly NSString Key = new NSString("NotCreditCardViewCell");
        public static readonly UINib Nib;
        private EventHandler addAction;
        #endregion

        #region Properties
        public EventHandler AddAction { get => addAction; set => addAction = value; }
        #endregion

        #region Constructors
        static NotCreditCardViewCell()
        {
            Nib = UINib.FromName("NotCreditCardViewCell", NSBundle.MainBundle);
        }

        protected NotCreditCardViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
        #endregion

        #region Overrides Methods
        public override void AwakeFromNib()
        {
            this.LoadHandlers();
            this.LoadCorners();
        }
        #endregion


        #region Public Methods
        public void SetTitle(string title)
        {
            titleLabel.Text = title;
        }

        public void SetDescription(string description)
        {
            descriptionLabel.Text = description;
        }

        public void ShowButton(string title)
        {
            actionButton.SetTitle(title, UIControlState.Normal);
            actionButton.Hidden = false;
        }

        public void HiddenTitle()
        {
            titleLabel.Hidden = true;
        }

        public void HiddenDescription()
        {
            descriptionLabel.Hidden = true;
        }

        public void HiddenButton()
        {
            actionButton.Hidden = true;
        }
        #endregion

        #region Private Methods
        private void LoadHandlers()
        {
            actionButton.TouchUpInside += SelectStoreButtonTouchUpInside;
        }

        private void LoadCorners()
        {
            actionButton.Layer.CornerRadius = ConstantStyle.CornerRadius;
        }
        #endregion

        void SelectStoreButtonTouchUpInside(object sender, EventArgs e)
        {
            addAction?.Invoke(sender, e);
        }
    }
}
