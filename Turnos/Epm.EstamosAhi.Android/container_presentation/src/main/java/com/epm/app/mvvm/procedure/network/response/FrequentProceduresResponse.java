
package com.epm.app.mvvm.procedure.network.response;

import java.util.List;
import com.google.gson.annotations.SerializedName;
public class FrequentProceduresResponse {

    @SerializedName("EstadoTransaccion")
    private Boolean transactionState;
    @SerializedName("TransactionServiceMessage")
    private ProcedureServiceMessage message;
    @SerializedName("Tramites")
    private List<Procedure> procedures;

    public Boolean getTransactionState() {
        return transactionState;
    }

    public void setTransactionState(Boolean transactionState) {
        this.transactionState = transactionState;
    }

    public ProcedureServiceMessage getMensaje() {
        return message;
    }

    public void setMensaje(ProcedureServiceMessage mensaje) {
        message = mensaje;
    }

    public List<Procedure> getTramites() {
        return procedures;
    }

    public void setTramites(List<Procedure> procedures) {
        this.procedures = procedures;
    }

}
