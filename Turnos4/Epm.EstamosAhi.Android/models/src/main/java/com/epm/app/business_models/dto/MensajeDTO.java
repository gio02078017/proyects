package com.epm.app.business_models.dto;

/**
 * Created by ocadavid on 23/01/2017.
 */
public class MensajeDTO {
    private int Codigo;
    private String Texto;

    public int getCodigo() {
        return Codigo;
    }

    public void setCodigo(int codigo) {
        Codigo = codigo;
    }

    public String getTexto() {
        return Texto;
    }

    public void setTexto(String texto) {
        Texto = texto;
    }
}
