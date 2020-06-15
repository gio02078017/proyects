package com.epm.app.mvvm.turn.network.response.cancelTurn;

import com.epm.app.mvvm.turn.network.response.Mensaje;
import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class CancelTurnResponse {

    @SerializedName("EstadoTransaccion")
    @Expose
    private Boolean estadoTransaccion;
    @SerializedName("TransactionServiceMessage")
    @Expose
    private Mensaje mensaje;

    public Boolean getEstadoTransaccion() {
        return estadoTransaccion;
    }

    public void setEstadoTransaccion(Boolean estadoTransaccion) {
        this.estadoTransaccion = estadoTransaccion;
    }

    public Mensaje getMensaje() {
        return mensaje;
    }

    public void setMensaje(Mensaje mensaje) {
        this.mensaje = mensaje;
    }

}
