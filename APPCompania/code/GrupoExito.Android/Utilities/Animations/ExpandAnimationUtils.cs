using Android.Animation;
using Android.Views;
using System.Collections.Generic;

namespace GrupoExito.Android.Utilities
{
    public class ExpandAnimationUtils
    {
        public static List<Animator> Build(ViewGroup view, int pivotX, int pivotY, float fraction, int duration, int delay)
        {
            List<Animator> animatorList = new List<Animator>();

            for (int i = 0; i < view.ChildCount; i++)
            {
                View childView = view.GetChildAt(i);

                int deltaX = pivotX - (childView.Left + childView.Width / 2);
                int deltaY = pivotY - (childView.Top + childView.Height / 2);

                ObjectAnimator xAnim = ObjectAnimator.OfFloat(childView, "translationX", fraction * deltaX, 0);
                ObjectAnimator yAnim = ObjectAnimator.OfFloat(childView, "translationY", fraction * deltaY, 0);

                xAnim.SetDuration(duration);
                xAnim.StartDelay = delay;
                yAnim.SetDuration(duration);
                yAnim.StartDelay = delay;

                animatorList.Add(xAnim);
                animatorList.Add(yAnim);
            }

            ObjectAnimator alphaAnim = ObjectAnimator.OfFloat(view, "alpha", 0f, 1f);

            alphaAnim.AnimationStart += (sender, args) =>
            {
                view.Visibility = ViewStates.Visible;
            };

            alphaAnim.SetDuration(duration);
            alphaAnim.StartDelay = delay;
            animatorList.Add(alphaAnim);

            return animatorList;
        }

        public static List<Animator> BuildReversed(ViewGroup view, int pivotX, int pivotY, float fraction, int duration, int delay)
        {
            List<Animator> animatorList = new List<Animator>();

            for (int i = 0; i < view.ChildCount; i++)
            {
                View childView = view.GetChildAt(i);

                int deltaX = pivotX - (childView.Left + childView.Width / 2);
                int deltaY = pivotY - (childView.Top + childView.Height / 2);

                ObjectAnimator xAnim = ObjectAnimator.OfFloat(childView, "translationX", 0, fraction * deltaX);
                ObjectAnimator yAnim = ObjectAnimator.OfFloat(childView, "translationY", 0, fraction * deltaY);

                xAnim.SetDuration(duration);
                xAnim.StartDelay = delay;
                yAnim.SetDuration(duration);
                yAnim.StartDelay = delay;

                animatorList.Add(xAnim);
                animatorList.Add(yAnim);
            }

            ObjectAnimator alphaAnim = ObjectAnimator.OfFloat(view, "alpha", 1f, 0f);

            alphaAnim.AnimationEnd += (sender, args) =>
            {
                view.Visibility = ViewStates.Invisible;
            };

            alphaAnim.SetDuration(duration);
            alphaAnim.StartDelay = delay;
            animatorList.Add(alphaAnim);

            return animatorList;
        }
    }
}