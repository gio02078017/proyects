package app.epm.com.factura_presentation.dto;

/**
 * Created by ocadavid on 15/12/2016.
 */

public class FacturaResponseDTO {

    private String DescripcionContrato;
    private Integer Id;
    private String DocumentoReferencia;
    private String FechaCreacion;
    private String FechaRecargo;
    private String FechaVencimiento;
    private String NumeroContrato;
    private String NumeroFactura;
    private Integer ValorFactura;
    private String Url;
    private Boolean EstadoPagoFactura;
    private Boolean FacturaVencida;

    private int Codigo;
    private String Texto;

    public String getDescripcionContrato() {
        return DescripcionContrato;
    }

    public void setDescripcionContrato(String descripcionContrato) {
        DescripcionContrato = descripcionContrato;
    }

    public Integer getId() {
        return Id;
    }

    public void setId(Integer id) {
        Id = id;
    }

    public String getDocumentoReferencia() {
        return DocumentoReferencia;
    }

    public void setDocumentoReferencia(String documentoReferencia) {
        DocumentoReferencia = documentoReferencia;
    }

    public String getFechaCreacion() {
        return FechaCreacion;
    }

    public void setFechaCreacion(String fechaCreacion) {
        FechaCreacion = fechaCreacion;
    }

    public String getFechaRecargo() {
        return FechaRecargo;
    }

    public void setFechaRecargo(String fechaRecargo) {
        FechaRecargo = fechaRecargo;
    }
    public String getFechaVencimiento() {
        return FechaVencimiento;
    }

    public void setFechaVencimiento(String fechaVencimiento) {
        FechaVencimiento = fechaVencimiento;
    }

    public String getNumeroContrato() {
        return NumeroContrato;
    }

    public void setNumeroContrato(String numeroContrato) {
        NumeroContrato = numeroContrato;
    }

    public String getNumeroFactura() {
        return NumeroFactura;
    }

    public void setNumeroFactura(String numeroFactura) {
        NumeroFactura = numeroFactura;
    }

    public Integer getValorFactura() {
        return ValorFactura;
    }

    public void setValorFactura(Integer valorFactura) {
        ValorFactura = valorFactura;
    }

    public String getUrl() {
        return Url;
    }

    public void setUrl(String url) {
        Url = url;
    }
    public Boolean getEstadoPagoFactura() {
        return EstadoPagoFactura;
    }

    public void setEstadoPagoFactura(Boolean estadoPagoFactura) {
        EstadoPagoFactura = estadoPagoFactura;
    }

    public Boolean getFacturaVencida() {
        return FacturaVencida;
    }

    public void setFacturaVencida(Boolean facturaVencida) {
        FacturaVencida = facturaVencida;
    }

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
}