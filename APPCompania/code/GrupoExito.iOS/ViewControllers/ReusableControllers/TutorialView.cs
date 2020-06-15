using System;
using Foundation;
using GrupoExito.iOS.Utilities.Constant;
using ObjCRuntime;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ReusableControllers
{
    public partial class TutorialView : UIView
    {
        #region Properties 
        public UIImageView TutorialImageView{get => tutorialImageView;}
        public UIButton TutorialButton{get => tutorialButton;}
        public UIView ViewTutorial { get => viewTutorial; }
        #endregion

        public static TutorialView Create()
        {
            var arr = NSBundle.MainBundle.LoadNib(ConstantReusableViewName.TutorialView, null, null);
            var v = Runtime.GetNSObject<TutorialView>(arr.ValueAt(0));
            return v;
        }

        public TutorialView(IntPtr handle) : base(handle)
        {
        }
    }
}

