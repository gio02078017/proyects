package app.epm.com.utilities.helpers;

import android.content.Context;
import android.provider.Settings.Secure;
import android.util.Log;

import app.epm.com.utilities.view.activities.BaseActivity;

public class IdDispositive  {

    private static Context context;

    public IdDispositive(Context context){
        this.context = context;
    }

    public static String getIdDispositive() {
        String id = "";
        if(context != null){
             id = Secure.getString(context.getContentResolver(), Secure.ANDROID_ID);
        }
        return id;
    }

}
