
package com.epm.app.mvvm.procedure.network.response;

import java.io.Serializable;
import java.util.List;
import com.google.gson.annotations.SerializedName;

public class Procedure implements Serializable {

    @SerializedName("Activo")
    private Boolean state;
    @SerializedName("Id")
    private String mId;
    @SerializedName("ListaServicios")
    private List<String> mListaServicios;
    @SerializedName("Nombre")
    private String name;

    public Procedure(Boolean state, String mId, List<String> mListaServicios, String name) {
        this.state = state;
        this.mId = mId;
        this.mListaServicios = mListaServicios;
        this.name = name;
    }

    public Boolean isActive() {
        return state;
    }

    public void setState(Boolean state) {
        this.state = state;
    }

    public String getId() {
        return mId;
    }

    public void setId(String id) {
        mId = id;
    }

    public List<String> getListaServicios() {
        return mListaServicios;
    }

    public void setListaServicios(List<String> listaServicios) {
        mListaServicios = listaServicios;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        name = name;
    }

}
