
package com.epm.app.mvvm.turn.network.response.nearbyOffices;

import java.util.List;

import com.epm.app.mvvm.turn.network.response.Mensaje;
import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class NearbyOfficesResponse {

    @SerializedName("Oficinas")
    @Expose
    private List<NearbyOfficesItem> nearbyOfficesItems = null;
    @SerializedName("Mensaje")
    @Expose
    private Mensaje mensaje;

    public List<NearbyOfficesItem> getNearbyOfficesItems() {
        return nearbyOfficesItems;
    }

    public void setNearbyOfficesItems(List<NearbyOfficesItem> nearbyOfficesItems) {
        this.nearbyOfficesItems = nearbyOfficesItems;
    }

    public Mensaje getMensaje() {
        return mensaje;
    }

    public void setMensaje(Mensaje mensaje) {
        this.mensaje = mensaje;
    }

}
