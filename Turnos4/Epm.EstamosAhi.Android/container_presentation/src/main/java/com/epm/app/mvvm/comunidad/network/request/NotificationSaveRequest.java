package com.epm.app.mvvm.comunidad.network.request;

import com.epm.app.mvvm.comunidad.network.response.notifications.TemplateOneSignal;
import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class NotificationSaveRequest {

    @SerializedName("idDispositivo")
    @Expose
    private String idDevice;
    @SerializedName("titulo")
    @Expose
    private String title;
    @SerializedName("mensaje")
    @Expose
    private String mensaje;
    @SerializedName("templateOneSignal")
    @Expose
    private TemplateOneSignal templateOneSignal;
    @SerializedName("idNotificacionOneSignal")
    @Expose
    private String notificationID;

    public String getIdDevice() {
        return idDevice;
    }

    public void setIdDevice(String idDevice) {
        this.idDevice = idDevice;
    }

    public String getTitle() {
        return title;
    }

    public void setTitle(String title) {
        this.title = title;
    }

    public String getMensaje() {
        return mensaje;
    }

    public void setMensaje(String mensaje) {
        this.mensaje = mensaje;
    }

    public TemplateOneSignal getTemplateOneSignal() {
        return templateOneSignal;
    }

    public void setTemplateOneSignal(TemplateOneSignal templateOneSignal) {
        this.templateOneSignal = templateOneSignal;
    }

    public String getNotificationID() {
        return notificationID;
    }

    public void setNotificationID(String notificationID) {
        this.notificationID = notificationID;
    }
}