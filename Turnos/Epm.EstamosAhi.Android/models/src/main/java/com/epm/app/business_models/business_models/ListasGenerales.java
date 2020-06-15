package com.epm.app.business_models.business_models;

import java.util.List;

/**
 * Created by mateoquicenososa on 23/11/16.
 */

public class ListasGenerales {
    private List<ItemGeneral> tiposIdentificacion;
    private List<ItemGeneral> tiposPersonas;
    private List<ItemGeneral> tiposViviendas;
    private List<ItemGeneral> generos;

    public List<ItemGeneral> getTiposIdentificacion() {
        return tiposIdentificacion;
    }

    public void setTiposIdentificacion(List<ItemGeneral> tiposIdentificacion) {
        this.tiposIdentificacion = tiposIdentificacion;
    }

    public List<ItemGeneral> getTiposPersonas() {
        return tiposPersonas;
    }

    public void setTiposPersonas(List<ItemGeneral> tiposPersonas) {
        this.tiposPersonas = tiposPersonas;
    }

    public List<ItemGeneral> getTiposViviendas() {
        return tiposViviendas;
    }

    public void setTiposViviendas(List<ItemGeneral> tiposViviendas) {
        this.tiposViviendas = tiposViviendas;
    }

    public List<ItemGeneral> getGeneros() {
        return generos;
    }

    public void setGeneros(List<ItemGeneral> generos) {
        this.generos = generos;
    }


}
