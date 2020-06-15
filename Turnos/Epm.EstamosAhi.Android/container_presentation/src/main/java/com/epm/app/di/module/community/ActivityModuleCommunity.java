package com.epm.app.di.module.community;

import com.epm.app.mvvm.comunidad.views.activities.AlertasActivity;
import com.epm.app.mvvm.comunidad.views.activities.DashboardComunityAlertActivity;
import com.epm.app.mvvm.comunidad.views.activities.InformationOfInterestActivity;
import com.epm.app.mvvm.comunidad.views.activities.NotificationCenterActivity;
import com.epm.app.mvvm.comunidad.views.activities.PrivacyPolicyActivity;
import com.epm.app.mvvm.comunidad.views.activities.RecuperationSubscriptionActitity;
import com.epm.app.mvvm.comunidad.views.activities.RedAlertInformationActivity;
import com.epm.app.mvvm.comunidad.views.activities.SplashActivity;
import com.epm.app.mvvm.comunidad.views.activities.TutorialActivity;
import com.epm.app.mvvm.comunidad.views.activities.UpdateSubscriptionActivity;
import com.epm.app.view.activities.LandingActivity;
import com.epm.app.view.activities.SplashAppActivity;

import dagger.Module;
import dagger.android.ContributesAndroidInjector;

/**
 * Created by Philippe on 02/03/2018.
 */

@Module
public abstract class ActivityModuleCommunity {
    @ContributesAndroidInjector
    abstract SplashActivity contributeSplashActivity();
    @ContributesAndroidInjector
    abstract TutorialActivity contributeTutorialActivity();
    @ContributesAndroidInjector
    abstract AlertasActivity contributeAlertasActivity();
    @ContributesAndroidInjector
    abstract LandingActivity contributeLandingActivity();
    @ContributesAndroidInjector
    abstract DashboardComunityAlertActivity contributeDashboardComunityAlertActivity();
    @ContributesAndroidInjector
    abstract UpdateSubscriptionActivity contributeUpdateSubscriptionActivity();
    @ContributesAndroidInjector
    abstract InformationOfInterestActivity contributeInformationOfInterestActivity();
    @ContributesAndroidInjector
    abstract RedAlertInformationActivity contributeRedAlertInformationActivity();
    @ContributesAndroidInjector
    abstract PrivacyPolicyActivity contributePrivacyPolicyActivity();
    @ContributesAndroidInjector
    abstract NotificationCenterActivity contributeNotificationCenterActivity();
    @ContributesAndroidInjector
    abstract SplashAppActivity contributeSplashAppActivity();
    @ContributesAndroidInjector
    abstract RecuperationSubscriptionActitity contributeRecuperationSubscriptionActitity();


}
