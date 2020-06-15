
package com.epm.app.mvvm.procedure.network.response.TypePerson;

import com.epm.app.mvvm.procedure.network.response.ProcedureServiceMessage;
import com.epm.app.mvvm.procedure.network.response.TypePerson.MasterProcess;
import com.google.gson.annotations.SerializedName;

public class TypePersonResponse {

    @SerializedName("TransactionServiceMessage")
    private ProcedureServiceMessage message;
    @SerializedName("MaestroTramite")
    private MasterProcess masterProcess;

    public ProcedureServiceMessage getMessage() {
        return message;
    }

    public void setMessage(ProcedureServiceMessage message) {
        this.message = message;
    }

    public MasterProcess getMasterProcess() {
        return masterProcess;
    }

    public void setMasterProcess(MasterProcess masterProcess) {
        this.masterProcess = masterProcess;
    }
}
