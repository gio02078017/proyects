
package com.epm.app.mvvm.procedure.network.response.TypePerson;

import com.google.gson.annotations.SerializedName;

import java.io.Serializable;
import java.util.List;

public class TypePersonItem implements Serializable {

    @SerializedName("IdDetalle")
    private String TypePersonId;
    @SerializedName("NombreDetalle")
    private String TypePersonName;
    @SerializedName("ActivoDetalle")
    private Boolean active;

    public TypePersonItem(String typePersonId, String typePersonName, Boolean active) {
        TypePersonId = typePersonId;
        TypePersonName = typePersonName;
        this.active = active;
    }

    public String getTypePersonId() {
        return TypePersonId;
    }

    public void setTypePersonId(String typePersonId) {
        TypePersonId = typePersonId;
    }

    public String getTypePersonName() {
        return TypePersonName;
    }

    public void setTypePersonName(String typePersonName) {
        TypePersonName = typePersonName;
    }

    public Boolean isActive() {
        return active;
    }

    public void setActive(Boolean state) {
        this.active = active;
    }
}
