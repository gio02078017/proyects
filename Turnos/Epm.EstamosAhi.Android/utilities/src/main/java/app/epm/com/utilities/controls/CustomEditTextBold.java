package app.epm.com.utilities.controls;

import android.content.Context;
import android.util.AttributeSet;
import android.widget.EditText;

/**
 * Created by JoseTabares on 25/04/16.
 */
public class CustomEditTextBold extends EditText {
    public CustomEditTextBold(Context context) {
        super(context);
        setCustomTypeFace();
    }

    public CustomEditTextBold(Context context, AttributeSet attrs) {
        super(context, attrs);
        setCustomTypeFace();
    }

    public CustomEditTextBold(Context context, AttributeSet attrs, int defStyleAttr) {
        super(context, attrs, defStyleAttr);
        setCustomTypeFace();
    }

    private void setCustomTypeFace() {
        setTypeface(FactoryControls.getTypefaceBold());
    }
}
