package app.epm.com.factura_presentation.dto;

import com.epm.app.business_models.dto.MensajeDTO;

import java.util.ArrayList;

/**
 * Created by mateoquicenososa on 10/01/17.
 */

public class InformacionPseDTO {

    private int IdTransaccion;

    private String CodigoTrazabilidad;

    private MensajeDTO mensajeDTO;

    private String URLEntidadFinanciera;

    private int EstadoTransaccion;

    private boolean TieneFacturasVencidas;

    private String URLRetorno;

    private ArrayList<FacturasPorTransaccionDTO> FacturasPorTransaccion;

    private String DireccionIPPersona;

    private String FechaTransaccion;

    public int getIdTransaccion() {
        return IdTransaccion;
    }

    public void setIdTransaccion(int idTransaccion) {
        IdTransaccion = idTransaccion;
    }

    public String getCodigoTrazabilidad() {
        return CodigoTrazabilidad;
    }

    public void setCodigoTrazabilidad(String codigoTrazabilidad) {
        CodigoTrazabilidad = codigoTrazabilidad;
    }

    public MensajeDTO getMensajeDTO() {
        return mensajeDTO;
    }

    public void setMensajeDTO(MensajeDTO mensajeDTO) {
        this.mensajeDTO = mensajeDTO;
    }

    public String getURLEntidadFinanciera() {
        return URLEntidadFinanciera;
    }

    public void setURLEntidadFinanciera(String URLEntidadFinanciera) {
        this.URLEntidadFinanciera = URLEntidadFinanciera;
    }

    public int getEstadoTransaccion() {
        return EstadoTransaccion;
    }

    public void setEstadoTransaccion(int estadoTransaccion) {
        EstadoTransaccion = estadoTransaccion;
    }

    public boolean isTieneFacturasVencidas() {
        return TieneFacturasVencidas;
    }

    public void setTieneFacturasVencidas(boolean tieneFacturasVencidas) {
        TieneFacturasVencidas = tieneFacturasVencidas;
    }

    public String getURLRetorno() {
        return URLRetorno;
    }

    public void setURLRetorno(String URLRetorno) {
        this.URLRetorno = URLRetorno;
    }

    public ArrayList<FacturasPorTransaccionDTO> getFacturasPorTransaccion() {
        return FacturasPorTransaccion;
    }

    public void setFacturasPorTransaccion(ArrayList<FacturasPorTransaccionDTO> facturasPorTransaccion) {
        FacturasPorTransaccion = facturasPorTransaccion;
    }

    public String getDireccionIPPersona() {
        return DireccionIPPersona;
    }

    public void setDireccionIPPersona(String direccionIPPersona) {
        DireccionIPPersona = direccionIPPersona;
    }

    public String getFechaTransaccion() {
        return FechaTransaccion;
    }

    public void setFechaTransaccion(String fechaTransaccion) {
        FechaTransaccion = fechaTransaccion;
    }
}
