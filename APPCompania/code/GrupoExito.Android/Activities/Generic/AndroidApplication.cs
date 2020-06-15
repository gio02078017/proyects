using Android.App;
using System;
using Xamarin.Facebook.AppEvents;

namespace GrupoExito.Android.Activities.Generic
{
    [Application]
    public class AndroidApplication : Application
    {
        public static AndroidApplication Current { get; set; }
        public static Activity CurrentActivity { get; set; }       

        public AndroidApplication(IntPtr handle, global::Android.Runtime.JniHandleOwnership transfer)
            : base(handle, transfer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
            Couchbase.Lite.Support.Droid.Activate(this);
            Current = this;
            AppEventsLogger.ActivateApp(this);
        }       
    }
}