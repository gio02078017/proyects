package com.epm.app.mvvm.comunidad.network.response.subscriptions;

import com.epm.app.mvvm.comunidad.network.response.Mensaje;
import com.google.gson.annotations.SerializedName;

public class Subscription {

    @SerializedName("EstadoTransaccion")
    public boolean stateTransaction;

    @SerializedName("Mensaje")
    public Mensaje mensaje;

    public boolean isStateTransaction() {
        return stateTransaction;
    }

    public void setStateTransaction(boolean stateTransaction) {
        this.stateTransaction = stateTransaction;
    }

    public Mensaje getMensaje() {
        return mensaje;
    }

    public void setMensaje(Mensaje mensaje) {
        this.mensaje = mensaje;
    }
}
