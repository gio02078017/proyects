package com.epm.app.mvvm.procedure.network.response;

import com.epm.app.mvvm.turn.network.response.Mensaje;
import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class DetailOfTheTransactionResponse {

    @SerializedName("DetalleTramite")
    @Expose
    private DetailTransactionResponse detailTransactionResponse;
    @SerializedName("TransactionServiceMessage")
    @Expose
    private Mensaje mensaje;

    public DetailTransactionResponse getDetailTransactionResponse() {
        return detailTransactionResponse;
    }

    public void setDetailTransactionResponse(DetailTransactionResponse detailTransactionResponse) {
        this.detailTransactionResponse = detailTransactionResponse;
    }

    public Mensaje getMensaje() {
        return mensaje;
    }

    public void setMensaje(Mensaje mensaje) {
        this.mensaje = mensaje;
    }

}
