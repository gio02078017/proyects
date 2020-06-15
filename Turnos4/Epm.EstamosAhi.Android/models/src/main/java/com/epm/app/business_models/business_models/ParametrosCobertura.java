package com.epm.app.business_models.business_models;

/**
 * Created by josetabaresramirez on 2/03/17.
 */

public class ParametrosCobertura {

    private int tipoServicio;
    private String municipio;
    private String departamento;

    public int getTipoServicio() {
        return tipoServicio;
    }

    public void setTipoServicio(int tipoServicio) {
        this.tipoServicio = tipoServicio;
    }

    public String getMunicipio() {
        return municipio;
    }

    public void setMunicipio(String municipio) {
        this.municipio = municipio;
    }

    public String getDepartamento() {
        return departamento;
    }

    public void setDepartamento(String departamento) {
        this.departamento = departamento;
    }
}
