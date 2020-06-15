package app.epm.com.facturadomain.business_models;

import java.io.Serializable;

/**
 * Created by mateoquicenososa on 10/01/17.
 */

public class FacturasPorTransaccion implements Serializable {

    private String documentoReferencia;

    private int idFacturaPorTransaccion;

    private int idFactura;

    private String fechaCreacion;

    public String getDocumentoReferencia() {
        return documentoReferencia;
    }

    public void setDocumentoReferencia(String documentoReferencia) {
        this.documentoReferencia = documentoReferencia;
    }

    public int getIdFacturaPorTransaccion() {
        return idFacturaPorTransaccion;
    }

    public void setIdFacturaPorTransaccion(int idFacturaPorTransaccion) {
        this.idFacturaPorTransaccion = idFacturaPorTransaccion;
    }

    public int getIdFactura() {
        return idFactura;
    }

    public void setIdFactura(int idFactura) {
        this.idFactura = idFactura;
    }

    public String getFechaCreacion() {
        return fechaCreacion;
    }

    public void setFechaCreacion(String fechaCreacion) {
        this.fechaCreacion = fechaCreacion;
    }
}
