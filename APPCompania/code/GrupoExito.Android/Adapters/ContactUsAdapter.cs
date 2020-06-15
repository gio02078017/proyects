using Android.Content;
using Android.Graphics;
using Android.Support.V7.Widget;
using Android.Views;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.ViewHolders;
using GrupoExito.Entities.Entiites;
using System.Collections.Generic;
using System.Linq;

namespace GrupoExito.Android.Adapters
{
    public class ContactUsAdapter : RecyclerView.Adapter
    {
        private IList<Contact> ListContacts { get; set; }
        private ContactUsHolder _ContactUsHolder { get; set; }
        private Context Context { get; set; }

        public ContactUsAdapter(IList<Contact> listContacts, Context context)
        {
            this.Context = context;
            this.ListContacts = listContacts;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ListItemContactUs, parent, false);
            ContactUsHolder _ContactUsHolder = new ContactUsHolder(itemView, ListContacts);
            return _ContactUsHolder;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            _ContactUsHolder = holder as ContactUsHolder;
            Contact _Contact = ListContacts[position];
            _ContactUsHolder.TvLocation.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            _ContactUsHolder.TvNumberTelePhone.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayMedium), TypefaceStyle.Normal);
            _ContactUsHolder.TvLocation.Text = _Contact.Name;

            if (_Contact.Number.Contains("01 8000") || _Contact.Number.Contains("018000"))
            {
                _ContactUsHolder.TvNumberTelePhone.Visibility = ViewStates.Gone;
                _ContactUsHolder.TvSimpleNumber.Visibility = ViewStates.Visible;
                _ContactUsHolder.TvSimpleNumber.Text = _Contact.Number;
            }
            else
            {
                _ContactUsHolder.TvSimpleNumber.Visibility = ViewStates.Gone;
                _ContactUsHolder.TvNumberTelePhone.Visibility = ViewStates.Visible;
                _ContactUsHolder.TvNumberTelePhone.Text = _Contact.Number;

            }
        }

        public override int ItemCount
        {
            get { return ListContacts != null ? ListContacts.Count() : 0; }
        }
    }
}