package com.epm.app.mvvm.comunidad.network.response.webViews;

import com.epm.app.mvvm.comunidad.network.response.Mensaje;
import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class PrivacyPolicy {


    @SerializedName("TransactionServiceMessage")
    @Expose
    private Mensaje mensaje;
    @SerializedName("UrlPoliticaDePrivacidad")
    @Expose
    private String urlPoliticaDePrivacidad;

    public String getUrlPoliticaDePrivacidad() {
        return urlPoliticaDePrivacidad;
    }

    public void setUrlPoliticaDePrivacidad(String urlPoliticaDePrivacidad) {
        this.urlPoliticaDePrivacidad = urlPoliticaDePrivacidad;
    }

    public Mensaje getMensaje() {
        return mensaje;
    }

    public void setMensaje(Mensaje mensaje) {
        this.mensaje = mensaje;
    }


}
