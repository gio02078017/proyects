using Android.Content;
using Android.Graphics;

namespace GrupoExito.Android.Utilities
{

    public class FontManager
    {
        private static FontManager instance;

        public const string RalewayExtraBold = "Fonts/Raleway-ExtraBold.ttf";
        public const string RalewaySemiBold = "Fonts/Raleway-SemiBold.ttf";
        public const string RalewayMedium = "Fonts/Raleway-Medium.ttf";
        public const string GothamBlack = "Fonts/Gotham-Black.otf";
        public const string GothamMedium = "Fonts/Gotham-Medium.otf";

        private FontManager() { }

        public static FontManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FontManager();
                }

                return instance;
            }
        }

        public Typeface GetTypeFace(Context context, string fontName)
        {
            return Typeface.CreateFromAsset(context.Assets, fontName);
        }
    }
}