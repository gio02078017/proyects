package app.epm.com.utilities.helpers;

import android.app.Activity;
import android.content.Context;
import android.content.pm.PackageManager;
import android.net.Uri;
import android.provider.Settings;
import androidx.core.app.ActivityCompat;

import app.epm.com.utilities.utils.Constants;

/**
 * Created by josetabaresramirez on 3/02/17.
 */

public class Permissions {
    public static boolean isGrantedPermissions(Context context, String permissionType) {
        int permission = ActivityCompat.checkSelfPermission(context, permissionType);

        return permission == PackageManager.PERMISSION_GRANTED;
    }

    public static void verifyPermissions(Activity activity, String[] permissionsType) {
        ActivityCompat.requestPermissions(activity, permissionsType, Constants.REQUEST_CODE_PERMISSION);
    }

    public static Uri uriApp(Context context){
        Uri uri = new Uri.Builder()
                .scheme("package")
                .opaquePart(context.getPackageName())
                .build();
        return uri;
    }

    public static int getLocationMode(Context context) throws Settings.SettingNotFoundException {
        return Settings.Secure.getInt(context.getContentResolver(), Settings.Secure.LOCATION_MODE);
    }


}
