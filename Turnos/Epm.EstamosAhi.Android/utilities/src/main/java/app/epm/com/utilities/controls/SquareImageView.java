package app.epm.com.utilities.controls;

import android.content.Context;
import android.util.DisplayMetrics;
import android.widget.ImageView;

/**
 * Created by Jquinterov on 2/28/2017.
 */

public class SquareImageView extends ImageView {
    Context context;

    public SquareImageView(Context context) {
        super(context);
        this.context = context;
    }

    @Override
    protected void onMeasure(int widthMeasureSpec, int heightMeasureSpec) {
        super.onMeasure(widthMeasureSpec, heightMeasureSpec);

        DisplayMetrics metrics = context.getApplicationContext().getResources().getDisplayMetrics();
        int densityDpi = metrics.densityDpi;

        int width = 100;

        if (densityDpi == 640) {
            width = 200;
        }
        setMeasuredDimension(width, width);
    }
}
