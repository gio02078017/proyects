
package com.epm.app.mvvm.procedure.network.request;

import com.google.gson.annotations.SerializedName;

public class TypePersonRequest {

    @SerializedName("idServicio")
    private String serviceId;

    @SerializedName("idTramite")
    private String processId;

    public TypePersonRequest(String serviceId, String processId) {
        this.serviceId = serviceId;
        this.processId = processId;
    }

    public String getServiceId() {
        return serviceId;
    }

    public void setServiceId(String serviceId) {
        this.serviceId = serviceId;
    }

    public String getProcessId() {
        return processId;
    }

    public void setProcessId(String processId) {
        this.processId = processId;
    }
}
