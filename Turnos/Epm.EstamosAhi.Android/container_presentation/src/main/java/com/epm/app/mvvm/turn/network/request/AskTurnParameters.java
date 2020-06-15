package com.epm.app.mvvm.turn.network.request;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class AskTurnParameters {

    @SerializedName("IdDispositivo")
    @Expose
    private String idDispositivo;
    @SerializedName("IdOficinaSentry")
    @Expose
    private String idOficinaSentry;
    @SerializedName("SistemaOperativo")
    @Expose
    private String sistemaOperativo;
    @SerializedName("NombreSolicitante")
    @Expose
    private String nombreSolicitante;
    @SerializedName("IdServicio")
    @Expose
    private String idServicio;
    @SerializedName("IdTramite")
    @Expose
    private String idTramite;
    @SerializedName("DetalleMaestros")
    @Expose
    private String detalleMaestros;
    @SerializedName("IdOneSignal")
    @Expose
    private String idOneSignal;
    @SerializedName("UsuarioAutenticado")
    @Expose
    private String usuarioAutenticado;

    public String getIdDispositivo() {
        return idDispositivo;
    }

    public void setIdDispositivo(String idDispositivo) {
        this.idDispositivo = idDispositivo;
    }

    public String getIdOficinaSentry() {
        return idOficinaSentry;
    }

    public void setIdOficinaSentry(String idOficinaSentry) {
        this.idOficinaSentry = idOficinaSentry;
    }

    public String getSistemaOperativo() {
        return sistemaOperativo;
    }

    public void setSistemaOperativo(String sistemaOperativo) {
        this.sistemaOperativo = sistemaOperativo;
    }

    public String getNombreSolicitante() {
        return nombreSolicitante;
    }

    public void setNombreSolicitante(String nombreSolicitante) {
        this.nombreSolicitante = nombreSolicitante;
    }

    public String getIdOneSignal() {
        return idOneSignal;
    }

    public void setIdOneSignal(String idOneSignal) {
        this.idOneSignal = idOneSignal;
    }

    public String getUsuarioAutenticado() {
        return usuarioAutenticado;
    }

    public void setUsuarioAutenticado(String usuarioAutenticado) {
        this.usuarioAutenticado = usuarioAutenticado;
    }

    public String getIdServicio() {
        return idServicio;
    }

    public void setIdServicio(String idServicio) {
        this.idServicio = idServicio;
    }

    public String getIdTramite() {
        return idTramite;
    }

    public void setIdTramite(String idTramite) {
        this.idTramite = idTramite;
    }

    public String getDetalleMaestros() {
        return detalleMaestros;
    }

    public void setDetalleMaestros(String detalleMaestros) {
        this.detalleMaestros = detalleMaestros;
    }
}
