package com.epm.app.mvvm.turn.network.response.askTurn;

import com.epm.app.mvvm.turn.network.response.Mensaje;
import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

import java.io.Serializable;

public class TurnResponse implements Serializable {

    @SerializedName("IdTurnoSentry")
    @Expose
    private Integer idTurnoSentry;
    @SerializedName("TurnoAsignado")
    @Expose
    private String turnoAsignado;
    @SerializedName("NombreSolicitante")
    @Expose
    private String nombreSolicitante;
    @SerializedName("NombreOficina")
    @Expose
    private String nombreOficina;
    @SerializedName("HoraCierre")
    @Expose
    private String horaCierre;
    @SerializedName("Mensaje")
    @Expose
    private Mensaje mensaje;

    private Boolean showTurn;

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

    public String getNombreOficina() {
        return nombreOficina;
    }

    public void setNombreOficina(String nombreOficina) {
        this.nombreOficina = nombreOficina;
    }

    public String getHoraCierre() {
        return horaCierre;
    }

    public void setHoraCierre(String horaCierre) {
        this.horaCierre = horaCierre;
    }

    public Mensaje getMensaje() {
        return mensaje;
    }

    public void setMensaje(Mensaje mensaje) {
        this.mensaje = mensaje;
    }

    public Boolean isShowTurn() {
        return showTurn;
    }

    public void setShowTurn(Boolean showTurn) {
        this.showTurn = showTurn;
    }
}
