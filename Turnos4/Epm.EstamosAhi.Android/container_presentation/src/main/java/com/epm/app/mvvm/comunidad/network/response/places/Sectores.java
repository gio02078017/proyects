package com.epm.app.mvvm.comunidad.network.response.places;

import com.google.gson.annotations.SerializedName;

public class Sectores {

    @SerializedName("IdSector")
    public int idSector;
    @SerializedName("Sector")
    public String sector;
    @SerializedName("Descripcion")
    public String descripcion;
    @SerializedName("Estado")
    public boolean estado;
    @SerializedName("IdMunicipio")
    public int idDepartamento;
    @SerializedName("Municipio")
    public int ordenGeograficoMunicipio;

    public int getIdSector() {
        return idSector;
    }

    public void setIdSector(int idSector) {
        this.idSector = idSector;
    }

    public String getDescripcion() {
        return descripcion;
    }

    public void setDescripcion(String descripcion) {
        this.descripcion = descripcion;
    }

    public boolean isEstado() {
        return estado;
    }

    public void setEstado(boolean estado) {
        this.estado = estado;
    }

    public int getIdDepartamento() {
        return idDepartamento;
    }

    public void setIdDepartamento(int idDepartamento) {
        this.idDepartamento = idDepartamento;
    }

    public int getOrdenGeograficoMunicipio() {
        return ordenGeograficoMunicipio;
    }

    public void setOrdenGeograficoMunicipio(int ordenGeograficoMunicipio) {
        this.ordenGeograficoMunicipio = ordenGeograficoMunicipio;
    }

    public String getSector() {
        return sector;
    }

    public void setSector(String sector) {
        this.sector = sector;
    }
}
