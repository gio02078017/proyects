package app.epm.com.utilities.controls;


import android.content.Context;
import android.util.AttributeSet;
import android.widget.Button;

/**
 * Created by JoseTabares on 25/04/16.
 */
public class CustomButtonNormal extends Button {
    public CustomButtonNormal(Context context) {
        super(context);
        setCustomTypeFace();
    }

    public CustomButtonNormal(Context context, AttributeSet attrs) {
        super(context, attrs);
        setCustomTypeFace();
    }

    public CustomButtonNormal(Context context, AttributeSet attrs, int defStyleAttr) {
        super(context, attrs, defStyleAttr);
        setCustomTypeFace();
    }

    private void setCustomTypeFace() {
        setTypeface(FactoryControls.getTypefaceNormal());
    }
}
