package com.epm.app.mvvm.comunidad.network.response.subscriptions;

import java.util.List;

import com.epm.app.mvvm.comunidad.network.response.places.ListSubscriptions;
import com.epm.app.mvvm.comunidad.network.response.Mensaje;
import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class GetSubscriptionNotificationsPush {

    @SerializedName("IdSuscripcionOneSignal")
    @Expose
    private String idSuscripcionOneSignal;
    @SerializedName("IdDispositivo")
    @Expose
    private String idDispositivo;
    @SerializedName("IdUsuario")
    @Expose
    private String idUsuario;
    @SerializedName("cambioIdDispositivo")
    @Expose
    private boolean cambioIdDispositivo;
    @SerializedName("cambioIdOneSignal")
    @Expose
    private boolean cambioIdOneSignal;
    @SerializedName("Suscripciones")
    @Expose
    private List<ListSubscriptions> suscripciones = null;
    @SerializedName("TransactionServiceMessage")
    @Expose
    private Mensaje mensaje;

    public String getIdSuscripcionOneSignal() {
        return idSuscripcionOneSignal;
    }

    public void setIdSuscripcionOneSignal(String idSuscripcionOneSignal) {
        this.idSuscripcionOneSignal = idSuscripcionOneSignal;
    }

    public String getIdDispositivo() {
        return idDispositivo;
    }

    public void setIdDispositivo(String idDispositivo) {
        this.idDispositivo = idDispositivo;
    }


    public String getIdUsuario() {
        return idUsuario;
    }

    public void setIdUsuario(String idUsuario) {
        this.idUsuario = idUsuario;
    }

    public List<ListSubscriptions> getSuscripciones() {
        return suscripciones;
    }

    public void setSuscripciones(List<ListSubscriptions> suscripciones) {
        this.suscripciones = suscripciones;
    }

    public Mensaje getMensaje() {
        return mensaje;
    }

    public void setMensaje(Mensaje mensaje) {
        this.mensaje = mensaje;
    }


    public boolean isCambioIdDispositivo() {
        return cambioIdDispositivo;
    }

    public boolean isCambioIdOneSignal() {
        return cambioIdOneSignal;
    }
}
