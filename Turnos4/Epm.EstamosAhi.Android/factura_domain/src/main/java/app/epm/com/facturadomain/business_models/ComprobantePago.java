package app.epm.com.facturadomain.business_models;

import java.util.List;

/**
 * Created by mateoquicenososa on 12/01/17.
 */

public class ComprobantePago extends Transaction{

    private String correos;

    private Number valorTotalPago;

    private String nombreEntidadFinanciera;

    private String direccionIp;

    private String fechaTransaccion;

    private List<FacturasPorTransaccion> facturasPorTransaccion;

    public String getCorreos() {
        return correos;
    }

    public void setCorreos(String correos) {
        this.correos = correos;
    }

    public Number getValorTotalPago() {
        return valorTotalPago;
    }

    public void setValorTotalPago(Number valorTotalPago) {
        this.valorTotalPago = valorTotalPago;
    }


    public String getNombreEntidadFinanciera() {
        return nombreEntidadFinanciera;
    }

    public void setNombreEntidadFinanciera(String nombreEntidadFinanciera) {
        this.nombreEntidadFinanciera = nombreEntidadFinanciera;
    }

    public String getDireccionIp() {
        return direccionIp;
    }

    public void setDireccionIp(String direccionIp) {
        this.direccionIp = direccionIp;
    }

    public List<FacturasPorTransaccion> getFacturasPorTransaccion() {
        return facturasPorTransaccion;
    }

    public void setFacturasPorTransaccion(List<FacturasPorTransaccion> facturasPorTransaccion) {
        this.facturasPorTransaccion = facturasPorTransaccion;
    }

    public String getFechaTransaccion() {
        return fechaTransaccion;
    }

    public void setFechaTransaccion(String fechaTransaccion) {
        this.fechaTransaccion = fechaTransaccion;
    }
}
