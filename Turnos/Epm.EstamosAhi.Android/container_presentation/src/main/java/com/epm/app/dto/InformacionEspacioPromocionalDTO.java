package com.epm.app.dto;

import java.util.Date;

/**
 * Created by mateoquicenososa on 3/5/18.
 */

public class InformacionEspacioPromocionalDTO {

    private String Imagen;
    private Date FechaCreacion;
    private int Modulo;

    public String getImagen() {
        return Imagen;
    }

    public void setImagen(String imagen) {
        Imagen = imagen;
    }

    public Date getFechaCreacion() {
        return FechaCreacion;
    }

    public void setFechaCreacion(Date fechaCreacion) {
        FechaCreacion = fechaCreacion;
    }

    public int getModulo() {
        return Modulo;
    }

    public void setModulo(int modulo) {
        Modulo = modulo;
    }
}
