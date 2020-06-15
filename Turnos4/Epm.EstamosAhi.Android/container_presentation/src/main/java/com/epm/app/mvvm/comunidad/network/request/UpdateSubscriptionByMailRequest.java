package com.epm.app.mvvm.comunidad.network.request;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class UpdateSubscriptionByMailRequest {

    @SerializedName("idTipoSuscripcionNotificacion")
    @Expose
    private int idTipoSuscripcionNotificacion;
    @SerializedName("correoElectronico")
    @Expose
    private String correoElectronico;
    @SerializedName("idDispositivo")
    @Expose
    private String idDispositivo;
    @SerializedName("idSuscripcionOneSignal")
    @Expose
    private String idSuscripcionOneSignal;

    public int getIdTipoSuscripcionNotificacion() {
        return idTipoSuscripcionNotificacion;
    }

    public void setIdTipoSuscripcionNotificacion(int idTipoSuscripcionNotificacion) {
        this.idTipoSuscripcionNotificacion = idTipoSuscripcionNotificacion;
    }

    public String getCorreoElectronico() {
        return correoElectronico;
    }

    public void setCorreoElectronico(String correoElectronico) {
        this.correoElectronico = correoElectronico;
    }

    public String getIdDispositivo() {
        return idDispositivo;
    }

    public void setIdDispositivo(String idDispositivo) {
        this.idDispositivo = idDispositivo;
    }

    public String getIdSuscripcionOneSignal() {
        return idSuscripcionOneSignal;
    }

    public void setIdSuscripcionOneSignal(String idSuscripcionOneSignal) {
        this.idSuscripcionOneSignal = idSuscripcionOneSignal;
    }

}