using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using GrupoExito.Entities.Entiites;
using System.Collections.Generic;

namespace GrupoExito.Android.ViewHolders
{
    public class ShoppingListViewHolder : RecyclerView.ViewHolder
    {
        public LinearLayout LyMyList { get; private set; }
        public ImageView IvLista { get; private set; }
        public ImageView IvItemDelete { get; private set; }
        public ImageView IvItemEdit { get; private set; }
        public TextView TvNameList { get; private set; }
        public TextView TvQuantityList { get; private set; }
        public TextView TvNameProductsList { get; private set; }
        public ShoppingListViewHolder(View itemView, Adapters.IMyList item, IList<ShoppingList> shoppingList) : base(itemView)
        {
            LyMyList = itemView.FindViewById<LinearLayout>(Resource.Id.lyMyList);
            IvLista = itemView.FindViewById<ImageView>(Resource.Id.ivLista);
            IvItemDelete = itemView.FindViewById<ImageView>(Resource.Id.ivItemDelete);
            IvItemEdit = itemView.FindViewById<ImageView>(Resource.Id.ivItemEdit);
            TvNameList = itemView.FindViewById<TextView>(Resource.Id.tvNameList);
            TvQuantityList = itemView.FindViewById<TextView>(Resource.Id.tvQuantityList);
            TvNameProductsList = itemView.FindViewById<TextView>(Resource.Id.tvNameProductsList);
            IvItemDelete.Click += delegate { item.OnDelateItemClicked(shoppingList[AdapterPosition]); };
            IvItemEdit.Click += delegate { item.OnEditItemClicked(shoppingList[AdapterPosition]); };
            LyMyList.Click += delegate { item.OnSelectItemClicked(shoppingList[AdapterPosition]); };
        }
    }
}