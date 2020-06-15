package com.epm.app.business_models.business_models;

import java.io.Serializable;

/**
 * Created by mateoquicenososa on 23/11/16.
 */

public class ItemGeneral implements Serializable {
    private int id;
    private String codigo;
    private String descripcion;

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public String getCodigo() {
        return codigo;
    }

    public void setCodigo(String codigo) {
        this.codigo = codigo;
    }

    public String getDescripcion() {
        return descripcion;
    }

    public void setDescripcion(String descripcion) {
        this.descripcion = descripcion;
    }
}
