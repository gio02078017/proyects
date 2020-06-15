package app.epm.com.factura_presentation.dto;

/**
 * Created by mateoquicenososa on 10/01/17.
 */

public class FacturasPorTransaccionDTO {

    private String DocumentoReferencia;

    private int IdFacturaPorTransaccion;

    private int IdFactura;

    private String FechaCreacion;

    public String getDocumentoReferencia() {
        return DocumentoReferencia;
    }

    public void setDocumentoReferencia(String documentoReferencia) {
        DocumentoReferencia = documentoReferencia;
    }

    public int getIdFacturaPorTransaccion() {
        return IdFacturaPorTransaccion;
    }

    public void setIdFacturaPorTransaccion(int idFacturaPorTransaccion) {
        IdFacturaPorTransaccion = idFacturaPorTransaccion;
    }

    public int getIdFactura() {
        return IdFactura;
    }

    public void setIdFactura(int idFactura) {
        IdFactura = idFactura;
    }

    public String getFechaCreacion() {
        return FechaCreacion;
    }

    public void setFechaCreacion(String fechaCreacion) {
        FechaCreacion = fechaCreacion;
    }
}
