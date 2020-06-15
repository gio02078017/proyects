using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using GrupoExito.Entities.Entiites;
using System.Collections.Generic;

namespace GrupoExito.Android.ViewHolders
{
    public class RecipeStepViewHolder : RecyclerView.ViewHolder
    {
        public TextView TvStep { get; private set; }
        public TextView TvItem { get; private set; }

        public RecipeStepViewHolder(View itemView, IList<string> listStep) : base(itemView)
        {
            TvStep = itemView.FindViewById<TextView>(Resource.Id.tvStep);
            TvItem = itemView.FindViewById<TextView>(Resource.Id.tvItem);
        }
    }
}