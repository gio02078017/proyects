package app.epm.com.factura_presentation.dto;

import java.util.List;

/**
 * Created by mateoquicenososa on 12/01/17.
 */

public class ComprobantePagoDTO {

    private String Correos;

    private Number ValorTotalPago;

    private int IdTransaccion;

    private String CodigoTrazabilidad;

    private int EstadoTransaccion;

    private String NombreEntidadFinanciera;

    private String DireccionIp;

    private String FechaTransaccion;

    private List<FacturasPorTransaccionDTO> FacturasPorTransaccion;

    public String getCorreos() {
        return Correos;
    }

    public void setCorreos(String correos) {
        Correos = correos;
    }

    public Number getValorTotalPago() {
        return ValorTotalPago;
    }

    public void setValorTotalPago(Number valorTotalPago) {
        ValorTotalPago = valorTotalPago;
    }

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

    public int getEstadoTransaccion() {
        return EstadoTransaccion;
    }

    public void setEstadoTransaccion(int estadoTransaccion) {
        EstadoTransaccion = estadoTransaccion;
    }

    public String getNombreEntidadFinanciera() {
        return NombreEntidadFinanciera;
    }

    public void setNombreEntidadFinanciera(String nombreEntidadFinanciera) {
        NombreEntidadFinanciera = nombreEntidadFinanciera;
    }

    public String getDireccionIp() {
        return DireccionIp;
    }

    public void setDireccionIp(String direccionIp) {
        DireccionIp = direccionIp;
    }

    public List<FacturasPorTransaccionDTO> getFacturasPorTransaccion() {
        return FacturasPorTransaccion;
    }

    public void setFacturasPorTransaccion(List<FacturasPorTransaccionDTO> facturasPorTransaccion) {
        FacturasPorTransaccion = facturasPorTransaccion;
    }

    public String getFechaTransaccion() {
        return FechaTransaccion;
    }

    public void setFechaTransaccion(String fechaTransaccion) {
        FechaTransaccion = fechaTransaccion;
    }
}
