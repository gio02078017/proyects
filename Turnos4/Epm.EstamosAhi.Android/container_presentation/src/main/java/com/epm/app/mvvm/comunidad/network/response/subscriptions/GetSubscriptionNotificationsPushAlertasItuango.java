package com.epm.app.mvvm.comunidad.network.response.subscriptions;

import com.epm.app.mvvm.comunidad.network.response.Mensaje;
import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class GetSubscriptionNotificationsPushAlertasItuango {


        @SerializedName("SuscripcionNotificacionesPushComunidad")
        @Expose
        private GetSubscriptionNotifications suscripcionNotificacionesPushComunidad;
        @SerializedName("Mensaje")
        @Expose
        private Mensaje mensaje;

        public GetSubscriptionNotifications getSuscripcionNotificacionesPushComunidad() {
            return suscripcionNotificacionesPushComunidad;
        }

        public void setSuscripcionNotificacionesPushComunidad(GetSubscriptionNotifications suscripcionNotificacionesPushComunidad) {
            this.suscripcionNotificacionesPushComunidad = suscripcionNotificacionesPushComunidad;
        }

        public Mensaje getMensaje() {
            return mensaje;
        }

        public void setMensaje(Mensaje mensaje) {
            this.mensaje = mensaje;
        }


}
