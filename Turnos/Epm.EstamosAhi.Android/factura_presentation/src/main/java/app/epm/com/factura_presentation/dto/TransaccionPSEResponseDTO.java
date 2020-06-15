package app.epm.com.factura_presentation.dto;

/**
 * Created by mateoquicenososa on 18/01/17.
 */

public class TransaccionPSEResponseDTO {

    private String NombreEstadoTransaccion;

    private int EstadoTransaccion;

    public String getNombreEstadoTransaccion() {
        return NombreEstadoTransaccion;
    }

    public void setNombreEstadoTransaccion(String nombreEstadoTransaccion) {
        NombreEstadoTransaccion = nombreEstadoTransaccion;
    }

    public int getEstadoTransaccion() {
        return EstadoTransaccion;
    }

    public void setEstadoTransaccion(int estadoTransaccion) {
        EstadoTransaccion = estadoTransaccion;
    }
}
