using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace GrupoExito.Android.ViewHolders
{
    public class SelectedListViewHolder : RecyclerView.ViewHolder
    {
        public RadioButton RbList { get; private set; }
       
        public SelectedListViewHolder(View itemView) : base(itemView)
        {
            RbList = itemView.FindViewById<RadioButton>(Resource.Id.rbList);
                   
        }
    }
}