package com.epm.app.mvvm.comunidad.network.response.places;

import com.epm.app.mvvm.comunidad.network.response.ErrorService;
import com.epm.app.mvvm.comunidad.network.response.Mensaje;
import com.google.gson.annotations.SerializedName;

import java.util.List;

public class ObtenerSectores extends ErrorService {

    @SerializedName("Sectores")
    public List<Sectores> sectores = null;
    @SerializedName("Mensaje")
    public Mensaje mensaje;

    public List<Sectores> getSectores() {
        return sectores;
    }

    public void setSectores(List<Sectores> municipios) {
        this.sectores = municipios;
    }

    public Mensaje getMensaje() {
        return mensaje;
    }

    public void setMensaje(Mensaje mensaje) {
        this.mensaje = mensaje;
    }
}
