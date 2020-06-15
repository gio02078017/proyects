
package com.epm.app.mvvm.turn.network.response.officeDetail;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

import java.io.Serializable;

public class Turno implements Serializable {

    @SerializedName("IdTurnoSentry")
    @Expose
    private Integer idTurnoSentry;
    @SerializedName("TurnoAsignado")
    @Expose
    private String turnoAsignado;
    @SerializedName("NombreSolicitante")
    @Expose
    private String nombreSolicitante;
    @SerializedName("IdTramite")
    @Expose
    private Integer idTramite;
    @SerializedName("EstadoTurno")
    @Expose
    private String estadoDeTurno;

    public Integer getIdTurnoSentry() {
        return idTurnoSentry;
    }

    public void setIdTurnoSentry(Integer idTurnoSentry) {
        this.idTurnoSentry = idTurnoSentry;
    }

    public String getTurnoAsignado() {
        return turnoAsignado;
    }

    public void setTurnoAsignado(String turnoAsignado) {
        this.turnoAsignado = turnoAsignado;
    }

    public String getNombreSolicitante() {
        return nombreSolicitante;
    }

    public void setNombreSolicitante(String nombreSolicitante) {
        this.nombreSolicitante = nombreSolicitante;
    }

    public Integer getIdTramite() {
        return idTramite;
    }

    public void setIdTramite(Integer idTramite) {
        this.idTramite = idTramite;
    }

    public String getEstadoDeTurno() {
        return estadoDeTurno;
    }

    public void setEstadoDeTurno(String estadoDeTurno) {
        this.estadoDeTurno = estadoDeTurno;
    }
}
