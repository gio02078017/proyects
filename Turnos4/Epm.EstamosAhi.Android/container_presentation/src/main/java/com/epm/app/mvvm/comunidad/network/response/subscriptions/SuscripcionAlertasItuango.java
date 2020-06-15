package com.epm.app.mvvm.comunidad.network.response.subscriptions;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class SuscripcionAlertasItuango {
    @SerializedName("idTipoSuscripcionNotificacion")
    @Expose
    private int idTipoSuscripcionNotificacion;
    @SerializedName("IdDispositivo")
    @Expose
    private String idDispositivo;
    @SerializedName("CorreoElectronico")
    @Expose
    private String correoElectronico;
    @SerializedName("Nombre")
    @Expose
    private String nombre;

    public int getIdTipoSuscripcionNotificacion() {
        return idTipoSuscripcionNotificacion;
    }

    public void setIdTipoSuscripcionNotificacion(int idTipoSuscripcionNotificacion) {
        this.idTipoSuscripcionNotificacion = idTipoSuscripcionNotificacion;
    }

    public String getIdDispositivo() {
        return idDispositivo;
    }

    public void setIdDispositivo(String idDispositivo) {
        this.idDispositivo = idDispositivo;
    }

    public String getCorreoElectronico() {
        return correoElectronico;
    }

    public void setCorreoElectronico(String correoElectronico) {
        this.correoElectronico = correoElectronico;
    }

    public String getNombre() {
        return nombre;
    }

    public void setNombre(String nombre) {
        this.nombre = nombre;
    }

}