package com.epm.app.mvvm.comunidad.network.response.notifications;

import com.epm.app.mvvm.comunidad.network.response.Mensaje;
import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class NotificationSaveResponse {

    @SerializedName("EstadoTransaccion")
    @Expose
    private Boolean stateTransaction;
    @SerializedName("IdNotificacionPush")
    @Expose
    private Integer idNotificationPush;
    @SerializedName("TransactionServiceMessage")
    @Expose
    private Mensaje mensaje;

    public Boolean getStateTransaction() {
        return stateTransaction;
    }

    public void setStateTransaction(Boolean stateTransaction) {
        this.stateTransaction = stateTransaction;
    }

    public Integer getIdNotificationPush() {
        return idNotificationPush;
    }

    public void setIdNotificationPush(Integer idNotificationPush) {
        this.idNotificationPush = idNotificationPush;
    }

    public Mensaje getMensaje() {
        return mensaje;
    }

    public void setMensaje(Mensaje mensaje) {
        this.mensaje = mensaje;
    }
}