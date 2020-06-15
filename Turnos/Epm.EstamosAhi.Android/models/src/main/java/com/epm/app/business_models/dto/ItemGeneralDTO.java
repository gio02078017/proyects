package com.epm.app.business_models.dto;

/**
 * Created by leidycarolinazuluagabastidas on 3/05/17.
 */

public class ItemGeneralDTO {

    private int Id;
    private String Codigo;
    private String Descripcion;

    public int getId() {
        return Id;
    }

    public void setId(int id) {
        Id = id;
    }

    public String getCodigo() {
        return Codigo;
    }

    public void setCodigo(String codigo) {
        Codigo = codigo;
    }

    public String getDescripcion() {
        return Descripcion;
    }

    public void setDescripcion(String descripcion) {
        Descripcion = descripcion;
    }
}
