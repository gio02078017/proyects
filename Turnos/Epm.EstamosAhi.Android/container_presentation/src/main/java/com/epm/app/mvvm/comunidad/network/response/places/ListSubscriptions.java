package com.epm.app.mvvm.comunidad.network.response.places;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class ListSubscriptions {

    @SerializedName("IdUsuarioSuscripcionNotificacion")
    @Expose
    private Integer idUsuarioSuscripcionNotificacion;
    @SerializedName("IdTipoSuscripcionNotificacion")
    @Expose
    private Integer idTipoSuscripcionNotificacion;
    @SerializedName("NombreSuscripcionNotificacion")
    @Expose
    private String nombreSuscripcionNotificacion;
    @SerializedName("FechaSuscripcion")
    @Expose
    private String fechaSuscripcion;
    @SerializedName("CorreoElectronico")
    @Expose
    private String correoElectronico;
    @SerializedName("EnvioNotificacion")
    @Expose
    private Boolean envioNotificacion;
    @SerializedName("AceptaTerminosCondiciones")
    @Expose
    private Boolean aceptaTerminosCondiciones;

    public Integer getIdUsuarioSuscripcionNotificacion() {
        return idUsuarioSuscripcionNotificacion;
    }

    public void setIdUsuarioSuscripcionNotificacion(Integer idUsuarioSuscripcionNotificacion) {
        this.idUsuarioSuscripcionNotificacion = idUsuarioSuscripcionNotificacion;
    }

    public Integer getIdTipoSuscripcionNotificacion() {
        return idTipoSuscripcionNotificacion;
    }

    public void setIdTipoSuscripcionNotificacion(Integer idTipoSuscripcionNotificacion) {
        this.idTipoSuscripcionNotificacion = idTipoSuscripcionNotificacion;
    }

    public String getNombreSuscripcionNotificacion() {
        return nombreSuscripcionNotificacion;
    }

    public void setNombreSuscripcionNotificacion(String nombreSuscripcionNotificacion) {
        this.nombreSuscripcionNotificacion = nombreSuscripcionNotificacion;
    }

    public String getFechaSuscripcion() {
        return fechaSuscripcion;
    }

    public void setFechaSuscripcion(String fechaSuscripcion) {
        this.fechaSuscripcion = fechaSuscripcion;
    }

    public Boolean getEnvioNotificacion() {
        return envioNotificacion;
    }

    public void setEnvioNotificacion(Boolean envioNotificacion) {
        this.envioNotificacion = envioNotificacion;
    }

    public Boolean getAceptaTerminosCondiciones() {
        return aceptaTerminosCondiciones;
    }

    public void setAceptaTerminosCondiciones(Boolean aceptaTerminosCondiciones) {
        this.aceptaTerminosCondiciones = aceptaTerminosCondiciones;
    }

    public String getCorreoElectronico() {
        return correoElectronico;
    }

    public void setCorreoElectronico(String correoElectronico) {
        this.correoElectronico = correoElectronico;
    }
}
