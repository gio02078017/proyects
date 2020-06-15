package app.epm.com.contacto_transparente_domain.business_models;

import java.io.Serializable;

/**
 * Created by leidycarolinazuluagabastidas on 15/03/17.
 */

public class Incidente implements Serializable {

    private String fechaCreacion;
    private String idGrupoInteres;
    private int idEstado;
    private String nombreDelEstado;
    private String nombreGrupoInteres;
    private String fechaDeSeguimiento;
    private String informeDeLaDenuncia;
    private String descripcion;
    private String lugarEnDondeSucedio;
    private String comoSeDioCuenta;
    private String fechaDeteccion;
    private String personasInvolucradas;
    private String personasInvolucradasEnLaEmpresa;
    private boolean anonimo;
    private String nombreContacto;
    private String telefonoContacto;
    private String correoElectronicoContacto;

    public String getFechaCreacion() {
        return fechaCreacion;
    }

    public void setFechaCreacion(String fechaCreacion) {
        this.fechaCreacion = fechaCreacion;
    }

    public String getIdGrupoInteres() {
        return idGrupoInteres;
    }

    public void setIdGrupoInteres(String idGrupoInteres) {
        this.idGrupoInteres = idGrupoInteres;
    }

    public int getIdEstado() {
        return idEstado;
    }

    public void setIdEstado(int idEstado) {
        this.idEstado = idEstado;
    }

    public String getNombreDelEstado() {
        return nombreDelEstado;
    }

    public void setNombreDelEstado(String nombreDelEstado) {
        this.nombreDelEstado = nombreDelEstado;
    }

    public String getNombreGrupoInteres() {
        return nombreGrupoInteres;
    }

    public void setNombreGrupoInteres(String nombreGrupoInteres) {
        this.nombreGrupoInteres = nombreGrupoInteres;
    }

    public String getFechaDeSeguimiento() {
        return fechaDeSeguimiento;
    }

    public void setFechaDeSeguimiento(String fechaDeSeguimiento) {
        this.fechaDeSeguimiento = fechaDeSeguimiento;
    }

    public String getInformeDeLaDenuncia() {
        return informeDeLaDenuncia;
    }

    public void setInformeDeLaDenuncia(String informeDeLaDenuncia) {
        this.informeDeLaDenuncia = informeDeLaDenuncia;
    }

    public String getDescripcion() {
        return descripcion;
    }

    public void setDescripcion(String descripcion) {
        this.descripcion = descripcion;
    }

    public String getLugarEnDondeSucedio() {
        return lugarEnDondeSucedio;
    }

    public void setLugarEnDondeSucedio(String lugarEnDondeSucedio) {
        this.lugarEnDondeSucedio = lugarEnDondeSucedio;
    }

    public String getComoSeDioCuenta() {
        return comoSeDioCuenta;
    }

    public void setComoSeDioCuenta(String comoSeDioCuenta) {
        this.comoSeDioCuenta = comoSeDioCuenta;
    }

    public String getFechaDeteccion() {
        return fechaDeteccion;
    }

    public void setFechaDeteccion(String fechaDeteccion) {
        this.fechaDeteccion = fechaDeteccion;
    }

    public String getPersonasInvolucradas() {
        return personasInvolucradas;
    }

    public void setPersonasInvolucradas(String personasInvolucradas) {
        this.personasInvolucradas = personasInvolucradas;
    }

    public String getPersonasInvolucradasEnLaEmpresa() {
        return personasInvolucradasEnLaEmpresa;
    }

    public void setPersonasInvolucradasEnLaEmpresa(String personasInvolucradasEnLaEmpresa) {
        this.personasInvolucradasEnLaEmpresa = personasInvolucradasEnLaEmpresa;
    }

    public boolean isAnonimo() {
        return anonimo;
    }

    public void setAnonimo(boolean anonimo) {
        this.anonimo = anonimo;
    }

    public String getNombreContacto() {
        return nombreContacto;
    }

    public void setNombreContacto(String nombreContacto) {
        this.nombreContacto = nombreContacto;
    }

    public String getTelefonoContacto() {
        return telefonoContacto;
    }

    public void setTelefonoContacto(String telefonoContacto) {
        this.telefonoContacto = telefonoContacto;
    }

    public String getCorreoElectronicoContacto() {
        return correoElectronicoContacto;
    }

    public void setCorreoElectronicoContacto(String correoElectronicoContacto) {
        this.correoElectronicoContacto = correoElectronicoContacto;
    }
}
