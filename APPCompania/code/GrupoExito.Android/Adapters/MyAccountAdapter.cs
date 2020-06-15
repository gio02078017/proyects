using Android.Content;
using Android.Graphics;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.ViewHolders;
using GrupoExito.Entities.Entiites;
using System.Collections.Generic;
using System.Linq;

namespace GrupoExito.Android.Adapters
{
    public class MyAccountAdapter : RecyclerView.Adapter
    {
        private IList<MenuItem> ListMenuItems { get; set; }
        private Context Context { get; set; }
        private IItemMenu MyAccountInterface { get; set; }
        private int LayoutHeight { get; set; }

        public MyAccountAdapter(IList<MenuItem> listMenuItems, Context context, IItemMenu MyAccountInterface)
        {
            this.ListMenuItems = listMenuItems;
            this.Context = context;
            this.MyAccountInterface = MyAccountInterface;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ListItemMyAccount, parent, false);
            MyAccountViewHolder myAccountViewHolder = new MyAccountViewHolder(itemView, MyAccountInterface, ListMenuItems);
            return myAccountViewHolder;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            MyAccountViewHolder myAccountViewHolder = holder as MyAccountViewHolder;
            MenuItem menuItem = ListMenuItems[position];
            myAccountViewHolder.IvItemAccount.SetImageResource(ConvertUtilities.ResourceId(menuItem.IconBlue));
            myAccountViewHolder.TvTitleItemAccount.Text = menuItem.Title;
            
            LinearLayout.LayoutParams lastTxtParams = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);

            if (!menuItem.Ispair)
            {
                if (position == 0)
                {
                    lastTxtParams.SetMargins(20, 0, 0, 0);
                    myAccountViewHolder.TvTitleItemAccount.LayoutParameters = lastTxtParams;
                    myAccountViewHolder.LyItemMyAccount.Orientation = LinearLayoutCompat.Horizontal;
                    myAccountViewHolder.ViewDivider.Visibility = ViewStates.Gone;
                }
                else
                {
                    lastTxtParams.TopMargin = 10;
                    myAccountViewHolder.TvTitleItemAccount.LayoutParameters = lastTxtParams;
                }

                if (position % 2 != 0)
                {
                    myAccountViewHolder.LyItemMyAccount.SetBackgroundResource(Resource.Drawable.border_right_along);
                }
            }
            else
            {
                lastTxtParams.TopMargin = 10;
                myAccountViewHolder.TvTitleItemAccount.LayoutParameters = lastTxtParams;

                if (position == 0 || position % 2 == 0)
                {
                    myAccountViewHolder.LyItemMyAccount.SetBackgroundResource(Resource.Drawable.border_right_along);
                }

                if (position <= 1)
                {
                    myAccountViewHolder.ViewDivider.Visibility = ViewStates.Gone;
                }
            }

            myAccountViewHolder.TvTitleItemAccount.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewaySemiBold), TypefaceStyle.Normal);           
        }

        public override int ItemCount
        {
            get { return ListMenuItems != null ? ListMenuItems.Count() : 0; }
        }
    }
}