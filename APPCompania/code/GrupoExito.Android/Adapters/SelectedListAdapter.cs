using Android.Content;
using Android.Graphics;
using Android.Support.V7.Widget;
using Android.Views;
using Com.Bumptech.Glide.Load.Engine;
using Com.Bumptech.Glide.Request;
using GrupoExito.Android.Interfaces;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.ViewHolders;
using GrupoExito.Entities.Entiites;
using System.Collections.Generic;
using System.Linq;

namespace GrupoExito.Android.Adapters
{
    public class SelectedListAdapter : RecyclerView.Adapter
    {
        private IList<ShoppingList> listShoppingList { get; set; }
        private ISelectedList InterfaceMyList { get; set; }
        private Context Context { get; set; }

        private int lastSelectedPosition = -1;

        public SelectedListAdapter(IList<ShoppingList> listShoppingList, Context context, ISelectedList interfaceMyList)
        {
            this.InterfaceMyList = interfaceMyList;
            this.Context = context;
            this.listShoppingList = listShoppingList;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ListItemRadioButton, parent, false);
            SelectedListViewHolder _SelectedListViewHolder = new SelectedListViewHolder(itemView);
            SetViewHolderEvents(_SelectedListViewHolder);
            return _SelectedListViewHolder;
        }

        private void SetViewHolderEvents(SelectedListViewHolder _SelectedListViewHolder)
        {
            _SelectedListViewHolder.RbList.Click += delegate
            {

                lastSelectedPosition = _SelectedListViewHolder.AdapterPosition;
                NotifyDataSetChanged();
                InterfaceMyList.OnItemSelected(listShoppingList[lastSelectedPosition]);
            };

        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var requestOptions = new RequestOptions()
                 .Apply(RequestOptions.DiskCacheStrategyOf(DiskCacheStrategy.All)
                 .DontAnimate()
                 .DontTransform());

            SelectedListViewHolder _SelectedListViewHolder = holder as SelectedListViewHolder;
            ShoppingList _ShoppingList = listShoppingList[position];
            _SelectedListViewHolder.RbList.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            _SelectedListViewHolder.RbList.Text = _ShoppingList.Name;
            _SelectedListViewHolder.RbList.Checked = lastSelectedPosition == position;

        }

        public void EventSelected(RecommendProductsViewHolder recommendProductsViewHolder, bool Selected)
        {
            if (Selected)
            {
                recommendProductsViewHolder.IvSelect.SetImageResource(Resource.Drawable.seleccionar_primario);
                recommendProductsViewHolder.LyRecommendList.SetBackgroundResource(Resource.Drawable.button_little_card_caducity);
                recommendProductsViewHolder.LySelectList.SetBackgroundResource(Resource.Drawable.button_little_primary);
            }
            else
            {
                recommendProductsViewHolder.IvSelect.SetImageResource(Resource.Drawable.checkbox);
                recommendProductsViewHolder.LyRecommendList.SetBackgroundResource(Color.Transparent);
                recommendProductsViewHolder.LySelectList.SetBackgroundResource(Color.Transparent);
            }
        }


        public override int ItemCount
        {
            get { return listShoppingList != null ? listShoppingList.Count() : 0; }
        }

        public override void OnViewRecycled(Java.Lang.Object holder)
        {
            base.OnViewRecycled(holder);
            RecyclerView.ViewHolder rvHolder = holder as RecyclerView.ViewHolder;
            RecommendProductsViewHolder viewHolder = holder as RecommendProductsViewHolder;
        }
    }
}