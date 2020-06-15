package app.epm.com.facturadomain.business_models;

import java.io.Serializable;

/**
 * Created by ocadavid on 16/12/2016.
 */

public class FacturaResponse  implements Serializable {


    public enum EstaInscrita{
        PARAINSCRIBIR(0), INSCRITA(1), NOINSCRITA(2);
        private int value;

        EstaInscrita(int value){
            this.value = value;
        }
    }

    private String descripcionContrato;
    private Integer id;
    private String documentoReferencia;
    private String fechaCreacion;
    private String fechaRecargo;
    private String fechaVencimiento;
    private String numeroContrato;
    private String numeroFactura;
    private Integer valorFactura;
    private String url;
    private boolean estadoPagoFactura;
    private boolean facturaVencida;
    private EstaInscrita estaInscrita;
    private boolean estaSeleccionadaParaPago;
    private boolean estaPendiente;
    private int code;
    private String text;
    private boolean eliminar;

    public String getDescripcionContrato() {
        return descripcionContrato;
    }

    public void setDescripcionContrato(String descripcionContrato) {
        this.descripcionContrato = descripcionContrato;
    }

    public Integer getId() {
        return id;
    }

    public void setId(Integer id) {
        this.id = id;
    }

    public String getDocumentoReferencia() {
        return documentoReferencia;
    }

    public void setDocumentoReferencia(String documentoReferencia) {
        this.documentoReferencia = documentoReferencia;
    }

    public String getFechaCreacion() {
        return fechaCreacion;
    }

    public void setFechaCreacion(String fechaCreacion) {
        this.fechaCreacion = fechaCreacion;
    }

    public String getFechaRecargo() {
        return fechaRecargo;
    }

    public void setFechaRecargo(String fechaRecargo) {
        this.fechaRecargo = fechaRecargo;
    }

    public String getFechaVencimiento() {
        return fechaVencimiento;
    }

    public void setFechaVencimiento(String fechaVencimiento) {
        this.fechaVencimiento = fechaVencimiento;
    }

    public String getNumeroContrato() {
        return numeroContrato;
    }

    public void setNumeroContrato(String numeroContrato) {
        this.numeroContrato = numeroContrato;
    }

    public String getNumeroFactura() {
        return numeroFactura;
    }

    public void setNumeroFactura(String numeroFactura) {
        this.numeroFactura = numeroFactura;
    }

    public Integer getValorFactura() {
        return valorFactura;
    }

    public void setValorFactura(Integer valorFactura) {
        this.valorFactura = valorFactura;
    }

    public String getUrl() {
        return url;
    }

    public void setUrl(String url) {
        this.url = url;
    }

    public boolean isEstadoPagoFactura() {
        return estadoPagoFactura;
    }

    public void setEstadoPagoFactura(boolean estadoPagoFactura) {
        this.estadoPagoFactura = estadoPagoFactura;
    }

    public boolean isFacturaVencida() {
        return facturaVencida;
    }

    public void setFacturaVencida(boolean facturaVencida) {
        this.facturaVencida = facturaVencida;
    }

    public EstaInscrita getEstaInscrita() {
        return estaInscrita;
    }

    public void setEstaInscrita(EstaInscrita estaInscrita) {
        this.estaInscrita = estaInscrita;
    }

    public boolean isEstaSeleccionadaParaPago() {
        return estaSeleccionadaParaPago;
    }

    public void setEstaSeleccionadaParaPago(boolean estaSeleccionadaParaPago) {
        this.estaSeleccionadaParaPago = estaSeleccionadaParaPago;
    }

    public boolean isEstaPendiente() {
        return estaPendiente;
    }

    public void setEstaPendiente(boolean estaPendiente) {
        this.estaPendiente = estaPendiente;
    }

    public int getCode() {
        return code;
    }

    public void setCode(int code) {
        this.code = code;
    }

    public String getText() {
        return text;
    }

    public void setText(String text) {
        this.text = text;
    }

    public boolean isEliminar() {
        return eliminar;
    }

    public void setEliminar(boolean eliminar) {
        this.eliminar = eliminar;
    }
}
