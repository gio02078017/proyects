package com.epm.app.business_models.dto;

/**
 * Created by Jquinterov on 3/8/2017.
 */

public class ParametrosEnviarCorreoDTO {
    private String CorreoElectronico;
    private String NumeroRadicado;
    private String NombreServicio;

    public String getCorreoElectronico() {
        return CorreoElectronico;
    }

    public void setCorreoElectronico(String correoElectronico) {
        CorreoElectronico = correoElectronico;
    }

    public String getNumeroRadicado() {
        return NumeroRadicado;
    }

    public void setNumeroRadicado(String numeroRadicado) {
        NumeroRadicado = numeroRadicado;
    }

    public String getNombreServicio() {
        return NombreServicio;
    }

    public void setNombreServicio(String nombreServicio) {
        NombreServicio = nombreServicio;
    }
}
