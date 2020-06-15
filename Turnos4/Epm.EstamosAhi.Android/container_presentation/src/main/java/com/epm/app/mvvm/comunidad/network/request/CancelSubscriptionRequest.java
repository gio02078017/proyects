package com.epm.app.mvvm.comunidad.network.request;


import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class CancelSubscriptionRequest {

    @SerializedName("idSuscripcion")
    @Expose
    private int idSuscripcion;
    @SerializedName("idDispositivo")
    @Expose
    private String idDispositivo;

    public int getIdSuscripcion() {
        return idSuscripcion;
    }

    public void setIdSuscripcion(int idSuscripcion) {
        this.idSuscripcion = idSuscripcion;
    }

    public String getIdDispositivo() {
        return idDispositivo;
    }

    public void setIdDispositivo(String idDispositivo) {
        this.idDispositivo = idDispositivo;
    }

}