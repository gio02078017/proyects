package com.epm.app.mvvm.comunidad.network.request;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class UpdateStatusSendNotificationRequest {

    @SerializedName("idTipoSuscripcionNotificacion")
    @Expose
    private Integer idTipoSuscripcionNotificacion;
    @SerializedName("idDispositivo")
    @Expose
    private String idDispositivo;
    @SerializedName("envioNotificacion")
    @Expose
    private Boolean envioNotificacion;

    public Integer getIdTipoSuscripcionNotificacion() {
        return idTipoSuscripcionNotificacion;
    }

    public void setIdTipoSuscripcionNotificacion(Integer idTipoSuscripcionNotificacion) {
        this.idTipoSuscripcionNotificacion = idTipoSuscripcionNotificacion;
    }

    public String getIdDispositivo() {
        return idDispositivo;
    }

    public void setIdDispositivo(String idDispositivo) {
        this.idDispositivo = idDispositivo;
    }

    public Boolean getEnvioNotificacion() {
        return envioNotificacion;
    }

    public void setEnvioNotificacion(Boolean envioNotificacion) {
        this.envioNotificacion = envioNotificacion;
    }

}

