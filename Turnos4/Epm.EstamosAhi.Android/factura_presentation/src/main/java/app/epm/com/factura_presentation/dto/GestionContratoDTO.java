package app.epm.com.factura_presentation.dto;

import java.util.List;

/**
 * Created by mateoquicenososa on 2/01/17.
 */

public class GestionContratoDTO {

    private String CorreoElectronico;
    private List<DataContratosDTO> Contratos;

    public String getCorreoElectronico() {
        return CorreoElectronico;
    }

    public void setCorreoElectronico(String correoElectronico) {
        CorreoElectronico = correoElectronico;
    }

    public List<DataContratosDTO> getContratos() {
        return Contratos;
    }

    public void setContratos(List<DataContratosDTO> contratos) {
        Contratos = contratos;
    }
}
