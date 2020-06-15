package app.epm.com.factura_presentation.dto;

/**
 * Created by ocadavid on 23/12/2016.
 */
public class HistoricoFacturaResponseDTO {

    private int Ano;

    private String BancoPagoFactura;

    private boolean EstadoPagoFactura;

    private String FechaPagoFactura;

    private String FechaRecargo;

    private String FechaVencimiento;

    private int Mes;

    private String NombreMes;

    private String NumeroFactura;

    private double ValorFactura;

    private String UrlFacturaDigital;

    public String getBancoPagoFactura() {
        return BancoPagoFactura;
    }

    public void setBancoPagoFactura(String bancoPagoFactura) {
        this.BancoPagoFactura = bancoPagoFactura;
    }

    public int getAnio() {
        return Ano;
    }

    public void setAnio(int ano) {
        this.Ano = Ano;
    }

    public boolean isEstadoPagoFactura() {
        return EstadoPagoFactura;
    }

    public void setEstadoPagoFactura(boolean estadoPagoFactura) {
        this.EstadoPagoFactura = estadoPagoFactura;
    }

    public String getFechaPagoFactura() {
        return FechaPagoFactura;
    }

    public void setFechaPagoFactura(String fechaPagoFactura) {
        this.FechaPagoFactura = fechaPagoFactura;
    }

    public String getFechaRecargo() {
        return FechaRecargo;
    }

    public void setFechaRecargo(String fechaRecargo) {
        this.FechaRecargo = fechaRecargo;
    }

    public String getFechaVencimiento() {
        return FechaVencimiento;
    }

    public void setFechaVencimiento(String fechaVencimiento) {
        this.FechaVencimiento = fechaVencimiento;
    }

    public int getMes() {
        return Mes;
    }

    public void setMes(int mes) {
        this.Mes = mes;
    }

    public String getNombreMes() {
        return NombreMes;
    }

    public void setNombreMes(String nombreMes) {
        this.NombreMes = nombreMes;
    }

    public String getNumeroFactura() {
        return NumeroFactura;
    }

    public void setNumeroFactura(String numeroFactura) {
        this.NumeroFactura = numeroFactura;
    }

    public double getValorFactura() {
        return ValorFactura;
    }

    public void setValorFactura(double valorFactura) {
        this.ValorFactura = valorFactura;
    }

    public String getUrlFacturaDigital() {
        return UrlFacturaDigital;
    }

    public void setUrlFacturaDigital(String urlFacturaDigital) {
        this.UrlFacturaDigital = urlFacturaDigital;
    }
}
