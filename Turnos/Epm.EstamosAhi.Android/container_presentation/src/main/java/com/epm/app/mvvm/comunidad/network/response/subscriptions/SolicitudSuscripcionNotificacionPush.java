package com.epm.app.mvvm.comunidad.network.response.subscriptions;

import com.google.gson.annotations.SerializedName;

public class SolicitudSuscripcionNotificacionPush {

    @SerializedName("nombres")
    public String name;

    @SerializedName("apellidos")
    public String lastName;

    @SerializedName("celular")
    public String celular;

    @SerializedName("correoElectronico")
    public String email;

    @SerializedName("")
    public int idMunicipio;

    @SerializedName("idSector")
    public int idSector;

    @SerializedName("idSuscripcionOneSignal")
    public String idOneSignal;

    @SerializedName("aceptaTerminosCondiciones")
    public boolean termsConditions;

    @SerializedName("envioNotificacion")
    public boolean notification;

    @SerializedName("idDispositivo")
    public String idDispositve;

    @SerializedName("idUsuario")
    public String idUser;

    @SerializedName("idTipoSuscripcionNotificacion")
    public int idTypeSuscription;

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

    public String getCelular() {
        return celular;
    }

    public void setCelular(String celular) {
        this.celular = celular;
    }

    public String getEmail() {
        return email;
    }

    public void setEmail(String email) {
        this.email = email;
    }

    public int getIdSector() {
        return idSector;
    }

    public void setIdSector(int idSector) {
        this.idSector = idSector;
    }

    public String getIdOneSignal() {
        return idOneSignal;
    }

    public void setIdOneSignal(String idOneSignal) {
        this.idOneSignal = idOneSignal;
    }

    public boolean isTermsConditions() {
        return termsConditions;
    }

    public void setTermsConditions(boolean termsConditions) {
        this.termsConditions = termsConditions;
    }

    public boolean isNotification() {
        return notification;
    }

    public void setNotification(boolean notification) {
        this.notification = notification;
    }

    public String getIdDispositve() {
        return idDispositve;
    }

    public void setIdDispositve(String idDispositve) {
        this.idDispositve = idDispositve;
    }

    public String getIdUser() {
        return idUser;
    }

    public void setIdUser(String idUser) {
        this.idUser = idUser;
    }

    public int getIdTypeSuscription() {
        return idTypeSuscription;
    }

    public void setIdTypeSuscription(int idTypeSuscription) {
        this.idTypeSuscription = idTypeSuscription;
    }
}
