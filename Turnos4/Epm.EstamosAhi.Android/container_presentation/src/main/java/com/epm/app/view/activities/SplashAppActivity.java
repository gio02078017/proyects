package com.epm.app.view.activities;

import android.app.Activity;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.pm.PackageInfo;
import android.content.pm.PackageManager;
import android.net.Uri;
import android.os.Build;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.support.v7.app.AlertDialog;
import android.util.Log;
import android.view.View;
import android.widget.ProgressBar;
import android.widget.TextView;

import java.util.Calendar;
import java.util.Date;
import java.util.concurrent.ExecutionException;

import com.epm.app.R;
import com.epm.app.dependency_injection.DomainModule;
import com.epm.app.mvvm.comunidad.repository.NotificationsRepository;
import com.epm.app.mvvm.turn.repository.TurnServicesRepository;
import com.epm.app.presenters.SplashAppPresenter;

import app.epm.com.container_domain.business_models.Authoken;
import app.epm.com.security_domain.business_models.UsuarioRequest;
import app.epm.com.utilities.custom_controls.CustomAlertDialog;
import app.epm.com.utilities.helpers.AppInformation;
import app.epm.com.utilities.helpers.CustomSharedPreferences;
import app.epm.com.utilities.helpers.IAppInformation;
import app.epm.com.utilities.helpers.ICustomNotificationHelper;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.helpers.VersionAppPlayStore;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.view.activities.BaseActivity;
import dagger.android.AndroidInjection;
import dagger.android.AndroidInjector;
import dagger.android.DispatchingAndroidInjector;
import dagger.android.support.HasSupportFragmentInjector;

import com.epm.app.view.views_activities.ISplashAppView;

import javax.inject.Inject;
//import com.microsoft.appcenter.AppCenter;
//import com.microsoft.appcenter.analytics.Analytics;
//import com.microsoft.appcenter.crashes.Crashes;



/**
 * Created by mateoquicenososa on 24/11/16.
 */

public class SplashAppActivity extends BaseActivity<SplashAppPresenter> implements ISplashAppView, HasSupportFragmentInjector {

    private ProgressBar splashApp_progressBar;
    private TextView splashApp_tvVersion;
    private ICustomSharedPreferences customSharedPreferences;
    private IAppInformation appInformation;
    private String versionPlayStore;
    private String versionApp;
    @Inject
    DispatchingAndroidInjector<Fragment> dispatchingAndroidInjector;
    @Inject
    NotificationsRepository notificationsRepository;
    @Inject
    public TurnServicesRepository turnServicesRepository;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_splash_app);
//        AppCenter.start(getApplication(), "1f47c64c-4b82-492a-be47-b49fda7d6da7",
//                Analytics.class, Crashes.class);
        appInformation = new AppInformation(this);
        customSharedPreferences = new CustomSharedPreferences(this);
        this.configureDagger();
        setPresenter(new SplashAppPresenter(DomainModule.getSecurityBLInstance(customSharedPreferences)));
        getPresenter().inject(this, getValidateInternet(), customSharedPreferences,notificationsRepository,turnServicesRepository);
        loadViews();
        createProgressDialog();
        showProgressBar();
        if (appInformation.isAppRunning()) {
            int apiLevelVersion = Build.VERSION.SDK_INT;
            if (apiLevelVersion == 19) {
                startLandingActivity();
                finish();
            }else{
                finish();
            }
            finish();
        } else {
            validateTimeGeneralListSaved();
        }
    }



    @Override
    public void loadCustomNotification(ICustomNotificationHelper customNotificationHelper) {
        //The method is not used.
    }

    /**
     * Valida tiempo de las listas guardadas.
     */
    private void validateTimeGeneralListSaved() {
        long dateSavedGeneralListLong = getDateSavedGeneralList();
        long currentDate = Calendar.getInstance().getTimeInMillis();
        if (currentDate > dateSavedGeneralListLong) {
            getPresenter().validateInternetToGetGeneralList();
        } else {
            validateUser();
        }
        /*else {
            new Handler().postDelayed(new Runnable() {
                @Override
                public void run() {
                    if (getValidateInternet().isConnected()) {
                        //getVersionAppPlayStore();
                        //getVersionApp();
                        //verifyVersionApp();
                        startLandingActivity();
                    } else {
                        showAlertDialogTryAgain(R.string.title_appreciated_user, R.string.text_validate_internet, R.string.text_intentar, R.string.text_abandonar);
                    }
                }
            }, Constants.POST_DELAY_SPLASH_TIME);
        }*/

    }

    @Override
    public void validateUser() {
        String invitado = getCustomSharedPreferences().getString(Constants.INVITADO);

        if (invitado.equals(Constants.FALSE)) {
            validateToken();
            getCustomSharedPreferences().addString(Constants.INVITADO, Constants.TRUE);
        }
    }

    /**
     * Valida si existe Token.
     */
    private void validateToken() {
        if (getCustomSharedPreferences().getString(Constants.TOKEN) == null || getCustomSharedPreferences().getString(Constants.TOKEN).isEmpty()) {
            getPresenter().validateInternetToGetGuestLogin();
        } else {
            getPresenter().validateInternetToGetAutoLogin(getCustomSharedPreferences().getString(Constants.TOKEN));
        }
    }

    /**
     * Inicia la actividad landing.
     */
    private void startLandingActivity() {
        Intent intent = new Intent(this, LandingActivity.class);
        int apiLevelVersion = Build.VERSION.SDK_INT;
        if (apiLevelVersion < 21) {
            intent.setFlags(Intent.FLAG_ACTIVITY_CLEAR_TASK);
        }
        Bundle bundle = new Bundle();
        UsuarioRequest usuarioRequest = new UsuarioRequest();
        usuarioRequest.setCorreoElectronico("test");
        bundle.putSerializable(Constants.USUARIO, usuarioRequest);
        intent.putExtras(bundle);
        startActivity(intent);
        if (apiLevelVersion >= 21) {
            finish();
        }
        if (apiLevelVersion >=16 && apiLevelVersion <= 18){
            finish();
        }

    }

    private long getDateSavedGeneralList() {
        String dateSavedGeneralList = customSharedPreferences.getString(Constants.CURRENT_DATE_SAVED_LIST);
        long dateMillis = 0;
        if (dateSavedGeneralList != null) {
            Date date = new Date(Long.parseLong(dateSavedGeneralList));
            Calendar calendar = Calendar.getInstance();
            calendar.setTime(date);
            calendar.set(Calendar.DAY_OF_MONTH, calendar.get(Calendar.DAY_OF_MONTH) + Constants.ONE);
            dateMillis = calendar.getTimeInMillis();
        }
        return dateMillis;
    }

    /**
     * Carga los controles de la vista.
     */
    private void loadViews() {
        splashApp_progressBar = (ProgressBar) findViewById(R.id.splashApp_progressBar);
        splashApp_tvVersion = (TextView) findViewById(R.id.splashApp_tvVersion);
        loadAppVersion();
    }

    /**
     * Carga la versión de la app.
     */
    private void loadAppVersion() {
        int verCode = 0;
        try {
            PackageInfo pInfo = getPackageManager().getPackageInfo(getPackageName(), 0);
            verCode = pInfo.versionCode;
        } catch (PackageManager.NameNotFoundException e) {
            e.printStackTrace();
        }
        splashApp_tvVersion.setText(customSharedPreferences.getString(Constants.APP_VERSION));
    }

    @Override
    public void showAlertDialogToLoadAgainOnUiThread(int title, int message) {
        showAlertDialogToLoadAgainOnUiThread(title, getResources().getString(message));
    }

    @Override
    public void showAlertDialogToLoadAgainOnUiThread(final int title, final String message) {
        runOnUiThread(() -> showAlertDialogToLoadAgain(title, message));
    }

    /**
     * Muestra alerta cuando no hay conexión a internet con boton cancelar y boton cargar de nuevo.
     *
     * @param title   title.
     * @param message message.
     */
    private void showAlertDialogToLoadAgain(int title, String message) {
        CustomAlertDialog customAlertDialog = new CustomAlertDialog(this);
        customAlertDialog.showAlertDialog(title, message, false, R.string.text_load_again, new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialog, int which) {
                showProgressBar();
                validateTimeGeneralListSaved();
                //validateToken();
            }
        }, R.string.text_abandonar, new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialogInterface, int i) {
                finish();
            }
        }, null);
    }

    @Override
    public String getCurrentDate() {
        Calendar calendar = Calendar.getInstance();
        return Long.toString(calendar.getTimeInMillis());
    }

    @Override
    public void hideProgressBarOnUiThread() {
        runOnUiThread(() -> hideProgressBar());
    }

    @Override
    public void validateUserIfHaveBeenLogin() {
        runOnUiThread(() -> {
            dismissProgressDialog();
            if (getValidateInternet().isConnected()) {
                //getVersionAppPlayStore();
                //getVersionApp();
                //verifyVersionApp();
                startLandingActivity();
            } else {
                showAlertDialogTryAgain(R.string.title_appreciated_user, R.string.text_validate_internet,R.string.text_load_again, R.string.text_abandonar);
            }
        });
    }

    private void showAlertDialogTryAgain(final int title, final int message, final int positive, final int negative) {
        runOnUiThread(() -> getCustomAlertDialog().showAlertDialog(title, message, false, positive, new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialogInterface, int i) {
               showProgressBar();
               validateTimeGeneralListSaved();
               //validateToken();
                /*final Handler handler = new Handler();
                handler.postDelayed(new Runnable() {
                    @Override
                    public void run() {
                        validateUserIfHaveBeenLogin();
                    }
                }, 500);*/

            }
        }, negative, new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialogInterface, int i) {
                finish();
            }
        }, null));
    }

    private void verifyVersionApp() {
        runOnUiThread(() -> {
            versionPlayStore = versionPlayStore.replace(".", "");
            versionApp = versionApp.replace(".", "");
            if (Integer.valueOf(versionPlayStore) > Integer.valueOf(versionApp)) {
                AlertDialog.Builder builder = new AlertDialog.Builder(SplashAppActivity.this);
                builder.setCancelable(false);
                builder.setTitle(R.string.title_updated_app);
                builder.setMessage(R.string.text_updated_app);
                builder.setPositiveButton(R.string.btn_acept, new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialogInterface, int i) {
                        startPlayStore();
                    }
                });
                builder.show();
            } else {
                startLandingActivity();
            }
        });
    }

    private void startPlayStore() {
        try {
            startActivity(new Intent(Intent.ACTION_VIEW, Uri.parse(Constants.URL_MARKET_APP_PLAY_STORE)));
        } catch (android.content.ActivityNotFoundException anfe) {
            startActivity(new Intent(Intent.ACTION_VIEW, Uri.parse(Constants.URL_APP_PLAY_STORE)));
        }
        finish();
    }

    private void getVersionAppPlayStore() {
        runOnUiThread(() -> {
            VersionAppPlayStore versionAppPlayStore = new VersionAppPlayStore();
            try {
                versionPlayStore = versionAppPlayStore.execute().get();
            } catch (InterruptedException interruptedException) {
                Log.i("Arcgis", "Interrupted");
                Thread.currentThread().interrupt();
            } catch (ExecutionException executionException) {
            }
        });
    }

    @Override
    public void setDataUser(Authoken authoken) {
        setUsuario(authoken.getUsuario());
        getUsuario().setInvitado(authoken.isInvitado());
        getCustomSharedPreferences().addString(Constants.TOKEN, authoken.getUsuario().getToken());
    }


    private void getVersionApp() {
        try {
            PackageInfo pInfo = getPackageManager().getPackageInfo(getPackageName(), 0);
            this.versionApp = pInfo.versionName;
        } catch (PackageManager.NameNotFoundException e) {
            Log.e("Exception", e.toString());
        }
    }

    private void showProgressBar() {
        splashApp_progressBar.setVisibility(View.VISIBLE);
    }

    private void hideProgressBar() {
        splashApp_progressBar.setVisibility(View.GONE);
    }

    private void configureDagger() {
        AndroidInjection.inject(this);
    }

    @Override
    public AndroidInjector<Fragment> supportFragmentInjector() {
        return dispatchingAndroidInjector;
    }
}