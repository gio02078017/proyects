using Android.Animation;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using Java.Lang;
using System;
using System.Collections.Generic;
using droid = Android;

namespace GrupoExito.Android.Utilities
{
    [Register("app.exito.dev.utilities.fabtoolbar")]

    public class FabToolbar : RelativeLayout
    {
        private int SHOW_ANIM_DURATION = 600;
        private int HIDE_ANIM_DURATION = 600;
        private int HORIZONTAL_MARGIN = 100;
        private int VERTICAL_MARGIN = 100;

        private int pivotX = -1;
        private int pivotY = -1;
        private float fraction = 0.2f;

        private int fabId = -1;
        private int containerId = -1;
        private int toolbarId = -1;

        private View toolbarLayout;
        private ImageView fab;
        private TransitionDrawable fabDrawable;
        private Drawable fabNormalDrawable;
        private RelativeLayout fabContainer;

        private Point toolbarPos = new Point();
        private Point toolbarSize = new Point();
        private Point fabPos = new Point();
        private Point fabSize = new Point();

        private bool isFab = true;
        private bool isToolbar = false;
        private bool isInit = true;

        private bool fabDrawableAnimationEnabled = true;

        private int originalToolbarSize = -1;
        private int originalFABSize;

        public FabToolbar(Context context) : base(context)
        {
        }

        public FabToolbar(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            ParseAttributes(attrs);
        }

        public FabToolbar(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            ParseAttributes(attrs);
        }

        private void ParseAttributes(IAttributeSet attrs)
        {
            TypedArray attributes = Context.ObtainStyledAttributes(attrs, Resource.Styleable.FabToolbar);

            SHOW_ANIM_DURATION = attributes.GetInt(Resource.Styleable.FabToolbar_showDuration, SHOW_ANIM_DURATION);
            HIDE_ANIM_DURATION = attributes.GetInt(Resource.Styleable.FabToolbar_hideDuration, HIDE_ANIM_DURATION);
            VERTICAL_MARGIN = attributes.GetDimensionPixelSize(Resource.Styleable.FabToolbar_verticalMargin, VERTICAL_MARGIN);
            HORIZONTAL_MARGIN = attributes.GetDimensionPixelSize(Resource.Styleable.FabToolbar_horizontalMargin, HORIZONTAL_MARGIN);
            pivotX = attributes.GetDimensionPixelSize(Resource.Styleable.FabToolbar_fadeInPivotX, -1);
            pivotY = attributes.GetDimensionPixelSize(Resource.Styleable.FabToolbar_fadeInPivotY, -1);
            fraction = attributes.GetFloat(Resource.Styleable.FabToolbar_fadeInFraction, fraction);
            fabId = attributes.GetResourceId(Resource.Styleable.FabToolbar_fabId, -1);
            containerId = attributes.GetResourceId(Resource.Styleable.FabToolbar_containerId, -1);
            toolbarId = attributes.GetResourceId(Resource.Styleable.FabToolbar_fabToolbarId, -1);
            fabDrawableAnimationEnabled = attributes.GetBoolean(Resource.Styleable.FabToolbar_fabDrawableAnimationEnabled, true);

            attributes.Recycle();
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            try
            {
                base.OnLayout(changed, l, t, r, b);

                if (!isInit)
                {
                    return;
                }

                toolbarLayout = FindViewById(toolbarId);

                if (toolbarLayout == null)
                {
                    throw new IllegalStateException("You have to place a view with id = R.id.fabtoolbar_toolbar inside FABToolbarLayout");
                }

                fabContainer = (RelativeLayout)FindViewById(containerId);

                if (fabContainer == null)
                {
                    throw new IllegalStateException("You have to place a FABContainer view with id = R.id.fabtoolbar_container inside FABToolbarLayout");
                }

                fab = (ImageView)fabContainer.FindViewById(fabId);

                if (fab == null)
                {
                    throw new IllegalStateException("You have to place a FAB view with id = R.id.fabtoolbar_fab inside FABContainer");
                }

                fab.Visibility = ViewStates.Invisible;

                Drawable tempDrawable = fab.Drawable;
                fabNormalDrawable = tempDrawable;

                if (fabDrawableAnimationEnabled)
                {
                    TransitionDrawable transitionDrawable;
                    if (Build.VERSION.SdkInt >= droid.OS.BuildVersionCodes.Lollipop)
                    {
                        transitionDrawable = new TransitionDrawable(new Drawable[] { tempDrawable, Resources.GetDrawable(Resource.Drawable.empty_drawable, Context.Theme) });
                    }
                    else
                    {
                        transitionDrawable = new TransitionDrawable(new Drawable[] { tempDrawable, Resources.GetDrawable(Resource.Drawable.empty_drawable) });
                    }

                    transitionDrawable.CrossFadeEnabled = fabDrawableAnimationEnabled;
                    fabDrawable = transitionDrawable;
                    fab.SetImageDrawable(transitionDrawable);
                }

                CustomGlobalLayoutListener customGlobalLayoutListener = new CustomGlobalLayoutListener();

                customGlobalLayoutListener.GlobalLayout += (object sender, EventArgs args) =>
                {
                    if (toolbarLayout.Width != 0 || toolbarLayout.Height != 0)
                    {
                        toolbarLayout.ViewTreeObserver.RemoveOnGlobalLayoutListener(customGlobalLayoutListener);
                        int[] pos = new int[2];

                        toolbarSize.Set(toolbarLayout.Width, toolbarLayout.Height);
                        toolbarLayout.GetLocationOnScreen(pos);
                        toolbarPos.Set(pos[0], pos[1]);

                        int[] fabContainerPos = new int[2];
                        fabContainer.GetLocationOnScreen(fabContainerPos);

                        RelativeLayout.LayoutParams fabParams = (RelativeLayout.LayoutParams)fab.LayoutParameters;
                        RelativeLayout.LayoutParams fabContainerParams = (RelativeLayout.LayoutParams)fabContainer.LayoutParameters;

                        int distanceVertical = (toolbarSize.Y - fab.Height) / 2;
                        int verticalMarginToSet = VERTICAL_MARGIN - distanceVertical;

                        if (fabParams.GetRules()[(int)LayoutRules.AlignParentStart] == (int)LayoutRules.True)
                        {
                            fabParams.MarginStart = HORIZONTAL_MARGIN;
                        }
                        else
                        {
                            fabParams.AddRule(LayoutRules.AlignParentEnd);
                            fabParams.MarginEnd = HORIZONTAL_MARGIN;
                        }

                        fab.LayoutParameters = fabParams;

                        CustomGlobalLayoutListener fabContainerGlobalLayoutListener = new CustomGlobalLayoutListener();
                        fabContainerGlobalLayoutListener.GlobalLayout += (object senderFab, EventArgs argsfab) =>
                        {
                            toolbarLayout.ViewTreeObserver.RemoveOnGlobalLayoutListener(fabContainerGlobalLayoutListener);
                            fab.Visibility = ViewStates.Visible;
                        };

                        fabContainer.ViewTreeObserver.AddOnGlobalLayoutListener(fabContainerGlobalLayoutListener);

                        fabContainerParams.Height = toolbarSize.Y;

                        if (originalToolbarSize == -1)
                        {
                            originalToolbarSize = toolbarSize.Y;
                        }

                        if (fabContainerParams.GetRules()[(int)LayoutRules.AlignParentTop] == (int)LayoutRules.True)
                        {
                            fabContainerParams.TopMargin = verticalMarginToSet;
                        }
                        else
                        {
                            fabContainerParams.AddRule(LayoutRules.AlignParentBottom);
                            fabContainerParams.BottomMargin = verticalMarginToSet;
                        }

                        fabContainer.LayoutParameters = fabContainerParams;
                        toolbarLayout.Visibility = ViewStates.Invisible;
                        toolbarLayout.Alpha = 0f;
                    }
                };

                toolbarLayout.ViewTreeObserver.AddOnGlobalLayoutListener(customGlobalLayoutListener);

                CustomGlobalLayoutListener fabGlobalLayoutListener = new CustomGlobalLayoutListener();
                fabGlobalLayoutListener.GlobalLayout += (object senderFab, EventArgs argsfab) =>
                {
                    if (fab.Width != 0 || fab.Height != 0)
                    {
                        fab.ViewTreeObserver.RemoveOnGlobalLayoutListener(fabGlobalLayoutListener);
                        fabSize.Set(fab.Width, fab.Height);

                        RelativeLayout.LayoutParams layoutParams = (RelativeLayout.LayoutParams)fab.LayoutParameters;
                        layoutParams.AddRule(LayoutRules.CenterVertical);
                        fab.LayoutParameters = layoutParams;
                    }
                };


                fab.ViewTreeObserver.AddOnGlobalLayoutListener(fabGlobalLayoutListener);

                CustomGlobalLayoutListener toolbarGlobalLayoutListener = new CustomGlobalLayoutListener();
                toolbarGlobalLayoutListener.GlobalLayout += (object senderFab, EventArgs argsfab) =>
                {
                    RelativeLayout.LayoutParams fabContainerParams = (RelativeLayout.LayoutParams)fabContainer.LayoutParameters;
                    fabContainerParams.Height = toolbarLayout.Height;
                    fabContainer.LayoutParameters = fabContainerParams;
                    int[] pos = new int[2];
                    toolbarSize.Set(toolbarLayout.Width, toolbarLayout.Height);
                    toolbarLayout.GetLocationOnScreen(pos);
                    toolbarPos.Set(pos[0], pos[1]);
                };

                toolbarLayout.ViewTreeObserver.AddOnGlobalLayoutListener(toolbarGlobalLayoutListener);
                fab.SetLayerType(LayerType.Software, null);
                SetClipChildren(false);
                isInit = false;
            }
            catch
            {

            }
        }

        public void Show()
        {
            if (!isFab)
            {
                return;
            }

            isFab = false;

            SetClipChildren(true);

            int[] fabP = new int[2];
            fab.GetLocationOnScreen(fabP);
            fabPos.Set(fabP[0], fabP[1]);
            originalFABSize = fab.Width;

            List<Animator> animators = new List<Animator>();
            int xDest = toolbarPos.X + (toolbarSize.X - fabSize.X) / 2;

            int[] fabConPos = new int[2];
            fabContainer.GetLocationOnScreen(fabConPos);

            int xDelta = xDest - fabPos.X;
            int yDelta = toolbarPos.Y - fabConPos[1];

            ObjectAnimator xAnim = ObjectAnimator.OfFloat(fab, "translationX", fab.TranslationX, fab.TranslationX + xDelta);
            ObjectAnimator yAnim = ObjectAnimator.OfFloat(fabContainer, "translationY", fabContainer.TranslationY, fabContainer.TranslationY + yDelta);

            xAnim.SetInterpolator(new AccelerateInterpolator());
            yAnim.SetInterpolator(new DecelerateInterpolator(3f));

            xAnim.SetDuration(SHOW_ANIM_DURATION / 2);
            yAnim.SetDuration(SHOW_ANIM_DURATION / 2);

            animators.Add(xAnim);
            animators.Add(yAnim);

            if (fabDrawable != null && fabDrawableAnimationEnabled)
            {
                fabDrawable.StartTransition(SHOW_ANIM_DURATION / 3);
            }

            if (!fabDrawableAnimationEnabled)
            {
                fab.SetImageDrawable(null);
            }

            int startRadius = fabSize.X / 2;
            int finalRadius = (int)(Java.Lang.Math.Sqrt(Java.Lang.Math.Pow(toolbarSize.X, 2) + Java.Lang.Math.Pow(toolbarSize.Y, 2)) / 2);
            int realRadius = (int)(98f * finalRadius / 55f);
            ValueAnimator sizeAnim = ValueAnimator.OfFloat(startRadius, realRadius);
            sizeAnim.Update += (sender, args) =>
            {
                float valFloat = (float)args.Animation.AnimatedValue;
                fab.ScaleX = valFloat / startRadius;
                fab.ScaleY = valFloat / startRadius;
            };

            sizeAnim.SetDuration(SHOW_ANIM_DURATION / 2);
            sizeAnim.StartDelay = SHOW_ANIM_DURATION / 4;
            animators.Add(sizeAnim);

            ViewGroup toolbarLayoutViewGroup = (ViewGroup)toolbarLayout;
            List<Animator> expandAnim = ExpandAnimationUtils.Build(toolbarLayoutViewGroup, pivotX != -1 ? pivotX : toolbarLayout.Width / 2, pivotY != -1 ? pivotY : toolbarLayout.Height / 2, fraction, SHOW_ANIM_DURATION / 3, 2 * SHOW_ANIM_DURATION / 3);

            animators.AddRange(expandAnim);

            AnimatorSet animatorSet = new AnimatorSet();
            animatorSet.PlayTogether(animators);
            animatorSet.AnimationEnd += (sender, args) =>
            {
                isToolbar = true;
            };

            animatorSet.Start();
        }

        public void Hide()
        {
            if (!isToolbar)
            {
                return;
            }

            isToolbar = false;

            int[] fabP = new int[2];
            fab.GetLocationOnScreen(fabP);
            Point tempFABPos = new Point();
            tempFABPos.Set(fabP[0], fabP[1]);

            List<Animator> reverseAnimators = new List<Animator>();

            int xSource = toolbarPos.X + (toolbarSize.X - originalFABSize) / 2;
            int distanceVertical = (toolbarSize.Y - fab.Height) / 2;
            int verticalMarginToSet = VERTICAL_MARGIN - distanceVertical;
            int yDest = toolbarPos.Y - verticalMarginToSet;

            int[] fabConPos = new int[2];
            fabContainer.GetLocationOnScreen(fabConPos);

            int xDelta = fabPos.X - xSource;
            int yDelta = yDest - fabConPos[1];

            ObjectAnimator xAnimR = ObjectAnimator.OfFloat(fab, "translationX", fab.TranslationX, fab.TranslationX + xDelta);
            ObjectAnimator yAnimR = ObjectAnimator.OfFloat(fabContainer, "translationY", fabContainer.TranslationY, fabContainer.TranslationY + yDelta);

            xAnimR.SetInterpolator(new DecelerateInterpolator());
            yAnimR.SetInterpolator(new AccelerateInterpolator());

            xAnimR.SetDuration(HIDE_ANIM_DURATION / 2);
            yAnimR.SetDuration(HIDE_ANIM_DURATION / 2);

            xAnimR.StartDelay = HIDE_ANIM_DURATION / 2;
            yAnimR.StartDelay = HIDE_ANIM_DURATION / 2;

            reverseAnimators.Add(xAnimR);
            reverseAnimators.Add(yAnimR);

            ValueAnimator drawableAnimR = ValueAnimator.OfFloat(0, 0);

            drawableAnimR.SetDuration(2 * HIDE_ANIM_DURATION / 3);

            drawableAnimR.AnimationEnd += (sender, args) =>
            {
                if (fabDrawableAnimationEnabled)
                {
                    fabDrawable.ReverseTransition(HIDE_ANIM_DURATION / 3);
                }
                else
                {
                    fab.SetImageDrawable(fabNormalDrawable);
                }
            };

            reverseAnimators.Add(drawableAnimR);

            int startRadius = originalToolbarSize / 2;
            int finalRadius = (int)(Java.Lang.Math.Sqrt(Java.Lang.Math.
                Pow(toolbarSize.X, 2) + Java.Lang.Math.Pow(toolbarSize.Y, 2)) / 2);
            int realRadius = (int)(98f * finalRadius / 55f);
            ValueAnimator sizeAnimR = ValueAnimator.OfFloat(realRadius, startRadius);

            sizeAnimR.Update += (sender, args) =>
            {
                float valFloat = (float)args.Animation.AnimatedValue;
                fab.ScaleX = valFloat / startRadius;
                fab.ScaleY = valFloat / startRadius;
            };

            sizeAnimR.SetDuration(HIDE_ANIM_DURATION / 2);
            sizeAnimR.StartDelay = HIDE_ANIM_DURATION / 4;
            reverseAnimators.Add(sizeAnimR);

            ViewGroup toolbarLayoutViewGroup = (ViewGroup)toolbarLayout;
            List<Animator> expandAnimR = ExpandAnimationUtils.BuildReversed(toolbarLayoutViewGroup, pivotX != -1 ? pivotX : toolbarLayout.Width / 2, pivotY != -1 ? pivotY : toolbarLayout.Height / 2, fraction, HIDE_ANIM_DURATION / 3, 0);

            reverseAnimators.AddRange(expandAnimR);

            AnimatorSet animatorSet = new AnimatorSet();
            animatorSet.PlayTogether(reverseAnimators);

            animatorSet.AnimationEnd += (sender, args) =>
            {
                isFab = true;
                SetClipChildren(false);
            };

            animatorSet.Start();
        }

        public bool IsFloatingActionButton()
        {
            return isFab;
        }

        public bool IsFabToolbar()
        {
            return isToolbar;
        }

        public bool IsFabDrawableAnimationEnabled()
        {
            return fabDrawableAnimationEnabled;
        }

        public void SetFabDrawableAnimationEnabled(bool fabDrawableAnimationEnabled)
        {
            this.fabDrawableAnimationEnabled = fabDrawableAnimationEnabled;
        }
    }
}