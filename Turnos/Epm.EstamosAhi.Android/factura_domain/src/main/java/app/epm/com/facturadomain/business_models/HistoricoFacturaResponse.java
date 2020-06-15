package app.epm.com.facturadomain.business_models;

import java.io.Serializable;

/**
 * Created by ocadavid on 23/12/2016.
 */
public class HistoricoFacturaResponse implements Serializable {

    private int ano;

    private String bancoPagoFactura;

    private boolean estadoPagoFactura;

    private String fechaPagoFactura;

    private String fechaRecargo;

    private String fechaVencimiento;

    private int mes;

    private String nombreMes;

    private String numeroFactura;

    private double valorFactura;

    private String urlFacturaDigital;

    public String getBancoPagoFactura() {
        return bancoPagoFactura;
    }

    public void setBancoPagoFactura(String bancoPagoFactura) {
        this.bancoPagoFactura = bancoPagoFactura;
    }

    public int getAnio() {
        return ano;
    }

    public void setAnio(int ano) {
        this.ano = ano;
    }

    public boolean isEstadoPagoFactura() {
        return estadoPagoFactura;
    }

    public void setEstadoPagoFactura(boolean estadoPagoFactura) {
        this.estadoPagoFactura = estadoPagoFactura;
    }

    public String getFechaPagoFactura() {
        return fechaPagoFactura;
    }

    public void setFechaPagoFactura(String fechaPagoFactura) {
        this.fechaPagoFactura = fechaPagoFactura;
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

    public int getMes() {
        return mes;
    }

    public void setMes(int mes) {
        this.mes = mes;
    }

    public String getNombreMes() {
        return nombreMes;
    }

    public void setNombreMes(String nombreMes) {
        this.nombreMes = nombreMes;
    }

    public String getNumeroFactura() {
        return numeroFactura;
    }

    public void setNumeroFactura(String numeroFactura) {
        this.numeroFactura = numeroFactura;
    }

    public double getValorFactura() {
        return valorFactura;
    }

    public void setValorFactura(double valorFactura) {
        this.valorFactura = valorFactura;
    }

    public String getUrlFacturaDigital() {
        return urlFacturaDigital;
    }

    public void setUrlFacturaDigital(String urlFacturaDigital) {
        this.urlFacturaDigital = urlFacturaDigital;
    }
}
