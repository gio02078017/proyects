package com.epm.app.mvvm.turn.network.request;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class CancelTurnParameters {

    @SerializedName("idDispositivo")
    @Expose
    private String idDispositivo;
    @SerializedName("idTurno")
    @Expose
    private Integer idTurno;
    @SerializedName("sistemaOperativo")
    @Expose
    private String sistemaOperativo;

    public String getIdDispositivo() {
        return idDispositivo;
    }

    public void setIdDispositivo(String idDispositivo) {
        this.idDispositivo = idDispositivo;
    }

    public Integer getIdTurno() {
        return idTurno;
    }

    public void setIdTurno(Integer idTurno) {
        this.idTurno = idTurno;
    }

    public String getSistemaOperativo() {
        return sistemaOperativo;
    }

    public void setSistemaOperativo(String sistemaOperativo) {
        this.sistemaOperativo = sistemaOperativo;
    }

}
