package com.epm.app.mvvm.comunidad.network.response.subscriptions;
import com.epm.app.mvvm.comunidad.network.response.Mensaje;
import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class GetNotificationsPush {

    @SerializedName("CantidadNotificacionesSinLeer")
    @Expose
    private Integer cantidadNotificacionesSinLeer;
    @SerializedName("TransactionServiceMessage")
    @Expose
    private Mensaje mensaje;

    public Integer getCantidadNotificacionesSinLeer() {
        return cantidadNotificacionesSinLeer;
    }

    public void setCantidadNotificacionesSinLeer(Integer cantidadNotificacionesSinLeer) {
        this.cantidadNotificacionesSinLeer = cantidadNotificacionesSinLeer;
    }

    public Mensaje getMensaje() {
        return mensaje;
    }

    public void setMensaje(Mensaje mensaje) {
        this.mensaje = mensaje;
    }

}