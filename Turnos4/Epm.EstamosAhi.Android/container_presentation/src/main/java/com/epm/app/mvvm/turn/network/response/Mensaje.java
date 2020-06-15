
package com.epm.app.mvvm.turn.network.response;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

import java.io.Serializable;

public class Mensaje implements Serializable {

    @SerializedName("Identificador")
    @Expose
    private Integer identificador;
    @SerializedName("Titulo")
    @Expose
    private String titulo;
    @SerializedName("Contenido")
    @Expose
    private String contenido;

    public Integer getIdentificador() {
        return identificador;
    }

    public void setIdentificador(Integer identificador) {
        this.identificador = identificador;
    }

    public String getTitulo() {
        return titulo;
    }

    public void setTitulo(String titulo) {
        this.titulo = titulo;
    }

    public String getContenido() {
        return contenido;
    }

    public void setContenido(String contenido) {
        this.contenido = contenido;
    }

}
