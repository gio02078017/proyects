using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace GrupoExito.Android.ViewHolders
{
    public class ProductsListViewHolder : RecyclerView.ViewHolder
    {
        public LinearLayout LyProductsCreateList { get; private set; }
        public ImageView IvProduct { get; private set; }
        public TextView TvProductName { get; private set; }
        public TextView TvDescription { get; private set; }
        public TextView TvPrice { get; private set; }
        public LinearLayout LyQuantity { get; private set; }
        public LinearLayout LyProductLabels { get; private set; }
        public LinearLayout LyUnit { get; private set; }
        public TextView TvUnit { get; private set; }
        public ImageView IvDelete { get; private set; }
        public LinearLayout LyWeight { get; private set; }
        public TextView TvWeight { get; private set; }
        public TextView TvPum { get; private set; }
        public View ViewProductDivider { get; private set; }
        public RelativeLayout RlQuantity { get; private set; }
        public ImageView IvSubstract { get; private set; }
        public TextView TvProductQuantity { get; private set; }
        public TextView TvProductQuantityWeight { get; private set; }
        public ImageView IvAdd { get; private set; }

        public ProductsListViewHolder(View itemView) : base(itemView)
        {
            LyProductsCreateList = itemView.FindViewById<LinearLayout>(Resource.Id.lyProductsCreateList);
            IvProduct = itemView.FindViewById<ImageView>(Resource.Id.ivProduct);
            TvProductName = itemView.FindViewById<TextView>(Resource.Id.tvProductName);
            TvDescription = itemView.FindViewById<TextView>(Resource.Id.tvDescription);
            TvPrice = itemView.FindViewById<TextView>(Resource.Id.tvPrice);
            TvPum = itemView.FindViewById<TextView>(Resource.Id.tvPum);
            LyQuantity = itemView.FindViewById<LinearLayout>(Resource.Id.lyQuantity);
            LyProductLabels = LyQuantity.FindViewById<LinearLayout>(Resource.Id.lyProductLabels);
            LyUnit = LyQuantity.FindViewById<LinearLayout>(Resource.Id.lyUnit);
            TvUnit = LyQuantity.FindViewById<TextView>(Resource.Id.tvUnit);
            LyWeight = LyQuantity.FindViewById<LinearLayout>(Resource.Id.lyWeight);
            TvWeight = LyQuantity.FindViewById<TextView>(Resource.Id.tvWeight);
            ViewProductDivider = LyQuantity.FindViewById<View>(Resource.Id.viewProductDivider);
            RlQuantity = LyQuantity.FindViewById<RelativeLayout>(Resource.Id.rlQuantity);
            IvSubstract = LyQuantity.FindViewById<ImageView>(Resource.Id.ivSubstract);
            TvProductQuantity = LyQuantity.FindViewById<TextView>(Resource.Id.tvProductQuantity);
            TvProductQuantityWeight = LyQuantity.FindViewById<TextView>(Resource.Id.tvProductQuantityWeight);
            IvAdd = LyQuantity.FindViewById<ImageView>(Resource.Id.ivAdd);
            IvDelete = LyQuantity.FindViewById<ImageView>(Resource.Id.ivDelete);            
        }
    }
}