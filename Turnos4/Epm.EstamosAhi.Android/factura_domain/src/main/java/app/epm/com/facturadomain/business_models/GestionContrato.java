package app.epm.com.facturadomain.business_models;

import java.util.List;

/**
 * Created by mateoquicenososa on 2/01/17.
 */

public class GestionContrato {

    private String correoElectronico;
    private List<DataContratos> contratos;

    public String getCorreoElectronico() {
        return correoElectronico;
    }

    public void setCorreoElectronico(String correoElectronico) {
        this.correoElectronico = correoElectronico;
    }

    public List<DataContratos> getContratos() {
        return contratos;
    }

    public void setContratos(List<DataContratos> contratos) {
        this.contratos = contratos;
    }
}
