package app.epm.com.facturadomain.business_models;

public class Transaction {



    private Integer idTransaccion;

    private String codigoTrazabilidad;

    private Integer estadoTransaccion;


    public Integer getIdTransaccion() {
        return idTransaccion;
    }

    public void setIdTransaccion(Integer idTransaccion) {
        this.idTransaccion = idTransaccion;
    }

    public String getCodigoTrazabilidad() {
        return codigoTrazabilidad;
    }

    public void setCodigoTrazabilidad(String codigoTrazabilidad) {
        this.codigoTrazabilidad = codigoTrazabilidad;
    }

    public Integer getEstadoTransaccion() {
        return estadoTransaccion;
    }

    public void setEstadoTransaccion(Integer estadoTransaccion) {
        this.estadoTransaccion = estadoTransaccion;
    }
}
