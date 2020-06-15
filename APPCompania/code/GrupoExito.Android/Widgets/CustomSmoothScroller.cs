using Android.Content;
using Android.Support.V7.Widget;

namespace GrupoExito.Android.Widgets
{
    public class CustomSmoothScroller : LinearSmoothScroller
    {
        public CustomSmoothScroller(Context context) : base(context)
        {
        }

        protected override int VerticalSnapPreference => SnapToStart;
    }
}