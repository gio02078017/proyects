package app.epm.com.utilities.helpers;

import android.app.ActivityManager;
import android.content.ComponentName;
import android.content.Context;
import android.content.pm.PackageInfo;
import android.content.pm.PackageManager;

import java.util.List;

/**
 * Created by JoseTabares on 6/05/16.
 */
public class AppInformation implements IAppInformation {

    private Context context;

    public AppInformation(Context context) {
        this.context = context;
    }

    @Override
    public String getAppVersion() {
        try {
            PackageInfo pInfo = context.getPackageManager().getPackageInfo(context.getPackageName(), 0);
            return "V " + pInfo.versionName;
        } catch (PackageManager.NameNotFoundException e) {
            return "V 1.0";
        }
    }

    @Override
    public boolean isAppRunning() {
        ActivityManager actM = (ActivityManager) context.getSystemService(Context.ACTIVITY_SERVICE);
        List<ActivityManager.RunningTaskInfo> listm = actM.getRunningTasks(1);
        int iNumActivity = listm.get(0).numActivities;
        return iNumActivity > 1;
    }


}
