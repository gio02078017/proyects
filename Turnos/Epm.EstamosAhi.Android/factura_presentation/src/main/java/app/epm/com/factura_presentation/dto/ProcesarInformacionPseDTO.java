package app.epm.com.factura_presentation.dto;

import java.util.ArrayList;
import java.util.List;

/**
 * Created by mateoquicenososa on 18/01/17.
 */

public class ProcesarInformacionPseDTO {

    private int  EntidadFinanciera;
    private int IdTransaccion;
    private String CodigoTrazabilidad;
    private int EstadoTransaccion;
    private List<FacturasPorTransaccionDTO> FacturasPorTransaccion;

    public int getEntidadFinanciera() {
        return EntidadFinanciera;
    }

    public void setEntidadFinanciera(int entidadFinanciera) {
        EntidadFinanciera = entidadFinanciera;
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

    public List<FacturasPorTransaccionDTO> getFacturasPorTransaccion() {
        return FacturasPorTransaccion;
    }

    public void setFacturasPorTransaccion(ArrayList<FacturasPorTransaccionDTO> facturasPorTransaccion) {
        FacturasPorTransaccion = facturasPorTransaccion;
    }
}
