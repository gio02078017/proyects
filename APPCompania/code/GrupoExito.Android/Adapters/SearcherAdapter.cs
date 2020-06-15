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
    public class SearcherAdapter : RecyclerView.Adapter
    {
        private IList<Item> Items { get; set; }
        private Context Context { get; set; }
        private ISearcherAdapter SearcherInterface { get; set; }

        public override int ItemCount
        {
            get { return Items != null ? Items.Count() : 0; }
        }

        public SearcherAdapter(IList<Item> items, Context context, ISearcherAdapter iSearcherAdapter)
        {
            this.Items = items;
            this.Context = context;
            this.SearcherInterface = iSearcherAdapter;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ListItemSuggestion, parent, false);
            SearcherViewHolder categoryViewHolder = new SearcherViewHolder(itemView, SearcherInterface, Items.ToList());
            return categoryViewHolder;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            SearcherViewHolder searcherViewHolder = holder as SearcherViewHolder;
            Item item = Items[position];
            searcherViewHolder.TvSuggest.Text = item.Text;
            searcherViewHolder.TvSuggest.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayMedium), TypefaceStyle.Normal);
        }
    }
}
