package com.epm.app.mvvm.comunidad.network.response.notifications;

import java.util.List;

import com.epm.app.mvvm.comunidad.network.response.Mensaje;
import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class NotificationList {

    @SerializedName("PaginaActual")
    @Expose
    private Integer actualPage;
    @SerializedName("RegistrosPorPagina")
    @Expose
    private Integer recordsPage;
    @SerializedName("TotalRegistros")
    @Expose
    private Integer totalRecords;
    @SerializedName("TotalPaginas")
    @Expose
    private Integer totalPages;
    @SerializedName("NotificacionesPushRecibidas")
    @Expose
    private List<ReceivePushNotification> receivePushNotification = null;
    @SerializedName("Mensaje")
    @Expose
    private Mensaje mensaje;

    public Integer getActualPage() {
        return actualPage;
    }

    public void setActualPage(Integer actualPage) {
        this.actualPage = actualPage;
    }

    public Integer getRecordsPage() {
        return recordsPage;
    }

    public void setRecordsPage(Integer recordsPage) {
        this.recordsPage = recordsPage;
    }

    public Integer getTotalRecords() {
        return totalRecords;
    }

    public void setTotalRecords(Integer totalRecords) {
        this.totalRecords = totalRecords;
    }

    public Integer getTotalPages() {
        return totalPages;
    }

    public void setTotalPages(Integer totalPages) {
        this.totalPages = totalPages;
    }

    public List<ReceivePushNotification> getReceivePushNotification() {
        return receivePushNotification;
    }

    public void setReceivePushNotification(List<ReceivePushNotification> receivePushNotification) {
        this.receivePushNotification = receivePushNotification;
    }

    public Mensaje getMensaje() {
        return mensaje;
    }

    public void setMensaje(Mensaje mensaje) {
        this.mensaje = mensaje;
    }

}