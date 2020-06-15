package app.epm.com.contacto_transparente_presentation.dto;

/**
 * Created by leidycarolinazuluagabastidas on 15/03/17.
 */

public class IncidentDTO {

    private int Codigo;
    private String Texto;
    private String FechaCreacion;
    private String IdGrupoInteres;
    private int IdEstado;
    private String NombreDelEstado;
    private String NombreGrupoInteres;
    private String FechaDeSeguimiento;
    private String InformeDeLaDenuncia;
    private String Descripcion;
    private String LugarEnDondeSucedio;
    private String ComoSeDioCuenta;
    private String FechaDeteccion;
    private String PersonasInvolucradas;
    private String PersonasInvolucradasEnLaEmpresa;
    private boolean Anonimo;
    private String NombreContacto;
    private String TelefonoContacto;
    private String CorreoElectronicoContacto;

    public int getCodigo() {
        return Codigo;
    }

    public void setCodigo(int codigo) {
        Codigo = codigo;
    }

    public String getTexto() {
        return Texto;
    }

    public void setTexto(String texto) {
        Texto = texto;
    }

    public String getFechaCreacion() {
        return FechaCreacion;
    }

    public void setFechaCreacion(String fechaCreacion) {
        FechaCreacion = fechaCreacion;
    }

    public String getIdGrupoInteres() {
        return IdGrupoInteres;
    }

    public void setIdGrupoInteres(String idGrupoInteres) {
        IdGrupoInteres = idGrupoInteres;
    }

    public int getIdEstado() {
        return IdEstado;
    }

    public void setIdEstado(int idEstado) {
        IdEstado = idEstado;
    }

    public String getNombreDelEstado() {
        return NombreDelEstado;
    }

    public void setNombreDelEstado(String nombreDelEstado) {
        NombreDelEstado = nombreDelEstado;
    }

    public String getNombreGrupoInteres() {
        return NombreGrupoInteres;
    }

    public void setNombreGrupoInteres(String nombreGrupoInteres) {
        NombreGrupoInteres = nombreGrupoInteres;
    }

    public String getFechaDeSeguimiento() {
        return FechaDeSeguimiento;
    }

    public void setFechaDeSeguimiento(String fechaDeSeguimiento) {
        FechaDeSeguimiento = fechaDeSeguimiento;
    }

    public String getInformeDeLaDenuncia() {
        return InformeDeLaDenuncia;
    }

    public void setInformeDeLaDenuncia(String informeDeLaDenuncia) {
        InformeDeLaDenuncia = informeDeLaDenuncia;
    }

    public String getDescripcion() {
        return Descripcion;
    }

    public void setDescripcion(String descripcion) {
        Descripcion = descripcion;
    }

    public String getLugarEnDondeSucedio() {
        return LugarEnDondeSucedio;
    }

    public void setLugarEnDondeSucedio(String lugarEnDondeSucedio) {
        LugarEnDondeSucedio = lugarEnDondeSucedio;
    }

    public String getComoSeDioCuenta() {
        return ComoSeDioCuenta;
    }

    public void setComoSeDioCuenta(String comoSeDioCuenta) {
        ComoSeDioCuenta = comoSeDioCuenta;
    }

    public String getFechaDeteccion() {
        return FechaDeteccion;
    }

    public void setFechaDeteccion(String fechaDeteccion) {
        FechaDeteccion = fechaDeteccion;
    }

    public String getPersonasInvolucradas() {
        return PersonasInvolucradas;
    }

    public void setPersonasInvolucradas(String personasInvolucradas) {
        PersonasInvolucradas = personasInvolucradas;
    }

    public String getPersonasInvolucradasEnLaEmpresa() {
        return PersonasInvolucradasEnLaEmpresa;
    }

    public void setPersonasInvolucradasEnLaEmpresa(String personasInvolucradasEnLaEmpresa) {
        PersonasInvolucradasEnLaEmpresa = personasInvolucradasEnLaEmpresa;
    }

    public boolean isAnonimo() {
        return Anonimo;
    }

    public void setAnonimo(boolean anonimo) {
        Anonimo = anonimo;
    }

    public String getNombreContacto() {
        return NombreContacto;
    }

    public void setNombreContacto(String nombreContacto) {
        NombreContacto = nombreContacto;
    }

    public String getTelefonoContacto() {
        return TelefonoContacto;
    }

    public void setTelefonoContacto(String telefonoContacto) {
        TelefonoContacto = telefonoContacto;
    }

    public String getCorreoElectronicoContacto() {
        return CorreoElectronicoContacto;
    }

    public void setCorreoElectronicoContacto(String correoElectronicoContacto) {
        CorreoElectronicoContacto = correoElectronicoContacto;
    }
}
