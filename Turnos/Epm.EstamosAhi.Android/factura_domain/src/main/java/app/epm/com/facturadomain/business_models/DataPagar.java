package app.epm.com.facturadomain.business_models;

import java.io.Serializable;
import java.util.List;

/**
 * Created by mateoquicenososa on 6/01/17.
 */

public class DataPagar implements Serializable {

    private Integer entidadFinanciera;
    private String numeroDocumento;
    private Integer idTipoDocumento;
    private Integer idTipoPersona;
    private String direccionIp;
    private List<FacturaResponse> FacturasPagar;

    public Integer getEntidadFinanciera() {
        return entidadFinanciera;
    }

    public void setEntidadFinanciera(Integer entidadFinanciera) {
        this.entidadFinanciera = entidadFinanciera;
    }

    public List<FacturaResponse> getFacturasPagar() {
        return FacturasPagar;
    }

    public void setFacturasPagar(List<FacturaResponse> facturasPagar) {
        FacturasPagar = facturasPagar;
    }

    public String getDireccionIp() {
        return direccionIp;
    }

    public void setDireccionIp(String direccionIp) {
        this.direccionIp = direccionIp;
    }

    public Integer getIdTipoPersona() {
        return idTipoPersona;
    }

    public void setIdTipoPersona(Integer idTipoPersona) {
        this.idTipoPersona = idTipoPersona;
    }

    public Integer getIdTipoDocumento() {
        return idTipoDocumento;
    }

    public void setIdTipoDocumento(Integer idTipoDocumento) {
        this.idTipoDocumento = idTipoDocumento;
    }

    public String getNumeroDocumento() {
        return numeroDocumento;
    }

    public void setNumeroDocumento(String numeroDocumento) {
        this.numeroDocumento = numeroDocumento;
    }
}
