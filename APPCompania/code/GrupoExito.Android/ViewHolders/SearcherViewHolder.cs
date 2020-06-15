using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using GrupoExito.Android.Adapters;
using GrupoExito.Entities;
using System.Collections.Generic;

namespace GrupoExito.Android.ViewHolders
{
    public class SearcherViewHolder : RecyclerView.ViewHolder
    {
        public LinearLayout LySearcher { get; private set; }
        public TextView TvSuggest { get; set; }

        public SearcherViewHolder(View itemView, ISearcherAdapter searcherAdapter, List<Item> items) : base(itemView)
        {
            LySearcher = itemView.FindViewById<LinearLayout>(Resource.Id.lySearcher);
            TvSuggest = itemView.FindViewById<TextView>(Resource.Id.tvSuggestion);
            LySearcher.Click += delegate { searcherAdapter.OnSuggestSelected(items[AdapterPosition]); };
        }
    }
}