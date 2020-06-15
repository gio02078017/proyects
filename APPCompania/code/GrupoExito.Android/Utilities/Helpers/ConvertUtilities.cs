using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Icu.Text;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Java.Util;
using System;
using System.Globalization;

namespace GrupoExito.Android.Utilities
{
    public static class ConvertUtilities
    {
        public static double StringToDouble(string number)
        {
            try
            {
                NumberFormat numberFormat = NumberFormat.GetInstance(Locale.Default);
                return numberFormat.Parse(number).DoubleValue();
            }
            catch (ClassNotFoundException)
            {
                return double.Parse(number);
            }
        }

        public static int StringToInteger(string number)
        {
            try
            {
                return Int32.Parse(number);
            }
            catch (FormatException)
            {
                return 0;
            }
        }

        public static int ResourceId(string imageName)
        {
            var resource = typeof(Resource.Drawable).GetField(imageName);
            return resource != null ? (int)resource.GetValue(null) : 0;
        }

        public static int ResourceId(string name, string type)
        {
            if (name != null && !name.Equals(""))
            {
                string imageName = !string.IsNullOrEmpty(type) ? name.ToLower() + "_" + type : name.ToLower();
                if (typeof(Resource.Drawable).GetField(imageName) != null)
                {
                    var resourceId = (int)typeof(Resource.Drawable).GetField(imageName).GetValue(null);
                    return resourceId;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        public static void MessageToast(string message, Context context, bool changeStyle = false)
        {
            if (message.Length > 0)
            {
                Toast toast = new Toast(context)
                {
                    Duration = ToastLength.Short
                };
                LayoutInflater _inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);
                View toastView = _inflater.Inflate(Resource.Layout.CustomToast, null);
                TextView texto = toastView.FindViewById<TextView>(Resource.Id.tvCustomToastText);
                texto.SetTypeface(FontManager.Instance.GetTypeFace(context, FontManager.RalewayMedium), TypefaceStyle.Normal);

                if (changeStyle)
                {
                    toastView.SetBackgroundResource(Resource.Drawable.button_blue_toast);
                    texto.SetTextColor(context.Resources.GetColor(Resource.Color.colorWhite));

                }

                texto.Text = message;
                toast.View = toastView;
                toast.Duration = ToastLength.Short;
                toast.SetGravity(GravityFlags.Center, 0, 0);
                toast.Show();
            }
        }

        public static void CustomColorMessageToast(string message, Context context, Color color, Color text)
        {
            if (message.Length > 0)
            {
                Toast toast = new Toast(context)
                {
                    Duration = ToastLength.Short
                };
                LayoutInflater _inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);
                View toastView = _inflater.Inflate(Resource.Layout.CustomToast, null);
                TextView texto = toastView.FindViewById<TextView>(Resource.Id.tvCustomToastText);
                texto.SetTypeface(FontManager.Instance.GetTypeFace(context, FontManager.RalewayMedium), TypefaceStyle.Normal);

                toastView.SetBackgroundResource(Resource.Drawable.button_blue_toast);
                toastView.Background = ConvertUtilities.ChangeColorButtonDrawable(context, 10, new Color(color));
                texto.SetTextColor(text);

                texto.Text = message;
                toast.View = toastView;
                toast.Duration = ToastLength.Short;
                toast.SetGravity(GravityFlags.Center, 0, 0);
                toast.Show();
            }
        }

        public static string CustomDatewithDayWeek(string date, bool capitalize = false)
        {
            DateTime parsedDate = DateTime.Parse(date, new CultureInfo("en-US", true));

            string finalDate = parsedDate.ToString("D", CultureInfo.CreateSpecificCulture("es-MX"));

            if (capitalize)
            {
                finalDate = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(finalDate);
            }

            return finalDate;
        }

        public static GradientDrawable ChangeColorButtonDrawable(Context context, int type, Color color)
        {

            GradientDrawable drawable = null;
            switch (type)
            {
                case 2:
                    drawable = (GradientDrawable)ContextCompat.GetDrawable(context, Resource.Drawable.button_base_little);
                    break;
                case 5:
                    drawable = (GradientDrawable)ContextCompat.GetDrawable(context, Resource.Drawable.button_base_half);
                    break;
                case 10:
                    drawable = (GradientDrawable)ContextCompat.GetDrawable(context, Resource.Drawable.button_base);
                    break;
                default:
                    drawable = (GradientDrawable)ContextCompat.GetDrawable(context, Resource.Drawable.button_base);
                    break;
            }

            if (drawable != null)
            {
                drawable.SetColor(color);
            }

            return drawable;
        }

        public static GradientDrawable ChangeColorCircleDrawable(Context context, int type, Color color)
        {
            GradientDrawable drawable = null;
            switch (type)
            {
                case 0:
                    drawable = (GradientDrawable)ContextCompat.GetDrawable(context, Resource.Drawable.circle_white);
                    break;
                case 2:
                    drawable = (GradientDrawable)ContextCompat.GetDrawable(context, Resource.Drawable.circle_write_with_size_small);
                    break;
                default:
                    drawable = (GradientDrawable)ContextCompat.GetDrawable(context, Resource.Drawable.circle_white);
                    break;
            }

            if (drawable != null)
            {
                drawable.SetColor(color);
            }

            return drawable;
        }

        public static GradientDrawable NewButtonDrawable(int type, Color color)
        {
            GradientDrawable drawable = new GradientDrawable();
            drawable.SetShape(ShapeType.Rectangle);
            drawable.SetCornerRadii(new float[] { 10, 10, 10, 10, 10, 10, 10, 10 });
            drawable.SetColor(Color.Green);
            return drawable;
        }

        public static int ConvertDpToPixels(double dp)
        {
            var pixelValue = Convert.ToInt32(dp * (double)Resources.System.DisplayMetrics.Density);
            return pixelValue;
        }
    }
}