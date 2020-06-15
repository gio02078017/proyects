package com.epm.app.mvvm.comunidad.network.response.places;

import com.epm.app.mvvm.comunidad.network.response.Mensaje;
import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

import java.util.List;


public class ObtenerMunicipios   {

    @SerializedName("Municipios")
    public List<Municipio> municipios = null;
    @SerializedName("TransactionServiceMessage")
    public Mensaje mensaje;
    @Expose(deserialize = false)
    public int code;
    @Expose(deserialize = false)
    public String message;

    public List<Municipio> getMunicipios() {
        return municipios;
    }

    public void setMunicipios(List<Municipio> municipios) {
        this.municipios = municipios;
    }

    public Mensaje getMensaje() {
        return mensaje;
    }

    public void setMensaje(Mensaje mensaje) {
        this.mensaje = mensaje;
    }

    public int getCode() {
        return code;
    }

    public void setCode(int code) {
        this.code = code;
    }

    public String getMessage() {
        return message;
    }

    public void setMessage(String message) {
        this.message = message;
    }
}