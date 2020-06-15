using Android.Content;
using Android.Graphics;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Com.Bumptech.Glide;
using GrupoExito.Android.Activities;
using GrupoExito.Android.Activities.Generic;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.ViewHolders;
using GrupoExito.Entities.Entiites;
using System.Collections.Generic;
using System.Linq;

namespace GrupoExito.Android.Adapters
{
    public class RecipeStepAdapter : RecyclerView.Adapter
    {
        private IList<string> ListStep;
        private RecipeStepViewHolder _RecipeStepViewHolder;
        private Context Context { get; set; }
        private bool StepPreparation;

        public RecipeStepAdapter(IList<string> ListStep, Context context, bool StepPreparation = false)
        {
            this.Context = context;
            this.ListStep = ListStep;
            this.StepPreparation = StepPreparation;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ListItemStepRecipe, parent, false);
            RecipeStepViewHolder _RecipeStepViewHolder = new RecipeStepViewHolder(itemView, ListStep);
            return _RecipeStepViewHolder;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            _RecipeStepViewHolder = holder as RecipeStepViewHolder;
            string itemStep = ListStep[position];
            _RecipeStepViewHolder.TvStep.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            _RecipeStepViewHolder.TvItem.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayMedium), TypefaceStyle.Normal);
            _RecipeStepViewHolder.TvStep.Text = StepPreparation ? (position + 1).ToString() : Context.GetString(Resource.String.str_asterisk); ;
            if (StepPreparation)
            {
                _RecipeStepViewHolder.TvStep.SetTextSize(ComplexUnitType.Sp, 12);
            }
            else
            {
                _RecipeStepViewHolder.TvStep.SetTextSize(ComplexUnitType.Sp, 12);
            }

            _RecipeStepViewHolder.TvItem.Text = itemStep;
        }



        public override int ItemCount
        {
            get { return ListStep != null ? ListStep.Count() : 0; }
        }
    }
}