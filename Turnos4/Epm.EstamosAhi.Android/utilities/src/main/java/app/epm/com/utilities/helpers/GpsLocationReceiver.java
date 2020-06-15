package app.epm.com.utilities.helpers;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;

/**
 * Created by josetabaresramirez on 3/02/17.
 */

public class GpsLocationReceiver extends BroadcastReceiver {

    static private IGpsLocationListener gpsLocationListener;

    @Override
    public void onReceive(Context context, Intent intent) {
        if (gpsLocationListener != null) {
            gpsLocationListener.notifyGpsState();
        }
    }

    public static void setGpsLocationListener(IGpsLocationListener gpsLocationListener) {
        GpsLocationReceiver.gpsLocationListener = gpsLocationListener;
    }
}
