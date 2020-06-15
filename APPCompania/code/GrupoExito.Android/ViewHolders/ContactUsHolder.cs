using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using GrupoExito.Entities.Entiites;
using System.Collections.Generic;

namespace GrupoExito.Android.ViewHolders
{
    class ContactUsHolder : RecyclerView.ViewHolder
    {   
        public TextView TvLocation { get; private set; }
        public TextView TvNumberTelePhone { get; private set; }
        public TextView TvSimpleNumber { get; private set; }
        
        public ContactUsHolder(View itemView, IList<Contact> listContacts) : base(itemView)
        {
            TvLocation = itemView.FindViewById<TextView>(Resource.Id.tvLocation);
            TvNumberTelePhone = itemView.FindViewById<TextView>(Resource.Id.tvNumberTelephone);
            TvSimpleNumber = itemView.FindViewById<TextView>(Resource.Id.tvSimpleNumber);
        }
    }
}