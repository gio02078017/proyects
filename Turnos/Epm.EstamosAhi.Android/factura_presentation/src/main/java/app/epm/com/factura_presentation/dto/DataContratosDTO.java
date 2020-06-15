package app.epm.com.factura_presentation.dto;

/**
 * Created by mateoquicenososa on 2/01/17.
 */

public class DataContratosDTO {
    private String Descripcion;
    private String Numero;
    private boolean RecibirFacturaDigital;
    private int Operacion;

    public String getDescripcion() {
        return Descripcion;
    }

    public void setDescripcion(String descripcion) {
        Descripcion = descripcion;
    }

    public String getNumero() {
        return Numero;
    }

    public void setNumero(String numero) {
        Numero = numero;
    }

    public boolean isRecibirFacturaDigital() {
        return RecibirFacturaDigital;
    }

    public void setRecibirFacturaDigital(boolean recibirFacturaDigital) {
        RecibirFacturaDigital = recibirFacturaDigital;
    }

    public int getOperacion() {
        return Operacion;
    }

    public void setOperacion(int operacion) {
        Operacion = operacion;
    }
}
