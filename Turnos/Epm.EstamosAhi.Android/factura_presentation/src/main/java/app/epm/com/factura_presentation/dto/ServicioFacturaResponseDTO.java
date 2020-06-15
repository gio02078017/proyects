package app.epm.com.factura_presentation.dto;

import androidx.annotation.Nullable;

import java.util.List;

/**
 * Created by ocadavid on 22/12/2016.
 */

public class ServicioFacturaResponseDTO {

    private String Servicio;

    private
    @Nullable
    int Consumo;

    private
    @Nullable
    double ValorAPagar;

    private String UnidadMedida;


    private List<DetalleServicioFacturaDTO> DetalleServicio;


    public String getServicio() {
        return Servicio;
    }

    public void setServicio(String servicio) {
        this.Servicio = servicio;
    }

    public
    @Nullable
    int getConsumo() {
        return Consumo;
    }

    public void setConsumo(@Nullable int consumo) {
        this.Consumo = consumo;
    }

    public
    @Nullable
    double getValorAPagar() {
        return ValorAPagar;
    }

    public void setValorAPagar(@Nullable double valorAPagar) {
        this.ValorAPagar = valorAPagar;
    }

    public String getUnidadDeMedida() {
        return UnidadMedida;
    }

    public void setUnidadDeMedida(String unidadDeMedida) {
        this.UnidadMedida = unidadDeMedida;
    }

    public List<DetalleServicioFacturaDTO> getDetalleServicio() {
        return DetalleServicio;
    }

    public void setDetalleServicio(List<DetalleServicioFacturaDTO> detalleServicio) {
        this.DetalleServicio = detalleServicio;
    }
}
