package com.epm.app.mvvm.util;

import java.io.Serializable;

public class CustomLocation implements Serializable {

    private double Latitud;
    private double Longitud;
    private double scale;

    public CustomLocation() {
    }

    public double getLatitud() {
        return Latitud;
    }

    public void setLatitud(double latitud) {
        Latitud = latitud;
    }

    public double getLongitud() {
        return Longitud;
    }

    public void setLongitud(double longitud) {
        Longitud = longitud;
    }

    public double getScale() {
        return scale;
    }

    public void setScale(double scale) {
        this.scale = scale;
    }
}
