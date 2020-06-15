using System;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using GrupoExito.iOS.Models;
using UIKit;

namespace GrupoExito.iOS.Views.PaymentViews.SummaryViews
{
    public partial class PurchaseSummaryViewCell : UITableViewCell
    {
        #region Attributes
        public static readonly NSString Key = new NSString("PurchaseSummaryViewCell");
        public static readonly UINib Nib;
        #endregion

        #region Constructors
        static PurchaseSummaryViewCell()
        {
            Nib = UINib.FromName("PurchaseSummaryViewCell", NSBundle.MainBundle);
        }

        protected PurchaseSummaryViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
        #endregion
        public override void LayoutSubviews()
        {
            this.LayoutIfNeeded();
        }

        #region Methods
        public void LoadData(PurchaseSummary purchaseSummary)
        {
            titleLabel.Text = purchaseSummary.title;
            valueLabel.Text = purchaseSummary.value;
            if (purchaseSummary.IsVerticalStackView)
            {
                contentViewStackView.Axis = UILayoutConstraintAxis.Vertical;
                valueLabel.TextAlignment = UITextAlignment.Left;
            }
            else
            {
                contentViewStackView.Axis = UILayoutConstraintAxis.Horizontal;
                valueLabel.TextAlignment = UITextAlignment.Right;
            }
            if (purchaseSummary.HasContinuousLine)
            {
                dashedLineView.BackgroundColor = UIColor.GroupTableViewBackgroundColor;
            }
            else
            {
                dashedLineView.BackgroundColor = UIColor.White;
                CreateDottedLine(dashedLineView);
            }
        }

        public void CreateDottedLine(UIView separatorView)
        {
            CAShapeLayer dashedLayer = new CAShapeLayer();
            CGRect frameSize = separatorView.Bounds;
            dashedLayer.Bounds = frameSize;
            dashedLayer.Position = new CGPoint(frameSize.Width / 2, frameSize.Height / 2);
            dashedLayer.FillColor = UIColor.Clear.CGColor;
            dashedLayer.StrokeColor = UIColor.GroupTableViewBackgroundColor.CGColor;
            dashedLayer.LineWidth = 1f;
            dashedLayer.LineJoin = CAShapeLayer.JoinRound;
            NSNumber[] patternArray = { 5, 5 };
            dashedLayer.LineDashPattern = patternArray;
            CGPath path = new CGPath();
            path.MoveToPoint(CGPoint.Empty);
            path.AddLineToPoint(new CGPoint(frameSize.Width, 0));
            dashedLayer.Path = path;

            separatorView.Layer.AddSublayer(dashedLayer);
        }
        #endregion
    }
}
