package com.epm.app.mvvm.comunidad.bussinesslogic;

import androidx.lifecycle.LiveData;
import androidx.lifecycle.MutableLiveData;

import com.epm.app.mvvm.comunidad.network.request.CancelSubscriptionRequest;
import com.epm.app.mvvm.comunidad.network.request.UpdateSubscriptionByMailRequest;
import com.epm.app.mvvm.comunidad.network.response.notifications.NotificationResponse;
import com.epm.app.mvvm.comunidad.network.response.subscriptions.CancelSubscriptionResponse;
import com.epm.app.mvvm.comunidad.network.response.subscriptions.GetDetailRedAlertResponse;
import com.epm.app.mvvm.comunidad.network.response.subscriptions.GetSubscriptionNotificationsPush;
import com.epm.app.mvvm.comunidad.network.response.subscriptions.GetSubscriptionNotificationsPushAlertasItuango;
import com.epm.app.mvvm.comunidad.network.response.subscriptions.RecoverySubscriptionResponse;
import com.epm.app.mvvm.comunidad.network.response.subscriptions.Subscription;
import com.epm.app.mvvm.comunidad.network.response.subscriptions.SolicitudActualizarEstadoSuscripcion;
import com.epm.app.mvvm.comunidad.network.response.subscriptions.SolicitudSuscripcionNotificacionPush;
import com.epm.app.mvvm.comunidad.network.response.subscriptions.RequestUpdateSubscription;
import com.epm.app.mvvm.comunidad.network.response.subscriptions.UpdateSubscription;
import com.epm.app.mvvm.util.IError;

import io.reactivex.Observable;

public interface ISubscriptionBL extends IError {


    MutableLiveData<Subscription> saveSubscription(String token, SolicitudSuscripcionNotificacionPush solicitudSuscripcionNotificacionPush);
    MutableLiveData<Subscription> updateSubscriptionStatus(String token, SolicitudActualizarEstadoSuscripcion solicitudActualizarEstadoSuscripcion);
    MutableLiveData<GetSubscriptionNotificationsPushAlertasItuango> getSubscriptionAlertasItuango(String token, String idDispositive);
    MutableLiveData<GetSubscriptionNotificationsPush> getListSubscriptionAlertas(String token, String idDispositive, int idAplication,String idSuscriptionOneSignal);
    Observable<UpdateSubscription> updateSubscription(String token, RequestUpdateSubscription requestUpdateSubscription);
    Observable<CancelSubscriptionResponse> cancelSubscription(String token, CancelSubscriptionRequest cancelSubscriptionRequest);
    MutableLiveData<RecoverySubscriptionResponse> getSubscriptionDeviceAlertsByEmail(String email, String token);
    MutableLiveData<NotificationResponse> updateSubscriptionDeviceAlertsByEmail(UpdateSubscriptionByMailRequest updateSubscriptionByMailRequest, String token);
    MutableLiveData<GetDetailRedAlertResponse> getDetailRedAlert(String idDevice, String token);


}
