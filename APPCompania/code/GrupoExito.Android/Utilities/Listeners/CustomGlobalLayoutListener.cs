using System;
using static Android.Views.ViewTreeObserver;

namespace GrupoExito.Android.Utilities
{
    public class CustomGlobalLayoutListener : Java.Lang.Object, IOnGlobalLayoutListener
    {
        public delegate void EventGlobalLayout(object sender, EventArgs e);
        public event EventGlobalLayout GlobalLayout;

        public CustomGlobalLayoutListener()
        {
        }        

        public void OnGlobalLayout()
        {
           GlobalLayout(this, null);
        }
    }
}