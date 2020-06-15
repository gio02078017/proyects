using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace GrupoExito.Android.Utilities
{
    public class CustomLinearLayoutManager : LinearLayoutManager
    {
        private bool isScrollEnabled = true;

        public CustomLinearLayoutManager(Context context) : base(context)
        {
        }

        public CustomLinearLayoutManager(Context context, int orientation, bool reverseLayout) : base(context, orientation, reverseLayout)
        {
        }

        public CustomLinearLayoutManager(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
        }

        public void SetScrollEnabled(bool flag)
        {
            this.isScrollEnabled = flag;
        }

        public override bool CanScrollHorizontally()
        {
            return base.CanScrollHorizontally();
        }
    }
}