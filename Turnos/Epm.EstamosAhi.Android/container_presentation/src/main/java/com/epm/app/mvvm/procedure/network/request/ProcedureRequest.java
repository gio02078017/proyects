
package com.epm.app.mvvm.procedure.network.request;


import com.google.gson.annotations.Expose;

import com.google.gson.annotations.SerializedName;

public class ProcedureRequest {

    @SerializedName("idServicio")
    @Expose
    private String serviceId;

    public ProcedureRequest(String serviceId) {
        this.serviceId = serviceId;
    }

    public String getServiceId() {
        return serviceId;
    }

    public void setServiceId(String serviceId) {
        this.serviceId = serviceId;
    }

}
