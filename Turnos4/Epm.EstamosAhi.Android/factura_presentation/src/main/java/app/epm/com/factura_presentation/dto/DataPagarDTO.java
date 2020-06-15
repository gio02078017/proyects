package app.epm.com.factura_presentation.dto;

import java.util.List;

/**
 * Created by mateoquicenososa on 6/01/17.
 */

public class DataPagarDTO {

    private int EntidadFinanciera;
    private String NumeroDocumento;
    private int IdTipoDocumento;
    private int IdTipoPersona;
    private String DireccionIp;
    private List<FacturaResponseDTO> FacturasPagar;

    public int getEntidadFinanciera() {
        return EntidadFinanciera;
    }

    public void setEntidadFinanciera(int entidadFinanciera) {
        EntidadFinanciera = entidadFinanciera;
    }

    public String getNumeroDocumento() {
        return NumeroDocumento;
    }

    public void setNumeroDocumento(String numeroDocumento) {
        NumeroDocumento = numeroDocumento;
    }

    public int getIdTipoDocumento() {
        return IdTipoDocumento;
    }

    public void setIdTipoDocumento(int idTipoDocumento) {
        IdTipoDocumento = idTipoDocumento;
    }

    public int getIdTipoPersona() {
        return IdTipoPersona;
    }

    public void setIdTipoPersona(int idTipoPersona) {
        IdTipoPersona = idTipoPersona;
    }

    public String getDireccionIp() {
        return DireccionIp;
    }

    public void setDireccionIp(String direccionIp) {
        DireccionIp = direccionIp;
    }

    public List<FacturaResponseDTO> getFacturasPagar() {
        return FacturasPagar;
    }

    public void setFacturasPagar(List<FacturaResponseDTO> facturasPagar) {
        FacturasPagar = facturasPagar;
    }
}
