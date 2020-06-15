using Android.Support.V4.View;
using Android.Views;

namespace GrupoExito.Android.Utilities
{
    public class ViewUtils
    {
        private ViewUtils()
        {
        }

        public static bool SetVisibility(View view, ViewStates visible)
        {
            ViewStates visibility;
            if (visible == ViewStates.Visible)
            {
                visibility = ViewStates.Visible;
            }
            else
            {
                visibility = ViewStates.Gone;
            }
            view.Visibility = visibility;
            return visible == ViewStates.Visible;
        }

        public static bool SetInVisibility(View v, bool visible)
        {
            ViewStates visibility;
            if (visible)
            {
                visibility = ViewStates.Visible;
            }
            else
            {
                visibility = ViewStates.Invisible;
            }
            v.Visibility = visibility;
            return visible;
        }

        public static float CenterX(View view)
        {
            return view.GetX() + (view.Width / 2f);
        }

        public static float CenterY(View view)
        {
            return view.GetY() + view.Height / 2f;
        }

        public static int GetRelativeTop(View myView)
        {
            if (myView.Parent == myView.RootView)
                return myView.Top;
            else
                return myView.Top + GetRelativeTop((View)myView.Parent);
        }
    }
}