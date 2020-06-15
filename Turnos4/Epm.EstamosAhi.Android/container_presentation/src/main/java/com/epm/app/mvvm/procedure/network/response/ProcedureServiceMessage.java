
package com.epm.app.mvvm.procedure.network.response;

import com.google.gson.annotations.SerializedName;
public class ProcedureServiceMessage {

    @SerializedName("Contenido")
    private String content;
    @SerializedName("Identificador")
    private Long identificator;
    @SerializedName("Titulo")
    private String title;

    public ProcedureServiceMessage() {
    }

    public ProcedureServiceMessage(String content, Long identificator, String title) {
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
