package com.epm.app.mvvm.comunidad.network.response.notifications;

import com.epm.app.mvvm.comunidad.network.response.Mensaje;
import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class ShowNotificationPushResponse {

    @SerializedName("MostrarNotificacionPush")
    @Expose
    private boolean mostrarNotificacionPush;
    @SerializedName("Mensaje")
    @Expose
    private Mensaje mensaje;

    public boolean isMostrarNotificacionPush() {
        return mostrarNotificacionPush;
    }

    public void setMostrarNotificacionPush(boolean mostrarNotificacionPush) {
        this.mostrarNotificacionPush = mostrarNotificacionPush;
    }

    public Mensaje getMensaje() {
        return mensaje;
    }

    public void setMensaje(Mensaje mensaje) {
        this.mensaje = mensaje;
    }

}