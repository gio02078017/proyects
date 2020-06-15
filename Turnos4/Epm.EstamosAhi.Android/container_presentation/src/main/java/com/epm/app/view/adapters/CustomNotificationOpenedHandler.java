package com.epm.app.view.adapters;

import android.util.Log;

import com.epm.app.App;
import com.epm.app.mvvm.comunidad.bussinesslogic.INotificationsBL;
import com.epm.app.mvvm.comunidad.network.request.UpdateNotificationStatusOneSignalRequest;
import com.epm.app.mvvm.comunidad.network.response.notifications.NotificationResponse;
import com.epm.app.mvvm.comunidad.network.response.notifications.UpdateStatusSendNotificationResponse;
import com.epm.app.mvvm.comunidad.network.response.subscriptions.GetNotificationsPush;
import com.epm.app.mvvm.comunidad.repository.NotificationsRepository;
import com.onesignal.OSNotificationOpenResult;
import com.onesignal.OneSignal;

import org.json.JSONException;
import org.json.JSONObject;
import org.json.JSONStringer;

import app.epm.com.utilities.helpers.ICustomSharedPreferences;
import app.epm.com.utilities.helpers.IdDispositive;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.utils.DisposableManager;
import app.epm.com.utilities.utils.NotificationManager;
import io.reactivex.Observable;
import io.reactivex.android.schedulers.AndroidSchedulers;
import io.reactivex.disposables.Disposable;
import io.reactivex.observers.DisposableObserver;
import io.reactivex.schedulers.Schedulers;

/**
 * Created by josetabaresramirez on 6/12/16.
 */

public class CustomNotificationOpenedHandler implements OneSignal.NotificationOpenedHandler {

    private ICustomSharedPreferences customSharedPreferences;
    private App app;
    private INotificationsBL notificationsBL;
    private UpdateNotificationStatusOneSignalRequest updateNotificationStatus;

    public CustomNotificationOpenedHandler(App app, ICustomSharedPreferences customSharedPreferences, NotificationsRepository notificationsRepository) {
        this.app = app;
        this.customSharedPreferences = customSharedPreferences;
        this.notificationsBL = notificationsRepository;
        this.updateNotificationStatus = new UpdateNotificationStatusOneSignalRequest();
    }

    @Override
    public void notificationOpened(OSNotificationOpenResult result) {
        JSONObject data = result.notification.payload.additionalData;
        if (data != null) {
            customSharedPreferences.addString(Constants.EXIST_NOTIFICATION, Constants.NOTIFICATION);
            updateNotification(result.notification.payload.notificationID);
            if (data != null && data.has(Constants.TYPE_FACTURA_NOTIFICATION)) {
                customSharedPreferences.addString(Constants.ACTION_TO_EXECUTE, Constants.TYPE_FACTURA_NOTIFICATION);
            } else {
                String tagNotification = "";
                try {
                    tagNotification = data.toJSONArray(data.names()).get(0).toString();
                } catch (JSONException e) {
                    Log.e("Exception", e.toString());
                }
                //customSharedPreferences.addBoolean(Constants.SHOW_BELL,false);
                switch (tagNotification) {
                    case Constants.MODULE_FACTURA:
                        customSharedPreferences.addString(Constants.ACTION_TO_EXECUTE, Constants.MODULE_FACTURA);
                        break;
                    case Constants.MODULE_NOTICIAS:
                        customSharedPreferences.addString(Constants.ACTION_TO_EXECUTE, Constants.MODULE_NOTICIAS);
                        break;
                    case Constants.MODULE_LINEAS_DE_ATENCION:
                        customSharedPreferences.addString(Constants.ACTION_TO_EXECUTE, Constants.MODULE_LINEAS_DE_ATENCION);
                        break;
                    case Constants.MODULE_SERVICIO_AL_CLIENTE:
                        customSharedPreferences.addString(Constants.ACTION_TO_EXECUTE, Constants.MODULE_SERVICIO_AL_CLIENTE);
                        break;
                    case Constants.MODULE_REPORTE_FRAUDES:
                        customSharedPreferences.addString(Constants.ACTION_TO_EXECUTE, Constants.MODULE_REPORTE_FRAUDES);
                        break;
                    case Constants.MODULE_CONTACTO_TRANSPARENTE:
                        customSharedPreferences.addString(Constants.ACTION_TO_EXECUTE, Constants.MODULE_CONTACTO_TRANSPARENTE);
                        break;
                    case Constants.MODULE_REPORTE_DANIOS:
                        customSharedPreferences.addString(Constants.ACTION_TO_EXECUTE, Constants.MODULE_REPORTE_DANIOS);
                        break;
                    case Constants.MODULE_EVENTOS:
                        customSharedPreferences.addString(Constants.ACTION_TO_EXECUTE, Constants.MODULE_EVENTOS);
                        break;
                    case Constants.MODULE_ESTACIONES_DE_GAS:
                        customSharedPreferences.addString(Constants.ACTION_TO_EXECUTE, Constants.MODULE_ESTACIONES_DE_GAS);
                        break;
                    case Constants.MODULE_DE_ALERTASHIDROITUANGO:
                        customSharedPreferences.addString(Constants.ACTION_TO_EXECUTE, Constants.MODULE_DE_ALERTASHIDROITUANGO);
                        break;
                    case Constants.MODULE_TURNO_ATENDIDO:
                        customSharedPreferences.addString(Constants.ACTION_TO_EXECUTE, Constants.MODULE_TURNO_ATENDIDO);
                        break;
                    case Constants.MODULE_TURNO_ABANDONADO:
                        customSharedPreferences.addString(Constants.ACTION_TO_EXECUTE, Constants.MODULE_TURNO_ABANDONADO);
                        break;
                    case Constants.MODULE_TURNO_AVANCE:
                        customSharedPreferences.addString(Constants.ACTION_TO_EXECUTE, Constants.MODULE_TURNO_AVANCE);
                        break;
                    default:
                        break;
                }
            }
        }
        app.notiftyFromOneSignal();
    }

    public void updateNotification(String idNotification) {
        updateNotificationStatus.setIdNotificationOneSignal(idNotification + IdDispositive.getIdDispositive());
        Observable<NotificationResponse> result = notificationsBL.updateStatusNotification(updateNotificationStatus);
        Disposable disposable = result.subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableObserver<NotificationResponse>() {
                    @Override
                    public void onNext(NotificationResponse notificationResponse) {
                        validateResponseUpdateStatusNotification(notificationResponse);
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

    public void validateResponseUpdateStatusNotification(NotificationResponse notificationResponse){
        if (notificationResponse != null && notificationResponse.getEstadoTransaccion()) {
            getNotificationsPush();
        }
    }


}