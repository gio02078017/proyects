package com.epm.app.mvvm.comunidad.network.response.subscriptions;

import com.epm.app.mvvm.comunidad.network.response.Mensaje;
import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class CancelSubscriptionResponse {

    @SerializedName("EstadoTransaccion")
    @Expose
    private boolean estadoTransaccion;
    @SerializedName("TransactionServiceMessage")
    @Expose
    private Mensaje message;

    public boolean isEstadoTransaccion() {
        return estadoTransaccion;
    }

    public void setEstadoTransaccion(boolean estadoTransaccion) {
        this.estadoTransaccion = estadoTransaccion;
    }

    public Mensaje getMessage() {
        return message;
    }

    public void setMessage(Mensaje message) {
        this.message = message;
    }

}