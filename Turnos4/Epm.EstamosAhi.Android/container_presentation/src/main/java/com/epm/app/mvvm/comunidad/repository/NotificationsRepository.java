package com.epm.app.mvvm.comunidad.repository;

import android.app.Notification;
import android.arch.lifecycle.LiveData;
import android.arch.lifecycle.MutableLiveData;

import com.epm.app.R;
import com.epm.app.mvvm.comunidad.bussinesslogic.INotificationsBL;
import com.epm.app.mvvm.comunidad.network.NotificationsServices;
import com.epm.app.mvvm.comunidad.network.request.NotificationSaveRequest;
import com.epm.app.mvvm.comunidad.network.request.ShowNotificationPushRequest;
import com.epm.app.mvvm.comunidad.network.request.SolicitudEliminarNotificacionPushRecibida;
import com.epm.app.mvvm.comunidad.network.request.UpdateNotificationStatusOneSignalRequest;
import com.epm.app.mvvm.comunidad.network.request.UpdateStatusNotificationRequest;
import com.epm.app.mvvm.comunidad.network.request.UpdateStatusSendNotificationRequest;
import com.epm.app.mvvm.comunidad.network.response.notifications.NotificationResponse;
import com.epm.app.mvvm.comunidad.network.response.notifications.NotificationList;
import com.epm.app.mvvm.comunidad.network.response.notifications.NotificationSaveResponse;
import com.epm.app.mvvm.comunidad.network.response.notifications.ShowNotificationPushResponse;
import com.epm.app.mvvm.comunidad.network.response.notifications.UpdateStatusSendNotificationResponse;
import com.epm.app.mvvm.comunidad.network.response.subscriptions.GetNotificationsPush;

import app.epm.com.utilities.helpers.ErrorMessage;
import app.epm.com.utilities.helpers.ValidateServiceCode;

import javax.inject.Inject;

import app.epm.com.utilities.helpers.IValidateInternet;
import app.epm.com.utilities.helpers.ValidateInternet;
import app.epm.com.utilities.utils.DisposableManager;
import io.reactivex.Observable;
import io.reactivex.android.schedulers.AndroidSchedulers;
import io.reactivex.disposables.Disposable;
import io.reactivex.schedulers.Schedulers;

public class NotificationsRepository implements INotificationsBL {

    private IValidateInternet validateInternet;
    private NotificationsServices notificationsServices;
    private MutableLiveData<ErrorMessage> error;

    @Inject
    public NotificationsRepository(NotificationsServices notificationsServices, ValidateInternet validateInternet) {
        this.validateInternet = validateInternet;
        this.notificationsServices = notificationsServices;
        error = new MutableLiveData<>();

    }

    @Override
    public Observable<GetNotificationsPush> getNotificationsPush(String idDispositive) {
        return notificationsServices.getNotificationsPush(idDispositive);
    }

    private void setError(int title, int message) {
        error.setValue(new ErrorMessage(title,message));
    }

    @Override
    public Observable<NotificationList> getListNotificationsPush(String idDispositive, String token, int recordsPage, int pageNumber) {
        return notificationsServices.getListNotificationsPush(idDispositive, recordsPage, pageNumber, token);
    }

    @Override
    public Observable<NotificationResponse> deleteNotificationsPush(int idNotification, String token) {
        SolicitudEliminarNotificacionPushRecibida solicitudEliminarNotificacionPushRecibida = new SolicitudEliminarNotificacionPushRecibida();
        solicitudEliminarNotificacionPushRecibida.setIdNotificacionPush(idNotification);
        return notificationsServices.deleteNotificationPush(solicitudEliminarNotificacionPushRecibida, token);
    }

    /**
     * Actualiza id de one signal por dispositivo con el id
     * que retorna el backend
     *
     * @return NotificationResponse
     */
    @Override
    public Observable<NotificationResponse> updateNotificationPush(int idNotification, String token) {
        UpdateStatusNotificationRequest notificationRequest = new UpdateStatusNotificationRequest();
        notificationRequest.setIdNotificationPush(idNotification);
        return  notificationsServices.updateNotificationPush(notificationRequest, token);
    }

    @Override
    public Observable<NotificationSaveResponse> saveNotificationPush(NotificationSaveRequest notificationSaveRequest) {
        return  notificationsServices.saveNotificationPush(notificationSaveRequest);
    }


    @Override
    public Observable<ShowNotificationPushResponse> isShowNotificationPush(ShowNotificationPushRequest showNotificationPushRequest, String token) {
        return notificationsServices.isShowNotificationPush(showNotificationPushRequest, token);
    }

    /**
     * Actualiza id de one signal por dispositivo, con el id
     * que trae la notificacion
     *
     * @return NotificationResponse
     */
    @Override
    public Observable<NotificationResponse> updateStatusNotification(UpdateNotificationStatusOneSignalRequest updateNotificationStatusOneSignal) {
        return  notificationsServices.updateNotificationOneSignal(updateNotificationStatusOneSignal);
    }

    @Override
    public Observable<UpdateStatusSendNotificationResponse> updateStatusSendNotification(UpdateStatusSendNotificationRequest updateStatusSendNotificationRequest, String token){
        return notificationsServices.updateStatusNotification(updateStatusSendNotificationRequest, token);
    }



}
