package com.epm.app.mvvm.comunidad.network.response.webViews;
import com.epm.app.mvvm.comunidad.network.response.Mensaje;
import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class InformationInterest {

    @SerializedName("UrlInformacionDeInteres")
    @Expose
    private String urlInformacionDeInteres;
    @SerializedName("Mensaje")
    @Expose
    private Mensaje mensaje;

    public String getUrlInformacionDeInteres() {
        return urlInformacionDeInteres;
    }

    public void setUrlInformacionDeInteres(String urlInformacionDeInteres) {
        this.urlInformacionDeInteres = urlInformacionDeInteres;
    }

    public Mensaje getMensaje() {
        return mensaje;
    }

    public void setMensaje(Mensaje mensaje) {
        this.mensaje = mensaje;
    }

}