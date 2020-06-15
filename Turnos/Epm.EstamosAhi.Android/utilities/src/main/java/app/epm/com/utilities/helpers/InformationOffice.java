package app.epm.com.utilities.helpers;
import java.io.Serializable;
import java.util.Date;

public class  InformationOffice implements Serializable {

    private Double currentLatitud;
    private Double currentLongitud;
    private Integer idOficina;
    private String idOficinaSentry;
    private String nombreOficina;
    private Double latitud;
    private Double longitud;
    private Boolean puntoFacil;
    private Date turnDate;

    public InformationOffice() {
    }

    public Double getCurrentLatitud() {
        return currentLatitud;
    }

    public void setCurrentLatitud(Double currentLatitud) {
        this.currentLatitud = currentLatitud;
    }

    public Double getCurrentLongitud() {
        return currentLongitud;
    }

    public void setCurrentLongitud(Double currentLongitud) {
        this.currentLongitud = currentLongitud;
    }

    public Integer getIdOficina() {
        return idOficina;
    }

    public void setIdOficina(Integer idOficina) {
        this.idOficina = idOficina;
    }

    public String getIdOficinaSentry() {
        return idOficinaSentry;
    }

    public void setIdOficinaSentry(String idOficinaSentry) {
        this.idOficinaSentry = idOficinaSentry;
    }

    public String getNombreOficina() {
        return nombreOficina;
    }

    public void setNombreOficina(String nombreOficina) {
        this.nombreOficina = nombreOficina;
    }

    public Double getLatitud() {
        return latitud;
    }

    public void setLatitud(Double latitud) {
        this.latitud = latitud;
    }

    public Double getLongitud() {
        return longitud;
    }

    public void setLongitud(Double longitud) {
        this.longitud = longitud;
    }

    public Boolean getPuntoFacil() {
        return puntoFacil;
    }

    public void setPuntoFacil(Boolean puntoFacil) {
        this.puntoFacil = puntoFacil;
    }

    public Date getTurnDate() {
        return turnDate;
    }

    public void setTurnDate(Date turnDate) {
        this.turnDate = turnDate;
    }
}
