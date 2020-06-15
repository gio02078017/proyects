package app.epm.com.contacto_transparente_presentation.dto;

import java.util.ArrayList;

/**
 * Created by leidycarolinazuluagabastidas on 16/03/17.
 */

public class EvidenceParametersDTO {

    private String CodigoDelActoIndebido;
    private ArrayList<EvidenceDTO> EvidenciasDelActoIndebido;

    public String getCodigoDelActoIndebido() {
        return CodigoDelActoIndebido;
    }

    public void setCodigoDelActoIndebido(String codigoDelActoIndebido) {
        CodigoDelActoIndebido = codigoDelActoIndebido;
    }

    public ArrayList<EvidenceDTO> getEvidenciasDelActoIndebido() {
        return EvidenciasDelActoIndebido;
    }

    public void setEvidenciasDelActoIndebido(ArrayList<EvidenceDTO> evidenciasDelActoIndebido) {
        EvidenciasDelActoIndebido = evidenciasDelActoIndebido;
    }
}
