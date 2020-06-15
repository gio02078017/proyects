package com.epm.app.business_models.business_models;

import java.io.Serializable;
import java.util.Date;

public class Usuario implements Serializable {

    private String correoElectronico;
    private String nombres;
    private String apellido;
    private Integer idTipoIdentificacion;
    private String numeroIdentificacion;
    private int idTipoPersona;
    private String telefono;
    private String celular;
    private String direccion;
    private int idTipoVivienda;
    private String pais;
    private String fechaNacimiento;
    private int idGenero;
    private String correoAlternativo;
    private boolean envioNotificacion;
    private boolean aceptoTerminosyCondiciones;
    private String fechaRegistro;
    private String token;
    private boolean activo;
    private String idUsuario;
    private String contrasenia;
    private boolean invitado;

    public Usuario() {
        this.correoElectronico = "" ;
        this.nombres = "";
        this.apellido = "";
        this.idTipoIdentificacion = 0;
        this.numeroIdentificacion = "";
        this.idTipoPersona = 0;
        this.telefono = "";
        this.celular = "";
        this.direccion = "";
        this.idTipoVivienda = 0;
        this.pais = "";
        this.fechaNacimiento = "";
        this.idGenero = 0;
        this.correoAlternativo = "";
        this.envioNotificacion = false;
        this.aceptoTerminosyCondiciones = false;
        this.fechaRegistro = "";
        this.token = "";
        this.activo = false;
        this.idUsuario = "";
        this.contrasenia = "";
        this.invitado = false;
    }

    public Integer getIdTipoIdentificacion() {
        return idTipoIdentificacion;
    }

    public void setIdTipoIdentificacion(Integer idTipoIdentificacion) {
        this.idTipoIdentificacion = idTipoIdentificacion;
    }

    public String getContrasenia() {
        return contrasenia;
    }

    public void setContrasenia(String contrasenia) {
        this.contrasenia = contrasenia;
    }

    public String getApellido() {
        return apellido;
    }

    public void setApellido(String apellido) {
        this.apellido = apellido;
    }

    public String getCelular() {
        return celular;
    }

    public void setCelular(String celular) {
        this.celular = celular;
    }

    public String getCorreoAlternativo() {
        return correoAlternativo;
    }

    public void setCorreoAlternativo(String correoAlternativo) {
        this.correoAlternativo = correoAlternativo;
    }

    public String getCorreoElectronico() {
        return correoElectronico;
    }

    public void setCorreoElectronico(String correoElectronico) {
        this.correoElectronico = correoElectronico;
    }

    public String getDireccion() {
        return direccion;
    }

    public void setDireccion(String direccion) {
        this.direccion = direccion;
    }

    public boolean isEnvioNotificacion() {
        return envioNotificacion;
    }

    public void setEnvioNotificacion(boolean envioNotificacion) {
        this.envioNotificacion = envioNotificacion;
    }

    public String getFechaNacimiento() {
        return fechaNacimiento;
    }

    public void setFechaNacimiento(String fechaNacimiento) {
        this.fechaNacimiento = fechaNacimiento;
    }

    public int getIdGenero() {
        return idGenero;
    }

    public void setIdGenero(int idGenero) {
        this.idGenero = idGenero;
    }

    public String getNombres() {
        return nombres;
    }

    public void setNombres(String nombres) {
        this.nombres = nombres;
    }

    public String getNumeroIdentificacion() {
        return numeroIdentificacion;
    }

    public void setNumeroIdentificacion(String numeroIdentificacion) {
        this.numeroIdentificacion = numeroIdentificacion;
    }

    public String getPais() {
        return pais;
    }

    public void setPais(String pais) {
        this.pais = pais;
    }

    public String getTelefono() {
        return telefono;
    }

    public void setTelefono(String telefono) {
        this.telefono = telefono;
    }

    public int getIdTipoPersona() {
        return idTipoPersona;
    }

    public void setIdTipoPersona(int idTipoPersona) {
        this.idTipoPersona = idTipoPersona;
    }

    public int getIdTipoVivienda() {
        return idTipoVivienda;
    }

    public void setIdTipoVivienda(int idTipoVivienda) {
        this.idTipoVivienda = idTipoVivienda;
    }

    public String getToken() {
        return token;
    }

    public void setToken(String token) {
        this.token = token;
    }

    public boolean isActivo() {
        return activo;
    }

    public void setActivo(boolean activo) {
        this.activo = activo;
    }

    public String getFechaRegistro() {
        return fechaRegistro;
    }

    public void setFechaRegistro(String fechaRegistro) {
        this.fechaRegistro = fechaRegistro;
    }

    public boolean isAceptoTerminosyCondiciones() {
        return aceptoTerminosyCondiciones;
    }

    public void setAceptoTerminosyCondiciones(boolean aceptoTerminosyCondiciones) {
        this.aceptoTerminosyCondiciones = aceptoTerminosyCondiciones;
    }

    public String getIdUsuario() {
        return idUsuario;
    }

    public void setIdUsuario(String idUsuario) {
        this.idUsuario = idUsuario;
    }

    public boolean isInvitado() {
        return invitado;
    }

    public void setInvitado(boolean invitado) {
        this.invitado = invitado;
    }
}
