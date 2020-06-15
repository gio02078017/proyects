package com.epm.app.mvvm.comunidad.network.request;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class UpdateStatusNotificationRequest {

    @SerializedName("IdNotificacionPush")
    @Expose
    private int idNotificationPush;

    public int getIdNotificationPush() {
        return idNotificationPush;
    }

    public void setIdNotificationPush(int idNotificationPush) {
        this.idNotificationPush = idNotificationPush;
    }
}
