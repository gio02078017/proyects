package com.epm.app.di.module;

import com.epm.app.services.NotificationListenerService;

import dagger.Module;
import dagger.android.ContributesAndroidInjector;
@Module
public abstract class ServiceBuilderModule {

    @ContributesAndroidInjector
    abstract NotificationListenerService contributeNotificationListenerService();
}
