using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace GrupoExito.Android.ViewHolders
{
    public class FilterViewHolder : RecyclerView.ViewHolder
    {
        public CheckBox ChkFilterCategory { get; private set; }

        public FilterViewHolder(View itemView) : base(itemView)
        {
            ChkFilterCategory = itemView.FindViewById<CheckBox>(Resource.Id.chkFilter);
        }
    }
}