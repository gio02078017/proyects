package app.epm.com.facturadomain.business_models;



import com.epm.app.business_models.business_models.Mensaje;

import java.io.Serializable;
import java.util.ArrayList;

/**
 * Created by mateoquicenososa on 10/01/17.
 */

public class InformacionPSE implements Serializable {

    private int idTransaccion;

    private String codigoTrazabilidad;

    private Mensaje mensaje;

    private String urlEntidadFinanciera;

    private int estadoTransaccion;

    private boolean tieneFacturasVencidas;

    private String urlRetorno;

    private ArrayList<FacturasPorTransaccion> facturasPorTransaccion;

    private String direccionIPPersona;

    private String fechaTransaccion;

     public int getIdTransaccion() {
        return idTransaccion;
    }

    public void setIdTransaccion(int idTransaccion) {
        this.idTransaccion = idTransaccion;
    }

    public String getCodigoTrazabilidad() {
        return codigoTrazabilidad;
    }

    public void setCodigoTrazabilidad(String codigoTrazabilidad) {
        this.codigoTrazabilidad = codigoTrazabilidad;
    }

    public Mensaje getMensaje() {
        return mensaje;
    }

    public void setMensaje(Mensaje mensaje) {
        this.mensaje = mensaje;
    }

    public String getUrlEntidadFinanciera() {
        return urlEntidadFinanciera;
    }

    public void setUrlEntidadFinanciera(String urlEntidadFinanciera) {
        this.urlEntidadFinanciera = urlEntidadFinanciera;
    }

    public int getEstadoTransaccion() {
        return estadoTransaccion;
    }

    public void setEstadoTransaccion(int estadoTransaccion) {
        this.estadoTransaccion = estadoTransaccion;
    }

    public boolean isTieneFacturasVencidas() {
        return tieneFacturasVencidas;
    }

    public void setTieneFacturasVencidas(boolean tieneFacturasVencidas) {
        this.tieneFacturasVencidas = tieneFacturasVencidas;
    }

    public String getUrlRetorno() {
        return urlRetorno;
    }

    public void setUrlRetorno(String urlRetorno) {
        this.urlRetorno = urlRetorno;
    }

    public ArrayList<FacturasPorTransaccion> getFacturasPorTransaccion() {
        return facturasPorTransaccion;
    }

    public void setFacturasPorTransaccion(ArrayList<FacturasPorTransaccion> facturasPorTransaccion) {
        this.facturasPorTransaccion = facturasPorTransaccion;
    }

    public String getDireccionIPPersona() {
        return direccionIPPersona;
    }

    public void setDireccionIPPersona(String direccionIPPersona) {
        this.direccionIPPersona = direccionIPPersona;
    }

    public String getFechaTransaccion() {
        return fechaTransaccion;
    }

    public void setFechaTransaccion(String fechaTransaccion) {
        this.fechaTransaccion = fechaTransaccion;
    }
}
