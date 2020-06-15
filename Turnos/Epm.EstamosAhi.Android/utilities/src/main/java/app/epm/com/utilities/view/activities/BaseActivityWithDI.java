package app.epm.com.utilities.view.activities;

import androidx.lifecycle.ViewModelProvider;
import android.content.ActivityNotFoundException;
import android.content.DialogInterface;
import android.content.Intent;
import android.net.Uri;
import android.provider.Settings;
import androidx.fragment.app.Fragment;
import androidx.appcompat.app.AlertDialog;

import javax.inject.Inject;

import app.epm.com.utilities.R;
import app.epm.com.utilities.helpers.Permissions;
import app.epm.com.utilities.utils.Constants;
import dagger.android.AndroidInjection;
import dagger.android.AndroidInjector;
import dagger.android.DispatchingAndroidInjector;
import dagger.android.support.HasSupportFragmentInjector;

public class BaseActivityWithDI extends BaseActivity implements HasSupportFragmentInjector {

    private boolean openGpsSettings;
    private boolean openWaze;
    private AlertDialog alertDialogtest;
    private boolean controlNeverAsk = false;

    @Inject
    DispatchingAndroidInjector<Fragment> dispatchingAndroidInjector;
    @Inject
    protected ViewModelProvider.Factory viewModelFactory;
    @Override
    public AndroidInjector<Fragment> supportFragmentInjector() {
        return dispatchingAndroidInjector;
    }

    protected void configureDagger() {
        AndroidInjection.inject(this);
    }

    protected boolean gpsIsOn() {
        boolean resultGPSIsOn = false;

        try {
            int gpsIsOn = Permissions.getLocationMode(getApplicationContext());

            if(gpsIsOn > 0) resultGPSIsOn = true;

        }catch(Settings.SettingNotFoundException e){
            resultGPSIsOn = false;
        }

        return resultGPSIsOn;
    }

    public void showGpsDialog()
    {
        try {
            getCustomAlertDialog().showAlertDialog(getName(), R.string.text_gps_disabled, false, R.string.text_aceptar, (dialog, which) -> {
                openGpsSettings = true;
                startActivityWithOutDoubleClick(new Intent(Settings.ACTION_LOCATION_SOURCE_SETTINGS),Constants.ACTION_TO_TURN_ON_GPS);
                dialog.dismiss();
            }, R.string.text_cancelar, (dialog, which) -> {
                dialog.dismiss();
                cancelPermisonGPS(true);
            }, null);
        }catch(Exception e){

        }
    }


    public void showMessageSettings()
    {
        try {
            getCustomAlertDialog().showAlertDialog(R.string.title_recommend_giving_permissions, R.string.text_recommend_giving_permissions, false, R.string.text_go_to_settings, new DialogInterface.OnClickListener() {
                @Override
                public void onClick(DialogInterface dialog, int which) {
                    openGpsSettings = true;
                    startActivityWithOutDoubleClick(new Intent(Settings.ACTION_APPLICATION_DETAILS_SETTINGS, Permissions.uriApp(getApplicationContext())),Constants.ACTION_TO_TURN_ON_PERMISSION);
                }
            }, R.string.text_cancelar, (dialog, which) -> {
                dialog.dismiss();
                controlNeverAsk = true;
                cancelPermisonGPS(true);
            }, null);
        }catch(Exception e){

        }
    }



    public void callWaze(double latitud, double longitud){
        openWaze = true;
        try {
            String url = String.format(Constants.URL_WAZE,Double.toString(latitud),Constants.URL_HALF_WAZE, Double.toString(longitud));
            Intent intent = new Intent(Intent.ACTION_VIEW, Uri.parse(url));
            startActivity(intent);
        }catch(ActivityNotFoundException e){
            Intent intent = new Intent( Intent.ACTION_VIEW, Uri.parse( Constants.URL_DEFAULT_WAZE ) );
            startActivity(intent);
        }
    }

    public void callGoogleMaps(double latitud, double longitud){
        Uri gmmIntentUri = Uri.parse(String.format(Constants.URL_GOOGLE_MAPS,Double.toString(latitud), Double.toString(longitud)));
        Intent mapIntent = new Intent(Intent.ACTION_VIEW, gmmIntentUri);
        mapIntent.setPackage(Constants.PACKAGE_GOOGLE_MAPS);
        startActivity(mapIntent);
    }

    @Override
    public Intent getDefaultIntent(Intent intent) {
        if (openGpsSettings) {
            openGpsSettings = false;
            return intent;
        }else if (openWaze) {
            openWaze = false;
            return intent;
        }  else {
            return super.getDefaultIntent(intent);
        }
    }

    public void cancelPermisonGPS(boolean defaultLocation){

    }

    public void showOrDimissProgressBar(Boolean isProgressShown) {
        if (isProgressShown != null && isProgressShown) {
            showProgressDIalog(R.string.text_please_wait);
        } else {
            dismissProgressDialog();
        }
    }

    protected void showAlertDialog(final String title, final String message) {
        runOnUiThread(() -> {
            showAlertDialogGeneralInformationOnUiThread(title, message);
        });
    }

    protected void showDialogNotInternetOnlyMessage() {
        getCustomAlertDialog().showAlertDialog(getName(), R.string.text_validate_internet, false, R.string.text_aceptar, (dialogInterface, i) -> {
        }, null);
    }


    public void validateAlertDialog(){
        if(getCustomAlertDialog().getAlertDialog() != null){
            getCustomAlertDialog().getAlertDialog().cancel();
        }
    }


    public AlertDialog getAlertDialogtest() {
        return alertDialogtest;
    }

    public boolean isControlNeverAsk() {
        return controlNeverAsk;
    }

    public void setControlNeverAsk(boolean controlNeverAsk) {
        this.controlNeverAsk = controlNeverAsk;
    }
}
