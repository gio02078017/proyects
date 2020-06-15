package app.epm.com.utilities.controls;

import android.content.Context;
import android.graphics.Typeface;

import app.epm.com.utilities.utils.Constants;

/**
 * Created by JoseTabares on 25/04/16.
 */
public class FactoryControls {

    private static Typeface typefaceNormal;
    private static Typeface typefaceBold;
    private static Context context;

    public static void setContext(Context context) {
        FactoryControls.context = context;
        configurateTypeFace();
    }

    private static void configurateTypeFace() {
        typefaceNormal = Typeface.createFromAsset(context.getAssets(), Constants.PATH_NORMAL_FONT);
        typefaceBold = Typeface.createFromAsset(context.getAssets(), Constants.PATH_BOLD_FONT);
    }

    public static Typeface getTypefaceBold() {
        return typefaceBold;
    }

    public static Typeface getTypefaceNormal() {
        return typefaceNormal;
    }
}
