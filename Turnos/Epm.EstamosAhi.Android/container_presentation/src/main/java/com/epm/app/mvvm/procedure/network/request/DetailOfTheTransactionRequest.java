package com.epm.app.mvvm.procedure.network.request;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

import java.util.List;

public class DetailOfTheTransactionRequest {

    @SerializedName("idServicio")
    @Expose
    private String idService;
    @SerializedName("idTramite")
    @Expose
    private String idProcedure;
    @SerializedName("detalleMaestros")
    @Expose
    private String masterDetails = null;


    public String getIdService() {
        return idService;
    }

    public void setIdService(String idService) {
        this.idService = idService;
    }

    public String getIdProcedure() {
        return idProcedure;
    }

    public void setIdProcedure(String idProcedure) {
        this.idProcedure = idProcedure;
    }

    public String getMasterDetails() {
        return masterDetails;
    }

    public void setMasterDetails(String masterDetails) {
        this.masterDetails = masterDetails;
    }

}
