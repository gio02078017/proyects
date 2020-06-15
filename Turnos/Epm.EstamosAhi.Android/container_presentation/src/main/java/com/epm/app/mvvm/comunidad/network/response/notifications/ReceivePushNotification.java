package com.epm.app.mvvm.comunidad.network.response.notifications;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class ReceivePushNotification {

    @SerializedName("TiempoCalculado")
    @Expose
    private String timeCalculated;
    @SerializedName("IdNotificacionPush")
    @Expose
    private Integer idNotificationPush;
    @SerializedName("IdDispositivo")
    @Expose
    private String idDevice;
    @SerializedName("IdTipoSuscripcionNotificacion")
    @Expose
    private Integer idTipoSuscripcionNotificacion;
    @SerializedName("Titulo")
    @Expose
    private String title;
    @SerializedName("TransactionServiceMessage")
    @Expose
    private String mensaje;
    @SerializedName("FechaHoraRecibida")
    @Expose
    private String dateTimeReceived;
    @SerializedName("Leida")
    @Expose
    private Boolean read;
    @SerializedName("TemplateOneSignal")
    @Expose
    private TemplateOneSignal templateOneSignal;

    public String getTimeCalculated() {
        return timeCalculated;
    }

    public void setTimeCalculated(String timeCalculated) {
        this.timeCalculated = timeCalculated;
    }

    public Integer getIdNotificationPush() {
        return idNotificationPush;
    }

    public void setIdNotificationPush(Integer idNotificationPush) {
        this.idNotificationPush = idNotificationPush;
    }

    public String getIdDevice() {
        return idDevice;
    }

    public void setIdDevice(String idDevice) {
        this.idDevice = idDevice;
    }

    public Integer getIdTipoSuscripcionNotificacion() {
        return idTipoSuscripcionNotificacion;
    }

    public void setIdTipoSuscripcionNotificacion(Integer idTipoSuscripcionNotificacion) {
        this.idTipoSuscripcionNotificacion = idTipoSuscripcionNotificacion;
    }

    public String getTitle() {
        return title;
    }

    public void setTitulo(String titulo) {
        this.title = titulo;
    }

    public String getMensaje() {
        return mensaje;
    }

    public void setMensaje(String mensaje) {
        this.mensaje = mensaje;
    }

    public String getDateTimeReceived() {
        return dateTimeReceived;
    }

    public void setDateTimeReceived(String dateTimeReceived) {
        this.dateTimeReceived = dateTimeReceived;
    }

    public Boolean getRead() {
        return read;
    }

    public void setRead(Boolean read) {
        this.read = read;
    }


    public TemplateOneSignal getTemplateOneSignal() {
        return templateOneSignal;
    }

    public void setTemplateOneSignal(TemplateOneSignal templateOneSignal) {
        this.templateOneSignal = templateOneSignal;
    }
}
