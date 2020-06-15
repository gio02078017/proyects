package app.epm.com.facturadomain.business_models;

import java.util.ArrayList;

/**
 * Created by mateoquicenososa on 18/01/17.
 */

public class ProcesarInformacionPSE extends Transaction{

    private Integer  entidadFinanciera;
    private ArrayList<FacturasPorTransaccion> facturasPorTransaccion;

    public Integer getEntidadFinanciera() {
        return entidadFinanciera;
    }

    public void setEntidadFinanciera(Integer entidadFinanciera) {
        this.entidadFinanciera = entidadFinanciera;
    }

    public ArrayList<FacturasPorTransaccion> getFacturasPorTransaccion() {
        return facturasPorTransaccion;
    }

    public void setFacturasPorTransaccion(ArrayList<FacturasPorTransaccion> facturasPorTransaccion) {
        this.facturasPorTransaccion = facturasPorTransaccion;
    }
}
