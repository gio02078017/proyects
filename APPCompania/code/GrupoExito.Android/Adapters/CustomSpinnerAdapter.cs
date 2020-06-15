using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using System;

namespace GrupoExito.Android.Utilities
{
    public class CustomSpinnerAdapter : ArrayAdapter<String>
    {
        private Typeface Typeface { get; set; }

        public CustomSpinnerAdapter(Context context, int resource, string[] objects, Typeface typeface) : base(context, resource, objects)
        {
            this.Typeface = typeface;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            TextView view = (TextView)  base.GetView(position, convertView, parent);
            view.SetTypeface(Typeface, TypefaceStyle.Normal);
            return base.GetView(position, convertView, parent);
        }

        public override View GetDropDownView(int position, View convertView, ViewGroup parent)
        {
            TextView view = (TextView)base.GetView(position, convertView, parent);
            view.SetTypeface(Typeface, TypefaceStyle.Normal);
            return base.GetDropDownView(position, convertView, parent);
        }
    }
}