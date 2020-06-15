package com.epm.app.mvvm.comunidad.network.request;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class UpdateNotificationStatusOneSignalRequest {

    @SerializedName("idNotificacionOneSignal")
    @Expose
    private String idNotificationOneSignal;

    public String getIdNotificationOneSignal() {
        return idNotificationOneSignal;
    }

    public void setIdNotificationOneSignal(String idNotificationOneSignal) {
        this.idNotificationOneSignal = idNotificationOneSignal;
    }

}
