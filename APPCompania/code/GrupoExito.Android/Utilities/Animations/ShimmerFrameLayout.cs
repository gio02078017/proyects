using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace GrupoExito.Android.Utilities
{
    [Register("app.exito.dev.utilities.shimmer")]
    public class ShimmerFrameLayout : FrameLayout
    {
        private readonly Paint mContentPaint = new Paint();
        private readonly ShimmerDrawable mShimmerDrawable = new ShimmerDrawable();

        public ShimmerFrameLayout(Context context) : base(context)
        {
            Init(context, null);
        }

        public ShimmerFrameLayout(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Init(context, attrs);
        }

        public ShimmerFrameLayout(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Init(context, attrs);
        }

        public ShimmerFrameLayout(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            Init(context, attrs);
        }

        private void Init(Context context, IAttributeSet attrs)
        {
            SetWillNotDraw(false);
            mShimmerDrawable.SetCallback(this);

            if (attrs == null)
            {
                SetShimmer(new Shimmer.AlphaHighlightBuilder().Build());
                return;
            }

            TypedArray a = context.ObtainStyledAttributes(attrs, Resource.Styleable.ShimmerFrameLayout, 0, 0);
            try
            {
                Shimmer.Builder shimmerBuilder = a.HasValue(Resource.Styleable.ShimmerFrameLayout_shimmer_colored)
                            && a.GetBoolean(Resource.Styleable.ShimmerFrameLayout_shimmer_colored, false)
                        ? (Shimmer.Builder) new Shimmer.ColorHighlightBuilder()
                        : (Shimmer.Builder) new Shimmer.AlphaHighlightBuilder();

                shimmerBuilder.SetDuration(200);
                shimmerBuilder.SetRepeatDelay(900);


                SetShimmer(shimmerBuilder.ConsumeAttributes(a).Build());
            }
            finally
            {
                a.Recycle();
            }
        }

        public ShimmerFrameLayout SetShimmer(Shimmer shimmer)
        {
            if (shimmer == null)
            {
                throw new IllegalArgumentException("Given null shimmer");
            }

            mShimmerDrawable.SetShimmer(shimmer);
            if (shimmer.ClipToChildren)
            {
                SetLayerType(LayerType.Hardware, mContentPaint);
            }
            else
            {
                SetLayerType(LayerType.None, null);
            }

            return this;
        }

        public void StartShimmer()
        {
            mShimmerDrawable.StartShimmer();
        }

        public void StopShimmer()
        {
            mShimmerDrawable.StopShimmer();
        }

        public bool IsShimmerStarted()
        {
            return mShimmerDrawable.IsShimmerStarted();
        }

        protected override void OnLayout(bool changed, int left, int top, int right, int bottom)
        {
            base.OnLayout(changed, left, top, right, bottom);
            int width = Width;
            int height = Height;
            mShimmerDrawable.SetBounds(0, 0, width, height);
        }

        protected override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();
            mShimmerDrawable.MaybeStartShimmer();
        }

        protected override void OnDetachedFromWindow()
        {
            base.OnDetachedFromWindow();
            StopShimmer();
        }

        protected override void DispatchDraw(Canvas canvas)
        {
            base.DispatchDraw(canvas);
            mShimmerDrawable.Draw(canvas);
        }

        protected override bool VerifyDrawable(Drawable who)
        {
            return base.VerifyDrawable(who) || who == mShimmerDrawable;
        }
    }
}