
package com.epm.app.mvvm.turn.network.response.nearbyOffices;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class NearbyOfficesItem {

    @SerializedName("TurnosEnEspera")
    @Expose
    private Integer turnosEnEspera;
    @SerializedName("TurnoAsignado")
    @Expose
    private String turnoAsignado;
    @SerializedName("Distancia")
    @Expose
    private Float distancia;
    @SerializedName("Unidad")
    @Expose
    private String unidad;
    @SerializedName("IdOficina")
    @Expose
    private Integer idOficina;
    @SerializedName("IdOficinaSentry")
    @Expose
    private String idOficinaSentry;
    @SerializedName("NombreOficina")
    @Expose
    private String nombreOficina;
    @SerializedName("Direccion")
    @Expose
    private String direccion;
    @SerializedName("Latitud")
    @Expose
    private String latitud;
    @SerializedName("Longitud")
    @Expose
    private String longitud;
    @SerializedName("Horario")
    @Expose
    private String horario;

    public Integer getTurnosEnEspera() {
        return turnosEnEspera;
    }

    public void setTurnosEnEspera(Integer turnosEnEspera) {
        this.turnosEnEspera = turnosEnEspera;
    }

    public String getTurnoAsignado() {
        return turnoAsignado;
    }

    public void setTurnoAsignado(String turnoAsignado) {
        this.turnoAsignado = turnoAsignado;
    }

    public Float getDistancia() {
        return distancia;
    }

    public void setDistancia(Float distancia) {
        this.distancia = distancia;
    }

    public String getUnidad() {
        return unidad;
    }

    public void setUnidad(String unidad) {
        this.unidad = unidad;
    }

    public Integer getIdOficina() {
        return idOficina;
    }

    public void setIdOficina(Integer idOficina) {
        this.idOficina = idOficina;
    }

    public String getIdOficinaSentry() {
        return idOficinaSentry;
    }

    public void setIdOficinaSentry(String idOficinaSentry) {
        this.idOficinaSentry = idOficinaSentry;
    }

    public String getNombreOficina() {
        return nombreOficina;
    }

    public void setNombreOficina(String nombreOficina) {
        this.nombreOficina = nombreOficina;
    }

    public String getDireccion() {
        return direccion;
    }

    public void setDireccion(String direccion) {
        this.direccion = direccion;
    }

    public String getLatitud() {
        return latitud;
    }

    public void setLatitud(String latitud) {
        this.latitud = latitud;
    }

    public String getLongitud() {
        return longitud;
    }

    public void setLongitud(String longitud) {
        this.longitud = longitud;
    }

    public String getHorario() {
        return horario;
    }

    public void setHorario(String horario) {
        this.horario = horario;
    }

}
