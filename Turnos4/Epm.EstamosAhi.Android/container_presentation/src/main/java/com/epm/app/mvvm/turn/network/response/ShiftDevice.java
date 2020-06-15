package com.epm.app.mvvm.turn.network.response;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class ShiftDevice {

    @SerializedName("IdTurnoSentry")
    @Expose
    private int idTurnoSentry;
    @SerializedName("TurnoAsignado")
    @Expose
    private String turnoAsignado;
    @SerializedName("NombreSolicitante")
    @Expose
    private String nombreSolicitante;
    @SerializedName("IdTramite")
    @Expose
    private int idTramite;

    public int getIdTurnoSentry() {
        return idTurnoSentry;
    }

    public void setIdTurnoSentry(int idTurnoSentry) {
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

    public int getIdTramite() {
        return idTramite;
    }

    public void setIdTramite(int idTramite) {
        this.idTramite = idTramite;
    }

}