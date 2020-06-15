package com.epm.app.services;


import android.app.Service;
import android.util.Log;

import com.epm.app.mvvm.comunidad.network.request.NotificationSaveRequest;
import com.epm.app.mvvm.comunidad.network.request.ShowNotificationPushRequest;
import com.epm.app.mvvm.comunidad.network.response.notifications.NotificationSaveResponse;
import com.epm.app.mvvm.comunidad.network.response.notifications.ShowNotificationPushResponse;
import com.epm.app.mvvm.comunidad.network.response.notifications.TemplateOneSignal;
import com.epm.app.mvvm.comunidad.network.response.subscriptions.GetNotificationsPush;
import com.epm.app.mvvm.comunidad.repository.NotificationsRepository;
import com.onesignal.NotificationExtenderService;
import com.onesignal.OSNotificationReceivedResult;

import org.json.JSONException;
import org.json.JSONObject;

import java.util.Iterator;

import javax.inject.Inject;

import app.epm.com.utilities.helpers.CustomSharedPreferences;
import app.epm.com.utilities.helpers.IdDispositive;
import app.epm.com.utilities.helpers.ValidateInternet;
import app.epm.com.utilities.utils.ChangeStateTurnManager;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.utils.DisposableManager;
import app.epm.com.utilities.utils.NotificationManager;
import dagger.android.AndroidInjection;
import dagger.android.AndroidInjector;
import dagger.android.DispatchingAndroidInjector;
import dagger.android.HasServiceInjector;
import io.reactivex.Observable;
import io.reactivex.android.schedulers.AndroidSchedulers;
import io.reactivex.disposables.Disposable;
import io.reactivex.observers.DisposableObserver;
import io.reactivex.schedulers.Schedulers;

public class NotificationListenerService extends NotificationExtenderService implements HasServiceInjector {

    private NotificationSaveRequest notificationSaveRequest;
    private TemplateOneSignal templateOneSignal;
    private String body;
    private String title;
    private String notificationID;
    private IdDispositive idDispositive;
    private ValidateInternet validateInternet;
    private ShowNotificationPushRequest showNotificationPushRequest;
    @Inject
    public NotificationsRepository notificationsBL;
    @Inject
    public CustomSharedPreferences customSharedPreferences;

    @Inject
    DispatchingAndroidInjector<Service> dispatchingAndroidInjector;

    OverrideSettings overrideSettings;

   /*
   public void inject(NotificationsRepository notificationsRepository, CustomSharedPreferences customSharedPreferences){
      this.notificationsBL = notificationsRepository;
      this.customSharedPreferences = customSharedPreferences;
   }*/

    @Override
    public void onCreate() {
        super.onCreate();
        idDispositive = new IdDispositive(this);
        AndroidInjection.inject(this);
        templateOneSignal = new TemplateOneSignal();
        notificationSaveRequest = new NotificationSaveRequest();
        validateInternet = new ValidateInternet(this);
        showNotificationPushRequest = new ShowNotificationPushRequest();
    }

    @Override
    protected boolean onNotificationProcessing(OSNotificationReceivedResult receivedResult) {
        // Read Properties from result

        OverrideSettings overrideSettings = new OverrideSettings();
      /*overrideSettings.extender = new NotificationCompat.Extender() {
         @Override
         public NotificationCompat.Builder extend(NotificationCompat.Builder builder) {
            // Sets the background notification color to Red on Android 5.0+ devices.
            //return builder.setColor(new BigInteger("FFFF0000", 16).intValue());
             return builder.setStyle(new NotificationCompat.BigTextStyle().bigText("datos"));
         }
      };*/


        title = receivedResult.payload.title;
        body = receivedResult.payload.body;
        notificationID = receivedResult.payload.notificationID;


        JSONObject data =receivedResult.payload.additionalData;

        Iterator<String> iterator = receivedResult.payload.additionalData.keys();
        try {
            loadDatesNotification(iterator.next(),data.toJSONArray(data.names()).get(0).toString());
            saveNotifications();
            validateIfShowNotification(overrideSettings);
        } catch (JSONException e) {
            Log.e("error","No se pudo guardar la notifiación");
        }

        return true;
    }

    private void validateIfShowNotification(OverrideSettings overrideSettings) {
        showNotificationPushRequest.setTemplateOneSignal(templateOneSignal);
        showNotificationPushRequest.setIdDispositivo(IdDispositive.getIdDispositive());
        this.overrideSettings = overrideSettings;
        serviceIsShowNotificationPush();
    }

    private void serviceIsShowNotificationPush(){
        Observable<ShowNotificationPushResponse> result =  notificationsBL.isShowNotificationPush(showNotificationPushRequest,customSharedPreferences.getString(Constants.TOKEN));
        Disposable disposable = result.subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableObserver<ShowNotificationPushResponse>() {
                    @Override
                    public void onNext(ShowNotificationPushResponse showNotificationPushResponse) {
                        validateResponseIsShowNotification(showNotificationPushResponse);
                    }

                    @Override
                    public void onError(Throwable e) {
                        Log.e("error",e.getMessage());
                    }

                    @Override
                    public void onComplete() {

                    }
                });
        DisposableManager.add(disposable);
    }

    private void validateResponseIsShowNotification(ShowNotificationPushResponse showNotificationPushResponse){
        Log.e("notificacion", String.valueOf(showNotificationPushResponse.isMostrarNotificacionPush()));
        if (showNotificationPushResponse.isMostrarNotificacionPush()){
            displayNotification(overrideSettings);
        }
    }

    public void saveNotifications() {
        Observable<NotificationSaveResponse> result =  notificationsBL.saveNotificationPush(notificationSaveRequest);
        Disposable disposable = result.subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableObserver<NotificationSaveResponse>() {
                    @Override
                    public void onNext(NotificationSaveResponse notificationSaveResponse) {
                        validateResponseSaveNotification(notificationSaveResponse);
                    }

                    @Override
                    public void onError(Throwable e) {
                        Log.e("error",e.getMessage());
                    }

                    @Override
                    public void onComplete() {

                    }
                });
        DisposableManager.add(disposable);
    }

    public void validateResponseSaveNotification(NotificationSaveResponse notificationSaveResponse){
        if (notificationSaveResponse != null && notificationSaveResponse.getStateTransaction()) {
            getNotificationsPush();
            clearDates();
            NotificationManager.getInstance().getNotificationSubject().UpdateShiftStatus(notificationSaveRequest.getTemplateOneSignal().getValue());
            validateShiftState(notificationSaveRequest.getTemplateOneSignal().getValue());
        }
    }

    public void clearDates(){
        title = Constants.EMPTY_STRING;
        body = Constants.EMPTY_STRING;
        notificationID = Constants.EMPTY_STRING;
    }

    public void loadDatesNotification(String keyTemplate, String valueTemplate) {
        templateOneSignal.setKey(keyTemplate);
        templateOneSignal.setValue(valueTemplate);
        notificationSaveRequest.setTemplateOneSignal(templateOneSignal);
        notificationSaveRequest.setIdDevice(IdDispositive.getIdDispositive());
        notificationSaveRequest.setTitle(title);
        notificationSaveRequest.setMensaje(body);
        notificationSaveRequest.setNotificationID(notificationID+IdDispositive.getIdDispositive());
    }


    public void getNotificationsPush() {
            Observable<GetNotificationsPush> result = notificationsBL.getNotificationsPush(IdDispositive.getIdDispositive());
            Disposable disposable = result.subscribeOn(Schedulers.io())
                    .observeOn(AndroidSchedulers.mainThread())
                    .subscribeWith(new DisposableObserver<GetNotificationsPush>() {
                        @Override
                        public void onNext(GetNotificationsPush getNotificationsPush) {
                            validateResponseGetNotifications(getNotificationsPush);
                        }

                        @Override
                        public void onError(Throwable e) {
                            Log.e("error",e.getMessage());
                        }

                        @Override
                        public void onComplete() {

                        }
                    });
            DisposableManager.add(disposable);
    }

    public void validateResponseGetNotifications(GetNotificationsPush getNotificationsPush){
        if (getNotificationsPush != null && getNotificationsPush.getCantidadNotificacionesSinLeer() != null) {
            customSharedPreferences.addInt(Constants.NUMBER_NOTIFICATIONS, getNotificationsPush.getCantidadNotificacionesSinLeer());
            NotificationManager.getInstance().getNotificationSubject().addNotification();
        }
    }

    private void validateShiftState(String valueTemplate) {
        switch (valueTemplate){
            case Constants.MODULE_TURNO_ATENDIDO:
            case Constants.MODULE_TURNO_ABANDONADO:
                ChangeStateTurnManager.getInstance().getNotificationSubject().changeState();
                customSharedPreferences.deleteValue(Constants.ASSIGNED_TRUN);
                customSharedPreferences.deleteValue(Constants.INFORMATION_OFFICE_JSON);
                break;
            default:
                break;
        }
    }


    @Override
    public AndroidInjector<Service> serviceInjector() {
        return dispatchingAndroidInjector;
    }

}
