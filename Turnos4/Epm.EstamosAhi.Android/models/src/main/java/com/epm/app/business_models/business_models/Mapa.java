package com.epm.app.business_models.business_models;

import java.io.Serializable;
import java.util.List;

/**
 * Created by leidycarolinazuluagabastidas on 9/03/17.
 */

public class Mapa implements Serializable {

    private String nombreServcio;

    private String urlBase;

    private String tipoServicio;

    private String labelServicio;

    private String urlServicio;

    private List<FieldMapa> fieldMapa;

    public String getNombreServcio() {
        return nombreServcio;
    }

    public void setNombreServcio(String nombreServcio) {
        this.nombreServcio = nombreServcio;
    }

    public String getUrlBase() {
        return urlBase;
    }

    public void setUrlBase(String urlBase) {
        this.urlBase = urlBase;
    }

    public String getTipoServicio() {
        return tipoServicio;
    }

    public void setTipoServicio(String tipoServicio) {
        this.tipoServicio = tipoServicio;
    }

    public String getLabelServicio() {
        return labelServicio;
    }

    public void setLabelServicio(String labelServicio) {
        this.labelServicio = labelServicio;
    }

    public String getUrlServicio() {
        return urlServicio;
    }

    public void setUrlServicio(String urlServicio) {
        this.urlServicio = urlServicio;
    }

    public List<FieldMapa> getFieldMapa() {
        return fieldMapa;
    }

    public void setFieldMapa(List<FieldMapa> fieldMapa) {
        this.fieldMapa = fieldMapa;
    }
}
