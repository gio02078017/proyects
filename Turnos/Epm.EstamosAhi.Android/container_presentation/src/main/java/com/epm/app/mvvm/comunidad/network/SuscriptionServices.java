package com.epm.app.mvvm.comunidad.network;

import com.epm.app.mvvm.comunidad.network.request.CancelSubscriptionRequest;
import com.epm.app.mvvm.comunidad.network.request.UpdateSubscriptionByMailRequest;
import com.epm.app.mvvm.comunidad.network.response.notifications.NotificationResponse;
import com.epm.app.mvvm.comunidad.network.response.subscriptions.CancelSubscriptionResponse;
import com.epm.app.mvvm.comunidad.network.response.subscriptions.GetDetailRedAlertResponse;
import com.epm.app.mvvm.comunidad.network.response.subscriptions.GetSubscriptionNotificationsPush;
import com.epm.app.mvvm.comunidad.network.response.places.ObtenerMunicipios;
import com.epm.app.mvvm.comunidad.network.response.places.ObtenerSectores;
import com.epm.app.mvvm.comunidad.network.response.subscriptions.GetSubscriptionNotificationsPushAlertasItuango;
import com.epm.app.mvvm.comunidad.network.response.subscriptions.RecoverySubscriptionResponse;
import com.epm.app.mvvm.comunidad.network.response.subscriptions.Subscription;
import com.epm.app.mvvm.comunidad.network.response.subscriptions.SolicitudActualizarEstadoSuscripcion;
import com.epm.app.mvvm.comunidad.network.response.subscriptions.SolicitudSuscripcionNotificacionPush;
import com.epm.app.mvvm.comunidad.network.response.subscriptions.RequestUpdateSubscription;
import com.epm.app.mvvm.comunidad.network.response.subscriptions.UpdateSubscription;

import io.reactivex.Observable;
import io.reactivex.Single;
import retrofit2.Response;
import retrofit2.http.Body;
import retrofit2.http.GET;
import retrofit2.http.Header;
import retrofit2.http.POST;
import retrofit2.http.Query;

public interface SuscriptionServices {

    /**
     * Get the list of the pots from the API
     * @param token
     */
    @GET("MaestrosAlerta/ObtenerMunicipios")
    Single<Response<ObtenerMunicipios>> getObtenerMunicipios(
            @Header("authToken") String token, @Query("idAlerta") int idSubscription
    );

    @GET("MaestrosAlerta/ObtenerSectores")
    Single<Response<ObtenerSectores>> getObtenerSectores(
            @Query("id") int id,
            @Header("authToken") String token
    );

    @POST("Alerta/GuardarSuscripcionNotificacionesPush")
    Single<Response<Subscription>> saveSuscriptionAlertas(@Header("authToken") String token, @Body SolicitudSuscripcionNotificacionPush solicitudSuscripcionNotificacionPush);

    @POST("Alerta/ActualizarEstadoSuscripcionNotificacionesPush")
    Single<Response<Subscription>> updateSuscriptionAlertasState(@Header("authToken") String token, @Body SolicitudActualizarEstadoSuscripcion solicitudActualizarEstadoSuscripcion);

    @POST("Alerta/ActualizarSuscripcionNotificacionesPush")
    Observable<UpdateSubscription> updateSuscriptionAlertas(@Header("authToken") String token, @Body RequestUpdateSubscription requestUpdateSubscription);

    /**
     * @param token
     * @param updateSubscriptionByMailRequest
     * @return
     */
    @POST("SuscripcionNotificaciones/ActualizarSuscripcionPorCorreo")
    Single<Response<NotificationResponse>> updateSubscriptionByMail(@Header("authToken") String token,
                                                                    @Body UpdateSubscriptionByMailRequest updateSubscriptionByMailRequest);


    /**
     *
     * @param idDispositive
     * @param token
     * @return
     */
    @GET("Alerta/ObtenerSuscripcionNotificacionesPushAlertasItuango")
    Single<Response<GetSubscriptionNotificationsPushAlertasItuango>> getSuscriptionNotificationsPushAlertasItuango(
            @Query("idDispositivo") String idDispositive,
            @Header("authtoken") String token
    );

    /**
     * Actualiza id de one signal por dispositivo
     * @param idDispositive
     * @param idAplication
     * @param token
     * @return
     */
    @GET("Alerta/ObtenerSuscripcionesNotificacionesPush")
    Single<Response<GetSubscriptionNotificationsPush>> getSuscriptionNotificationsPush(
            @Query("idDispositivo") String idDispositive,
            @Query("idAplicacion") int idAplication,
            @Query("idSuscripcionOneSignal") String idSuscriptionOneSignal,
            @Header("authtoken") String token
    );

    /**
     *
     * @param idDispositive
     * @param token
     * @return
     */
    @GET("Alerta/ObtenerDetalleNotificacionPush")
    Single<Response<GetDetailRedAlertResponse>> getDetailRedAlert(
            @Query("idDispositivo") String idDispositive,
            @Header("authtoken") String token
    );



    /**
     * @param token
     * @param cancelSubscriptionRequest
     * @return
     */
    @POST("Alerta/CancelarSuscripcionNotificacionesPush")
    Observable<CancelSubscriptionResponse> cancelSubscription(@Header("authToken") String token,
                                                               @Body CancelSubscriptionRequest cancelSubscriptionRequest);


    @GET("SuscripcionNotificaciones/ObtenerSuscripcionDispositivoAlertasPorCorreoElectronico")
    Single<Response<RecoverySubscriptionResponse>> getSubscriptionDeviceAlertsByEmail(
            @Query("correoElectronico") String email,
            @Header("authToken") String token
    );




}
