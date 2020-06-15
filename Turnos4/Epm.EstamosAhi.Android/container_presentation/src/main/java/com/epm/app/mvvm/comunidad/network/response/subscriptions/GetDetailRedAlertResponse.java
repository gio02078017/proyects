package com.epm.app.mvvm.comunidad.network.response.subscriptions;

import com.epm.app.mvvm.comunidad.network.response.Mensaje;
import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class GetDetailRedAlertResponse {

    @SerializedName("DetalleNotificacionPush")
    @Expose
    private DetailRedAlert detalleNotificacionPush;
    @SerializedName("Mensaje")
    @Expose
    private Mensaje mensaje;

    public DetailRedAlert getDetalleNotificacionPush() {
        return detalleNotificacionPush;
    }

    public void setDetalleNotificacionPush(DetailRedAlert detalleNotificacionPush) {
        this.detalleNotificacionPush = detalleNotificacionPush;
    }

    public Mensaje getMensaje() {
        return mensaje;
    }

    public void setMensaje(Mensaje mensaje) {
        this.mensaje = mensaje;
    }

}
