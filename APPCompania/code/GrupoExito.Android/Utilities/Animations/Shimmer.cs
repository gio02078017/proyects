using Android.Animation;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Util;
using Java.Lang;
using System;

namespace GrupoExito.Android.Utilities
{
    public class Shimmer : IDisposable
    {
        private static readonly int COMPONENT_COUNT = 4;

        public class ShapeFlags
        {
            public const int LINEAR = 0;
            public const int RADIAL = 1;
        }

        public class DirectionFlags
        {
            public const int LEFT_TO_RIGHT = 0;
            public const int TOP_TO_BOTTOM = 1;
            public const int RIGHT_TO_LEFT = 2;
            public const int BOTTOM_TO_TOP = 3;
        }
        
        private RectF Bounds = new RectF();

        private float WidthRatio = 1f;
        private float HeightRatio = 1f;
        private float Intensity = 0f;
        private float Dropoff = 0.5f;     
        
          private int[] colors = new int[COMPONENT_COUNT];
        public int[] Colors { get => colors; set => colors = value; }

        private long animationDuration = 1000L;
        public long AnimationDuration { get => animationDuration; set => animationDuration = value; }

        private long repeatDelay;
        public long RepeatDelay { get => repeatDelay; set => repeatDelay = value; }

        private bool alphaShimmer = true;
        public bool AlphaShimmer { get => alphaShimmer; set => alphaShimmer = value; }

        private float tilt = 20f;
        public float Tilt { get => tilt; set => tilt = value; }

        private bool autoStart = true;
        public bool AutoStart { get => autoStart; set => autoStart = value; }

        private bool clipToChildren = true;
        public bool ClipToChildren { get => clipToChildren; set => clipToChildren = value; }

        private int repeatCount = ValueAnimator.Infinite;
        public int RepeatCount { get => repeatCount; set => repeatCount = value; }

        private ValueAnimatorRepeatMode repeatMode = ValueAnimatorRepeatMode.Restart;
        public ValueAnimatorRepeatMode RepeatMode { get => repeatMode; set => repeatMode = value; }

        private int direction = DirectionFlags.LEFT_TO_RIGHT;
        public int Direction { get => direction; set => direction = value; }

        private int shape = ShapeFlags.LINEAR;
        public int Shape { get => shape; set => shape = value; }

        private int highlightColor = Color.White;
        public int HighlightColor { get => highlightColor; set => highlightColor = value; }

        private int baseColor = 0x4cffffff;
        public int BaseColor { get => baseColor; set => baseColor = value; }

        private int fixedWidth = 0;
        public int FixedWidth { get => fixedWidth; set => fixedWidth = value; }

        private int fixedHeight = 0;
        public int FixedHeight { get => fixedHeight; set => fixedHeight = value; }

        private float[] positions = new float[COMPONENT_COUNT];
        public float[] Positions { get => positions; set => positions = value; }

        public int Width(int width)
        {
            return FixedWidth > 0 ? FixedWidth : Java.Lang.Math.Round(WidthRatio * width);
        }

        public int Height(int height)
        {
            return FixedHeight > 0 ? FixedHeight : Java.Lang.Math.Round(HeightRatio * height);
        }

        public void UpdateColors()
        {
            switch (Shape)
            {
                default:
                case ShapeFlags.LINEAR:
                    Colors[0] = BaseColor;
                    Colors[1] = HighlightColor;
                    Colors[2] = HighlightColor;
                    Colors[3] = BaseColor;
                    break;
                case ShapeFlags.RADIAL:
                    Colors[0] = HighlightColor;
                    Colors[1] = HighlightColor;
                    Colors[2] = BaseColor;
                    Colors[3] = BaseColor;
                    break;
            }
        }

        public void UpdatePositions()
        {
            switch (Shape)
            {
                default:
                case ShapeFlags.LINEAR:
                    Positions[0] = Java.Lang.Math.Max((1f - Intensity - Dropoff) / 2f, 0f);
                    Positions[1] = Java.Lang.Math.Max((1f - Intensity - 0.001f) / 2f, 0f);
                    Positions[2] = Java.Lang.Math.Min((1f + Intensity + 0.001f) / 2f, 1f);
                    Positions[3] = Java.Lang.Math.Min((1f + Intensity + Dropoff) / 2f, 1f);
                    break;
                case ShapeFlags.RADIAL:
                    Positions[0] = 0f;
                    Positions[1] = Java.Lang.Math.Min(Intensity, 1f);
                    Positions[2] = Java.Lang.Math.Min(Intensity + Dropoff, 1f);
                    Positions[3] = 1f;
                    break;
            }
        }

        public void UpdateBounds(int viewWidth, int viewHeight)
        {
            int magnitude = Java.Lang.Math.Max(viewWidth, viewHeight);
            double rad = Java.Lang.Math.Pi / 2f - Java.Lang.Math.ToRadians(Tilt % 90f);
            double hyp = magnitude / Java.Lang.Math.Sin(rad);
            int padding = 3 * Java.Lang.Math.Round((float)(hyp - magnitude) / 2f);
            Bounds.Set(-padding, -padding, Width(viewWidth) + padding, Height(viewHeight) + padding);
        }

        public void Dispose()
        {
            this.Bounds.Dispose();
        }

        public abstract class Builder : IDisposable
        {
            private Shimmer shimmer = new Shimmer();
            public Shimmer Shimmer { get => shimmer; set => shimmer = value; }

            protected abstract Builder GetThis();

            public Builder ConsumeAttributes(Context context, IAttributeSet attrs)
            {
                TypedArray attributes = context.ObtainStyledAttributes(attrs, Resource.Styleable.ShimmerFrameLayout, 0, 0);
                return ConsumeAttributes(attributes);
            }

            public virtual Builder ConsumeAttributes(TypedArray attributes)
            {
                if (attributes.HasValue(Resource.Styleable.ShimmerFrameLayout_shimmer_clip_to_children))
                {
                    SetClipToChildren(
                        attributes.GetBoolean(
                            Resource.Styleable.ShimmerFrameLayout_shimmer_clip_to_children, Shimmer.ClipToChildren));
                }
                if (attributes.HasValue(Resource.Styleable.ShimmerFrameLayout_shimmer_auto_start))
                {
                    SetAutoStart(
                        attributes.GetBoolean(Resource.Styleable.ShimmerFrameLayout_shimmer_auto_start, Shimmer.AutoStart));
                }
                if (attributes.HasValue(Resource.Styleable.ShimmerFrameLayout_shimmer_base_alpha))
                {
                    SetBaseAlpha(attributes.GetFloat(Resource.Styleable.ShimmerFrameLayout_shimmer_base_alpha, 0.3f));
                }
                if (attributes.HasValue(Resource.Styleable.ShimmerFrameLayout_shimmer_highlight_alpha))
                {
                    SetHighlightAlpha(attributes.GetFloat(Resource.Styleable.ShimmerFrameLayout_shimmer_highlight_alpha, 1f));
                }
                if (attributes.HasValue(Resource.Styleable.ShimmerFrameLayout_shimmer_duration))
                {
                    SetDuration(
                        attributes.GetInt(
                            Resource.Styleable.ShimmerFrameLayout_shimmer_duration, (int)Shimmer.AnimationDuration));
                }
                if (attributes.HasValue(Resource.Styleable.ShimmerFrameLayout_shimmer_repeat_count))
                {
                    SetRepeatCount(
                        attributes.GetInt(Resource.Styleable.ShimmerFrameLayout_shimmer_repeat_count, Shimmer.RepeatCount));
                }
                if (attributes.HasValue(Resource.Styleable.ShimmerFrameLayout_shimmer_repeat_delay))
                {
                    SetRepeatDelay(
                        attributes.GetInt(
                            Resource.Styleable.ShimmerFrameLayout_shimmer_repeat_delay, (int)Shimmer.RepeatDelay));
                }
                if (attributes.HasValue(Resource.Styleable.ShimmerFrameLayout_shimmer_repeat_mode))
                {
                    SetRepeatMode(
                        attributes.GetInt(Resource.Styleable.ShimmerFrameLayout_shimmer_repeat_mode, (int)Shimmer.RepeatMode));
                }

                if (attributes.HasValue(Resource.Styleable.ShimmerFrameLayout_shimmer_direction))
                {
                    int direction =
                        attributes.GetInt(Resource.Styleable.ShimmerFrameLayout_shimmer_direction, Shimmer.Direction);
                    switch (direction)
                    {
                        default:
                        case DirectionFlags.LEFT_TO_RIGHT:
                            SetDirection(DirectionFlags.LEFT_TO_RIGHT);
                            break;
                        case DirectionFlags.TOP_TO_BOTTOM:
                            SetDirection(DirectionFlags.TOP_TO_BOTTOM);
                            break;
                        case DirectionFlags.RIGHT_TO_LEFT:
                            SetDirection(DirectionFlags.RIGHT_TO_LEFT);
                            break;
                        case DirectionFlags.BOTTOM_TO_TOP:
                            SetDirection(DirectionFlags.BOTTOM_TO_TOP);
                            break;
                    }
                }

                if (attributes.HasValue(Resource.Styleable.ShimmerFrameLayout_shimmer_shape))
                {
                    int shape = attributes.GetInt(Resource.Styleable.ShimmerFrameLayout_shimmer_shape, Shimmer.Shape);
                    switch (shape)
                    {
                        default:
                        case ShapeFlags.LINEAR:
                            SetShape(ShapeFlags.LINEAR);
                            break;
                        case ShapeFlags.RADIAL:
                            SetShape(ShapeFlags.RADIAL);
                            break;
                    }
                }

                if (attributes.HasValue(Resource.Styleable.ShimmerFrameLayout_shimmer_dropoff))
                {
                    SetDropoff(attributes.GetFloat(Resource.Styleable.ShimmerFrameLayout_shimmer_dropoff, Shimmer.Dropoff));
                }
                if (attributes.HasValue(Resource.Styleable.ShimmerFrameLayout_shimmer_fixed_width))
                {
                    SetFixedWidth(
                        attributes.GetDimensionPixelSize(
                            Resource.Styleable.ShimmerFrameLayout_shimmer_fixed_width, Shimmer.FixedWidth));
                }
                if (attributes.HasValue(Resource.Styleable.ShimmerFrameLayout_shimmer_fixed_height))
                {
                    SetFixedHeight(
                        attributes.GetDimensionPixelSize(
                            Resource.Styleable.ShimmerFrameLayout_shimmer_fixed_height, Shimmer.FixedHeight));
                }
                if (attributes.HasValue(Resource.Styleable.ShimmerFrameLayout_shimmer_intensity))
                {
                    SetIntensity(
                        attributes.GetFloat(Resource.Styleable.ShimmerFrameLayout_shimmer_intensity, Shimmer.Intensity));
                }
                if (attributes.HasValue(Resource.Styleable.ShimmerFrameLayout_shimmer_width_ratio))
                {
                    SetWidthRatio(
                        attributes.GetFloat(Resource.Styleable.ShimmerFrameLayout_shimmer_width_ratio, Shimmer.WidthRatio));
                }
                if (attributes.HasValue(Resource.Styleable.ShimmerFrameLayout_shimmer_height_ratio))
                {
                    SetHeightRatio(
                        attributes.GetFloat(Resource.Styleable.ShimmerFrameLayout_shimmer_height_ratio, Shimmer.HeightRatio));
                }
                if (attributes.HasValue(Resource.Styleable.ShimmerFrameLayout_shimmer_tilt))
                {
                    SetTilt(attributes.GetFloat(Resource.Styleable.ShimmerFrameLayout_shimmer_tilt, Shimmer.Tilt));
                }
                return GetThis();
            }

            public Builder SetDirection(int direction)
            {
                Shimmer.Direction = direction;
                return GetThis();
            }

            public Builder SetShape(int shape)
            {
                Shimmer.Shape = shape;
                return GetThis();
            }

            public Builder SetFixedWidth(int fixedWidth)
            {
                if (fixedWidth < 0)
                {
                    throw new IllegalArgumentException("Given invalid width: " + fixedWidth);
                }

                Shimmer.FixedWidth = fixedWidth;
                return GetThis();
            }

            public Builder SetFixedHeight(int fixedHeight)
            {
                if (fixedHeight < 0)
                {
                    throw new IllegalArgumentException("Given invalid height: " + fixedHeight);
                }

                Shimmer.FixedHeight = fixedHeight;
                return GetThis();
            }

            public Builder SetWidthRatio(float widthRatio)
            {
                if (widthRatio < 0f)
                {
                    throw new IllegalArgumentException("Given invalid width ratio: " + widthRatio);
                }

                Shimmer.WidthRatio = widthRatio;
                return GetThis();
            }

            public Builder SetHeightRatio(float heightRatio)
            {
                if (heightRatio < 0f)
                {
                    throw new IllegalArgumentException("Given invalid height ratio: " + heightRatio);
                }

                Shimmer.HeightRatio = heightRatio;
                return GetThis();
            }

            public Builder SetIntensity(float intensity)
            {
                if (intensity < 0f)
                {
                    throw new IllegalArgumentException("Given invalid intensity value: " + intensity);
                }

                Shimmer.Intensity = intensity;
                return GetThis();
            }

            public Builder SetDropoff(float dropoff)
            {
                if (dropoff < 0f)
                {
                    throw new IllegalArgumentException("Given invalid dropoff value: " + dropoff);
                }

                Shimmer.Dropoff = dropoff;
                return GetThis();
            }

            public Builder SetTilt(float tilt)
            {
                Shimmer.Tilt = tilt;
                return GetThis();
            }

            public Builder SetBaseAlpha(float alpha)
            {
                int intAlpha = (int)(Clamp(0f, 1f, alpha) * 255f);
                Shimmer.BaseColor = intAlpha << 24 | (Shimmer.BaseColor & 0x00FFFFFF);
                return GetThis();
            }

            public Builder SetHighlightAlpha(float alpha)
            {
                int intAlpha = (int)(Clamp(0f, 1f, alpha) * 255f);
                Shimmer.HighlightColor = intAlpha << 24 | (Shimmer.HighlightColor & 0x00FFFFFF);
                return GetThis();
            }

            public Builder SetClipToChildren(bool status)
            {
                Shimmer.ClipToChildren = status;
                return GetThis();
            }

            public Builder SetAutoStart(bool status)
            {
                Shimmer.AutoStart = status;
                return GetThis();
            }

            public Builder SetRepeatCount(int repeatCount)
            {
                Shimmer.RepeatCount = repeatCount;
                return GetThis();
            }

            public Builder SetRepeatMode(int mode)
            {
                Shimmer.RepeatMode = (ValueAnimatorRepeatMode)mode;
                return GetThis();
            }

            public Builder SetRepeatDelay(long millis)
            {
                if (millis < 0)
                {
                    throw new IllegalArgumentException("Given a negative repeat delay: " + millis);
                }
                Shimmer.RepeatDelay = millis;
                return GetThis();
            }

            public Builder SetDuration(long millis)
            {
                if (millis < 0)
                {
                    throw new IllegalArgumentException("Given a negative duration: " + millis);
                }

                Shimmer.AnimationDuration = millis;
                return GetThis();
            }

            public Shimmer Build()
            {
                Shimmer.UpdateColors();
                Shimmer.UpdatePositions();
                return Shimmer;
            }

            private static float Clamp(float min, float max, float value)
            {
                return Java.Lang.Math.Min(max, Java.Lang.Math.Max(min, value));
            }

            public void Dispose()
            {
                Shimmer.Dispose();
            }
        }

        public class AlphaHighlightBuilder : Builder
        {
            public AlphaHighlightBuilder()
            {
                Shimmer.AlphaShimmer = true;
            }

            protected override Builder GetThis()
            {
                return this;
            }
        }

        public class ColorHighlightBuilder : Builder
        {
            public ColorHighlightBuilder()
            {
                Shimmer.AlphaShimmer = false;
            }

            public Builder SetHighlightColor(int color)
            {
                Shimmer.HighlightColor = color;
                return GetThis();
            }

            public Builder SetBaseColor(int color)
            {
                Shimmer.BaseColor = (int)(Shimmer.BaseColor & 0xFF000000) | (color & 0x00FFFFFF);
                return GetThis();
            }

            public override Builder ConsumeAttributes(TypedArray attributes)
            {
                base.ConsumeAttributes(attributes);
                if (attributes.HasValue(Resource.Styleable.ShimmerFrameLayout_shimmer_base_color))
                {
                    SetBaseColor(
                        attributes.GetColor(Resource.Styleable.ShimmerFrameLayout_shimmer_base_color, Shimmer.BaseColor));
                }
                if (attributes.HasValue(Resource.Styleable.ShimmerFrameLayout_shimmer_highlight_color))
                {
                    SetHighlightColor(
                        attributes.GetColor(
                            Resource.Styleable.ShimmerFrameLayout_shimmer_highlight_color, Shimmer.HighlightColor));
                }
                return GetThis();
            }


            protected override Builder GetThis()
            {
                return this;
            }
        }
    }
}