
package com.epm.app.mvvm.procedure.network.response.GuideProceduresAndRequirementsCategory;

import java.util.List;

import com.epm.app.mvvm.procedure.network.response.GuideProceduresAndRequirementsCategory.GuideProceduresAndRequirementsCategoryItem;
import com.epm.app.mvvm.procedure.network.response.ProcedureServiceMessage;
import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class GuideProceduresAndRequirementsCategoryResponse {

    @SerializedName("EstadoTransaccion")
    @Expose
    private Boolean transactionState;
    @SerializedName("Categorias")
    @Expose
    private List<GuideProceduresAndRequirementsCategoryItem> guideProceduresAndRequirementsCategoryItems = null;
    @SerializedName("TransactionServiceMessage")
    @Expose
    private ProcedureServiceMessage message;

    public Boolean getTransactionState() {
        return transactionState;
    }

    public void setTransactionState(Boolean transactionState) {
        this.transactionState = transactionState;
    }

    public List<GuideProceduresAndRequirementsCategoryItem> getGuideProceduresAndRequirementsCategoryItems() {
        return guideProceduresAndRequirementsCategoryItems;
    }

    public void setGuideProceduresAndRequirementsCategoryItems(List<GuideProceduresAndRequirementsCategoryItem> guideProceduresAndRequirementsCategoryItems) {
        this.guideProceduresAndRequirementsCategoryItems = guideProceduresAndRequirementsCategoryItems;
    }

    public ProcedureServiceMessage getMessage() {
        return message;
    }

    public void setMessage(ProcedureServiceMessage message) {
        this.message = message;
    }

}
