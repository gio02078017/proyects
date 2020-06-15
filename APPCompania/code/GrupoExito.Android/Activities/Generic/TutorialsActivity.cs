using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Widget;
using GrupoExito.Android.Activities.Base;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.Utilities.Constants;
using GrupoExito.DataAgent.Services.Generic;
using GrupoExito.Entities.Entiites;
using GrupoExito.Entities.Entiites.Generic.Contents;
using GrupoExito.Logic.Models.Generic;
using System.Collections.Generic;
using System.Linq;

namespace GrupoExito.Android.Activities.Generic
{
    [Activity(Label = "Tutoriales", ScreenOrientation = ScreenOrientation.Portrait)]
    public class TutorialsActivity : BaseActivity
    {

        #region Controls

        private LinearLayout LyTutorial;
        private ImageView IvTutorial;

        #endregion

        #region Properties

        private Bitmap bitImage, bitMapScaled;
        private IList<Tutorial> ListTutorials;
        private ContentsModel _ContentsModel;
        private Tutorial _TutorialItem;
        private int CountImages;
        private int NextPositionImage;
        private bool DisableBackPressed = false;
        private string ActivityTutorial { get; set; }

        #endregion

        public override void OnBackPressed()
        {
            if (!DisableBackPressed)
            {
                Finish();
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ActivityTutorials);
            _ContentsModel = new ContentsModel(new ContentsService(DeviceManager.Instance));

            if (SupportActionBar != null)
            {
                SupportActionBar.Hide();
            }

            SetControlsProperties();
            Bundle bundle = Intent.Extras;

            if (bundle != null)
            {
                if (!string.IsNullOrEmpty(Intent.Extras.GetString(ConstantPreference.ActivityTutorial)))
                {
                    DisableBackPressed = true;
                    ActivityTutorial = Intent.Extras.GetString(ConstantPreference.ActivityTutorial);
                    LogicalTutorial();
                }
                else
                {
                    OnBackPressed();
                    Finish();
                }
            }
            else
            {
                OnBackPressed();
                Finish();
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            bitImage.Recycle();
            bitMapScaled.Recycle();
        }

        private void SetControlsProperties()
        {
            LyTutorial = FindViewById<LinearLayout>(Resource.Id.lyTutorial);
            IvTutorial = FindViewById<ImageView>(Resource.Id.ivTutorial);
            LyTutorial.Click += delegate { EventTutorial(); };
            IvTutorial.Click += delegate { EventTutorial(); };
        }

        private void DrawTutorial(string image)
        {
            var metrics = Resources.DisplayMetrics;
            var height = metrics.HeightPixels;
            var width = metrics.WidthPixels;
            bitImage = BitmapFactory.DecodeResource(this.Resources, ConvertUtilities.ResourceId(image));
            bitMapScaled = Bitmap.CreateScaledBitmap(bitImage, width, height, false);
            IvTutorial.SetImageBitmap(bitMapScaled);
        }

        private void EventTutorial()
        {
            if (NextPositionImage == CountImages)
            {
                DisableBackPressed = false;
                OnBackPressed();
                Finish();
                DeviceManager.Instance.SaveAccessPreference(ActivityTutorial, true);
            }
            else
            {
                DrawTutorial(_TutorialItem.ImagesTutorial[NextPositionImage].Image);
                NextPositionImage = NextPositionImage + 1;
            }
        }

        private void LogicalTutorial()
        {
            ListTutorials = _ContentsModel.GetTutorials();

            if (ListTutorials != null && ListTutorials.Any())
            {
                foreach (var tutorial in ListTutorials)
                {
                    if (tutorial.Name.Equals(ActivityTutorial))
                    {
                        _TutorialItem = tutorial;
                    }
                }
            }

            if (_TutorialItem != null)
            {
                CountImages = _TutorialItem.ImagesTutorial.Count;
                DrawTutorial(_TutorialItem.ImagesTutorial[NextPositionImage].Image);
                NextPositionImage = NextPositionImage + 1;
            }
            else
            {
                OnBackPressed();
            }
        }
    }
}
