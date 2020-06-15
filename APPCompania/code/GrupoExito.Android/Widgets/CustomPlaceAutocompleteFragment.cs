using Android.App;
using Android.Content;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.Gms.Location.Places;
using Android.Gms.Location.Places.UI;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;

namespace GrupoExito.Android.Widgets
{
    public class CustomPlaceAutoCompleteFragment : PlaceAutocompleteFragment
    {
        private EditText EditSearch;
        private View ZzaRh { get; set; }
        private View ZzaRi { get; set; }
        private EditText ZzaRj { get; set; }
        private LatLngBounds ZzaRk { get; set; }
        private AutocompleteFilter ZzaRl { get; set; }
        private IPlaceSelectionListener ZzaRm { get; set; }

        public CustomPlaceAutoCompleteFragment()
        {
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View var4 = inflater.Inflate(Resource.Layout.LayoutAutocomplete, container, false);
            EditSearch = (EditText)var4.FindViewById(Resource.Id.editWorkLocation);
            EditSearch.Click += delegate { ZzzG(); };
            return var4;
        }

        public override void OnDestroyView()
        {
            this.ZzaRh = null;
            this.ZzaRi = null;
            this.EditSearch = null;
            base.OnDestroyView();
        }

        public override void SetBoundsBias(LatLngBounds bounds)
        {
            this.ZzaRk = bounds;
        }

        public override void SetFilter(AutocompleteFilter filter)
        {
            this.ZzaRl = filter;
        }

        public new void SetText(String text)
        {
            this.EditSearch.Text = text;
        }

        public new void SetHint(String hint)
        {
            this.EditSearch.Hint = hint;
            this.ZzaRh.ContentDescription = hint;
        }

        public override void SetOnPlaceSelectedListener(IPlaceSelectionListener placeSelectionListener)
        {
            this.ZzaRm = placeSelectionListener;
        }

        private void ZzzF()
        {
            bool var = !EditSearch.Text.ToString().Equals("");
        }

        private void ZzzG()
        {
            int varInt = -1;

            try
            {
                Intent varIntent = (new PlaceAutocomplete.IntentBuilder(2)).SetBoundsBias(this.ZzaRk).SetFilter(this.ZzaRl).Build(this.Activity);
                this.StartActivityForResult(varIntent, 1);
            }
            catch (GooglePlayServicesRepairableException var3)
            {
                varInt = var3.ConnectionStatusCode;
            }
            catch (GooglePlayServicesNotAvailableException var4)
            {
                varInt = var4.ErrorCode;
            }

            if (varInt != -1)
            {
                GoogleApiAvailability var5 = GoogleApiAvailability.Instance;
                var5.ShowErrorDialogFragment(this.Activity, varInt, 2);
            }
        }

        public void onActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (requestCode == 1)
            {
                if (resultCode == Result.Ok)
                {
                    IPlace varPlace = PlaceAutocomplete.GetPlace(this.Activity, data);

                    if (this.ZzaRm != null)
                    {
                        this.ZzaRm.OnPlaceSelected(varPlace);
                    }

                    this.SetText(varPlace.NameFormatted.ToString());
                }
                else
                {
                    Statuses varStatuses = PlaceAutocomplete.GetStatus(this.Activity, data);

                    if (this.ZzaRm != null)
                    {
                        this.ZzaRm.OnError(varStatuses);
                    }
                }
            }

            base.OnActivityResult(requestCode, resultCode, data);
        }
    }
}