package app.epm.com.facturadomain.business_models;

import java.io.Serializable;

/**
 * Created by mateoquicenososa on 18/01/17.
 */

public class TransaccionPSEResponse implements Serializable {

    private String nombreEstadoTransaccion;

    private int estadoTransaccion;

    public String getNombreEstadoTransaccion() {
        return nombreEstadoTransaccion;
    }

    public void setNombreEstadoTransaccion(String nombreEstadoTransaccion) {
        this.nombreEstadoTransaccion = nombreEstadoTransaccion;
    }

    public int getEstadoTransaccion() {
        return estadoTransaccion;
    }

    public void setEstadoTransaccion(int estadoTransaccion) {
        this.estadoTransaccion = estadoTransaccion;
    }
}
