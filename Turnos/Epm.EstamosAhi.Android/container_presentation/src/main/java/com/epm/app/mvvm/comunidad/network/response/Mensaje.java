package com.epm.app.mvvm.comunidad.network.response;

import com.google.gson.annotations.SerializedName;

public class Mensaje {

    @SerializedName("Identificador")
    private int identificador;
    @SerializedName("Titulo")
    private String titulo;
    @SerializedName("Contenido")
    private String contenido;

    public int getIdentificador() {
        return identificador;
    }

    public void setIdentificador(int identificador) {
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
