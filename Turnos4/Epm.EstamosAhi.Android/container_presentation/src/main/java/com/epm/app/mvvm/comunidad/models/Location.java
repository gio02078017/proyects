package com.epm.app.mvvm.comunidad.models;

public class Location {

    private double currentLocationLatitude;
    private double currentLocationLongitude;
    private double arriveLocationLatitude;
    private double arriveLocationLongitude;

    public double getCurrentLocationLatitude() {
        return currentLocationLatitude;
    }

    public void setCurrentLocationLatitude(double currentLocationLatitude) {
        this.currentLocationLatitude = currentLocationLatitude;
    }

    public double getCurrentLocationLongitude() {
        return currentLocationLongitude;
    }

    public void setCurrentLocationLongitude(double currentLocationLongitude) {
        this.currentLocationLongitude = currentLocationLongitude;
    }

    public double getArriveLocationLatitude() {
        return arriveLocationLatitude;
    }

    public void setArriveLocationLatitude(double arriveLocationLatitude) {
        this.arriveLocationLatitude = arriveLocationLatitude;
    }

    public double getArriveLocationLongitude() {
        return arriveLocationLongitude;
    }

    public void setArriveLocationLongitude(double arriveLocationLongitude) {
        this.arriveLocationLongitude = arriveLocationLongitude;
    }
}
