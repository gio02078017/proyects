using Com.Bumptech.Glide.Load;
using Com.Bumptech.Glide.Load.Engine;
using Com.Bumptech.Glide.Request;
using Com.Bumptech.Glide.Request.Target;
using System;

namespace GrupoExito.Android.Utilities
{
    public class CustomGlideListener : Java.Lang.Object, IRequestListener
    {
        public delegate void LoadImageFailed(object sender, EventArgs e);
        public event LoadImageFailed ImageFailed;

        public delegate void LoadImageSuccess(object sender, EventArgs e);
        public event LoadImageSuccess ImageSuccess;

        public CustomGlideListener()
        {
        }

        public bool OnLoadFailed(GlideException p0, Java.Lang.Object p1, ITarget p2, bool p3)
        {
            ImageFailed(this, null);
            return false;
        }

        public bool OnResourceReady(Java.Lang.Object p0, Java.Lang.Object p1, ITarget p2, DataSource p3, bool p4)
        {
            ImageSuccess(this, null);
            return false;
        }
    }
}