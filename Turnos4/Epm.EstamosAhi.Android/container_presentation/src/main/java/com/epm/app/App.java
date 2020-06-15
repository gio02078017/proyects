package com.epm.app;


import android.app.Activity;
import android.app.Fragment;
import android.app.Service;
import android.content.Context;
import android.content.Intent;
import android.content.pm.PackageInfo;
import android.content.pm.PackageManager;
import android.support.multidex.MultiDex;
import android.support.multidex.MultiDexApplication;
import android.util.Log;

import com.crashlytics.android.Crashlytics;
import com.epm.app.di.component.DaggerApplicationComponent;
import com.epm.app.mvvm.comunidad.repository.NotificationsRepository;
import com.epm.app.mvvm.comunidad.views.activities.NotificationCenterActivity;
import com.epm.app.mvvm.comunidad.views.activities.SplashActivity;
import com.epm.app.mvvm.turn.views.activities.ChannelsOfAttentionActivity;
import com.epm.app.mvvm.turn.views.activities.CustomerServiceMenuOptionActivity;
import com.epm.app.mvvm.turn.views.activities.ShiftInformationActivity;
import com.epm.app.view.activities.EstacionesDeGasActivity;
import com.epm.app.view.activities.EventosActivity;
import com.epm.app.view.activities.LandingActivity;
import com.epm.app.view.activities.LineasDeAtencionActivity;
import com.epm.app.view.activities.NoticiasActivity;
import com.epm.app.view.activities.SplashAppActivity;
import com.epm.app.view.adapters.CustomNotificationOpenedHandler;
import com.google.android.gms.analytics.GoogleAnalytics;
import com.google.android.gms.analytics.HitBuilders;
import com.google.android.gms.analytics.Tracker;
import com.onesignal.OneSignal;

import javax.inject.Inject;

import app.epm.com.contacto_transparente_presentation.view.activities.HomeContactoTransparenteActivity;
import app.epm.com.factura_presentation.view.activities.ConsultFacturaActivity;
import app.epm.com.factura_presentation.view.activities.FacturasConsultadasActivity;
import app.epm.com.reporte_danios_presentation.view.activities.ServiciosDanioActivity;
import app.epm.com.reporte_fraudes_presentation.view.activities.ServicesDeFraudesActivity;
import app.epm.com.security_presentation.view.activities.ChangePasswordActivity;
import app.epm.com.security_presentation.view.activities.MyProfileActivity;
import app.epm.com.security_presentation.view.activities.RegisterLoginActivity;
import app.epm.com.utilities.controls.FactoryControls;
import app.epm.com.utilities.helpers.CustomApplicationContext;
import app.epm.com.utilities.helpers.CustomSharedPreferences;
import app.epm.com.utilities.helpers.ICustomApplicationContext;
import app.epm.com.utilities.helpers.ICustomNotificationHelper;
import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.utils.Constants;
import dagger.android.AndroidInjector;
import dagger.android.DispatchingAndroidInjector;
import dagger.android.HasActivityInjector;
import dagger.android.HasFragmentInjector;
import dagger.android.HasServiceInjector;
import io.fabric.sdk.android.Fabric;

/**
 * Created by josetabaresramirez on 15/11/16.
 */

public class App extends MultiDexApplication implements ICustomApplicationContext, HasActivityInjector, HasServiceInjector, HasFragmentInjector {

    private ICustomSharedPreferences customSharedPreferences;
    private ICustomNotificationHelper customNotificationHelper;
    private Tracker mTracker;


    @Inject
    DispatchingAndroidInjector<Activity> dispatchingAndroidInjector;

    @Inject
    DispatchingAndroidInjector<Service> dispatchingServiceInjector;

    @Inject
    DispatchingAndroidInjector<Fragment> fragmentInjector;

    private static Context context;

    @Inject
    public NotificationsRepository notificationsRepository;

    @Override
    public void onCreate() {
        super.onCreate();
        customSharedPreferences = new CustomSharedPreferences(this);
        context = getApplicationContext();
        Fabric.with(this, new Crashlytics());
        this.initDagger();
        CustomApplicationContext.createInstance(this);
        getOneSignalId();
        OneSignal.startInit(this)
                .setNotificationOpenedHandler(new CustomNotificationOpenedHandler(this, new CustomSharedPreferences(this),notificationsRepository))
                .inFocusDisplaying(OneSignal.OSInFocusDisplayOption.Notification)
                .init();
        setContextToFactoryControls();
        loadAppVersion();
        loadClassesName();
    }

    private void getOneSignalId() {
        OneSignal.idsAvailable((userId, registrationId) -> saveOneSignalId(userId));
    }

    private void saveOneSignalId(String userId) {
        if (userId != null && !userId.isEmpty()) {
            customSharedPreferences.addString(Constants.ONE_SIGNAL_ID, userId);
        } else {
            getOneSignalId();
        }
    }

    @Override
    protected void attachBaseContext(Context base) {
        super.attachBaseContext(base);
        MultiDex.install(this);
    }

    private void loadClassesName() {
        customSharedPreferences.addString(Constants.LANDING_CLASS, LandingActivity.class.getName());
        customSharedPreferences.addString(Constants.REGISTER_LOGIN_CLASS, RegisterLoginActivity.class.getName());
        customSharedPreferences.addString(Constants.CHANGE_PSWORD_CLASS, ChangePasswordActivity.class.getName());
        customSharedPreferences.addString(Constants.MY_PROFILE_CLASS, MyProfileActivity.class.getName());
        customSharedPreferences.addString(Constants.FACTURA_CLASS, FacturasConsultadasActivity.class.getName());
        customSharedPreferences.addString(Constants.INVITADO, Constants.LOGIN_BOOLEAN);
        customSharedPreferences.addString(Constants.NOTIFICATION_CENTER_CLASS, NotificationCenterActivity.class.getName());
        customSharedPreferences.addString(Constants.SPLASH_CLASS, SplashAppActivity.class.getName());
        customSharedPreferences.addString(Constants.SHIFT_INFORMATION_ACTIVITY, ShiftInformationActivity.class.getName());
        customSharedPreferences.addString(Constants.GAS_STATIONS, EstacionesDeGasActivity.class.getName());
        customSharedPreferences.addString(Constants.HIDROITUANGO_ALERS, SplashActivity.class.getName());
        customSharedPreferences.addString(Constants.ATTENDED_SHIFT, CustomerServiceMenuOptionActivity.class.getName());
        customSharedPreferences.addString(Constants.EVENTS_ACTIVITY, EventosActivity.class.getName());
        customSharedPreferences.addString(Constants.FRAUD_REPORT, ServicesDeFraudesActivity.class.getName());
        customSharedPreferences.addString(Constants.TRANSPARENT_CONTACT, HomeContactoTransparenteActivity.class.getName());
        customSharedPreferences.addString(Constants.NEWS_ACTIVITY, NoticiasActivity.class.getName());
        customSharedPreferences.addString(Constants.ATENTION_LINES_ACTIVITY, LineasDeAtencionActivity.class.getName());
        customSharedPreferences.addString(Constants.CUSTOMER_SERVICE, CustomerServiceMenuOptionActivity.class.getName());
        customSharedPreferences.addString(Constants.DAMAGE_REPORT, ServiciosDanioActivity.class.getName());
        customSharedPreferences.addString(Constants.ATENTION_CHANNEL, ChannelsOfAttentionActivity.class.getName());
        customSharedPreferences.addString(Constants.CHECK_INVOCE, ConsultFacturaActivity.class.getName());
    }

    public void setCustomNotificationHelper(ICustomNotificationHelper customNotificationHelper) {
        this.customNotificationHelper = customNotificationHelper;
    }

    @Override
    public void sendReport(String report) {
        mTracker = getDefaultTracker();
        mTracker.setScreenName(report);
        mTracker.send(new HitBuilders.ScreenViewBuilder().build());
    }

    /**
     * Gets the default Tracker for this Application.
     *
     * @return Tracker.
     */
    synchronized public Tracker getDefaultTracker() {
        if (mTracker == null) {
            GoogleAnalytics analytics = GoogleAnalytics.getInstance(this);
            // To enable debug logging use: adb shell setprop log.tag.GAv4 DEBUG
           mTracker = analytics.newTracker(R.xml.global_tracker);
        }
        return mTracker;
    }

    private void loadAppVersion() {
        PackageInfo pInfo;
        try {
            pInfo = getPackageManager().getPackageInfo(getPackageName(), 0);
            String version = "v " + pInfo.versionName;
            customSharedPreferences.addString(Constants.APP_VERSION, version);
        } catch (PackageManager.NameNotFoundException e) {
            customSharedPreferences.addString(Constants.APP_VERSION, "v 3");
        }
    }

    private void setContextToFactoryControls() {
        FactoryControls.setContext(this);
    }

    public void notiftyFromOneSignal() {
        if (customNotificationHelper != null) {
            customNotificationHelper.notifyFromOneSignal();
        } else {
            notifyFromOneSignal();
        }
    }

    public void notifyFromOneSignal() {
        String actionToExecute = customSharedPreferences.getString(Constants.ACTION_TO_EXECUTE);
        if (actionToExecute != null) {
            try {
                Class clazz = Class.forName(customSharedPreferences.getString(Constants.SPLASH_CLASS));
                Intent intent = new Intent(this, clazz);
                intent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
                startActivity(intent);
            } catch (ClassNotFoundException e) {
                Log.e("Exception", e.toString());
            }
        }
    }


    private void initDagger(){
        DaggerApplicationComponent.builder().create(this).inject(this);
    }

    @Override
    public AndroidInjector<Activity> activityInjector() {
        return dispatchingAndroidInjector;
    }

    @Override
    public AndroidInjector<Service> serviceInjector() {
        return dispatchingServiceInjector;
    }

    @Override
    public AndroidInjector<Fragment> fragmentInjector() {
        return fragmentInjector;
    }

    public static Context getContext() {
        return context;
    }
}