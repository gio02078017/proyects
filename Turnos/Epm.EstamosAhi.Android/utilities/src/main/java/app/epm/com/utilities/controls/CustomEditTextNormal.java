package app.epm.com.utilities.controls;

import android.content.Context;
import android.util.AttributeSet;
import android.widget.EditText;

/**
 * Created by JoseTabares on 25/04/16.
 */
public class CustomEditTextNormal extends EditText {
    public CustomEditTextNormal(Context context) {
        super(context);
        setCustomTypeFace();
    }

    public CustomEditTextNormal(Context context, AttributeSet attrs) {
        super(context, attrs);
        setCustomTypeFace();
    }

    public CustomEditTextNormal(Context context, AttributeSet attrs, int defStyleAttr) {
        super(context, attrs, defStyleAttr);
        setCustomTypeFace();
    }

    private void setCustomTypeFace() {
        setTypeface(FactoryControls.getTypefaceNormal());
    }
}
