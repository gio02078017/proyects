package com.epm.app.mvvm.comunidad.network.response.subscriptions;

import com.epm.app.mvvm.comunidad.network.response.Mensaje;
import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class RecoverySubscriptionResponse {
    @SerializedName("EstadoTransaccion")
    @Expose
    private boolean estadoTransaccion;
    @SerializedName("SuscripcionAlertasItuango")
    @Expose
    private SuscripcionAlertasItuango suscripcionAlertasItuango;
    @SerializedName("TransactionServiceMessage")
    @Expose
    private Mensaje mensaje;

    public boolean isEstadoTransaccion() {
        return estadoTransaccion;
    }

    public void setEstadoTransaccion(boolean estadoTransaccion) {
        this.estadoTransaccion = estadoTransaccion;
    }

    public SuscripcionAlertasItuango getSuscripcionAlertasItuango() {
        return suscripcionAlertasItuango;
    }

    public void setSuscripcionAlertasItuango(SuscripcionAlertasItuango suscripcionAlertasItuango) {
        this.suscripcionAlertasItuango = suscripcionAlertasItuango;
    }

    public Mensaje getMensaje() {
        return mensaje;
    }

    public void setMensaje(Mensaje mensaje) {
        this.mensaje = mensaje;
    }

}

