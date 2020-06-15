package app.epm.com.facturadomain.business_models;

import android.support.annotation.Nullable;

import java.util.List;

/**
 * Created by ocadavid on 22/12/2016.
 */
public class ServicioFacturaResponse {

    private String servicio;

    private @Nullable
    int consumo;

    private @Nullable double valorAPagar;

    private String unidadMedida;

    private List<DetalleServicioFactura> detalleServicio;

    public String getServicio() {
        return servicio;
    }

    public void setServicio(String servicio) {
        this.servicio = servicio;
    }

    public @Nullable int getConsumo() {
        return consumo;
    }

    public void setConsumo(@Nullable int consumo) {
        this.consumo = consumo;
    }

    public @Nullable double getValorAPagar() {
        return valorAPagar;
    }

    public void setValorAPagar(@Nullable double valorAPagar) {
        this.valorAPagar = valorAPagar;
    }

    public String getUnidadDeMedida() {
        return unidadMedida;
    }

    public void setUnidadDeMedida(String unidadDeMedida) {
        this.unidadMedida = unidadDeMedida;
    }

    public List<DetalleServicioFactura> getDetalleServicio() {
        return detalleServicio;
    }

    public void setDetalleServicio(List<DetalleServicioFactura> detalleServicio) {
        this.detalleServicio = detalleServicio;
    }
}
