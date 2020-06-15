using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Animation;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace GrupoExito.Android.Utilities
{
    public class CustomAnimatorListener : AnimatorListenerAdapter
    {

        public delegate void AnimationEndHandler(object sender, EventArgs e);
        public event AnimationEndHandler AnimationEnd;

        public delegate void AnimationStartHandler(object sender, EventArgs e);
        public event AnimationStartHandler AnimationStart;

        public CustomAnimatorListener()
        {
        }
        public override void OnAnimationEnd(Animator animation)
        {
            base.OnAnimationEnd(animation);
            AnimationEnd(this, null);
        }

        public override void OnAnimationCancel(Animator animation) { }
        public override void OnAnimationRepeat(Animator animation) { }
        public override void OnAnimationStart(Animator animation)
        {
            base.OnAnimationStart(animation);
            AnimationStart(this, null);
        }
    }
}