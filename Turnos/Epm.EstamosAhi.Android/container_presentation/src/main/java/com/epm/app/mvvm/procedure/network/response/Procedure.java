
package com.epm.app.mvvm.procedure.network.response;

import java.io.Serializable;
import java.util.List;
import com.google.gson.annotations.SerializedName;

public class Procedure implements Serializable {

    @SerializedName("Activo")
    private Boolean state;
    @SerializedName("Id")
    private String id;
    @SerializedName("ListaServicios")
    private List<String> services;
    @SerializedName("Nombre")
    private String name;

    public Procedure(Boolean state, String id, List<String> services, String name) {
        this.state = state;
        this.id = id;
        this.services = services;
        this.name = name;
    }

    public Boolean isActive() {
        return state;
    }

    public void setState(Boolean state) {
        this.state = state;
    }

    public String getId() {
        return id;
    }

    public void setId(String id) {
        this.id = id;
    }

    public List<String> getServices() {
        return services;
    }

    public void setServices(List<String> services) {
        this.services = services;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

}
