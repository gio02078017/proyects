package com.epm.app.mvvm.comunidad.network.response.subscriptions;

import com.google.gson.annotations.SerializedName;

public class GetSubscriptionNotifications {

    @SerializedName("IdSuscripcion")
    public int idSuscription;

    @SerializedName("Nombres")
    public String name;

    @SerializedName("Apellidos")
    public String lastName;

    @SerializedName("Celular")
    public String telephone;

    @SerializedName("CorreoElectronico")
    public String email;

    @SerializedName("IdMunicipio")
    public int municipality;

    @SerializedName("IdSector")
    public int sector;

    @SerializedName("IdSuscripcionOneSignal")
    public String idOneSignal;

    @SerializedName("IdDispositivo")
    public String idDispositive;

    @SerializedName("EnvioNotificacion")
    public boolean notification;

    @SerializedName("AceptaTerminosCondiciones")
    public boolean acceptTermsConditions;


    public int getIdSuscription() {
        return idSuscription;
    }

    public void setIdSuscription(int idSuscription) {
        this.idSuscription = idSuscription;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public String getLastName() {
        return lastName;
    }

    public void setLastName(String lastName) {
        this.lastName = lastName;
    }

    public String getTelephone() {
        return telephone;
    }

    public void setTelephone(String telephone) {
        this.telephone = telephone;
    }

    public String getEmail() {
        return email;
    }

    public void setEmail(String email) {
        this.email = email;
    }

    public int getMunicipality() {
        return municipality;
    }

    public void setMunicipality(int municipality) {
        this.municipality = municipality;
    }

    public int getSector() {
        return sector;
    }

    public void setSector(int sector) {
        this.sector = sector;
    }

    public String getIdOneSignal() {
        return idOneSignal;
    }

    public void setIdOneSignal(String idOneSignal) {
        this.idOneSignal = idOneSignal;
    }

    public String getIdDispositive() {
        return idDispositive;
    }

    public void setIdDispositive(String idDispositive) {
        this.idDispositive = idDispositive;
    }

    public boolean isNotification() {
        return notification;
    }

    public void setNotification(boolean notification) {
        this.notification = notification;
    }

    public boolean isAcceptTermsConditions() {
        return acceptTermsConditions;
    }

    public void setAcceptTermsConditions(boolean acceptTermsConditions) {
        this.acceptTermsConditions = acceptTermsConditions;
    }
}
