using Android.Content;
using Android.Graphics;
using Android.Support.V7.Widget;
using Android.Views;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.ViewHolders;
using GrupoExito.Entities;
using System.Collections.Generic;
using System.Linq;

namespace GrupoExito.Android.Adapters
{
    public class MyAddressAdapter : RecyclerView.Adapter
    {
        private IList<UserAddress> ListUserAddress { get; set; }
        private Context Context { get; set; }
        private IMyAddress AddressInterface { get; set; }
        private bool FromHome { get; set; }

        public MyAddressAdapter(IList<UserAddress> listUserAddress, Context context, IMyAddress addressInterface, bool fromHome = false)
        {
            this.ListUserAddress = listUserAddress;
            this.Context = context;
            this.AddressInterface = addressInterface;
            this.FromHome = fromHome;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ListItemMyAddress, parent, false);
            MyAddressViewHolder myAccountViewHolder = new MyAddressViewHolder(itemView, AddressInterface, ListUserAddress);
            return myAccountViewHolder;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            MyAddressViewHolder myAddressViewHolder = holder as MyAddressViewHolder;
            UserAddress userAddress = ListUserAddress[position];
            myAddressViewHolder.TvItemTypeAddress.Text = userAddress.Description;
            myAddressViewHolder.TvItemAddress.Text = userAddress.AddressComplete;

            if (FromHome)
            {
                if (ConvertUtilities.ResourceId(userAddress.Description, "primario") != 0)
                {
                    myAddressViewHolder.IvItemAddress.SetImageResource(ConvertUtilities.ResourceId(userAddress.Description, "primario"));
                }
                else
                {
                    myAddressViewHolder.IvItemAddress.SetImageResource(ConvertUtilities.ResourceId("otro", "primario"));
                }

                myAddressViewHolder.TvItemTypeAddress.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
                myAddressViewHolder.TvItemAddress.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayMedium), TypefaceStyle.Normal);
            }
            else
            {
                if (ConvertUtilities.ResourceId(userAddress.Description, "") != 0)
                {
                    myAddressViewHolder.IvItemAddress.SetImageResource(ConvertUtilities.ResourceId(userAddress.Description, ""));
                }
                else
                {
                    myAddressViewHolder.IvItemAddress.SetImageResource(ConvertUtilities.ResourceId("otro", ""));
                }

                myAddressViewHolder.TvItemTypeAddress.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
                myAddressViewHolder.TvItemAddress.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            }

            myAddressViewHolder.ViewDivider.Visibility = FromHome ? ViewStates.Gone : ViewStates.Visible;
            myAddressViewHolder.ViewTopDivider.Visibility = FromHome ? ViewStates.Gone : ViewStates.Visible;

            if (userAddress.IsDefaultAddress && FromHome)
            {
                myAddressViewHolder.LyAddress.SetBackgroundResource(Resource.Drawable.button_yellow);
                myAddressViewHolder.TvItemAddress.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
                myAddressViewHolder.IvItemDeleteAddress.Visibility = ViewStates.Gone;
            }
            else
            {
                myAddressViewHolder.LyAddress.SetBackgroundColor(Color.White);
                myAddressViewHolder.IvItemDeleteAddress.Visibility = ViewStates.Visible;

                if (FromHome)
                {
                    myAddressViewHolder.TvItemAddress.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayMedium), TypefaceStyle.Normal);
                }
                else
                {
                    myAddressViewHolder.TvItemAddress.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
                }
            }

            if (userAddress.IsDefaultAddress)
            {
                myAddressViewHolder.TvItemAddress.SetTextColor(Color.White);
                myAddressViewHolder.IvItemEditAddress.SetImageResource(Resource.Drawable.lapiz_primario);
                myAddressViewHolder.TvItemTypeAddress.SetTextColor(Color.White);

                if (FromHome)
                {
                    if (ConvertUtilities.ResourceId(userAddress.Description, "primario") != 0)
                    {
                        myAddressViewHolder.IvItemAddress.SetImageResource(ConvertUtilities.ResourceId(userAddress.Description, "primario"));
                    }
                    else
                    {
                        myAddressViewHolder.IvItemAddress.SetImageResource(ConvertUtilities.ResourceId("otro", "primario"));
                    }
                }
                else
                {
                    if (ConvertUtilities.ResourceId(userAddress.Description, "") != 0)
                    {
                        myAddressViewHolder.IvItemAddress.SetImageResource(ConvertUtilities.ResourceId(userAddress.Description, ""));
                    }
                    else
                    {
                        myAddressViewHolder.IvItemAddress.SetImageResource(ConvertUtilities.ResourceId("otro", ""));
                    }
                }
            }
            else
            {
                myAddressViewHolder.TvItemAddress.SetTextColor(Color.Black);
                myAddressViewHolder.TvItemTypeAddress.SetTextColor(Color.Black);

                if (ConvertUtilities.ResourceId(userAddress.Description, "") != 0)
                {
                    myAddressViewHolder.IvItemAddress.SetImageResource(ConvertUtilities.ResourceId(userAddress.Description, ""));
                }
                else
                {
                    myAddressViewHolder.IvItemAddress.SetImageResource(ConvertUtilities.ResourceId("otro", ""));
                }

                myAddressViewHolder.IvItemEditAddress.SetImageResource(Resource.Drawable.lapiz_editar);
            }
        }

        public override int ItemCount
        {
            get { return ListUserAddress != null ? ListUserAddress.Count() : 0; }
        }
    }
}