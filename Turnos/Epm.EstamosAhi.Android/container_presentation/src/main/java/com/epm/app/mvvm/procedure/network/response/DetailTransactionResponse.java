package com.epm.app.mvvm.procedure.network.response;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

import java.util.List;

public class DetailTransactionResponse {


    @SerializedName("IdServicio")
    @Expose
    private String idServicio;
    @SerializedName("IdTramite")
    @Expose
    private String idTramite;
    @SerializedName("Activo")
    @Expose
    private Boolean activo;
    @SerializedName("Nombre")
    @Expose
    private String nombre;
    @SerializedName("QueEs")
    @Expose
    private String queEs;
    @SerializedName("QueNecesito")
    @Expose
    private String queNecesito;
    @SerializedName("TiposDeCanal")
    @Expose
    private List<ChannelTypesResponse> channelTypes = null;

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

    public Boolean getActivo() {
        return activo;
    }

    public void setActivo(Boolean activo) {
        this.activo = activo;
    }

    public String getNombre() {
        return nombre;
    }

    public void setNombre(String nombre) {
        this.nombre = nombre;
    }

    public String getQueEs() {
        return queEs;
    }

    public void setQueEs(String queEs) {
        this.queEs = queEs;
    }

    public String getQueNecesito() {
        return queNecesito;
    }

    public void setQueNecesito(String queNecesito) {
        this.queNecesito = queNecesito;
    }

    public List<ChannelTypesResponse> getChannelTypes() {
        return channelTypes;
    }

    public void setChannelTypes(List<ChannelTypesResponse> channelTypes) {
        this.channelTypes = channelTypes;
    }

}
