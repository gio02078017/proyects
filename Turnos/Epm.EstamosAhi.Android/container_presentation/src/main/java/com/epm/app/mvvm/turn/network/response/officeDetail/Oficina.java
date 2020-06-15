
package com.epm.app.mvvm.turn.network.response.officeDetail;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

import java.io.Serializable;

public class Oficina  implements Serializable {

    @SerializedName("IdOficinaSentry")
    @Expose
    private String idOficinaSentry;
    @SerializedName("NombreOficina")
    @Expose
    private String nombre;
    @SerializedName("Direccion")
    @Expose
    private String direccion;
    @SerializedName("Horario")
    @Expose
    private String horario;
    @SerializedName("Imagen")
    @Expose
    private String imagen;
    @SerializedName("Latitud")
    @Expose
    private Float latitud;
    @SerializedName("Longitud")
    @Expose
    private Float longitud;
    @SerializedName("EstadoOficina")
    @Expose
    private String estadoOficina;
    @SerializedName("TurnosEnEspera")
    @Expose
    private Integer turnosEnEspera;
    @SerializedName("UltimoTurnoLlamado")
    @Expose
    private String ultimoTurnoLlamado;
    @SerializedName("TiempoPromedioEspera")
    @Expose
    private String tiempoPromedio;
    @SerializedName("HoraCierre")
    @Expose
    private String horaCierre;

    public String getIdOficinaSentry() {
        return idOficinaSentry;
    }

    public void setIdOficinaSentry(String idOficinaSentry) {
        this.idOficinaSentry = idOficinaSentry;
    }

    public String getNombre() {
        return nombre;
    }

    public void setNombre(String nombre) {
        this.nombre = nombre;
    }

    public String getDireccion() {
        return direccion;
    }

    public void setDireccion(String direccion) {
        this.direccion = direccion;
    }

    public String getHorario() {
        return horario;
    }

    public void setHorario(String horario) {
        this.horario = horario;
    }

    public String getImagen() {
        return imagen;
    }

    public void setImagen(String imagen) {
        this.imagen = imagen;
    }

    public Float getLatitud() {
        return latitud;
    }

    public void setLatitud(Float latitud) {
        this.latitud = latitud;
    }

    public Float getLongitud() {
        return longitud;
    }

    public void setLongitud(Float longitud) {
        this.longitud = longitud;
    }

    public String getEstadoOficina() {
        return estadoOficina;
    }

    public void setEstadoOficina(String estadoOficina) {
        this.estadoOficina = estadoOficina;
    }

    public Integer getTurnosEnEspera() {
        return turnosEnEspera;
    }

    public void setTurnosEnEspera(Integer turnosEnEspera) {
        this.turnosEnEspera = turnosEnEspera;
    }

    public String getUltimoTurnoLlamado() {
        return ultimoTurnoLlamado;
    }

    public void setUltimoTurnoLlamado(String ultimoTurnoLlamado) {
        this.ultimoTurnoLlamado = ultimoTurnoLlamado;
    }

    public String getTiempoPromedio() {
        return tiempoPromedio;
    }

    public void setTiempoPromedio(String tiempoPromedio) {
        this.tiempoPromedio = tiempoPromedio;
    }

    public String getHoraCierre() {
        return horaCierre;
    }

    public void setHoraCierre(String horaCierre) {
        this.horaCierre = horaCierre;
    }

}
