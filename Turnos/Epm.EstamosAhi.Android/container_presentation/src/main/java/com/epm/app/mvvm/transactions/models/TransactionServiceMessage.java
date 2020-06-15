package com.epm.app.mvvm.transactions.models;

import com.google.gson.annotations.SerializedName;

public class TransactionServiceMessage {

    @SerializedName("Contenido")
    private String content;
    @SerializedName("Identificador")
    private Long identificator;
    @SerializedName("Titulo")
    private String title;

    public TransactionServiceMessage() {
    }

    public TransactionServiceMessage(String content, Long identificator, String title) {
        this.content = content;
        this.identificator = identificator;
        this.title = title;
    }

    public String getContent() {
        return content;
    }

    public void setContent(String contenido) {
        content = contenido;
    }

    public Long getIdentificator() {
        return identificator;
    }

    public void setIdentificator(Long identificator) {
        this.identificator = identificator;
    }

    public String getTitle() {
        return title;
    }

    public void setTitle(String title) {
        this.title = title;
    }

}
