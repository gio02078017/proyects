package com.epm.app.mvvm.turn.network.request;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class OfficeDetailParameters {

    @SerializedName("IdDispositivo")
    @Expose
    private String idDispositivo;
    @SerializedName("IdOficinaSentry")
    @Expose
    private String idOficinaSentry;
    @SerializedName("SistemaOperativo")
    @Expose
    private String sistemaOperativo;
    @SerializedName("Latitud")
    @Expose
    private Float latitud;
    @SerializedName("Longitud")
    @Expose
    private Float longitud;

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

    public Float getLatitud() {
        return latitud;
    }

    public void setLatitud(Float latitud) {
        this.latitud = latitud;
    }

    public Float getLongitud() {
        return longitud;
    }

    public void setLongitud(Float longitud) {
        this.longitud = longitud;
    }

}
