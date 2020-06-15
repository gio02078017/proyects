package com.epm.app.mvvm.comunidad.network.response.subscriptions;

import com.google.gson.annotations.SerializedName;

public class SolicitudActualizarEstadoSuscripcion {

    @SerializedName("envioNotificacion")
    private boolean envioNotificacion;
    @SerializedName("idSuscripcionOneSignal")
    private String idSuscripcionOneSignal;
    @SerializedName("idDispositivo")
    private String idDispositivo;
    @SerializedName("cambioIdDispositivo")
    private boolean cambioIdDispositivo;
    @SerializedName("cambioIdOneSignal")
    private boolean cambioIdOneSignal;
    @SerializedName("idTipoSuscripcionNotificacion")
    private Integer idTipoSuscripcionNotificacion;
    @SerializedName("correoElectronico")
    private String correoElectronico;

    public boolean getEnvioNotificacion() {
        return envioNotificacion;
    }

    public void setEnvioNotificacion(boolean envioNotificacion) {
        this.envioNotificacion = envioNotificacion;
    }

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

    public Integer getIdTipoSuscripcionNotificacion() {
        return idTipoSuscripcionNotificacion;
    }

    public void setIdTipoSuscripcionNotificacion(Integer idTipoSuscripcionNotificacion) {
        this.idTipoSuscripcionNotificacion = idTipoSuscripcionNotificacion;
    }

    public String getCorreoElectronico() {
        return correoElectronico;
    }

    public void setCorreoElectronico(String correoElectronico) {
        this.correoElectronico = correoElectronico;
    }

    public boolean getCambioIdDispositivo() {
        return cambioIdDispositivo;
    }

    public void setCambioIdDispositivo(boolean cambioIdDispositivo) {
        this.cambioIdDispositivo = cambioIdDispositivo;
    }

    public boolean getCambioIdOneSignal() {
        return cambioIdOneSignal;
    }

    public void setCambioIdOneSignal(boolean cambioIdOneSignal) {
        this.cambioIdOneSignal = cambioIdOneSignal;
    }
}
