package com.epm.app.mvvm.comunidad.network;


import com.epm.app.mvvm.comunidad.network.request.NotificationSaveRequest;
import com.epm.app.mvvm.comunidad.network.request.ShowNotificationPushRequest;
import com.epm.app.mvvm.comunidad.network.request.UpdateNotificationStatusOneSignalRequest;
import com.epm.app.mvvm.comunidad.network.request.UpdateStatusNotificationRequest;
import com.epm.app.mvvm.comunidad.network.request.UpdateStatusSendNotificationRequest;
import com.epm.app.mvvm.comunidad.network.response.notifications.NotificationList;
import com.epm.app.mvvm.comunidad.network.response.notifications.NotificationSaveResponse;
import com.epm.app.mvvm.comunidad.network.response.notifications.ShowNotificationPushResponse;
import com.epm.app.mvvm.comunidad.network.response.notifications.UpdateStatusSendNotificationResponse;
import com.epm.app.mvvm.comunidad.network.response.subscriptions.GetNotificationsPush;
import com.epm.app.mvvm.comunidad.network.request.SolicitudEliminarNotificacionPushRecibida;
import com.epm.app.mvvm.comunidad.network.response.notifications.NotificationResponse;

import io.reactivex.Observable;
import io.reactivex.Single;
import retrofit2.Response;
import retrofit2.http.Body;
import retrofit2.http.GET;
import retrofit2.http.Header;
import retrofit2.http.POST;
import retrofit2.http.Query;

public interface NotificationsServices {

    /**
     * Obtiene la notificaciones push sin leer
     * @param idDevice
     * @return
     */
    @GET("AlertaNotificacionPush/ObtenerNumeroNotificacionesPushSinLeerPorDispositivo")
    Observable<GetNotificationsPush> getNotificationsPush(
            @Query("idDispositivo") String idDevice);



    /**
     * Obtiene la lista de notificaciones
     * @param idDevice
     * @param token
     * @param pageNumber
     * @param recordsPage
     * @return
     */
    @GET("AlertaNotificacionPush/ObtenerHistoricoDeNotificacionesPush")
    Observable<NotificationList> getListNotificationsPush(
            @Query("idDispositivo") String idDevice,
            @Query("registrosPorPagina")Integer recordsPage,
            @Query("numeroPagina")Integer pageNumber,
            @Header("authtoken") String token
    );

    @POST("Alerta/ActualizarEstadoEnvioNotificacion")
    Observable<UpdateStatusSendNotificationResponse> updateStatusNotification(@Body UpdateStatusSendNotificationRequest updateNotificationStatusOneSignalRequest, @Header("authToken") String token);


    @POST("AlertaNotificacionPush/ActualizarEstadoLecturaNotificacionIdNotificacionOneSignal")
    Observable<NotificationResponse> updateNotificationOneSignal(@Body UpdateNotificationStatusOneSignalRequest updateNotificationStatusOneSignalRequest);

    @POST("AlertaNotificacionPush/EliminarNotificacionPushRecibida")
    Observable<NotificationResponse> deleteNotificationPush(@Body SolicitudEliminarNotificacionPushRecibida solicitudEliminarNotificacionPushRecibida, @Header("authToken") String token);

    @POST("AlertaNotificacionPush/ActualizarEstadoLecturaNotificacionPushRecibida")
    Observable<NotificationResponse> updateNotificationPush(@Body UpdateStatusNotificationRequest updateStatusNotificationRequest, @Header("authToken") String token);

    @POST("AlertaNotificacionPush/GuardarNotificacionPushPorDispositivo")
    Observable<NotificationSaveResponse> saveNotificationPush(@Body NotificationSaveRequest notificationSaveRequest);

    @POST("AlertaNotificacionPush/MostrarNotificacionPush")
    Observable<ShowNotificationPushResponse> isShowNotificationPush(@Body ShowNotificationPushRequest showNotificationPushRequest, @Header("authtoken") String token);

}
