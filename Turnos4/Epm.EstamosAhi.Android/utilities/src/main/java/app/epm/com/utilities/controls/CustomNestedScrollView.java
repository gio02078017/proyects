package app.epm.com.utilities.controls;

import android.content.Context;
import android.graphics.Rect;
import android.support.v4.widget.NestedScrollView;
import android.util.AttributeSet;
import android.view.View;

/**
 * Created by josetabaresramirez on 5/09/16.
 */
public class CustomNestedScrollView extends NestedScrollView {
    /**
     * Cosntructor.
     *
     * @param context context.
     */
    public CustomNestedScrollView(Context context) {
        super(context);
    }

    /**
     * Constructor.
     *
     * @param context context.
     * @param attrs   attrs.
     */
    public CustomNestedScrollView(Context context, AttributeSet attrs) {
        super(context, attrs);
    }

    /**
     * Constructor.
     *
     * @param context      context.
     * @param attrs        attrs.
     * @param defStyleAttr defStyleAttr.
     */
    public CustomNestedScrollView(Context context, AttributeSet attrs, int defStyleAttr) {
        super(context, attrs, defStyleAttr);
    }

    @Override
    public void requestChildFocus(View child, View focused) {
        //The method is not used.
    }

    @Override
    protected boolean onRequestFocusInDescendants(int direction, Rect previouslyFocusedRect) {
        return false;
    }
}
