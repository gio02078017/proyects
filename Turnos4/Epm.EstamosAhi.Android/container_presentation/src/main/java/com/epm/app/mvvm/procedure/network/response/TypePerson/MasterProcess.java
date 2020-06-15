
package com.epm.app.mvvm.procedure.network.response.TypePerson;
import com.google.gson.annotations.SerializedName;

import java.util.List;


public class MasterProcess {

    @SerializedName("Detalle")
    private List<TypePersonItem> typePersonItem;

    public MasterProcess(List<TypePersonItem> typePersonItem) {
        this.typePersonItem = typePersonItem;
    }

    public List<TypePersonItem> getTypePersonItem() {
        return typePersonItem;
    }

    public void setTypePersonItem(List<TypePersonItem> typePersonItem) {
        this.typePersonItem = typePersonItem;
    }
}
