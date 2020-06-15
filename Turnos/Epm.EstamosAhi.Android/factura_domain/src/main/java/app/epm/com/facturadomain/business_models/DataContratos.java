package app.epm.com.facturadomain.business_models;

/**
 * Created by mateoquicenososa on 2/01/17.
 */

public class DataContratos {

    private String descripcion;
    private String numero;
    private boolean recibirFacturaDigital;
    private boolean eliminar;
    private int operacion;


    public String getDescripcion() {
        return descripcion;
    }

    public void setDescripcion(String descripcion) {
        this.descripcion = descripcion;
    }

    public String getNumero() {
        return numero;
    }

    public void setNumero(String numero) {
        this.numero = numero;
    }

    public boolean isRecibirFacturaDigital() {
        return recibirFacturaDigital;
    }

    public void setRecibirFacturaDigital(boolean recibirFacturaDigital) {
        this.recibirFacturaDigital = recibirFacturaDigital;
    }

    public boolean isEliminar() {
        return eliminar;
    }

    public void setEliminar(boolean eliminar) {
        this.eliminar = eliminar;
    }

    public int getOperacion() {
        return operacion;
    }

    public void setOperacion(int operacion) {
        this.operacion = operacion;
    }
}
