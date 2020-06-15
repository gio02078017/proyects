package com.epm.app.di.module;

import android.app.Application;
import android.content.Context;
import android.content.res.Resources;
import android.os.SystemClock;

import com.epm.app.App;
import com.epm.app.di.module.community.ViewModelModuleCommunity;
import com.epm.app.di.module.procedure.ViewModelModuleProcedure;
import com.epm.app.di.module.transactions.ViewModelModuleTransactions;
import com.epm.app.di.module.turn.ViewModelModuleTurn;
import com.epm.app.mvvm.comunidad.models.SharedPreferencesModel;
import com.epm.app.mvvm.comunidad.network.NotificationsServices;
import com.epm.app.mvvm.comunidad.network.WebViewService;
import com.epm.app.mvvm.comunidad.network.SuscriptionServices;
import com.epm.app.mvvm.comunidad.network.request.UpdateStatusSendNotificationRequest;
import com.epm.app.mvvm.comunidad.network.request.UpdateSubscriptionByMailRequest;
import com.epm.app.mvvm.comunidad.network.response.subscriptions.SolicitudSuscripcionNotificacionPush;
import com.epm.app.mvvm.comunidad.repository.RepositoryShared;
import com.epm.app.mvvm.procedure.network.ProcedureApiServices;
import com.epm.app.mvvm.transactions.network.TransactionServices;
import com.epm.app.mvvm.turn.network.TurnApiServices;
import com.epm.app.mvvm.util.FakeInterceptor;

import java.io.IOException;
import java.util.concurrent.TimeUnit;

import javax.inject.Named;
import javax.inject.Singleton;

import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.helpers.ValidateInternet;
import dagger.Module;
import dagger.Provides;
import io.reactivex.schedulers.Schedulers;
import okhttp3.Dispatcher;
import okhttp3.Interceptor;
import okhttp3.OkHttpClient;
import okhttp3.Response;
import okhttp3.logging.HttpLoggingInterceptor;
import retrofit2.Retrofit;
import retrofit2.adapter.rxjava2.RxJava2CallAdapterFactory;
import retrofit2.converter.gson.GsonConverterFactory;


@Module(includes = {
        ViewModelModuleCommunity.class,
        ViewModelModuleTurn.class,
        ViewModelModuleProcedure.class,
        ViewModelModuleTransactions.class
})
public class ApplicationModule {

    @Singleton
    @Provides
    public Application providerApplication(App app) {
        return app;
    }

    @Provides
    public UpdateStatusSendNotificationRequest providerUpdateStatusSendNotificationRequest() {
        return new UpdateStatusSendNotificationRequest();
    }

    @Provides
    public SolicitudSuscripcionNotificacionPush providerSolicitudSuscripcionNotificacionPush() {
        return new SolicitudSuscripcionNotificacionPush();
    }

    @Singleton
    @Provides
    public Resources providerResources(Application application) {
        return application.getResources();
    }

    @Singleton
    @Provides
    public UpdateSubscriptionByMailRequest providerUpdateSubscriptionByMailRequest() {
        return new UpdateSubscriptionByMailRequest();
    }

    @Singleton
    @Provides
    public Context providerContext(){
        return App.getContext();
    }

    @Provides
    @Singleton
    public ValidateInternet providerValidateInternet() {
        return new ValidateInternet(App.getContext());
    }

    @Provides
    @Singleton
    public SuscriptionServices providerPostApi(Retrofit restrofit) {
        return restrofit.create(SuscriptionServices.class);
    }

    @Provides
    @Singleton
    public WebViewService providerPostApiInformation(Retrofit retrofit) {
        return retrofit.create(WebViewService.class);
    }

    @Provides
    @Singleton
    public NotificationsServices providerNotificationsServices(Retrofit retrofit) {
        return retrofit.create(NotificationsServices.class);
    }

    @Provides
    @Singleton
    public OkHttpClient.Builder providerHttpClient() {
        OkHttpClient.Builder client = new OkHttpClient.Builder()
                .readTimeout(60, TimeUnit.SECONDS)
                .connectTimeout(60, TimeUnit.SECONDS);
        return client;
    }


    @Provides
    @Singleton
    public Retrofit providerRetrofit() {
        OkHttpClient.Builder okHttpClient = new OkHttpClient.Builder()
                .readTimeout(Constants.SIXTY, TimeUnit.SECONDS)
                .connectTimeout(Constants.SIXTY, TimeUnit.SECONDS)
                .retryOnConnectionFailure(true);

        HttpLoggingInterceptor interceptor = new HttpLoggingInterceptor();
        interceptor.setLevel(HttpLoggingInterceptor.Level.BODY);
        okHttpClient.addInterceptor(interceptor).build();

        return new Retrofit.Builder()
                .baseUrl(Constants.URL_DLLO_EPM+ "/")
                .addConverterFactory(GsonConverterFactory.create())
                .addCallAdapterFactory(RxJava2CallAdapterFactory.createWithScheduler(Schedulers.io()))
                .client(okHttpClient.build())
                .build();
    }

    @Provides
    @Singleton
    @Named("retrofitMock")
    public Retrofit providerRetrofitMpck() {
        OkHttpClient.Builder okHttpClient = new OkHttpClient.Builder()
                .readTimeout(Constants.SIXTY, TimeUnit.SECONDS)
                .connectTimeout(Constants.SIXTY, TimeUnit.SECONDS)
                .retryOnConnectionFailure(true);

        HttpLoggingInterceptor interceptor = new HttpLoggingInterceptor();
        interceptor.setLevel(HttpLoggingInterceptor.Level.BODY);



        okHttpClient.addInterceptor( new FakeInterceptor()).build();

        return new Retrofit.Builder()
                .baseUrl(Constants.URL_DLLO_EPM+ "/")
                .addConverterFactory(GsonConverterFactory.create())
                .addCallAdapterFactory(RxJava2CallAdapterFactory.createWithScheduler(Schedulers.io()))
                .client(okHttpClient.build())
                .build();
    }



    @Provides
    @Singleton
    public SharedPreferencesModel providerSharedPreferencesModel() {
        return new SharedPreferencesModel();
    }

    @Named("repositoryShared")
    @Provides
    public RepositoryShared providerRepositoryShared(SharedPreferencesModel sharedPreferencesModel) {
        return new RepositoryShared(sharedPreferencesModel);
    }

    @Provides
    @Singleton
    public TurnApiServices providerTurnApiServices(Retrofit retrofit){
        return retrofit.create(TurnApiServices.class);
    }

    @Provides
    @Singleton
    public ProcedureApiServices providerProcedureApiServices(Retrofit retrofit){
        return retrofit.create(ProcedureApiServices.class);
    }

    @Provides
    @Singleton
    public TransactionServices providerTransactionServices(Retrofit retrofit){
        return retrofit.create(TransactionServices.class);
    }
}
