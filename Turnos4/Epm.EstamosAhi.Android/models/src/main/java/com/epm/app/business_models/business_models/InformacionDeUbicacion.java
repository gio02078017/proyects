package com.epm.app.business_models.business_models;

import java.io.Serializable;

/**
 * Created by josetabaresramirez on 23/02/17.
 */

public class InformacionDeUbicacion implements Serializable {

    private String direccion;
    private String municipio;
    private String deparatamento;
    private String pais;

    public String getDireccion() {
        return direccion;
    }

    public void setDireccion(String direccion) {
        this.direccion = direccion;
    }

    public String getMunicipio() {
        return municipio;
    }

    public void setMunicipio(String municipio) {
        this.municipio = municipio;
    }

    public String getDeparatamento() {
        return deparatamento;
    }

    public void setDeparatamento(String deparatamento) {
        this.deparatamento = deparatamento;
    }

    public String getPais() {
        return pais;
    }

    public void setPais(String pais) {
        this.pais = pais;
    }
}
