using Android.Animation;
using Android.Graphics;
using Android.Graphics.Drawables;
using Java.Lang;

namespace GrupoExito.Android.Utilities
{
    public class ShimmerDrawable : Drawable, ValueAnimator.IAnimatorUpdateListener
    {
        private readonly ValueAnimator.IAnimatorUpdateListener mUpdateListener;

        public void OnAnimationUpdate(ValueAnimator animation)
        {
            InvalidateSelf();
        }

        private readonly Paint mShimmerPaint = new Paint();
        private readonly Rect mDrawRect = new Rect();
        private readonly Matrix mShaderMatrix = new Matrix();
        private ValueAnimator mValueAnimator;
        private Shimmer mShimmer;

        public ShimmerDrawable()
        {
            mUpdateListener = this;
            mShimmerPaint.AntiAlias = true;
        }

        public void SetShimmer(Shimmer shimmer)
        {
            mShimmer = shimmer ?? throw new IllegalArgumentException("Given null shimmer");
            mShimmerPaint.SetXfermode(new PorterDuffXfermode(
                    mShimmer.AlphaShimmer ? PorterDuff.Mode.DstIn : PorterDuff.Mode.SrcIn));
            UpdateShader();
            UpdateValueAnimator();
            InvalidateSelf();
        }

        public void StartShimmer()
        {
            if (mValueAnimator != null && !IsShimmerStarted() && Callback != null)
            {
                mValueAnimator.Start();
            }
        }

        public void StopShimmer()
        {
            if (mValueAnimator != null && IsShimmerStarted())
            {
                mValueAnimator.Cancel();
            }
        }

        public bool IsShimmerStarted()
        {
            return mValueAnimator != null && mValueAnimator.IsStarted;
        }

        protected override void OnBoundsChange(Rect bounds)
        {
            base.OnBoundsChange(bounds);
            int width = bounds.Width();
            int height = bounds.Height();
            mDrawRect.Set(0, 0, width, height);
            UpdateShader();
            MaybeStartShimmer();
        }

        public override void Draw(Canvas canvas)
        {
            if (mShimmer == null || mShimmerPaint.Shader == null)
            {
                return;
            }

            float tiltTan = (float)Java.Lang.Math.Tan(Java.Lang.Math.ToRadians(mShimmer.Tilt));
            float translateHeight = mDrawRect.Height() + tiltTan * mDrawRect.Width();
            float translateWidth = mDrawRect.Width() + tiltTan * mDrawRect.Height();
            float dx;
            float dy;
            float animatedValue = mValueAnimator != null ? mValueAnimator.AnimatedFraction : 0f;

            switch (mShimmer.Direction)
            {
                default:
                case Shimmer.DirectionFlags.LEFT_TO_RIGHT:
                    dx = Offset(-translateWidth, translateWidth, animatedValue);
                    dy = 0;
                    break;
                case Shimmer.DirectionFlags.RIGHT_TO_LEFT:
                    dx = Offset(translateWidth, -translateWidth, animatedValue);
                    dy = 0f;
                    break;
                case Shimmer.DirectionFlags.TOP_TO_BOTTOM:
                    dx = 0f;
                    dy = Offset(-translateHeight, translateHeight, animatedValue);
                    break;
                case Shimmer.DirectionFlags.BOTTOM_TO_TOP:
                    dx = 0f;
                    dy = Offset(translateHeight, -translateHeight, animatedValue);
                    break;
            }

            mShaderMatrix.Reset();
            mShaderMatrix.SetRotate(mShimmer.Tilt, mDrawRect.Width() / 2f, mDrawRect.Height() / 2f);
            mShaderMatrix.PostTranslate(dx, dy);
            mShimmerPaint.Shader.SetLocalMatrix(mShaderMatrix);
            canvas.DrawRect(mDrawRect, mShimmerPaint);
        }

        public override void SetAlpha(int alpha)
        {
        }

        public override void SetColorFilter(ColorFilter colorFilter)
        {
        }

        public override int Opacity => mShimmer != null && (mShimmer.ClipToChildren || mShimmer.AlphaShimmer)
                ? (int)Format.Translucent
                : (int)Format.Opaque;

        private float Offset(float start, float end, float percent)
        {
            return start + (end - start) * percent;
        }

        private void UpdateValueAnimator()
        {
            if (mShimmer == null)
            {
                return;
            }

            bool started;

            if (mValueAnimator != null)
            {
                started = mValueAnimator.IsStarted;
                mValueAnimator.Cancel();
                mValueAnimator.RemoveAllUpdateListeners();
            }
            else
            {
                started = false;
            }

            mValueAnimator =
                ValueAnimator.OfFloat(0f, 1f + (float)(mShimmer.RepeatDelay / mShimmer.AnimationDuration));
            mValueAnimator.RepeatMode = mShimmer.RepeatMode;
            mValueAnimator.RepeatCount = mShimmer.RepeatCount;
            mValueAnimator.SetDuration(mShimmer.AnimationDuration + mShimmer.RepeatDelay);
            mValueAnimator.AddUpdateListener(mUpdateListener);

            if (started)
            {
                mValueAnimator.Start();
            }
        }

        public void MaybeStartShimmer()
        {
            if (mValueAnimator != null
                && !mValueAnimator.IsStarted
                && mShimmer != null
                && mShimmer.AutoStart
                && Callback != null)
            {
                mValueAnimator.Start();
            }
        }

        private void UpdateShader()
        {
            Rect bounds = Bounds;
            int boundsWidth = bounds.Width();
            int boundsHeight = bounds.Height();

            if (boundsWidth == 0 || boundsHeight == 0 || mShimmer == null)
            {
                return;
            }

            int width = mShimmer.Width(boundsWidth);
            int height = mShimmer.Height(boundsHeight);

            Shader shader;
            switch (mShimmer.Shape)
            {
                default:
                case Shimmer.ShapeFlags.LINEAR:
                    bool vertical =
                        mShimmer.Direction == Shimmer.DirectionFlags.TOP_TO_BOTTOM
                            || mShimmer.Direction == Shimmer.DirectionFlags.BOTTOM_TO_TOP;
                    int endX = vertical ? 0 : width;
                    int endY = vertical ? height : 0;
                    shader =
                        new LinearGradient(
                            0, 0, endX, endY, mShimmer.Colors, mShimmer.Positions, Shader.TileMode.Clamp);
                    break;
                case Shimmer.ShapeFlags.RADIAL:
                    shader =
                        new RadialGradient(
                            width / 2f,
                            height / 2f,
                            (float)(Java.Lang.Math.Max(width, height) / Java.Lang.Math.Sqrt(2)),
                            mShimmer.Colors,
                            mShimmer.Positions,
                            Shader.TileMode.Clamp);
                    break;
            }

            mShimmerPaint.SetShader(shader);
        }
    }
}