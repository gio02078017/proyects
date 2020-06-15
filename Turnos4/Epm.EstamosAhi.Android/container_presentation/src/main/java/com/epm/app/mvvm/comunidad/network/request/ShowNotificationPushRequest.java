package com.epm.app.mvvm.comunidad.network.request;

import com.epm.app.mvvm.comunidad.network.response.notifications.TemplateOneSignal;
import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class ShowNotificationPushRequest {

    @SerializedName("idDispositivo")
    @Expose
    private String idDispositivo;
    @SerializedName("templateOneSignal")
    @Expose
    private TemplateOneSignal templateOneSignal;

    public String getIdDispositivo() {
        return idDispositivo;
    }

    public void setIdDispositivo(String idDispositivo) {
        this.idDispositivo = idDispositivo;
    }

    public TemplateOneSignal getTemplateOneSignal() {
        return templateOneSignal;
    }

    public void setTemplateOneSignal(TemplateOneSignal templateOneSignal) {
        this.templateOneSignal = templateOneSignal;
    }

}