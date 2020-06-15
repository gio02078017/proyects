package com.epm.app.mvvm.transactions.network.response;


import com.epm.app.mvvm.transactions.models.TransactionServiceMessage;
import com.epm.app.mvvm.transactions.models.Transaction;
import com.google.gson.annotations.SerializedName;

import java.util.List;


public class TransactionListResponse {

    @SerializedName("EstadoTransaccion")
    private boolean transactionState;
    @SerializedName("TransaccionesRapidas")
    private List<Transaction> transaction = null;
    @SerializedName("Mensaje")
    private TransactionServiceMessage transactionServiceMessage;

    public boolean getTransactionState() {
        return transactionState;
    }

    public void setTransactionState(boolean estadoTransaccion) {
        this.transactionState = estadoTransaccion;
    }

    public List<Transaction> getTransaction() {
        return transaction;
    }

    public void setTransaction(List<Transaction> transaction) {
        this.transaction = transaction;
    }

    public TransactionServiceMessage getTransactionServiceMessage() {
        return transactionServiceMessage;
    }

    public void setTransactionServiceMessage(TransactionServiceMessage transactionServiceMessage) {
        this.transactionServiceMessage = transactionServiceMessage;
    }

}


