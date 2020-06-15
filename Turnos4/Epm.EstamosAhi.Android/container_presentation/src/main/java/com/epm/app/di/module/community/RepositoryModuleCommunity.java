package com.epm.app.di.module.community;


import android.app.Application;

import com.epm.app.mvvm.comunidad.network.NotificationsServices;
import com.epm.app.mvvm.comunidad.network.SuscriptionServices;
import com.epm.app.mvvm.comunidad.network.WebViewService;
import com.epm.app.mvvm.comunidad.repository.NotificationsRepository;
import com.epm.app.mvvm.comunidad.repository.PlacesRepository;
import com.epm.app.mvvm.comunidad.repository.SubscriptionRepository;
import com.epm.app.mvvm.comunidad.repository.WebViewRepository;

import javax.inject.Singleton;

import app.epm.com.utilities.helpers.CustomSharedPreferences;
import app.epm.com.utilities.helpers.ValidateInternet;
import dagger.Module;
import dagger.Provides;

@Module(includes = ViewModelModuleCommunity.class)
public class RepositoryModuleCommunity {

    @Provides
    @Singleton
    public CustomSharedPreferences providerCustomSharedPreferences(Application application){
        return new CustomSharedPreferences(application);
    }

    @Provides
    public PlacesRepository providerPlacesRepository(SuscriptionServices suscriptionServices, ValidateInternet validateInternet){
        return new PlacesRepository(suscriptionServices,validateInternet);

    }

    @Provides
    public SubscriptionRepository providerActualizationSubscriptionRepository(SuscriptionServices suscriptionServices, ValidateInternet validateInternet){
        return new SubscriptionRepository(suscriptionServices,validateInternet);
    }

    @Provides
    public WebViewRepository providerInformationInterestRepository(WebViewService webViewService, ValidateInternet validateInternet){
        return new WebViewRepository(webViewService,validateInternet);
    }


    @Provides
    public NotificationsRepository providerNotificationsRepository(NotificationsServices notificationsServices, ValidateInternet validateInternet){
        return new NotificationsRepository(notificationsServices,validateInternet);
    }

}
