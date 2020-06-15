using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using GrupoExito.Android.Utilities;
using System;
using System.Collections.Generic;

namespace GrupoExito.Android.Adapters
{
    public class MySpinnerAdapter : ArrayAdapter<String>
    {
        public MySpinnerAdapter(Context context, int textViewResourceId, IList<string> objects) : base(context, textViewResourceId, objects)
        {
        }

        public MySpinnerAdapter(Activities.Payments.PaymentActivity paymentActivity, Context Context, int Resource, IList<string> Items) : base(Context, Resource, Items)
        {

        }

        public override View GetDropDownView(int position, View convertView, ViewGroup parent)
        {
            TextView view = (TextView)base.GetDropDownView(position, convertView, parent);
            view.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            return view;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            TextView view = (TextView)base.GetView(position, convertView, parent);
            view.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            return view;
        }
    }
}