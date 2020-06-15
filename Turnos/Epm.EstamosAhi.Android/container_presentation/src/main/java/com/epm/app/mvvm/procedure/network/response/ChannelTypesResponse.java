package com.epm.app.mvvm.procedure.network.response;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class ChannelTypesResponse {

    @SerializedName("Id")
    @Expose
    private String id;
    @SerializedName("Nombre")
    @Expose
    private String name;
    @SerializedName("Activo")
    @Expose
    private Boolean active;

    public String getId() {
        return id;
    }

    public void setId(String id) {
        this.id = id;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public Boolean getActive() {
        return active;
    }

    public void setActive(Boolean active) {
        this.active = active;
    }


}
