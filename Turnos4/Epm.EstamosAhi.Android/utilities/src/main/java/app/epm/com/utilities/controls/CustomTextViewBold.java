package app.epm.com.utilities.controls;

import android.content.Context;
import android.util.AttributeSet;
import android.widget.TextView;

/**
 * Created by JoseTabares on 25/04/16.
 */
public class CustomTextViewBold extends TextView {
    public CustomTextViewBold(Context context) {
        super(context);
        setCustomTypeFace();
    }

    public CustomTextViewBold(Context context, AttributeSet attrs) {
        super(context, attrs);
        setCustomTypeFace();
    }

    public CustomTextViewBold(Context context, AttributeSet attrs, int defStyleAttr) {
        super(context, attrs, defStyleAttr);
        setCustomTypeFace();
    }

    private void setCustomTypeFace() {
        setTypeface(FactoryControls.getTypefaceBold());
    }
}
