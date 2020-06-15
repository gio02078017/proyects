package com.epm.app.mvvm.comunidad.network.response.subscriptions;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class DetailRedAlert {

    @SerializedName("Nombre")
    @Expose
    private String name;
    @SerializedName("ReferenciaDescripcion")
    @Expose
    private String referenceDescription;
    @SerializedName("Latitud")
    @Expose
    private String latitude;
    @SerializedName("Longitud")
    @Expose
    private String longitude;
    @SerializedName("PuntoReferencia")
    @Expose
    private Object referencePoint;

    public String getNombre() {
        return name;
    }

    public void setNombre(String nombre) {
        this.name = nombre;
    }

    public String getReferenciaDescripcion() {
        return referenceDescription;
    }

    public void setReferenciaDescripcion(String referenciaDescripcion) {
        this.referenceDescription = referenciaDescripcion;
    }

    public String getLatitud() {
        return latitude;
    }

    public void setLatitud(String latitud) {
        this.latitude = latitud;
    }

    public String getLongitud() {
        return longitude;
    }

    public void setLongitud(String longitud) {
        this.longitude = longitud;
    }

    public Object getPuntoReferencia() {
        return referencePoint;
    }

    public void setPuntoReferencia(Object puntoReferencia) {
        this.referencePoint = puntoReferencia;
    }


}
