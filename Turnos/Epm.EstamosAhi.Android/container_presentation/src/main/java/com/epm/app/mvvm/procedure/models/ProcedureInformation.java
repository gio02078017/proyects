package com.epm.app.mvvm.procedure.models;

import java.io.Serializable;

public class ProcedureInformation implements Serializable {


    private String idService;
    private String idProcedure;
    private String idMasterDetails;
    private String nameProcedure;

    public ProcedureInformation(String idService, String idProcedure, String idMasterDetails) {
        this.idService = idService;
        this.idProcedure = idProcedure;
        this.idMasterDetails = idMasterDetails;
    }

    public ProcedureInformation() {

    }

    public String getNameProcedure() {
        return nameProcedure;
    }

    public void setNameProcedure(String nameProcedure) {
        this.nameProcedure = nameProcedure;
    }

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

    public String getIdMasterDetails() {
        return idMasterDetails;
    }

    public void setIdMasterDetails(String idMasterDetails) {
        this.idMasterDetails = idMasterDetails;
    }
}
