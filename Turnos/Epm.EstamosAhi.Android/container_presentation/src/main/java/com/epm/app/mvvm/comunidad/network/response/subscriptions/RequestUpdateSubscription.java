package com.epm.app.mvvm.comunidad.network.response.subscriptions;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class RequestUpdateSubscription {

    @SerializedName("IdMunicipio")
    @Expose
    private Integer idMunicipality;
    @SerializedName("IdUsuario")
    @Expose
    private String idUser;
    @SerializedName("IdSuscripcion")
    @Expose
    private Integer idSuscription;
    @SerializedName("Nombres")
    @Expose
    private String name;
    @SerializedName("Apellidos")
    @Expose
    private String lastName;
    @SerializedName("Celular")
    @Expose
    private String telephone;
    @SerializedName("CorreoElectronico")
    @Expose
    private String correoElectronico;
    @SerializedName("IdSector")
    @Expose
    private Integer idSector;
    @SerializedName("IdSuscripcionOneSignal")
    @Expose
    private String idSuscripcionOneSignal;
    @SerializedName("AceptaTerminosCondiciones")
    @Expose
    private Boolean aceptaTerminosCondiciones;
    @SerializedName("EnvioNotificacion")
    @Expose
    private Boolean envioNotificacion;
    @SerializedName("IdDispositivo")
    @Expose
    private String idDispositivo;

    public Integer getIdMunicipality() {
        return idMunicipality;
    }

    public void setIdMunicipality(Integer idMunicipality) {
        this.idMunicipality = idMunicipality;
    }

    public String getIdUser() {
        return idUser;
    }

    public void setIdUser(String idUser) {
        this.idUser = idUser;
    }

    public Integer getIdSuscription() {
        return idSuscription;
    }

    public void setIdSuscription(Integer idSuscription) {
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

    public String getCorreoElectronico() {
        return correoElectronico;
    }

    public void setCorreoElectronico(String correoElectronico) {
        this.correoElectronico = correoElectronico;
    }

    public Integer getIdSector() {
        return idSector;
    }

    public void setIdSector(Integer idSector) {
        this.idSector = idSector;
    }

    public String getIdSuscripcionOneSignal() {
        return idSuscripcionOneSignal;
    }

    public void setIdSuscripcionOneSignal(String idSuscripcionOneSignal) {
        this.idSuscripcionOneSignal = idSuscripcionOneSignal;
    }

    public Boolean getAceptaTerminosCondiciones() {
        return aceptaTerminosCondiciones;
    }

    public void setAceptaTerminosCondiciones(Boolean aceptaTerminosCondiciones) {
        this.aceptaTerminosCondiciones = aceptaTerminosCondiciones;
    }

    public Boolean getEnvioNotificacion() {
        return envioNotificacion;
    }

    public void setEnvioNotificacion(Boolean envioNotificacion) {
        this.envioNotificacion = envioNotificacion;
    }

    public String getIdDispositivo() {
        return idDispositivo;
    }

    public void setIdDispositivo(String idDispositivo) {
        this.idDispositivo = idDispositivo;
    }

}
