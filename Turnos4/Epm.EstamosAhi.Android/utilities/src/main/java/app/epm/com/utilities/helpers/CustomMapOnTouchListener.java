package app.epm.com.utilities.helpers;

import android.content.Context;
import android.view.MotionEvent;

import com.esri.arcgisruntime.mapping.view.DefaultMapViewOnTouchListener;
import com.esri.arcgisruntime.mapping.view.MapView;

/**
 * Created by josetabaresramirez on 9/02/17.
 */

public class CustomMapOnTouchListener extends DefaultMapViewOnTouchListener {

    private ICustomMapOnTouchListener customMapOnTouchListener;

    public CustomMapOnTouchListener(Context context, MapView view, ICustomMapOnTouchListener customMapOnTouchListener) {
        super(context, view);
        this.customMapOnTouchListener = customMapOnTouchListener;
    }

    @Override
    public boolean onSingleTapUp(MotionEvent e) {
        return true;
    }

    @Override
    public boolean onSingleTapConfirmed(MotionEvent e) {
        customMapOnTouchListener.onSingleTap(e);
        return true;
    }
}
